using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class MoveDown : ICommand
    {
        private MoveSystems moveSystems;

        public MoveDown(MoveSystems moveSystems)
        {
            this.moveSystems = moveSystems;
        }

        public void Execute()
        {
            this.moveSystems.MoveDown();
        }
    }
}