using SplashKitSDK;

namespace DescendBelow
{
    public abstract class Enemy : Character, IDestroyable
    {
        private uint _timeSinceLastAtk;
        public Enemy(Point2D position, double width, double height, Bitmap sprite, Vector2D initialVelocity, int maxHealth) : base(position, width, height, sprite, initialVelocity, maxHealth, 10)
        {
            _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
        }

        public override void Draw(DrawingOptions options)
        {
            base.Draw(options);

            SplashKit.FillRectangle(Color.RGBColor(217, 87, 99), _position.X - 24, _position.Y + _height / 2 + 8, 48, 10);
            SplashKit.FillRectangle(Color.RGBColor(55, 148, 110), _position.X - 24, _position.Y + _height / 2 + 8, 48 * _health / _maxHealth, 10);
        }

        public bool CanDestroy
        {
            get { return IsDead(); }
        }

        public abstract void Attack(Player player);

        public virtual void Move(Player player) {
            if (SplashKit.PointPointDistance(Position, player.Position) >= 144) {
                Velocity = SplashKit.VectorMultiply(SplashKit.UnitVector(SplashKit.VectorPointToPoint(Position, player.Position)), 50);
            } else {
                Velocity = SplashKit.VectorTo(0, 0);
            }
        }

        public override void Update(uint fps)
        {
            Player? targetPlayer = Game.CurrentGame?.CurrentPlayer;
            if (SplashKit.TimerTicks("gameTimer") - _timeSinceLastAtk >= 1000 && targetPlayer != null) {
                Attack(targetPlayer);
                _timeSinceLastAtk = SplashKit.TimerTicks("gameTimer");
            }

            if (targetPlayer != null) {
                Move(targetPlayer);
            }

            
            base.Update(fps);
        }
    }
}