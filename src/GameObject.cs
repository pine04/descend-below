using SplashKitSDK;

namespace DescendBelow {
    public abstract class GameObject {
        protected Point2D _position;
        protected double _width, _height;
        protected Bitmap _sprite;

        public GameObject(Point2D position, double width, double height, Bitmap sprite) {
            _position = position;
            _width = width;
            _height = height;
            _sprite = sprite;
        }

        public virtual void Draw(DrawingOptions options) {
            _sprite.Draw(_position.X - _width / 2, _position.Y - _height / 2, options);
        }

        public abstract void Update(uint fps);

        public Point2D Position {
            get { return _position; }
        }

        public double Width {
            get { return _width; }
        }

        public double Height {
            get { return _height; }
        }
    }
}