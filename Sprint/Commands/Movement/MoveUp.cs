using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class MoveUp : ICommand
    {
        private MoveSystems moveSystems;

        public MoveUp(MoveSystems moveSystems)
        {
            this.moveSystems = moveSystems;
        }

        public void Execute()
        {
            this.moveSystems.MoveUp();
        }
    }
}