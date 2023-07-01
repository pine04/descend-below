using SplashKitSDK;

namespace DescendBelow {
    public class Enemy : DynamicObject {
        public Enemy(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity) : base(position, width, height, sprite, initialVelocity) { }
    }
}