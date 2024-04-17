
using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System.Windows.Markup;

namespace Sprint.Items.Effects
{
    internal class FairyCompanionEffect : IEffect
    {

        public string spriteName;
        public string spriteFile;

        private Companion companion;
        private Player player;

        public void Execute(Player player)
        {
            // Create new companion on first execution
            if(companion == null)
            {
                this.player = player;
                ISprite sprite = player.GetSpriteLoader().BuildSprite(spriteFile, spriteName);
                companion = new Companion(sprite, player);
                // Connect to player health updates to save player if health is zero
                player.OnPlayerHealthChange += onPlayerHealthChanged;
            }
            // Add to room
            companion.SetDisable(false);
        }

        private void onPlayerHealthChanged(double prevHp, double nextHp)
        {
            // Check if player about to die
            if(nextHp <= 0)
            {
                // Give player health back
                player.Heal(0.5f);
                // Remove fairy
                player.OnPlayerHealthChange -= onPlayerHealthChanged;
                // Roundabout way of removing the item from inventory and disabling companion
                player.GetInventory().GetPowerup("fairy").Undo(player);
                player = null;
                companion = null;
            }
        }


        public void Reverse(Player player)
        {
            // Disable it
            companion.SetDisable(true);
        }

        public IEffect Clone()
        {
            return new FairyCompanionEffect() { spriteName = spriteName, spriteFile = spriteFile };
        }

    }
}
