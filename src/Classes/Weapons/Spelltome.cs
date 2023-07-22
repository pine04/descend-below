using SplashKitSDK;

namespace DescendBelow {
    public class Spelltome : Weapon {
        private int _attackCounter;

        public Spelltome(int damage, double attackCooldown) : base("Spell Tome", "Weapon damage: " + damage + ". Empowers every 3 attacks.", SplashKit.BitmapNamed("spelltome"), damage, attackCooldown) {
            _attackCounter = 0;
        }

        public override void Attack(Point2D startPosition, Point2D target)
        {
            base.Attack(startPosition, target);

            _attackCounter = (_attackCounter + 1) % 3;
            Vector2D direction = SplashKit.VectorPointToPoint(startPosition, target);

            if (_attackCounter == 2) {
                Game.CurrentGame?.AddGameObjectOnScreen(new SpellballExProjectile(startPosition, direction, 450, ProjectileType.Friendly, _damage * 3));
            } else {
                Game.CurrentGame?.AddGameObjectOnScreen(new SpellballProjectile(startPosition, direction, 350, ProjectileType.Friendly, _damage));
            }
        }
    }
}