
using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System.Windows.Markup;

namespace Sprint.Items.Effects
{
    internal class CompanionEffect : IEffect
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
                ISprite sprite = player.GetSpriteLoader().BuildSprite(spriteFile, spriteName);
                companion = new Companion(sprite, player);
            }
            // Add to room
            companion.SetDisable(false);
        }

        public void Reverse(Player player)
        {
            // Disable it
            companion.SetDisable(true);
        }

        public IEffect Clone()
        {
            return new CompanionEffect() { spriteName = spriteName, spriteFile = spriteFile };
        }
    }
}
