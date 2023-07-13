using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace DescendBelow
{
    public class Program
    {
        public static void Main()
        {
            Game game = Game.CreateGame();
            game.Run();
            game.CleanUp();
        }
    }
}
