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
            // Create new pause state and set as current game state
            receiver.GameState = new PauseState(receiver, returnState);
            receiver.GameState.MakeCommands();
        }
    }
}
