using Sprint.GameStates;
using Sprint.Interfaces;

namespace Sprint.Functions.States
{
    internal class MoveHeartDown : ICommand
    {
        private GameOverState receiver;

        public MoveHeartDown(GameOverState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.MoveHeartDown();
        }
    }
}
