using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace DescendBelow {
    public class Game {
        private enum GameState {
            Playing,
            Paused,
            Lost,
            OpenChest
        }
        
        public static Game? CurrentGame;
        private Window _window;
        private DrawingOptions _options;
        private GameState _state;
        private Floor _floor;
        private int _floorCounter;
        private Room _currentRoom;
        private List<GameObject> _objectsOnScreen;
        private Player _player;
        private Chest? _activeChest;

        private Game() {
            LoadResources();

            _window = new Window("Descend Below", 1200, 768);
            _options = SplashKit.OptionDefaults();
            SplashKit.PlayMusic("music", 200);
            SplashKit.SetMusicVolume(0.75f);
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

                if (SplashKit.KeyTyped(KeyCode.EKey)) {
                    _player.UseSpell();
                }

                if (SplashKit.KeyTyped(KeyCode.QKey)) {
                    _player.DrinkPotion();
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
            } else if (_state == GameState.OpenChest) {
                if (SplashKit.KeyTyped(KeyCode.EscapeKey)) {
                    CloseChest();
                }
                if (SplashKit.MouseClicked(MouseButton.LeftButton)) {
                    HandleChestInteraction();
                }
            }
        }

        private void Update() {
            if (_state == GameState.Playing) {

                foreach (GameObject gameObject in new List<GameObject>(_objectsOnScreen)) {
                    IDestroyable? destroyable = gameObject as IDestroyable;
                    if (destroyable != null && destroyable.CanDestroy) {
                        destroyable.Destroy();

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

            SplashKit.DrawBitmap("grassfloor", 96, 96);

            List<GameObject> orderedObjects = new List<GameObject>(_objectsOnScreen);
            orderedObjects.Sort(CompareByZIndex);
            foreach (GameObject gameObject in orderedObjects) {
                gameObject.Draw(_options);
            }

            _player.DrawPlayerStats(96, 648);

            DrawGameState();

            SplashKit.DrawText("Floor " + _floorCounter, Color.White, "pixel", 16, 656, 96);
            _floor.DrawMinimap(656, 120, _currentRoom);

            DrawGameInstructions(656, 256);

            if (_state == GameState.OpenChest && _activeChest != null) {
                _activeChest.DrawChestContent(656, 500);
            }
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

        private void DrawGameState() {
            if (_state == GameState.Paused) {
                SplashKit.DrawText("Paused, press ESC to continue.", Color.White, "pixel", 32, 128, 60);
            }

            if (_state == GameState.OpenChest) {
                SplashKit.DrawText("Press ESC to close the chest.", Color.White, "pixel", 32, 128, 60);
            }

            if (_state == GameState.Lost) {
                SplashKit.FillRectangle(Color.RGBAColor(1, 1, 1, 0.5), 96, 96, 528, 528);
                SplashKit.DrawText("YOU DIED", Constants.LostTextColor, "pixel", 48, 264, 360);

                int resultWidth = SplashKit.TextWidth("You made it through " + _floorCounter + " floors.", "pixel", 24);
                SplashKit.DrawText("You made it through " + _floorCounter + " floor(s).", Constants.LostTextColor, "pixel", 24, 360 - resultWidth / 2, 416);

                SplashKit.DrawText("Press ENTER to play again.", Constants.LostTextColor, "pixel", 24, 208, 448);
            }
        }

        private void DrawGameInstructions(double x, double y) {
            SplashKit.DrawText("Instructions:", Color.White, "pixel", 24, x, y);
            SplashKit.DrawText("Clear rooms and find the room with a staircase.", Color.White, "pixel", 16, x, y + 28);
            SplashKit.DrawText("Interact with the staircase to descend below. How far can you go?", Color.White, "pixel", 16, x, y + 48);

            SplashKit.DrawText("WASD to move.", Color.White, "pixel", 16, x, y + 76);
            SplashKit.DrawText("Left click to attack.", Color.White, "pixel", 16, x, y + 96);
            SplashKit.DrawText("Right click to interact with a nearby object.", Color.White, "pixel", 16, x, y + 116);
            SplashKit.DrawText("E to use spell (only if equipped.)", Color.White, "pixel", 16, x, y + 136);
            SplashKit.DrawText("Q to use a healing charge.", Color.White, "pixel", 16, x, y + 156);
            SplashKit.DrawText("ESC to pause/unpause.", Color.White, "pixel", 16, x, y + 176);
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
            _floorCounter++;
            _floor = Floor.CreateFloor(_floorCounter);
            EnterRoom(_floor.StartRoom, Direction.North);
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

        public void OpenChest(Chest chest) {
            SplashKit.PauseTimer("gameTimer");
            _state = GameState.OpenChest;
            _activeChest = chest;
        }

        private void CloseChest() {
            SplashKit.ResumeTimer("gameTimer");
            _state = GameState.Playing;
            _activeChest = null;
        }

        private int GetItemIndexFromMousePosition(Point2D mousePosition) {
            int x = 656, y = 500;

            if (mousePosition.X < x + 450 || mousePosition.X > x + 519) {
                return -1;
            }

            int yLeft = (int)Math.Ceiling((mousePosition.Y - y - 35) / 72);
            int yRight = (int)Math.Floor((mousePosition.Y - y - 8) / 72);

            if (yLeft != yRight) {
                return -1;
            } else {
                return yLeft;
            }
        }

        private void HandleChestInteraction() {
            int index = GetItemIndexFromMousePosition(SplashKit.MousePosition());
            Item? chestItem = _activeChest?.GetItem(index);

            if (chestItem != null) {
                Item? playerItem = _player.TakeNewItem(chestItem);

                if (playerItem != null) {
                    _activeChest?.AddItem(playerItem);
                }
            }
        }

        public List<Enemy> GetAllEnemies() {
            List<Enemy> enemies = new List<Enemy>();

            foreach (GameObject gameObject in _objectsOnScreen) {
                Enemy? enemy = gameObject as Enemy;
                if (enemy != null) {
                    enemies.Add(enemy);
                }
            }

            return enemies;
        }
    }
}