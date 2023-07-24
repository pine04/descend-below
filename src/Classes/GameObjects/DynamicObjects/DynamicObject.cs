using SplashKitSDK;

namespace DescendBelow {
    // Represents a game object that can move with a velocity.
    public abstract class DynamicObject : GameObject {
        // Depending on the X component of the velocity, the sprite of the game object can be flipped across the Y axis.
        protected enum FacingDirection {
            Left,
            Right
        }

        // The velocity should be defined in pixels/second.
        protected Vector2D _velocity;

        public DynamicObject(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, int zIndex = 1) : base(position, width, height, sprite, zIndex) {
            _velocity = initialVelocity;
        }

        // Updates the game object's position every frame according to the velocity.
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

        // Moves the object to a set location.
        public void MoveTo(Point2D destination) {
            _position = destination;
        }

        // Moves the object by a displacement vector.
        public void MoveBy(Vector2D displacement) {
            _position = SplashKit.PointAt(
                _position.X + displacement.X,
                _position.Y + displacement.Y
            );
        }

        protected virtual FacingDirection GetFacingDirection() {
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