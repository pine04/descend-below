using SplashKitSDK;
using System.Collections.Generic;

namespace DescendBelow {
    public class LightningSpell : Spell {
        private int _damage;

        public LightningSpell(int damage, double cooldown) : base("Lightning Spell", "Deals " + damage + " to all enemies. Cooldown: " + cooldown + "s.", SplashKit.BitmapNamed("lightning"), cooldown) {
            _damage = damage;
        }

        public override void CastSpell()
        {
            base.CastSpell();

            List<Enemy>? enemies = Game.CurrentGame?.GetAllEnemies();

            if (enemies != null) {
                foreach (Enemy enemy in enemies) {
                    enemy.Damage(_damage);
                }
            }

            SplashKit.PlaySoundEffect("lightning");
        }
    }
}