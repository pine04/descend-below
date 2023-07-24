using SplashKitSDK;

namespace DescendBelow {
    // Defines the Bow weapon.
    public class Bow : Weapon {
        private int _attackCounter;
        public Bow(int damage, double attackCooldown) : base("Bow (Weapon)", "Fires three arrows every 5 attacks. Damage: " + damage, SplashKit.BitmapNamed("bow"), damage, attackCooldown) {
            _attackCounter = 0;
        }

        protected override void PerformAttack(Point2D target)
        {
            if (_holder == null) {
                return;
            }
            
            Point2D startPosition = _holder.Position;
            
            _attackCounter = (_attackCounter + 1) % 5;

            Vector2D direction = SplashKit.VectorPointToPoint(startPosition, target);
            Game.CurrentGame?.AddGameObjectOnScreen(new Projectile(startPosition, 15, 39, SplashKit.BitmapNamed("arrow"), direction, 600, ProjectileType.Friendly, _damage));

            if (_attackCounter == 4) {
                Game.CurrentGame?.AddGameObjectOnScreen(new Projectile(startPosition, 15, 39, SplashKit.BitmapNamed("arrow"), SplashKit.VectorFromAngle(SplashKit.VectorAngle(direction) - 10, 1), 600, ProjectileType.Friendly, (int)(_damage * 0.5)));
                Game.CurrentGame?.AddGameObjectOnScreen(new Projectile(startPosition, 15, 39, SplashKit.BitmapNamed("arrow"), SplashKit.VectorFromAngle(SplashKit.VectorAngle(direction) + 10, 1), 600, ProjectileType.Friendly, (int)(_damage * 0.5)));
            }

            _attackCounter += 1;
            SplashKit.PlaySoundEffect("arrow");
        }
    }
}