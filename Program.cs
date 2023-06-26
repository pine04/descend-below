using System;
using SplashKitSDK;

namespace DescendBelow
{
    public class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Descend Below", 1200, 900);
            DrawingOptions options = SplashKit.OptionScaleBmp(4, 4);

            Bitmap sprite = new Bitmap("player", "arrow.png");
            Player player = new Player(new Point2D() { X = 64, Y = 64 }, 20, 52, sprite, new Vector2D() { X = 0, Y = 0 });

            do {
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
                SplashKit.DrawText(player.Velocity.X.ToString(), Color.Black, 0, 0);
                SplashKit.DrawText(player.Velocity.Y.ToString(), Color.Black, 0, 20);

                gameWindow.Refresh(60);
            } while (!gameWindow.CloseRequested);
        }
    }
}
