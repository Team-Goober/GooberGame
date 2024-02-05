using Sprint.Interfaces;

namespace Sprint.Testing
{
    internal class ChangeLoopingCommand : ICommand
    {

        private IAtlas receiver;
        private bool changeLoopTo;

        public ChangeLoopingCommand(IAtlas receiver, bool changeLoopTo)
        {
            this.receiver = receiver;
            this.changeLoopTo = changeLoopTo;
        }

        public void Execute()
        {
            // Set looping to desired state
            receiver.SetLooping(changeLoopTo);
        }
    }
}
