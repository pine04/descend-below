using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace DescendBelow {
    // Defines the Lightning spell.
    public class LightningSpell : Spell {
        private int _damage;

        public LightningSpell(int damage, double cooldown) : base("Lightning Spell", "Deals " + damage + " damage to all enemies. Cooldown: " + Math.Round(cooldown) + "s.", SplashKit.BitmapNamed("lightning"), cooldown) {
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

        public override void DrawSpell(double x, double y)
        {
            base.DrawSpell(x, y);

            SplashKit.DrawText("DMG: " + _damage, Color.White, "pixel", 20, x + 64, y + 12);
        }
    }
}