using Sprint.Interfaces;
namespace Sprint.Testing
{
    internal class ResetAnimCommand : ICommand
    {

        private IAtlas receiver;

        public ResetAnimCommand(IAtlas receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            // Reset animation
            receiver.Reset();
        }
    }
}
