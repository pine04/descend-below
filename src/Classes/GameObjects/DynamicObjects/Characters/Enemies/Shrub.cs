using SplashKitSDK;

namespace DescendBelow {
    public class Shrub : Enemy {
        public Shrub(Point2D position) : base(position, 48, 48, SplashKit.BitmapNamed("shrub"), SplashKit.VectorTo(0, 0), 200) { }

        public override void Attack(Player player)
        {
            Vector2D direction = SplashKit.VectorPointToPoint(Position, player.Position);
            Game.CurrentGame?.AddGameObjectOnScreen(
                new Projectile(Position, 15, 21, SplashKit.BitmapNamed("leaf"), direction, 200, ProjectileType.Hostile, 20)
            );
        }
    }
}