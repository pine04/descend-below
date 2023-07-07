using SplashKitSDK;

namespace DescendBelow {
    public abstract class Interactible : StaticObject {
        protected double _range;

        public Interactible(Point2D position, double width, double height, Bitmap sprite, double range) : base(position, width, height, sprite) {
            _range = range;
        }

        public bool IsNearPlayer(Player p) {
            return SplashKit.PointPointDistance(p.Position, Position) <= _range;
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(options);
            if (IsNearPlayer(Game.CurrentGame?.CurrentPlayer)) {
                SplashKit.DrawBitmap("interactionArrow", Position.X - 14, Position.Y - Height / 2 - 45);
            }
        }

        public bool IsHoveredOn(Point2D mousePosition) {
            return mousePosition.X >= Position.X - Width / 2 && mousePosition.X <= Position.X + Width / 2 &&
                    mousePosition.Y >= Position.Y - Height / 2 && mousePosition.Y <= Position.Y + Height / 2;
        }

        public abstract void HandleInteraction();
    }
}