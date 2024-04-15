using Sprint.Levels;

namespace Sprint.Interfaces.Powerups
{
    internal interface IAbility : IPowerup
    {

        // Prepares changes that determine when powerup can activate
        // Returns true if ability is able to activate
        public bool ReadyUp();

        // Runs ability behavior
        public void Activate();

        // Ends activation period of behavior
        public void Complete();

        // Return if item is currently in an active period
        public bool IsActive();

    }
}
