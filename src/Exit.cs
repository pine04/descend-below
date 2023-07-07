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
            if (c.GameObject is Player && _sourceRoom.IsClear()) {
                Game.CurrentGame?.EnterRoom(_destinationRoom, _direction.GetOpposite());
            }

            if (c.GameObject is Player && !_sourceRoom.IsClear()) {
                Collider.ResolveDynamicStatic(c, _collider);
            }
        }

        public override void Draw(DrawingOptions options)
        {
            if (!_sourceRoom.IsClear()) {
                base.Draw(options);
            }
        }
    }
}