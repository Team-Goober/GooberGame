using Sprint.Interfaces;

namespace Sprint.Testing
{
    internal class ChangeAnimCommand : ICommand
    {

        private ISprite receiver;
        private string anim;

        public ChangeAnimCommand(ISprite receiver, string anim)
        {
            this.receiver = receiver;
            this.anim = anim;
        }

        public void Execute()
        {
            receiver.SetAnimation(anim);
        }
    }
}
