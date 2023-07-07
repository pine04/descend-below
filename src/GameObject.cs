using SplashKitSDK;

namespace DescendBelow {
    public abstract class GameObject {
        protected Point2D _position;
        protected double _width, _height;
        protected Bitmap _sprite;
        protected int _zIndex;

        public GameObject(Point2D position, double width, double height, Bitmap sprite, int zIndex = 1) {
            _position = position;
            _width = width;
            _height = height;
            _sprite = sprite;
            _zIndex = zIndex;
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

        public int ZIndex {
            get { return _zIndex; }
        }
    }
}