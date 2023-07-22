using SplashKitSDK;
using System;

namespace DescendBelow {
    public abstract class Spell : Item {
        private double _cooldown;
        private uint _lastCastTime;

        public Spell(string name, string description, Bitmap icon, double cooldown) : base(name, description, icon) {
            _cooldown = cooldown;
            _lastCastTime = uint.MinValue;
        }

        public bool ReadyToCast() {
            return SplashKit.TimerTicks("gameTimer") - _lastCastTime >= _cooldown * 1000;
        }

        public virtual void CastSpell() {
            _lastCastTime = SplashKit.TimerTicks("gameTimer");
        }

        public void DrawSpell(double x, double y) {
            _icon.Draw(x, y);

            double percentageCooldown = Math.Clamp((SplashKit.TimerTicks("gameTimer") - _lastCastTime) / (_cooldown * 1000), 0, 1);
            double cooldownOverlayX = x + 3;
            double cooldownOverlayY = y + 3 + (percentageCooldown) * 42;
            double cooldownOverlayWidth = 42;
            double cooldownOverlayHeight = Math.Ceiling(42 * (1 - percentageCooldown));
            SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 0.5), cooldownOverlayX, cooldownOverlayY, cooldownOverlayWidth, cooldownOverlayHeight);
        }
    }
}