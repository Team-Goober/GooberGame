using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class MoveRight : ICommand
    {
        private MoveSystems moveSystems;

        public MoveRight(MoveSystems moveSystems)
        {
            this.moveSystems = moveSystems;
        }

        public void Execute()
        {
            this.moveSystems.MoveRight();
        }
    }
}