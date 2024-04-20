using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.States
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
