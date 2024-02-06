using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class NextItem : ICommand
    {
        private CycleItem cycleItem;

        public NextItem(CycleItem newCycleItem)
        {
            this.cycleItem = newCycleItem;
        }

        public void Execute() 
        {
            this.cycleItem.Next();
        }
    }
}
