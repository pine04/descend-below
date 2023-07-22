using SplashKitSDK;

namespace DescendBelow {
    public enum ProjectileType { Friendly, Hostile }
    
    public class Projectile : DynamicObject, ICollidable, IDestroyable {
        private double _rotation;
        private Collider _collider;
        private bool _canDestroy;
        private ProjectileType _projectileType;
        private int _damage;

        public Projectile(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, double targetSpeed, ProjectileType type, int damage) : base(position, width, height, sprite, initialVelocity, 50) {
            _velocity = SplashKit.VectorMultiply(SplashKit.UnitVector(initialVelocity), targetSpeed);
            _rotation = SplashKit.VectorAngle(_velocity);
            _collider = new Collider(this, _rotation + 90);
            _projectileType = type;
            _canDestroy = false;
            _damage = damage;
        }

        public override void Draw(DrawingOptions options) {
            double angle = 90 + _rotation;
            base.Draw(SplashKit.OptionRotateBmp(angle, options));
        }

        public virtual void Collide(Collider c) {
            if (c.GameObject is StaticObject) {
                _canDestroy = true;
            } else if (c.GameObject is Player && _projectileType == ProjectileType.Hostile) {
                _canDestroy = true;
                Player? player = c.GameObject as Player;
                player?.Damage(_damage);
            } else if (c.GameObject is Enemy && _projectileType == ProjectileType.Friendly) {
                _canDestroy = true;
                Enemy? enemy = c.GameObject as Enemy;
                enemy?.Damage(_damage);
            }
        }

        public Collider Collider {
            get { return _collider; }
        }

        public bool CanDestroy {
            get { return _canDestroy; }
        }

        public void Destroy() { }
    }
}