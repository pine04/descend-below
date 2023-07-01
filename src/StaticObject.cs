using SplashKitSDK;

namespace DescendBelow {
    public class StaticObject : GameObject {
        public StaticObject(Point2D position, double width, double height, Bitmap sprite) : base(position, width, height, sprite) { }

        public override void Update(uint fps) { }
    }
}