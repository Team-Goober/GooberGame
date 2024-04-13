using Sprint.Interfaces;

namespace Sprint.Functions
{
    internal class Reset : ICommand
    {
        private DungeonState dungeon;

        public Reset(DungeonState dungeon)
        {
            this.dungeon = dungeon;
        }

        public void Execute()
        {
            this.dungeon.ResetReq();

        }
    }
}