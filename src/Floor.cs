using SplashKitSDK;
using System;

namespace DescendBelow {
    public class Floor {
        private Room[,] _rooms;
        private Room _startRoom;

        public Floor(bool isStarterFloor) {
            if (!isStarterFloor) {
                string[,] layout = GetFloorLayout();
                _rooms = CreateRooms(layout);
                ConnectRooms();
            } else {
                _rooms = new Room[5,5];
                _startRoom = new Room(true, false, false, false, false);
                _rooms[2, 2] = _startRoom;
            }
        }

        private Room[,] CreateRooms(string[,] layout) {
            bool hasNorthExit, hasEastExit, hasSouthExit, hasWestExit;
            Room[,] rooms = new Room[5, 5];

            for (int i = 0; i < layout.GetLength(0); i++) {
                for (int j = 0; j < layout.GetLength(1); j++) {
                    if (layout[i, j] == "O") {
                        continue;
                    }

                    if (i > 0) {
                        hasNorthExit = layout[i - 1, j] != "O";
                    } else {
                        hasNorthExit = false;
                    }

                    if (i < 4) {
                        hasSouthExit = layout[i + 1, j] != "O";
                    } else {
                        hasSouthExit = false;
                    }

                    if (j > 0) {
                        hasWestExit = layout[i, j - 1] != "O";
                    } else {
                        hasWestExit = false;
                    }

                    if (j < 4) {
                        hasEastExit = layout[i, j + 1] != "O";
                    } else {
                        hasEastExit = false;
                    }

                    rooms[i, j] = new Room(false, hasNorthExit, hasEastExit, hasSouthExit, hasWestExit);

                    // Consider removign dis.
                    if (layout[i, j] == "S") {
                        _startRoom = rooms[i, j];
                    }
                }
            }

            return rooms;
        }

        public void ConnectRooms() {
            for (int i = 0; i < _rooms.GetLength(0); i++) {
                for (int j = 0; j < _rooms.GetLength(1); j++) {
                    if (_rooms[i, j] == null) {
                        continue;
                    }

                    if (i > 0 && _rooms[i - 1, j] != null) {
                        _rooms[i, j].AddExit(new Exit(SplashKit.PointAt(360, 108), 48, 24, SplashKit.BitmapNamed("blockedExitHorizontal"), Direction.North, _rooms[i, j], _rooms[i - 1, j]));
                    }

                    if (i < 4 && _rooms[i + 1, j] != null) {
                        _rooms[i, j].AddExit(new Exit(SplashKit.PointAt(360, 612), 48, 24, SplashKit.BitmapNamed("blockedExitHorizontal"), Direction.South, _rooms[i, j], _rooms[i + 1, j]));
                    }

                    if (j > 0 && _rooms[i, j - 1] != null) {
                        _rooms[i, j].AddExit(new Exit(SplashKit.PointAt(108, 360), 24, 48, SplashKit.BitmapNamed("blockedExitVertical"), Direction.West, _rooms[i, j], _rooms[i, j - 1]));
                    }

                    if (j < 4 && _rooms[i, j + 1] != null) {
                        _rooms[i, j].AddExit(new Exit(SplashKit.PointAt(612, 360), 24, 48, SplashKit.BitmapNamed("blockedExitVertical"), Direction.East, _rooms[i, j], _rooms[i, j + 1]));
                    }
                }
            }
        }

        private void PrintArr(string[,] arr) {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == "O") {
                        Console.Write(" " + "\t");
                        continue;
                    }
                    Console.Write(arr[i,j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void DrawMap(Room currentRoom) {
            SplashKit.FillRectangle(Color.RGBColor(155, 173, 183), 656, 96, 96, 96);
            Color tileColor;

            for (int i = 0; i < _rooms.GetLength(0); i++) {
                for (int j = 0; j < _rooms.GetLength(1); j++) {
                    if (_rooms[i, j] == null || !_rooms[i, j].Entered) {
                        continue;
                    }
                    
                    if (_rooms[i, j] == currentRoom) {
                        tileColor = Color.RGBColor(217, 87, 99);
                    } else {
                        tileColor = Color.White;
                    }
                        
                    SplashKit.FillRectangle(tileColor, 656 + 20 * j, 96 + 20 * i, 16, 16);
                }
            }
        }

        private string[,] GetFloorLayout() {
            Bitmap floorShapesBmp = SplashKit.BitmapNamed("floorShapes");
            int shapeKind = new Random().Next(0, 10);

            string[,] layout = new string[5,5];

            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    Color pixelColor = SplashKit.GetPixel(floorShapesBmp, x + shapeKind * 5, 4 - y);

                    if (CompareColor(pixelColor, Color.RGBColor(251, 242, 54))) {
                        layout[y, x] = "S";
                    } else if (CompareColor(pixelColor, Color.RGBColor(238, 195, 154))) {
                        layout[y, x] = "X";
                    } else if (CompareColor(pixelColor, Color.RGBColor(153, 229, 80))) {
                        layout[y, x] = "E";
                    } else {
                        layout[y, x] = "O";
                    }
                }
            }

            return layout;
        }

        private bool CompareColor(Color c1, Color c2) {
            return c1.A == c2.A && c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        public Room StartRoom {
            get { return _startRoom; }
        }
    }
}