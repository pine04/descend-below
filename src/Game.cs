using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace DescendBelow {
    public class Game {
        public static Game? CurrentGame;
        private Window _window;
        private Player _player;
        private Floor _floor;
        private Room _currentRoom;
        private List<GameObject> _objectsOnScreen;
        private DrawingOptions _options;

        public Game() {
            LoadResources();
            _window = new Window("Descend Below (Lite)", 1200, 768);
            _objectsOnScreen = new List<GameObject>();
            _player = new Player(SplashKit.PointAt(360, 360), 15, 39, SplashKit.BitmapNamed("arrow"), new Vector2D() { X = 0, Y = 0 });
            _objectsOnScreen.Add(_player);
            _options = SplashKit.OptionDefaults();
            _floor = new Floor();
            _currentRoom = _floor.StartRoom;
            _currentRoom.Enter();

            LoadRoom();

            CurrentGame = this;
        }

        private void LoadRoom() {
            _objectsOnScreen = new List<GameObject>();
            _objectsOnScreen.Add(_player);

            foreach (GameObject gameObject in _currentRoom.GameObjects) {
                _objectsOnScreen.Add(gameObject);
            }
        }

        public void Run() {
            do {
                HandleInputs();
                Update();
                HandleCollisions();
                Draw();

                _window.Refresh(60);
            } while (!_window.CloseRequested);
        }

        public void CleanUp() {
            SplashKit.FreeResourceBundle("game-resources");
        }

        private void LoadResources() {
            SplashKit.LoadResourceBundle("game-resources", "resources.txt");
        }

        private void HandleInputs() {
            SplashKit.ProcessEvents();

            _player.Halt();
            if (SplashKit.KeyDown(KeyCode.WKey)) {
                _player.MoveUp();
            }
            if (SplashKit.KeyDown(KeyCode.AKey)) {
                _player.MoveLeft();
            }
            if (SplashKit.KeyDown(KeyCode.SKey)) {
                _player.MoveDown();
            }
            if (SplashKit.KeyDown(KeyCode.DKey)) {
                _player.MoveRight();
            }

            if (SplashKit.MouseDown(MouseButton.LeftButton)) {
                _player.Attack(SplashKit.MousePosition());
            }
            
            if (SplashKit.KeyTyped(KeyCode.ReturnKey)) {
                _currentRoom.Clear();
            }
        }

        private void Update() {
            foreach (GameObject gameObject in new List<GameObject>(_objectsOnScreen)) {
                IDestroyable? destroyable = gameObject as IDestroyable;
                if (destroyable != null && destroyable.CanDestroy) {
                    _objectsOnScreen.Remove(gameObject);
                }
            }

            foreach (GameObject gameObject in _objectsOnScreen) {
                gameObject.Update(60);
            }
        }

        private void HandleCollisions() {
            for (int i = 0; i < _objectsOnScreen.Count; i++) {
                ICollidable? gameObject = _objectsOnScreen[i] as ICollidable;
                
                if (gameObject != null) {
                    for (int j = i + 1; j < _objectsOnScreen.Count; j++) {
                        ICollidable? targetObject = _objectsOnScreen[j] as ICollidable;

                        if (targetObject != null && gameObject.Collider.IsCollidingWith(targetObject.Collider)) {
                            gameObject.Collide(targetObject.Collider);
                            targetObject.Collide(gameObject.Collider);
                        }
                    }
                }
            }
        }

        private void Draw() {
            SplashKit.ClearScreen(SplashKit.RGBColor(95, 85, 106));

            SplashKit.FillRectangle(Color.RGBColor(123, 114, 67), 96, 96, 528, 528);
            foreach (GameObject gameObject in _objectsOnScreen) {
                gameObject.Draw(_options);
            }

            _floor.DrawMap(_currentRoom);
        }

        public void AddGameObjectOnScreen(GameObject gameObject) {
            _objectsOnScreen.Add(gameObject);
        }

        public void EnterRoom(Room room, Direction enterDirection) {
            _currentRoom = room;
            _currentRoom.Enter();
            LoadRoom();

            _player.MoveTo(SplashKit.PointAt(360, 360));
        }
    }
}