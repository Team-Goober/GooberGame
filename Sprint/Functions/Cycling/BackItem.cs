using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class BackItem : ICommand
    {
        private CycleItem cycleItem;

        public BackItem(CycleItem newCycleItem)
        {
            this.cycleItem = newCycleItem;
        }

        public void Execute()
        {
            this.cycleItem.Back();
        }
    }
}
