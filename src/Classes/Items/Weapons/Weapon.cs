using SplashKitSDK;

namespace DescendBelow {
    // Represents a weapon that can be used by the player to attack.
    public abstract class Weapon : Item {
        protected Player? _holder;
        protected int _damage;
        protected double _attackCooldown;
        protected uint _timeSinceLastAtk; 

        public Weapon(string name, string description, Bitmap icon, int damage, double attackCooldown) : base(name, description, icon) {
            _damage = damage;
            _attackCooldown = attackCooldown;
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public void Attack(Point2D target) {
            if (ReadyForAttack()) {
                PerformAttack(target);
                IncurCooldown();
            }
        }

        protected virtual bool ReadyForAttack() {
            return SplashKit.TimerTicks("gameTimer") - _timeSinceLastAtk >= _attackCooldown * 1000;
        }

        protected abstract void PerformAttack(Point2D target);

        protected virtual void IncurCooldown() {
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public void DrawWeaponStat(double x, double y) {
            _icon.Draw(x, y);
            SplashKit.DrawText("DMG: " + _damage, Color.White, "pixel", 20, x + _icon.Width + 8, y + _icon.Height / 2 - 10);
        }

        public virtual void Upgrade() {
            _damage += 3;
        }

        public void ChangeHolder(Player? player) {
            _holder = player;
        }
    }
}