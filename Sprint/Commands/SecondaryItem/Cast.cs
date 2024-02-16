using Sprint.Interfaces;


namespace Sprint.Commands
{
    internal class Cast : ICommand
    {
        private Player player;

        public Cast(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.Cast();
        }
    }
}