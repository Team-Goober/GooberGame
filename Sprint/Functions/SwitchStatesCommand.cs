
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
            receiver.PassToState(state);
        }
    }
}
