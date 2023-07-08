using SplashKitSDK;

namespace DescendBelow {
    public abstract class Character : DynamicObject, ICollidable {
        protected int _health, _maxHealth;
        private Collider _collider;

        public Character(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, int maxHealth, int zIndex = 1) : base(position, width, height, sprite, initialVelocity, zIndex) {
            _collider = new Collider(this, 0);
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public Collider Collider {
            get { return _collider; }
        }

        public virtual void Collide(Collider c) {
            if (c.GameObject is StaticObject) {
                Collider.ResolveDynamicStatic(_collider, c);              
            }
        }

        public virtual void Damage(int amount) {
            if (amount <= 0) {
                return;
            }

            _health -= amount;

            if (_health < 0) {
                _health = 0;
            }
        }

        public bool IsDead() {
            return _health == 0;
        }

        public virtual void Heal(int amount) {
            if (amount <= 0) {
                return;
            }

            _health += amount;

            if (_health > _maxHealth) {
                _health = _maxHealth;
            }
        }

        public int Health {
            get { return _health; }
        }

        public int MaxHealth {
            get { return _maxHealth; }
        }
    }
}