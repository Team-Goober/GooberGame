using Sprint.Characters;

namespace Sprint.Interfaces.Powerups
{
    internal interface IEffect
    {
        // Executes powerup behavior on given player
        void Execute(Player player);

        // Undoes whatever change was done in execute
        void Reverse(Player player);
    }
}
