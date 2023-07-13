using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace DescendBelow {
    public class Game {
        public static Game? CurrentGame;
        private Window _window;
        private DrawingOptions _options;
        private GameState _state;
        private Floor _floor;
        private int _floorCounter;
        private Room _currentRoom;
        private List<GameObject> _objectsOnScreen;
        private Player _player;

        private Game() {
            LoadResources();

            _window = new Window("Descend Below (Lite)", 1200, 768);
            _options = SplashKit.OptionDefaults();
            SplashKit.PlayMusic("music", 200);
            SplashKit.StartTimer("gameTimer");

            _objectsOnScreen = new List<GameObject>();
            _player = new Player(SplashKit.PointAt(360, 360), SplashKit.VectorTo(0, 0), 250);
            _objectsOnScreen.Add(_player);

            _floor = Floor.CreateFloorZero();
            _currentRoom = _floor.StartRoom;
            EnterRoom(_currentRoom, Direction.North);

            _state = GameState.Playing;
            _floorCounter = 0;
        }

        public static Game CreateGame() {
            if (CurrentGame == null) {
                CurrentGame = new Game();
            }
            return CurrentGame;
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
                    _player.MoveAlong(SplashKit.VectorTo(0, -1));
                }
                if (SplashKit.KeyDown(KeyCode.SKey)) {
                    _player.MoveAlong(SplashKit.VectorTo(0, 1));
                }
                if (SplashKit.KeyDown(KeyCode.AKey)) {
                    _player.MoveAlong(SplashKit.VectorTo(-1, 0));
                }
                if (SplashKit.KeyDown(KeyCode.DKey)) {
                    _player.MoveAlong(SplashKit.VectorTo(1, 0));
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
                        Interactable? interactable = gameObject as Interactable;

                        if (interactable == null) {
                            continue;
                        }

                        if (interactable.IsNearPlayer(_player) && interactable.IsHoveredOn(SplashKit.MousePosition())) {
                            interactable.HandleInteraction();
                        }
                    }
                }
            } else if (_state == GameState.Paused) {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey)) {
                    _state = GameState.Playing;
                    SplashKit.ResumeTimer("gameTimer");
                }
            } else if (_state == GameState.Lost) {
                if (SplashKit.KeyTyped(KeyCode.ReturnKey)) {
                    ResetGame();
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
            SplashKit.ClearScreen(Constants.BackgroundColor);

            SplashKit.FillRectangle(Color.RGBColor(153, 230, 95), 96, 96, 528, 528);
            List<GameObject> orderedObjects = new List<GameObject>(_objectsOnScreen);
            orderedObjects.Sort(CompareByZIndex);
            foreach (GameObject gameObject in orderedObjects) {
                gameObject.Draw(_options);
            }

            SplashKit.DrawBitmap("heart", 96, 648);
            SplashKit.DrawText(_player.Health + "/" + _player.MaxHealth, Color.White, "pixel", 32, 160, 656);

            if (_state == GameState.Paused) {
                SplashKit.DrawText("Paused, press ESC to continue.", Color.White, "pixel", 32, 128, 60);
            }

            if (_state == GameState.Lost) {
                SplashKit.DrawText("YOU DIED", Color.RGBColor(196, 36, 48), "pixel", 48, 360, 360);
            }

            SplashKit.DrawText("Floor " + _floorCounter, Color.White, "pixel", 16, 656, 96);

            _floor.DrawMinimap(656, 120, _currentRoom);
        }

        private int CompareByZIndex(GameObject a, GameObject b) {
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

            if (enterDirection == Direction.North) {
                _player.MoveTo(SplashKit.PointAt(360, 216));
            } else if (enterDirection == Direction.East) {
                _player.MoveTo(SplashKit.PointAt(504, 360));
            } else if (enterDirection == Direction.South) {
                _player.MoveTo(SplashKit.PointAt(360, 504));            
            } else if (enterDirection == Direction.West) {
                _player.MoveTo(SplashKit.PointAt(216, 360));
            }
        }

        private void LoadRoom() {
            _objectsOnScreen = new List<GameObject>();
            _objectsOnScreen.Add(_player);

            foreach (GameObject gameObject in _currentRoom.GameObjects) {
                _objectsOnScreen.Add(gameObject);
            }
        }

        public Player CurrentPlayer {
            get { return _player; }
        }

        public void EnterNewFloor() {
            _floor = Floor.CreateFloor();
            EnterRoom(_floor.StartRoom, Direction.North);
            _floorCounter++;
        }

        private void ResetGame() {
            _objectsOnScreen = new List<GameObject>();
            _player = new Player(SplashKit.PointAt(360, 360), new Vector2D() { X = 0, Y = 0 }, 250);
            _objectsOnScreen.Add(_player);
            _floor = Floor.CreateFloorZero();
            _currentRoom = _floor.StartRoom;
            _currentRoom.Enter();
            _state = GameState.Playing;
            _floorCounter = 0;
            LoadRoom();
        }
    }
}