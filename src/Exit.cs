using SplashKitSDK;

namespace DescendBelow {
    public class Exit : StaticObject, ICollidable {
        private Collider _collider;
        private Room _sourceRoom, _destinationRoom;
        private Direction _direction;

        public Exit(Point2D position, double width, double height, Bitmap sprite, Direction direction, Room sourceRoom, Room destinationRoom) : base(position, width, height, sprite) {
            _collider = new Collider(this, 0);
            _sourceRoom = sourceRoom;
            _destinationRoom = destinationRoom;
            _direction = direction;
        }

        public Collider Collider {
            get { return _collider; }
        }

        public void Collide(Collider c) {
            if (c.GameObject is Player && _sourceRoom.Cleared) {
                Game.CurrentGame?.EnterRoom(_destinationRoom, GetOppositeDirection(_direction));
            }

            if (c.GameObject is Player && !_sourceRoom.Cleared) {
                Collider.ResolveDynamicStatic(c, _collider);
            }
        }

        private Direction GetOppositeDirection(Direction direction) {
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

        public override void Draw(DrawingOptions options)
        {
            if (!_sourceRoom.Cleared) {
                base.Draw(options);
            }
        }
    }
}