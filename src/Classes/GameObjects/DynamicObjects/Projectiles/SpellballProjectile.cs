using SplashKitSDK;

namespace DescendBelow {
    // Defines the behavior of the spell ball projectile, which heals its owner when it hits an enemy.
    public class SpellballProjectile : Projectile {
        private Animation _fireballAnimation;
        private Character _owner;

        public SpellballProjectile(Point2D position, Vector2D initialVelocity, double targetSpeed, ProjectileType type, int damage, Character owner) : base(position, 27, 42, SplashKit.BitmapNamed("spellball"), initialVelocity, targetSpeed, type, damage) {
            _fireballAnimation = SplashKit.AnimationScriptNamed("fireball").CreateAnimation("fly");
            _owner = owner;
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

        public override void Collide(Collider c)
        {
            base.Collide(c);

            if (c.GameObject is Enemy) {
                _owner.Heal((int)(_damage * 0.3));
            }
        }
    }
}