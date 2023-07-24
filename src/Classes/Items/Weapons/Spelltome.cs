using SplashKitSDK;

namespace DescendBelow {
    // Defines the Spell Tome weapon.
    public class Spelltome : Weapon {
        public Spelltome(int damage, double attackCooldown) : base("Spell Tome (Weapon)", "Expends health for life-stealing blasts. Damage: " + damage + ".", SplashKit.BitmapNamed("spelltome"), damage, attackCooldown) { }

        protected override void PerformAttack(Point2D target)
        {
            if (_holder == null) {
                return;
            }
            
            Point2D startPosition = _holder.Position;
            Vector2D direction = SplashKit.VectorPointToPoint(startPosition, target);
            Game.CurrentGame?.AddGameObjectOnScreen(new SpellballProjectile(startPosition, direction, 350, ProjectileType.Friendly, _damage, _holder));
            _holder.DamageWithoutPenalty((int)(_damage * 0.25));

            SplashKit.PlaySoundEffect("fireball");
        }

        protected override bool ReadyForAttack()
        {
            return base.ReadyForAttack() && _holder?.Health > _damage * 0.25;
        }
    }
}