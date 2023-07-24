using SplashKitSDK;

namespace DescendBelow {
    // Represents a static game object that can be interacted with by right clicking.
    public abstract class Interactable : StaticObject {
        protected double _range;
        private Animation _rightClickBlinkAnimation;

        public Interactable(Point2D position, double width, double height, Bitmap sprite, double range) : base(position, width, height, sprite) {
            _range = range;
            _rightClickBlinkAnimation = SplashKit.AnimationScriptNamed("rightclick").CreateAnimation("blink");
        }

        public bool IsNearPlayer(Player p) {
            return SplashKit.PointPointDistance(p.Position, Position) <= _range;
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(options);
            if (Game.CurrentGame != null && IsNearPlayer(Game.CurrentGame.CurrentPlayer)) {
                SplashKit.DrawBitmap("rightclick", Position.X - 11, Position.Y - Height / 2 - 42, SplashKit.OptionWithAnimation(_rightClickBlinkAnimation));
            }
        }

        public override void Update(uint fps)
        {
            base.Update(fps);
            _rightClickBlinkAnimation.Update();
        }

        public bool IsHoveredOn(Point2D mousePosition) {
            return mousePosition.X >= Position.X - Width / 2 && mousePosition.X <= Position.X + Width / 2 &&
                    mousePosition.Y >= Position.Y - Height / 2 && mousePosition.Y <= Position.Y + Height / 2;
        }

        // This abstract method will be overriden by child classes to define their behavior when interacted with.
        public abstract void HandleInteraction();
    }
}