using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace DescendBelow {
    public class Chest : Interactable, ICollidable {
        private Collider _collider;
        public List<Item> _items;

        public Chest(Point2D position) : base(position, 48, 48, SplashKit.BitmapNamed("chest"), 96) { 
            _collider = new Collider(this, 0);

            _items = new List<Item>() {
                new Spelltome(15, 0.4),
                new LightningSpell(100, 10),
                new HealSpell(0.25, 15)
            };
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
                SplashKit.DrawBitmap("takebutton", x + 240, y + 8 + 72 * i);
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