using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class SwitchStatesCommand : ICommand
    {

        IGameState receiver;
        IGameState state;

        public SwitchStatesCommand(IGameState receiver, IGameState state)
        {
            this.receiver = receiver;
            this.state = state;
        }

        public void Execute()
        {
            // Tell state to close out and switch game to next state
            receiver.PassToState(state);
        }
    }
}
