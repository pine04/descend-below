using SplashKitSDK;
using System;

namespace DescendBelow {
    public class HealSpell : Spell {
        private double _healPercentage;

        public HealSpell(double healPercentage, double cooldown) : base("Heal Spell", "Heals you for " + healPercentage * 100 + "% of your missing health. Cooldown: " + cooldown + "s.", SplashKit.BitmapNamed("heal"), cooldown) {
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
    }
}