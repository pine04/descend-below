using System;
using System.Collections.Generic;
using SplashKitSDK;
using System.IO;

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
