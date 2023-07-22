using SplashKitSDK;
using System;

namespace DescendBelow {
    public class Player : Character {
        private Weapon _weapon;
        private Spell? _spell;
        private uint _timeSinceLastHit, _timeSinceLastRegen;
        private int _experience, _level;
        private Animation _playerIdleAnimation;
        private Animation _playerWalkAnimation;

        public Player(Point2D position, Vector2D initialVelocity, int maxHealth) : base(position, 42, 42, SplashKit.BitmapNamed("player"), initialVelocity, maxHealth, 20) {
            _weapon = new Bow(25, .5);
            _timeSinceLastHit = 0;
            _timeSinceLastRegen = 0;
            _experience = 0;
            _level = 1;

            _playerIdleAnimation = SplashKit.AnimationScriptNamed("player").CreateAnimation("idle");
            _playerWalkAnimation = SplashKit.AnimationScriptNamed("player").CreateAnimation("walk");
        }

        public void Halt() {
            Velocity = SplashKit.VectorTo(0, 0);
        }

        public void MoveAlong(Vector2D direction) {
            Velocity = SplashKit.VectorAdd(Velocity, SplashKit.VectorMultiply(direction, 150));
        }

        public void Attack(Point2D target) {
            if (_weapon.ReadyForAttack()) {
                _weapon.Attack(_position, target);
            }
        }

        public void UseSpell() {
            if (_spell != null && _spell.ReadyToCast()) {
                _spell.CastSpell();
            }
        }

        public override void Damage(int amount)
        {
            base.Damage(amount);
            _timeSinceLastHit = SplashKit.TimerTicks("gameTimer");
            SplashKit.PlaySoundEffect("hurt");
        }

        private void Regenerate() {
            Heal(1);
            _timeSinceLastRegen = SplashKit.TimerTicks("gameTimer");
        }

        public override void Update(uint fps)
        {
            base.Update(fps);
            if (SplashKit.TimerTicks("gameTimer") - _timeSinceLastHit >= 3000 && SplashKit.TimerTicks("gameTimer") - _timeSinceLastRegen >= 500) {
                Regenerate();
            }

            _playerIdleAnimation.Update();
            _playerWalkAnimation.Update();
        }

        public override void Draw(DrawingOptions options) {
            if (SplashKit.IsZeroVector(Velocity)) {
                base.Draw(SplashKit.OptionWithAnimation(_playerIdleAnimation, options));
            } else {
                base.Draw(SplashKit.OptionWithAnimation(_playerWalkAnimation, options));
            }
        }

        public void DrawPlayerStats(double x, double y) {
            DrawHealthbar(x, y);
            DrawExperienceBar(x, y + 48);
            _weapon.DrawWeaponStat(x + 256, y + 4);
            _spell?.DrawSpell(x + 290, y + 4);
        }

        private void DrawHealthbar(double x, double y) {
            SplashKit.DrawBitmap("heart", x, y);
            SplashKit.DrawBitmap("healthbar", x + 48, y);
            double percentageHealth = (double)_health / _maxHealth;
            SplashKit.FillRectangle(Color.RGBColor(196, 36, 48), x + 48 + 240 * percentageHealth, y, Math.Ceiling(240 - percentageHealth * 240), 24);
        }

        private void DrawExperienceBar(double x, double y) {
            SplashKit.DrawBitmap("xp", x, y);
            SplashKit.DrawBitmap("xpbar", x + 48, y);
            double percentageXp = (double)_experience / GetExperienceToLevelUp(_level);
            SplashKit.FillRectangle(Color.RGBColor(12, 46, 68), x + 48 + 240 * percentageXp, y, Math.Ceiling(240 - percentageXp * 240), 24);
        }

        public void AddExperience(int amount) {
            _experience += amount;

            int experienceNeeded = GetExperienceToLevelUp(_level);

            if (_experience >= experienceNeeded) {
                LevelUp();
                _experience -= experienceNeeded;
            }
        }

        private int GetExperienceToLevelUp(int currentLevel) {
            return 50 + (currentLevel - 1) * 60;
        }

        private void LevelUp() {
            _level += 1;
            _maxHealth += 10;
            _health = _maxHealth;
            _weapon.Upgrade();
            SplashKit.PlaySoundEffect("levelUp");
        }

        public Item? TakeNewItem(Item newItem) {
            if (newItem is Weapon) {
                Weapon? newWeapon = newItem as Weapon;
                Weapon oldWeapon = _weapon;
                if (newWeapon != null) {
                    _weapon = newWeapon;
                }
                return oldWeapon;
            }

            if (newItem is Spell) {
                Spell? newSpell = newItem as Spell;
                Spell? oldSpell = _spell;

                if (newSpell != null) {
                    _spell = newSpell;
                }
                return oldSpell;
            }

            return null;
        }

        public int Health {
            get { return _health; }
        }

        public int MaxHealth {
            get { return _maxHealth; }
        }
    }
}