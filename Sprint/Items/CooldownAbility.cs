using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
using System.Reflection.Metadata;

namespace Sprint.Items
{
    internal class CooldownAbility : IAbility, ICooldownPowerup
    {

        /*
         *  Represents an ability which must be kept in a slot and can be selected and used.
         */

        private ISprite sprite;
        private IEffect onActivate;
        private string label;
        private Player player;
        private string description;
        private Timer cooldownTimer;

        private TimeSpan lastUpdate;

        public CooldownAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
            this.description = description;
            cooldownTimer = new Timer(1.0f);
        }

        public bool ReadyUp()
        {
            if (cooldownTimer.Ended || cooldownTimer.TimeLeft == cooldownTimer.Duration)
            {
                cooldownTimer.Start();
                return true;
            }
            return false;
        }

        public void Activate()
        {
            onActivate?.Execute(player);
        }

        public void Apply(Player player)
        {
            this.player = player;
            player.GetInventory().AddToSlots(this);
            player.GetInventory().AddPowerup(this);
        }

        public bool CanPickup(Inventory inventory)
        {
            return !inventory.HasPowerup(label) && inventory.SlotsAvailable();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);

            if (!cooldownTimer.Ended)
            {
                Texture2D overlayColor;
                overlayColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                overlayColor.SetData(new Color[] { new Color(Color.Black, CharacterConstants.DISABLED_OPACITY) } );
                int side = CharacterConstants.POWERUP_SIDE_LENGTH;
                float height = side * (float)(cooldownTimer.TimeLeft / cooldownTimer.Duration);
                float ypos = position.Y + side / 2.0f - height;
                spriteBatch.Draw(overlayColor, new Rectangle((int)(position.X - side / 2.0f), (int)ypos, side, (int)height), Color.White);
            }
        }
        public string GetLabel()
        {
            return label;
        }


        public string GetDescription()
        {
            return description;
        }

        public void Update(GameTime gameTime)
        {
            if(gameTime.TotalGameTime != lastUpdate)
            {
                cooldownTimer.Update(gameTime);
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }

        public void SetDuration(float duration)
        {
            cooldownTimer.SetDuration(duration);
        }

        public void SetTimeLeft(float duration)
        {
            cooldownTimer.SubtractTime(duration);
        }

        public float GetTimeLeft()
        {
            return (float)cooldownTimer.TimeLeft.TotalSeconds;
        }
    }
}
