using Sprint.Interfaces;


namespace Sprint.Commands
{
    internal class Melee : ICommand
    {
        private Player player;

        public Melee(Player player)
        {
            this.player = player;
        }

        public void Execute()
        {
            this.player.Attack();
        }
    }
}