using SplashKitSDK;
using System.Collections.Generic;

namespace DescendBelow {
    // Defines an interactible object that stores items which can be taken by the player.
    public class Chest : Interactable, ICollidable {
        private Collider _collider;
        public List<Item> _items;

        public Chest(Point2D position, int floorLevel) : base(position, 48, 48, SplashKit.BitmapNamed("chest"), 96) { 
            _collider = new Collider(this, 0);
            _items = GenerateChestContent(floorLevel);
        }

        // This static method generates the chest content. The items' stats scale with floor level.
        private static List<Item> GenerateChestContent(int floorLevel) {
            List<Item> items = new List<Item>();

            items.Add(new HealthPotion(RandGen.RandomIntBetween(1, 3)));

            if (RandGen.RandomDoubleBetween(0, 1) <= 0.8) {
                if (RandGen.RandomDoubleBetween(0, 1) >= 0.5) {
                    items.Add(new Bow(12 + floorLevel * 15, 0.75 + RandGen.RandomDoubleBetween(-0.25, 0.25)));
                } else {
                    items.Add(new Spelltome(10 + floorLevel * 12, 0.4 + RandGen.RandomDoubleBetween(-0.1, 0.1)));
                }
            }

            if (RandGen.RandomDoubleBetween(0, 1) <= 0.4) {
                if (RandGen.RandomDoubleBetween(0, 1) >= 0.5) {
                    items.Add(new LightningSpell(90 + 10 * floorLevel, 15 + RandGen.RandomDoubleBetween(-2, 2)));
                } else {
                    items.Add(new HealSpell(0.7 + RandGen.RandomDoubleBetween(-0.2, 0.2), 60 + RandGen.RandomDoubleBetween(-5, 5)));
                }
            }

            return items;
        }

        public override void HandleInteraction()
        {
            Game.CurrentGame?.OpenChest(this);
        }

        public Collider Collider {
            get { return _collider; }
        }

        public void Collide(Collider c) { }

        public void DrawChestContent(double x, double y) {
            for (int i = 0; i < _items.Count; i++) {
                _items[i].DrawItem(x, y + 72 * i);
                SplashKit.DrawBitmap("takebutton", x + 450, y + 8 + 72 * i);
            }
        }

        public Item? GetItem(int index) {
            if (index < 0 || index >= _items.Count) {
                return null;
            }

            Item item = _items[index];
            _items.Remove(item);
            return item;
        }

        public void AddItem(Item item) {
            _items.Add(item);
        }
    }
}