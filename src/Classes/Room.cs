using System.Collections.Generic;
using SplashKitSDK;

namespace DescendBelow {
    public class Room {
        private List<GameObject> _gameObjects;
        private bool _entered;

        public Room(bool isStarterRoom, bool hasNorthExit, bool hasEastExit, bool hasSouthExit, bool hasWestExit) {
            _gameObjects = new List<GameObject>();
            _entered = false;
            if (isStarterRoom) {
                _gameObjects.Add(new Wall(SplashKit.PointAt(360, 120), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(360, 600), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(600, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
                _gameObjects.Add(new Wall(SplashKit.PointAt(120, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
                _gameObjects.Add(new Staircase(SplashKit.PointAt(360, 360)));
            } else {
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

                _gameObjects.Add(new Wizard(SplashKit.PointAt(200, 200)));

                _gameObjects.Add(new Staircase(SplashKit.PointAt(300, 300)));
            }

        }

        public bool Entered {
            get { return _entered; }
        }

        public bool IsClear() {
            foreach (GameObject gameObject in _gameObjects) {
                if (gameObject is Enemy) {
                    return false;
                }
            }
            return true;
        }

        public List<GameObject> GameObjects {
            get { return _gameObjects; }
        }

        public void Enter() {
            _entered = true;
        }

        public void AddExit(Exit exit) {
            _gameObjects.Add(exit);
        }
    }
}