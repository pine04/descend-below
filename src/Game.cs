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
        private GameState _state;
        private int _floorCounter;

        public Game() {
            LoadResources();
            _window = new Window("Descend Below (Lite)", 1200, 768);
            _objectsOnScreen = new List<GameObject>();
            _player = new Player(SplashKit.PointAt(360, 360), 15, 39, SplashKit.BitmapNamed("arrow"), new Vector2D() { X = 0, Y = 0 }, 250);
            _objectsOnScreen.Add(_player);
            _options = SplashKit.OptionDefaults();
            _floor = new Floor(true);
            _currentRoom = _floor.StartRoom;
            _currentRoom.Enter();
            _state = GameState.Playing;
            _floorCounter = 0;

            LoadRoom();

            CurrentGame = this;
            SplashKit.PlayMusic("circumambient");
            SplashKit.StartTimer("gameTimer");
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

            if (_state == GameState.Playing) {

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

                if (SplashKit.KeyTyped(KeyCode.EscapeKey)) {
                    _state = GameState.Paused;
                    SplashKit.PauseTimer("gameTimer");
                }

                if (SplashKit.MouseClicked(MouseButton.RightButton)) {
                    foreach (GameObject gameObject in _objectsOnScreen) {
                        Interactible? interactible = gameObject as Interactible;

                        if (interactible == null) {
                            continue;
                        }

                        if (interactible.IsNearPlayer(_player) && interactible.IsHoveredOn(SplashKit.MousePosition())) {
                            interactible.HandleInteraction();
                        }
                    }
                }
            } else if (_state == GameState.Paused) {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey)) {
                    _state = GameState.Playing;
                    SplashKit.ResumeTimer("gameTimer");
                }
            }

        }

        private void Update() {
            if (_state == GameState.Playing) {

                foreach (GameObject gameObject in new List<GameObject>(_objectsOnScreen)) {
                    IDestroyable? destroyable = gameObject as IDestroyable;
                    if (destroyable != null && destroyable.CanDestroy) {
                        _objectsOnScreen.Remove(gameObject);

                        if (gameObject is Enemy) {
                            _currentRoom.GameObjects.Remove(gameObject);
                        }
                    }
                }

                foreach (GameObject gameObject in new List<GameObject>(_objectsOnScreen)) {
                    gameObject.Update(60);
                }

                if (_player.IsDead()) {
                    _state = GameState.Lost;
                }
            }
        }

        private void HandleCollisions() {
            if (_state == GameState.Playing) {

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
        }

        private void Draw() {
            SplashKit.ClearScreen(SplashKit.RGBColor(69, 40, 60));

            SplashKit.FillRectangle(Color.RGBColor(153, 230, 95), 96, 96, 528, 528);
            List<GameObject> orderedObjects = new List<GameObject>(_objectsOnScreen);
            orderedObjects.Sort(CompareByZIndex);
            foreach (GameObject gameObject in orderedObjects) {
                gameObject.Draw(_options);
            }

            SplashKit.DrawBitmap("heart", 96, 648);
            SplashKit.DrawText(_player.Health + "/" + _player.MaxHealth, Color.White, "pixel", 32, 160, 656);

            if (_state == GameState.Paused) {
                SplashKit.DrawText("Paused", Color.White, "pixel", 32, 128, 60);
            }

            if (_state == GameState.Lost) {
                SplashKit.DrawText("YOU DIED", Color.RGBColor(196, 36, 48), "pixel", 48, 360, 360);
            }

            SplashKit.DrawText("Floor " + _floorCounter, Color.White, "pixel", 16, 0, 0);

            _floor.DrawMap(_currentRoom);
        }

        public int CompareByZIndex(GameObject a, GameObject b) {
            if (a.ZIndex < b.ZIndex) {
                return -1;
            } else if (a.ZIndex == b.ZIndex) {
                return 0;
            } else {
                return 1;
            }
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

        public Player CurrentPlayer {
            get { return _player; }
        }

        public void EnterNewFloor() {
            _floor = new Floor(false);
            EnterRoom(_floor.StartRoom, Direction.North);
            _floorCounter++;
        }
    }
}