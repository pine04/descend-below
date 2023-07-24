using SplashKitSDK;

namespace DescendBelow {
    // Represents any game object that does not move.
    public abstract class StaticObject : GameObject {
        public StaticObject(Point2D position, double width, double height, Bitmap sprite, int zIndex = 1) : base(position, width, height, sprite, zIndex) { }

        public override void Update(uint fps) { }
    }
}