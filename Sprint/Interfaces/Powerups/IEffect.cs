using Sprint.Characters;

namespace Sprint.Interfaces.Powerups
{
    internl interface IEffect
    {
        // Executes powerup behavior on given player
        void Execute(Player player);

        // Undoes whatever change was done in execute
        void Reverse(Player player);

        // Create a copy of this effect
        IEffect Clone();
    }
}
