using SplashKitSDK;

namespace DescendBelow {
    public class Collider {
        private GameObject _gameObject;
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

        private Quad GetColliderBox() {
            Quad colliderBox = _baseColliderBox;
            Matrix2D translationMatrix = SplashKit.TranslationMatrix(_gameObject.Position.X, _gameObject.Position.Y);
            SplashKit.ApplyMatrix(translationMatrix, ref colliderBox);

            return colliderBox;
        }

        public GameObject GameObject {
            get { return _gameObject; }
        }

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