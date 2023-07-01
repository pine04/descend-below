using System.Collections.Generic;
using SplashKitSDK;

namespace DescendBelow {
    public class Room {
        private List<GameObject> _gameObjects;
        private bool _cleared;
        private bool _entered;

        public Room(bool hasNorthExit, bool hasEastExit, bool hasSouthExit, bool hasWestExit) {
            _gameObjects = new List<GameObject>();
            _cleared = false;
            _entered = false;

            if (hasNorthExit) {
                _gameObjects.Add(new Wall(SplashKit.PointAt(240, 120), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(480, 120), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
            } else {
                _gameObjects.Add(new Wall(SplashKit.PointAt(360, 120), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            }

            if (hasSouthExit) {
                _gameObjects.Add(new Wall(SplashKit.PointAt(240, 600), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(480, 600), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
            } else {
                _gameObjects.Add(new Wall(SplashKit.PointAt(360, 600), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            }

            if (hasEastExit) {
                _gameObjects.Add(new Wall(SplashKit.PointAt(600, 216), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(600, 504), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
            } else {
                _gameObjects.Add(new Wall(SplashKit.PointAt(600, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            }

            if (hasWestExit) {
                _gameObjects.Add(new Wall(SplashKit.PointAt(120, 216), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(120, 504), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
            } else {
                _gameObjects.Add(new Wall(SplashKit.PointAt(120, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            }
        }

        public bool Cleared {
            get { return _cleared; }
        }

        public bool Entered {
            get { return _entered; }
        }

        public void CheckIfCleared() {
            foreach (GameObject gameObject in _gameObjects) {
                if (gameObject is Enemy) {
                    _cleared = false;
                    return;
                }
            }
            _cleared = true;
        }

        public List<GameObject> GameObjects {
            get { return _gameObjects; }
        }

        public void Enter() {
            _entered = true;
        }

        public void Clear() {
            _cleared = true;
        }

        public void AddExit(Exit exit) {
            _gameObjects.Add(exit);
        }
    }
}