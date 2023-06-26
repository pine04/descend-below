using SplashKitSDK;

namespace DescendBelow {
    public abstract class DynamicObject : GameObject {
        protected Vector2D _velocity;

        public DynamicObject(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity) : base(position, width, height, sprite) {
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
    }
}