using SplashKitSDK;

namespace DescendBelow {
    public class Player : Character {
        private Weapon _weapon;
        private uint _timeSinceLastHit, _timeSinceLastRegen;

        public Player(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, int maxHealth) : base(position, width, height, sprite, initialVelocity, maxHealth, 20) {
            _weapon = new Bow(10, .5);
            _timeSinceLastHit = 0;
            _timeSinceLastRegen = 0;
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

        public void Attack(Point2D target) {
            _weapon.Attack(_position, target);
        }

        public override void Damage(int amount)
        {
            base.Damage(amount);
            _timeSinceLastHit = SplashKit.TimerTicks("gameTimer");
        }

        private void Regenerate() {
            Heal(1);
            _timeSinceLastRegen = SplashKit.TimerTicks("gameTimer");
        }

        public override void Update(uint fps)
        {
            base.Update(fps);
            if (SplashKit.TimerTicks("gameTimer") - _timeSinceLastHit >= 3000 && SplashKit.TimerTicks("gameTimer") - _timeSinceLastRegen >= 500) {
                Regenerate();
            }
        }
    }
}