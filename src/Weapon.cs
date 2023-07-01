using SplashKitSDK;

namespace DescendBelow {
    public abstract class Weapon {
        protected double _damage;
        protected double _attackCooldown;
        protected uint _timeSinceLastAtk; 

        public Weapon(double damage, double attackCooldown) {
            _damage = damage;
            _attackCooldown = attackCooldown;
            _timeSinceLastAtk = SplashKit.CurrentTicks();
        }

        protected bool ReadyForAttack() {
            return SplashKit.CurrentTicks() - _timeSinceLastAtk >= _attackCooldown * 1000;
        }

        protected void IncurCooldown() {
            _timeSinceLastAtk = SplashKit.CurrentTicks();
        }

        public abstract void Attack(Point2D startPosition, Point2D target);
    }
}