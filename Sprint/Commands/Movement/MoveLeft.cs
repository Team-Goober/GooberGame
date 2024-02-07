using Sprint.Interfaces;


namespace Sprint.Commands
{
    internal class MoveLeft : ICommand
    {
        private MoveSystems moveSystems;

        public MoveLeft(MoveSystems moveSystems)
        {
            this.moveSystems = moveSystems;
        }

        public void Execute()
        {
            this.moveSystems.MoveLeft();
        }
    }
}