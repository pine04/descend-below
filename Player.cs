using SplashKitSDK;

namespace DescendBelow {
    public class Player : DynamicObject, ICollidable {
        private Collider _collider;
        public Player(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity) : base(position, width, height, sprite, initialVelocity) {
            _collider = new Collider(this, 0);
        }

        public void Halt() {
            Velocity = new Vector2D() { X = 0, Y = 0 };
        }

        public void MoveLeft() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = -1, Y = 0 }, 100));
        }

        public void MoveRight() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 1, Y = 0 }, 100));
        }

        public void MoveUp() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 0, Y = -1 }, 100));
        }

        public void MoveDown() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 0, Y = 1 }, 100));
        }

        public Collider Collider {
            get { return _collider; }
        }

        public void Collide(Collider c) {
            if (c.GameObject is Wall) {
                Console.WriteLine("Has collided with wall.");
            }
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(options);
            SplashKit.DrawQuad(Color.Black, _collider.GetColliderBox());
        }
    }
}