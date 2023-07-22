using SplashKitSDK;

namespace DescendBelow {
    public abstract class Weapon : Item {
        protected int _damage;
        protected double _attackCooldown;
        protected uint _timeSinceLastAtk; 

        public Weapon(string name, string description, Bitmap icon, int damage, double attackCooldown) : base(name, description, icon) {
            _damage = damage;
            _attackCooldown = attackCooldown;
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public bool ReadyForAttack() {
            return SplashKit.TimerTicks("gameTimer") - _timeSinceLastAtk >= _attackCooldown * 1000;
        }

        public virtual void Attack(Point2D startPosition, Point2D target) {
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public void DrawWeaponStat(double x, double y) {
            _icon.Draw(0, 0);
            SplashKit.DrawBitmap("damageIcon", x, y);
            SplashKit.DrawText(_damage.ToString(), Color.White, "pixel", 24, x + 32, y + 6);
        }

        public virtual void Upgrade() {
            _damage += 3;
        }
    }
}