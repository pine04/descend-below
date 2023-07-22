using SplashKitSDK;

namespace DescendBelow {
    public class SpellballProjectile : Projectile {
        private Animation _fireballAnimation;

        public SpellballProjectile(Point2D position, Vector2D initialVelocity, double targetSpeed, ProjectileType type, int damage) : base(position, 27, 42, SplashKit.BitmapNamed("spellball"), initialVelocity, targetSpeed, type, damage) {
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