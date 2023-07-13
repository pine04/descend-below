using System;
using SplashKitSDK;

namespace DescendBelow {
    public class Staircase : Interactable {
        public Staircase(Point2D position) : base(position, 45, 36, SplashKit.BitmapNamed("staircase"), 48) { }

        public override void HandleInteraction()
        {
            Game.CurrentGame?.EnterNewFloor();
        }
    }
}