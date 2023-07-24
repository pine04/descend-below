using SplashKitSDK;

namespace DescendBelow {
    // Defines the walls that make up the border of the game map.
    public class Wall : StaticObject, ICollidable {
        private Collider _collider;

        public Wall(Point2D position, double width, double height, Bitmap sprite) : base(position, width, height, sprite) {
            _collider = new Collider(this, 0);
        }

        public Collider Collider {
            get { return _collider; }
        }

        public void Collide(Collider c) { }
    }
}