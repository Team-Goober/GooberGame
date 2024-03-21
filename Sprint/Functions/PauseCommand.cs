
using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class PauseCommand : ICommand
    {

        Goober receiver;
        IGameState returnState;

        public PauseCommand(Goober receiver, IGameState returnState)
        {
            this.receiver = receiver;
            this.returnState = returnState;
        }

        public void Execute()
        {
            receiver.GameState = new PauseState(receiver, returnState);
        }
    }
}
