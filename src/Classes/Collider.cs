using SplashKitSDK;

namespace DescendBelow {
    // Defines the logic of a collider, which is a special object associated with every game object that can collide with other objects.
    public class Collider {
        private GameObject _gameObject;
        // The collider's shape is a Quad.
        private Quad _baseColliderBox;

        public Collider(GameObject gameObject, double rotation) {
            _gameObject = gameObject;

            _baseColliderBox = SplashKit.QuadFrom(
                SplashKit.PointAt(-gameObject.Width / 2, -gameObject.Height / 2),
                SplashKit.PointAt(gameObject.Width / 2, -gameObject.Height / 2),
                SplashKit.PointAt(-gameObject.Width / 2, gameObject.Height / 2),
                SplashKit.PointAt(gameObject.Width / 2, gameObject.Height / 2)
            );
            Matrix2D rotationMatrix = SplashKit.RotationMatrix(rotation);
            SplashKit.ApplyMatrix(rotationMatrix, ref _baseColliderBox);
        }

        public bool IsCollidingWith(Collider c) {
            return SplashKit.QuadsIntersect(GetColliderBox(), c.GetColliderBox());
        }

        // This method calculates the position of the collider. Since the game object can move, the collider's position should reflect the game
        // object's position. This method takes the base collider shape calculated on object creation, then translate it by the position vector
        // of the game object.
        private Quad GetColliderBox() {
            Quad colliderBox = _baseColliderBox;
            Matrix2D translationMatrix = SplashKit.TranslationMatrix(_gameObject.Position.X, _gameObject.Position.Y);
            SplashKit.ApplyMatrix(translationMatrix, ref colliderBox);

            return colliderBox;
        }

        public GameObject GameObject {
            get { return _gameObject; }
        }

        // A static method used to resolve a collision between a dynamic object and a static object.
        // The dynamic object is pushed in the opposite direction of its velocity until it no longer collides with the static object.
        // If the dynamic object's velocity is 0 at the time of the collision, it is pushed along the (-1, 0) vector.
        public static void ResolveDynamicStatic(Collider dynamicCollider, Collider staticCollider) {
            DynamicObject? dynamicObject = dynamicCollider.GameObject as DynamicObject;

            if (dynamicObject == null) {
                return;
            }

            Vector2D vel = SplashKit.UnitVector(dynamicObject.Velocity);
            if (SplashKit.VectorMagnitude(vel) == 0) {
                vel = SplashKit.VectorTo(1, 0);
            }
            Vector2D reverse = SplashKit.VectorMultiply(vel, -1);

            while (dynamicCollider.IsCollidingWith(staticCollider)) {
                dynamicObject.MoveBy(reverse);
            }  
        }
    }
}