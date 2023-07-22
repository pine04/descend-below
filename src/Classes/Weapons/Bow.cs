using SplashKitSDK;

namespace DescendBelow {
    public class Bow : Weapon {
        public Bow(int damage, double attackCooldown) : base("Bow", "Weapon damage: " + damage, SplashKit.BitmapNamed("bow"), damage, attackCooldown) { }

        public override void Attack(Point2D startPosition, Point2D target)
        {
            base.Attack(startPosition, target);
            Vector2D direction = SplashKit.VectorPointToPoint(startPosition, target);
            Game.CurrentGame?.AddGameObjectOnScreen(new Projectile(startPosition, 15, 39, SplashKit.BitmapNamed("arrow"), direction, 600, ProjectileType.Friendly, _damage));
            SplashKit.PlaySoundEffect("arrow");
        }
    }
}