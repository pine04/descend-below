using System;
using SplashKitSDK;

namespace DescendBelow
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Descend Below", 1200, 900);
            /////// SCALE ORIGIN IS NOT THE CENTER OR THE TOP LEFT OF THE BITMAP!!!!
            DrawingOptions options = SplashKit.OptionScaleBmp(4, 4);

            Bitmap sprite = new Bitmap("player", "arrow.png");
            Bitmap wallSprite = new Bitmap("wall", "wall.png");

            Player player = new Player(new Point2D() { X = 64, Y = 64 }, 20, 52, sprite, new Vector2D() { X = 0, Y = 0 });
            Wall wall = new Wall(SplashKit.PointAt(128, 128), 64, 64, wallSprite);

            PrintQuad(player.Collider.GetColliderBox());
            PrintQuad(wall.Collider.GetColliderBox());

            do {
                if (wall.Collider.IsCollidingWith(player.Collider)) {
                    wall.Collide(player.Collider);
                    player.Collide(wall.Collider);
                }

                SplashKit.ProcessEvents();

                player.Halt();
                if (SplashKit.KeyDown(KeyCode.WKey)) {
                    player.MoveUp();
                }
                if (SplashKit.KeyDown(KeyCode.AKey)) {
                    player.MoveLeft();
                }
                if (SplashKit.KeyDown(KeyCode.SKey)) {
                    player.MoveDown();
                }
                if (SplashKit.KeyDown(KeyCode.DKey)) {
                    player.MoveRight();
                }

                player.Update(60);

                SplashKit.ClearScreen();

                player.Draw(options);
                wall.Draw(options);
                SplashKit.DrawText(player.Velocity.X.ToString(), Color.Black, 0, 0);
                SplashKit.DrawText(player.Velocity.Y.ToString(), Color.Black, 0, 20);

                wallSprite.Draw(300, 300, SplashKit.OptionScaleBmp(5, 5));
                SplashKit.DrawPixel(Color.White, 300, 300);

                gameWindow.Refresh(60);
            } while (!gameWindow.CloseRequested);
        }

        public static void PrintQuad(Quad q) {
            Console.WriteLine("Top left: {0} {1}", q.Points[0].X, q.Points[0].Y);
            Console.WriteLine("Top right: {0} {1}", q.Points[1].X, q.Points[1].Y);
            Console.WriteLine("Bottom left: {0} {1}", q.Points[2].X, q.Points[2].Y);
            Console.WriteLine("Bottom right: {0} {1}", q.Points[3].X, q.Points[3].Y);
        }
    }
}
