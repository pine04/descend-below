using SplashKitSDK;
using System;

namespace DescendBelow {
    // Defines the heal spell.
    public class HealSpell : Spell {
        private double _healPercentage;

        public HealSpell(double healPercentage, double cooldown) : base("Heal Spell", "Heals for " + Math.Round(healPercentage * 100) + "% of missing health. Cooldown: " + Math.Round(cooldown) + "s.", SplashKit.BitmapNamed("heal"), cooldown) {
            _healPercentage = healPercentage;
        }

        public override void CastSpell()
        {
            base.CastSpell();

            if (Game.CurrentGame != null) {
                int health = Game.CurrentGame.CurrentPlayer.Health;
                int maxHealth = Game.CurrentGame.CurrentPlayer.MaxHealth;

                Game.CurrentGame?.CurrentPlayer.Heal(Convert.ToInt32((maxHealth - health) * _healPercentage));
            }

            SplashKit.PlaySoundEffect("heal");
        }

        public override void DrawSpell(double x, double y)
        {
            base.DrawSpell(x, y);

            SplashKit.DrawText("HEAL %: " + Math.Round(_healPercentage * 100), Color.White, "pixel", 20, x + 64, y + 12);
        }
    }
}