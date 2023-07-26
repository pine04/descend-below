using SplashKitSDK;

namespace DescendBelow {
    // Defines the logic of a Floor, which consists of Rooms that the player can navigate.
    public class Floor {
        private Room[,] _rooms;
        private Room _startRoom;

        // Prevents the use of "new Floor()" outside of the Floor class. Outside users must use the static CreateFloorZero or CreateFloor methods
        // to create a new floor.
        private Floor() {
            _rooms = new Room[5, 5];
            _startRoom = Room.CreateRoomZero();
        }

        // Creates the initial floor, providing a start for the player.
        public static Floor CreateFloorZero() {
            Floor floor = new Floor();
            floor._rooms[2, 2] = Room.CreateRoomZero();
            floor._startRoom = floor._rooms[2, 2];

            return floor;
        }

        // Creates a normal floor.
        public static Floor CreateFloor(int floorLevel) {
            Floor floor = new Floor();

            string[,] layout = GetFloorLayout();
            GenerateRooms(floor, floorLevel, layout);
            ConnectRooms(floor);

            (int row, int col) = GetStartRoomPosition(layout);
            floor._startRoom = floor._rooms[row, col];

            return floor;
        }

        // Reads a random floor layout from a bitmap file, returning it as a 2D string array.
        private static string[,] GetFloorLayout() {
            Bitmap floorShapesBmp = SplashKit.BitmapNamed("floorShapes");
            int shapeKind = RandGen.RandomIntBetween(0, 10);

            string[,] layout = new string[5,5];

            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 5; y++) {
                    Color pixelColor = SplashKit.GetPixel(floorShapesBmp, x + shapeKind * 5, 4 - y);

                    if (pixelColor.Equals(Constants.TemplateStartRoomColor)) {
                        layout[y, x] = "S";
                    } else if (pixelColor.Equals(Constants.TemplateNormalRoomColor)) {
                        layout[y, x] = "X";
                    } else if (pixelColor.Equals(Constants.TemplateEndRoomColor)) {
                        layout[y, x] = "E";
                    } else {
                        layout[y, x] = "O";
                    }
                }
            }

            return layout;
        }

        // Generates the rooms for a floor given the supplied layout.
        private static void GenerateRooms(Floor floor, int floorLevel, string[,] layout) {
            bool hasNorthExit, hasEastExit, hasSouthExit, hasWestExit;

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

                    if (layout[i, j] == "E") {
                        floor._rooms[i, j] = Room.CreateEndRoom(floorLevel, hasNorthExit, hasEastExit, hasSouthExit, hasWestExit);
                    } else {
                        floor._rooms[i, j] = Room.CreateRoom(floorLevel, hasNorthExit, hasEastExit, hasSouthExit, hasWestExit);
                    }
                }
            }
        }

        // Connects the rooms of the given floor.
        private static void ConnectRooms(Floor floor) {
            for (int i = 0; i < floor._rooms.GetLength(0); i++) {
                for (int j = 0; j < floor._rooms.GetLength(1); j++) {
                    if (floor._rooms[i, j] == null) {
                        continue;
                    }

                    if (i > 0 && floor._rooms[i - 1, j] != null) {
                        floor._rooms[i, j].AddExit(Direction.North, floor._rooms[i - 1, j]);
                    }

                    if (i < 4 && floor._rooms[i + 1, j] != null) {
                        floor._rooms[i, j].AddExit(Direction.South, floor._rooms[i + 1, j]);
                    }

                    if (j > 0 && floor._rooms[i, j - 1] != null) {
                        floor._rooms[i, j].AddExit(Direction.West, floor._rooms[i, j - 1]);
                    }

                    if (j < 4 && floor._rooms[i, j + 1] != null) {
                        floor._rooms[i, j].AddExit(Direction.East, floor._rooms[i, j + 1]);
                    }
                }
            }
        }

        // Given the layout of a floor, identify the index of its starting room.
        private static (int, int) GetStartRoomPosition(string[,] layout) {
            for (int i = 0; i < layout.GetLength(0); i++) {
                for (int j = 0; j < layout.GetLength(1); j++) {
                    if (layout[i, j] == "S") {
                        return (i, j);
                    }
                }
            }

            return (0, 0);
        }

        public void DrawMinimap(double x, double y, Room currentRoom) {
            SplashKit.FillRectangle(Constants.MinimapBackgroundColor, x, y, 96, 96);
            Color tileColor;

            for (int i = 0; i < _rooms.GetLength(0); i++) {
                for (int j = 0; j < _rooms.GetLength(1); j++) {
                    if (_rooms[i, j] == null || !_rooms[i, j].Entered) {
                        continue;
                    }
                    
                    if (_rooms[i, j] == currentRoom) {
                        tileColor = Constants.MinimapCurrentRoomColor;
                    } else {
                        tileColor = Constants.MinimapRoomColor;
                    }
                        
                    SplashKit.FillRectangle(tileColor, x + 20 * j, y + 20 * i, 16, 16);
                }
            }
        }

        public Room StartRoom {
            get { return _startRoom; }
        }
    }
}