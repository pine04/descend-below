using SplashKitSDK;

namespace DescendBelow {
    // Defines a health potion that can heal the player.
    public class HealthPotion : Item {
        private int _charges;

        public HealthPotion(int charges) : base("Health Potion", "Heals for 100 HP. " + charges + " charges.", SplashKit.BitmapNamed("potion")) {
            _charges = charges;
        }

        public void Drink(Player p) {
            if (_charges <= 0) {
                return;
            }

            p.Heal(100);
            _charges -= 1;
            SplashKit.PlaySoundEffect("potion");
        }

        public int GetCharges() {
            return _charges;
        }

        public void AddCharges(int charges) {
            _charges += charges;
        }

        public void DrawHealthPotion(double x, double y) {
            _icon.Draw(x, y);
            SplashKit.DrawText("x" + _charges, Color.White, "pixel", 20, x + 24, y + 32);
        }
    }
}