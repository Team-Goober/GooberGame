using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.DeathState
{
    internal class MoveHeartUp : ICommand
    {
        private GameOverState receiver;

        public MoveHeartUp(GameOverState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.MoveHeartUp();
        }
    }
}
