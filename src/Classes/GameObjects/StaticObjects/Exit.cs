using SplashKitSDK;

namespace DescendBelow {
    // Defines the exit gates that take the player to other rooms.
    public class Exit : StaticObject, ICollidable {
        private Collider _collider;
        private Room _sourceRoom, _destinationRoom;
        private Direction _direction;
        private Animation _arrowAnimation;

        public Exit(Point2D position, double width, double height, Bitmap sprite, Direction direction, Room sourceRoom, Room destinationRoom) : base(position, width, height, sprite) {
            _collider = new Collider(this, 0);
            _sourceRoom = sourceRoom;
            _destinationRoom = destinationRoom;
            _direction = direction;
            _arrowAnimation = SplashKit.AnimationScriptNamed("exitArrow").CreateAnimation("bob");
        }

        public Collider Collider {
            get { return _collider; }
        }

        // If the room is not clear, the exit acts as a wall. Otherwise, it is open and the player is taken to the destination room.
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
            } else {
                DrawExitArrow(options);
            }
        }

        private void DrawExitArrow(DrawingOptions options) {
            double x = 0, y = 0, rotation = 0;

            if (_direction == Direction.North) {
                x = Position.X - 13;
                y = Position.Y + 16;
                rotation = 0;
            } else if (_direction == Direction.East) {
                x = Position.X - 55;
                y = Position.Y - 18;
                rotation = 90;
            } else if (_direction == Direction.South) {
                x = Position.X - 13;
                y = Position.Y - 55;
                rotation = 180;
            } else if (_direction == Direction.West) {
                x = Position.X + 13;
                y = Position.Y - 18;
                rotation = 270;
            }

            SplashKit.DrawBitmap("exitArrow", x, y, SplashKit.OptionWithAnimation(_arrowAnimation, SplashKit.OptionRotateBmp(rotation, options)));
        }

        public override void Update(uint fps)
        {
            _arrowAnimation.Update();
        }
    }
}