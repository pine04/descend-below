using SplashKitSDK;

namespace DescendBelow {
    public abstract class DynamicObject : GameObject {
        protected enum FacingDirection {
            Left,
            Right
        }

        protected Vector2D _velocity;

        public DynamicObject(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, int zIndex = 1) : base(position, width, height, sprite, zIndex) {
            _velocity = initialVelocity;
        }

        public override void Update(uint fps) {
            Point2D newPosition = new Point2D() {
                X = _position.X + _velocity.X * (1.0 / fps),
                Y = _position.Y + _velocity.Y * (1.0 / fps)
            };

            _position = newPosition;
        }

        public Vector2D Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public void MoveTo(Point2D destination) {
            _position = destination;
        }

        public void MoveBy(Vector2D displacement) {
            _position = SplashKit.PointAt(
                _position.X + displacement.X,
                _position.Y + displacement.Y
            );
        }

        protected FacingDirection GetFacingDirection() {
            if (Velocity.X >= 0) {
                return FacingDirection.Right;
            } else {
                return FacingDirection.Left;
            }
        }

        public override void Draw(DrawingOptions options)
        {
            if (GetFacingDirection() == FacingDirection.Left) {
                base.Draw(SplashKit.OptionFlipY(options));
            } else {
                base.Draw(options);
            }
        }
    }
}