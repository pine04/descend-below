using System;
using SplashKitSDK;

namespace DescendBelow {
    public class Player : DynamicObject, ICollidable {
        private Collider _collider;
        public Player(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity) : base(position, width, height, sprite, initialVelocity) {
            _collider = new Collider(this, 45);
        }

        public void Halt() {
            Velocity = new Vector2D() { X = 0, Y = 0 };
        }

        public void MoveLeft() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = -1, Y = 0 }, 100));
        }

        public void MoveRight() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 1, Y = 0 }, 100));
        }

        public void MoveUp() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 0, Y = -1 }, 100));
        }

        public void MoveDown() {
            _velocity = SplashKit.VectorAdd(_velocity, SplashKit.VectorMultiply(new Vector2D() { X = 0, Y = 1 }, 100));
        }

        public Collider Collider {
            get { return _collider; }
        }

        public void Collide(Collider c) {
            if (c.GameObject is Wall) {
                Console.WriteLine("Has collided");
                /*
                Vector2D vel = SplashKit.UnitVector(_velocity);

                Quad dynamicBox = _collider.GetColliderBox();
                Quad staticBox = c.GetColliderBox();

                double dynamicDot1 = SplashKit.DotProduct(vel, SplashKit.VectorTo(dynamicBox.Points[0]));
                double dynamicDot2 = SplashKit.DotProduct(vel, SplashKit.VectorTo(dynamicBox.Points[1]));
                double dynamicDot3 = SplashKit.DotProduct(vel, SplashKit.VectorTo(dynamicBox.Points[2]));
                double dynamicDot4 = SplashKit.DotProduct(vel, SplashKit.VectorTo(dynamicBox.Points[3]));

                double dynamicMin = Min(dynamicDot1, dynamicDot2, dynamicDot3, dynamicDot4);
                double dynamicMax = Max(dynamicDot1, dynamicDot2, dynamicDot3, dynamicDot4);

                double staticDot1 = SplashKit.DotProduct(vel, SplashKit.VectorTo(staticBox.Points[0]));
                double staticDot2 = SplashKit.DotProduct(vel, SplashKit.VectorTo(staticBox.Points[1]));
                double staticDot3 = SplashKit.DotProduct(vel, SplashKit.VectorTo(staticBox.Points[2]));
                double staticDot4 = SplashKit.DotProduct(vel, SplashKit.VectorTo(staticBox.Points[3]));

                double staticMin = Min(staticDot1, staticDot2, staticDot3, staticDot4);
                double staticMax = Max(staticDot1, staticDot2, staticDot3, staticDot4);


                double overlap = Math.Abs(GetOverlap(dynamicMin, dynamicMax, staticMin, staticMax));

                Console.WriteLine("Range: {0} {1} {2} {3}", dynamicMin, dynamicMax, staticMin, staticMax);
                Console.WriteLine("Overlap: {0}", overlap);

                Vector2D moveVector = SplashKit.VectorMultiply(vel, -(overlap));
                Console.WriteLine("{0} {1}", moveVector.X, moveVector.Y);
                MoveBy(moveVector); */
            }
        }

        private void MoveBy(Vector2D displacement) {
            _position = SplashKit.PointAt(
                _position.X + displacement.X,
                _position.Y + displacement.Y
            );
        }

        private double Min(params double[] nums) {
            double min = Double.MaxValue;
            foreach (double n in nums) {
                if (n < min) {
                    min = n;
                }
            }
            return min;
        }

        private double Max(params double[] nums) {
            double max = Double.MinValue;
            foreach (double n in nums) {
                if (n > max) {
                    max = n;
                }
            }
            return max;
        }

        private double GetOverlap(double min1, double max1, double min2, double max2) {
            if (min2 >= min1 && min2 <= max1) {
                return Min(max1, max2) - min2;
            }
            if (min1 >= min2 && min1 <= max2) {
                return Min(max1, max2) - min1;
            }

            return 0;
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(options);
            SplashKit.DrawQuad(Color.Black, _collider.GetColliderBox());
        }
    }
}