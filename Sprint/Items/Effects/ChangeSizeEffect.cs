
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items.Effects
{
    internal class ChangeSizeEffect : IEffect
    {
        private float scale;

        public ChangeSizeEffect(float scale)
        {
            this.scale = scale;
        }

        public void Execute(Player player)
        {
            // Change player size
            player.SetScale(scale);
        }

        public void Reverse(Player player)
        {
            // Reset to default size
            player.SetScale(1);
        }
    }
}
