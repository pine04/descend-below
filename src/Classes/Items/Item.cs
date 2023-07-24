using SplashKitSDK;

namespace DescendBelow {
    // Represents an object that has a name, description, and icon.
    public abstract class Item {
        protected string _name, _description;
        protected Bitmap _icon;

        public Item(string name, string description, Bitmap icon) {
            _name = name;
            _description = description;
            _icon = icon;
        }

        public void DrawItem(double x, double y) {
            _icon.Draw(x, y);
            SplashKit.DrawText(_name, Color.White, "pixel", 22, x + 64, y);
            SplashKit.DrawText(_description, Color.White, "pixel", 16, x + 64, y + 28);
        }
    }
}