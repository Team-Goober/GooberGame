using Sprint.Levels;

namespace Sprint.Interfaces.Powerups
{
    internal interface IAbility : IPowerup
    {

        public bool ReadyUp();

        public void Activate();

    }
}
