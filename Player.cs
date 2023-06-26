using SplashKitSDK;

namespace DescendBelow {
    public class Player : DynamicObject {
        public Player(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity) : base(position, width, height, sprite, initialVelocity) { }

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
    }
}