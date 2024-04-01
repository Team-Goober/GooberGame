using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Functions.Collision;

public class NullCommand : ICommand

{
    public void Execute()
    {
        // Do nothing
    }
}
