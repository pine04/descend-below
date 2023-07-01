using SplashKitSDK;

namespace DescendBelow {
    public class Bow : Weapon {
        public Bow(double damage, double attackCooldown) : base(damage, attackCooldown) { }

        public override void Attack(Point2D startPosition, Point2D target)
        {
            if (ReadyForAttack()) {
                Vector2D direction = SplashKit.VectorPointToPoint(startPosition, target);
                Game.CurrentGame?.AddGameObjectOnScreen(new Projectile(startPosition, 15, 39, SplashKit.BitmapNamed("arrow"), direction, 300));
                IncurCooldown();
            }
        }
    }
}