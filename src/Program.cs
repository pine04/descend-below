using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace DescendBelow
{
    public class Program
    {
        public static void Main()
        {
            Game game = new Game();
            game.Run();
            game.CleanUp();
        }
    }
}
