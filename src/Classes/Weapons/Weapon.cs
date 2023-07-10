using SplashKitSDK;

namespace DescendBelow {
    public abstract class Weapon {
        protected int _damage;
        protected double _attackCooldown;
        protected uint _timeSinceLastAtk; 

        public Weapon(int damage, double attackCooldown) {
            _damage = damage;
            _attackCooldown = attackCooldown;
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        protected bool ReadyForAttack() {
            return SplashKit.TimerTicks("gameTimer") - _timeSinceLastAtk >= _attackCooldown * 1000;
        }

        protected void IncurCooldown() {
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public abstract void Attack(Point2D startPosition, Point2D target);
    }
}