using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Interfaces
{
    public interface ICollidable
    {
        Rectangle GetBoundingBox();

        object GetNativeType();
    }
}
