using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class ReleaseLeft : ICommand
    {
        private Player player;

        public ReleaseLeft(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.ReleaseLeft();
        }
    }

    internal class ReleaseRight : ICommand
    {
        private Player player;

        public ReleaseRight(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.ReleaseRight();
        }
    }

    internal class ReleaseUp : ICommand
    {
        private Player player;

        public ReleaseUp(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.ReleaseUp();
        }
    }

    internal class ReleaseDown : ICommand
    {
        private Player player;

        public ReleaseDown(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.ReleaseDown();
        }
    }
}
