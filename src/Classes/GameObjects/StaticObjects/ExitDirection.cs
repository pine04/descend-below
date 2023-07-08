namespace DescendBelow {
    public enum Direction {
        North,
        East,
        South,
        West
    }

    public static class Extensions {
        public static Direction GetOpposite(this Direction direction) {
            switch (direction) {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.South;
                case Direction.West:
                    return Direction.East;
                default:
                    return Direction.North;
            }
        }
    }
}