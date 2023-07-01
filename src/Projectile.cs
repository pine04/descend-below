using SplashKitSDK;

namespace DescendBelow {
    public class Projectile : DynamicObject, ICollidable, IDestroyable {
        private double _rotation;
        private Collider _collider;
        private bool _canDestroy;

        public Projectile(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, double targetSpeed) : base(position, width, height, sprite, initialVelocity) {
            _velocity = SplashKit.VectorMultiply(SplashKit.UnitVector(initialVelocity), targetSpeed);
            _rotation = SplashKit.VectorAngle(_velocity);
            _collider = new Collider(this, _rotation + 90);
            _canDestroy = false;
        }

        public override void Draw(DrawingOptions options) {
            double angle = 90 + _rotation;
            _sprite.Draw(_position.X - _width / 2, _position.Y - _height / 2, SplashKit.OptionRotateBmp(angle, options));
        }

        public void Collide(Collider c) {
            if (c.GameObject is StaticObject) {
                _canDestroy = true;
            }
        }

        public Collider Collider {
            get { return _collider; }
        }

        public bool CanDestroy {
            get { return _canDestroy; }
        }
    }
}