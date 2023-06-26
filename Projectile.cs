/* using SplashKitSDK;

namespace DescendBelow {
    public class Projectile : DynamicObject {

        public Projectile(Vector2D position, Vector2D direction, float speed, Bitmap sprite) {
            _position = position;
            _direction = SplashKit.UnitVector(direction);
            _speed = speed;
            _sprite = sprite;
        }

        public void Update() {
            Vector2D velocity = new Vector2D() {
                X = _direction.X * _speed,
                Y = _direction.Y * _speed
            };
            _position = SplashKit.VectorAdd(_position, velocity);
        }

        public void Draw(DrawingOptions options) {
            double angle = 90 - SplashKit.VectorAngle(new Vector2D() { X = _direction.X, Y = -_direction.Y });
            _sprite.Draw(_position.X, _position.Y, SplashKit.OptionRotateBmp(angle, options));
        }
    }
} */