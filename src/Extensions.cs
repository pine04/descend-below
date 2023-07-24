using SplashKitSDK;

namespace DescendBelow {
    public static class Extensions {
        // Gets the opposite direction.
        public static Direction GetOpposite(this Direction direction) {
            switch (direction) {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.North;
            }
        }

        // Checks if two colors are the same.
        public static bool Equals(this Color color, Color targetColor) {
            return color.A == targetColor.A && color.R == targetColor.R && color.G == targetColor.G && color.B == targetColor.B;
        }
    }
}