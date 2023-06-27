using SplashKitSDK;

namespace DescendBelow {
    public class Collider {
        private GameObject _gameObject;
        private Quad _baseColliderBox;

        public Collider(GameObject gameObject, double rotation) {
            _gameObject = gameObject;

            Console.WriteLine("Position: {0} {1}", gameObject.Position.X, gameObject.Position.Y);
            Console.WriteLine("Width: {0}", gameObject.Width);
            Console.WriteLine("Height: {0}", gameObject.Height);

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

        public Quad GetColliderBox() {
            Quad colliderBox = _baseColliderBox;
            Matrix2D translationMatrix = SplashKit.TranslationMatrix(_gameObject.Position.X, _gameObject.Position.Y);
            SplashKit.ApplyMatrix(translationMatrix, ref colliderBox);

            return colliderBox;
        }

        public GameObject GameObject {
            get { return _gameObject; }
        }
    }
}