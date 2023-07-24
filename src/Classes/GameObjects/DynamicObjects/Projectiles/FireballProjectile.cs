using SplashKitSDK;

namespace DescendBelow {
    // Defines the behavior of a fireball projectile, which has an animation.
    public class FireballProjectile : Projectile {
        private Animation _fireballAnimation;

        public FireballProjectile(Point2D position, Vector2D initialVelocity, double targetSpeed, ProjectileType type, int damage) : base(position, 21, 39, SplashKit.BitmapNamed("fireball"), initialVelocity, targetSpeed, type, damage) {
            _fireballAnimation = SplashKit.AnimationScriptNamed("fireball").CreateAnimation("fly");
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(SplashKit.OptionWithAnimation(_fireballAnimation, options));
        }

        public override void Update(uint fps)
        {
            base.Update(fps);
            _fireballAnimation.Update();
        }
    }
}