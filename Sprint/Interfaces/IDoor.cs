using Microsoft.Xna.Framework;
using Sprint.Characters;

namespace Sprint.Interfaces
{
    internal interface IDoor : IGameObject, ICollidable
    {

        public void SwitchRoom();

        public Vector2 PlayerSpawnPosition();

        public bool IsOpen();

        public void SetOpen(bool open);

        public IDoor GetOtherFace();

        public void SetOtherFace(IDoor other);

        public Vector2 SideOfRoom();

        public Point GetRoomIndices();

    }
}
