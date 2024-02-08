using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces
{
    internal interface IProjectileFactory
    {

        // Sets the position at which projectiles are created
        void SetStartPosition(Vector2 pos);

        // Create whatever shots must be created
        void Create(Vector2 direction);

    }
}
