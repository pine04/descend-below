using System.Collections.Generic;
using SplashKitSDK;
using System;

namespace DescendBelow {
    public class Room {
        private List<GameObject> _gameObjects;
        private bool _entered;

        private Room() { 
            _gameObjects = new List<GameObject>();
            _entered = false;
        }

        public static Room CreateRoomZero() {
            Room room = new Room();

            room._gameObjects.Add(new Wall(SplashKit.PointAt(360, 120), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            room._gameObjects.Add(new Wall(SplashKit.PointAt(360, 600), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            room._gameObjects.Add(new Wall(SplashKit.PointAt(600, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            room._gameObjects.Add(new Wall(SplashKit.PointAt(120, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            room._gameObjects.Add(new Staircase(SplashKit.PointAt(360, 360)));

            return room;
        }

        public static Room CreateRoom(bool hasNorthExit, bool hasEastExit, bool hasSouthExit, bool hasWestExit) {
            Room room = new Room();

            if (hasNorthExit) {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(240, 120), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
                room._gameObjects.Add(new Wall(SplashKit.PointAt(480, 120), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
            } else {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(360, 120), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            }

            if (hasSouthExit) {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(240, 600), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
                room._gameObjects.Add(new Wall(SplashKit.PointAt(480, 600), 192, 48, SplashKit.BitmapNamed("shortHorizontalGrassWall")));
            } else {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(360, 600), 432, 48, SplashKit.BitmapNamed("longHorizontalGrassWall")));
            }

            if (hasEastExit) {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(600, 216), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
                room._gameObjects.Add(new Wall(SplashKit.PointAt(600, 504), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
            } else {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(600, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            }

            if (hasWestExit) {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(120, 216), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
                room._gameObjects.Add(new Wall(SplashKit.PointAt(120, 504), 48, 240, SplashKit.BitmapNamed("shortVerticalGrassWall")));
            } else {
                room._gameObjects.Add(new Wall(SplashKit.PointAt(120, 360), 48, 528, SplashKit.BitmapNamed("longVerticalGrassWall")));
            }

            Random random = new Random();
            int enemyCount = random.Next(3, 6);
            for (int i = 0; i < enemyCount; i++) {
                if (random.NextDouble() >= 0.5) {
                    room._gameObjects.Add(new Shrub(GetRandomEnemySpawnPosition()));
                } else {
                    room._gameObjects.Add(new Wizard(GetRandomEnemySpawnPosition()));
                }
            }

            return room;
        }

        public static Room CreateEndRoom(bool hasNorthExit, bool hasEastExit, bool hasSouthExit, bool hasWestExit) {
            Room room = CreateRoom(hasNorthExit, hasEastExit, hasSouthExit, hasWestExit);
            room._gameObjects.Add(new Staircase(SplashKit.PointAt(360, 360)));
            return room;
        }

        private static Point2D GetRandomEnemySpawnPosition() {
            Rectangle[] spawnZones = new Rectangle[] {
                SplashKit.RectangleFrom(SplashKit.PointAt(192, 192), 96, 96),
                SplashKit.RectangleFrom(SplashKit.PointAt(432, 192), 96, 96),
                SplashKit.RectangleFrom(SplashKit.PointAt(288, 288), 144, 144),
                SplashKit.RectangleFrom(SplashKit.PointAt(192, 432), 96, 96),
                SplashKit.RectangleFrom(SplashKit.PointAt(432, 432), 96, 96)
            };

            Random random = new Random();
            Rectangle zone = spawnZones[random.Next(5)];
            double x = zone.X + zone.Width * random.NextDouble();
            double y = zone.Y + zone.Height * random.NextDouble();

            return SplashKit.PointAt(x, y);
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

        public void AddExit(Direction direction, Room destination) {
            if (direction == Direction.North) {
                _gameObjects.Add(new Exit(SplashKit.PointAt(360, 108), 48, 24, SplashKit.BitmapNamed("blockedExitHorizontal"), direction, this, destination));
            } else if (direction == Direction.South) {
                _gameObjects.Add(new Exit(SplashKit.PointAt(360, 612), 48, 24, SplashKit.BitmapNamed("blockedExitHorizontal"), direction, this, destination));
            } else if (direction == Direction.East) {
                _gameObjects.Add(new Exit(SplashKit.PointAt(612, 360), 24, 48, SplashKit.BitmapNamed("blockedExitVertical"), direction, this, destination));
            } else if (direction == Direction.West) {
                _gameObjects.Add(new Exit(SplashKit.PointAt(108, 360), 24, 48, SplashKit.BitmapNamed("blockedExitVertical"), direction, this, destination));
            }
        }
    }
}