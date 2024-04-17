using Sprint.Characters;

namespace Sprint.Interfaces.Powerups
{
    public interface IEffect
    {
        // Executes powerup behavior on given player
        internal void Execute(Player player);

        // Undoes whatever change was done in execute
        internal void Reverse(Player player);

        // Create a copy of this effect
        internal IEffect Clone();
    }
}
