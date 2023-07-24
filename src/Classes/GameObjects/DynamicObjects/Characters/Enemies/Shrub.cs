using System;
using SplashKitSDK;

namespace DescendBelow {
    // Defines the Shrub enemy.
    public class Shrub : Enemy {
        public Shrub(Point2D position, int floorLevel) : base(position, 48, 48, SplashKit.BitmapNamed("shrub"), SplashKit.VectorTo(0, 0), 140 + 10 * floorLevel, 3 + 2 * floorLevel, RandGen.RandomDoubleBetween(1.5, 2.5), 8 + 2 * floorLevel) { }

        public override void Attack(Player player)
        {
            Vector2D direction = SplashKit.VectorPointToPoint(Position, player.Position);
            Game.CurrentGame?.AddGameObjectOnScreen(
                new Projectile(Position, 15, 21, SplashKit.BitmapNamed("leaf"), direction, 300, ProjectileType.Hostile, _attackDamage)
            );
            SplashKit.PlaySoundEffect("leaf");
        }

        public override void Move(Player player) {
            if (SplashKit.PointPointDistance(Position, player.Position) >= 192) {
                Velocity = SplashKit.VectorMultiply(SplashKit.UnitVector(SplashKit.VectorPointToPoint(Position, player.Position)), 72);
            } else {
                Velocity = SplashKit.VectorTo(0, 0);
            }
        }
    }
}