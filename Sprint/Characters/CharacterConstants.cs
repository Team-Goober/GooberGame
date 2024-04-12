using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Characters
{
    internal class CharacterConstants
    {
        public const int PLAYER_SPEED = 300;
        public const int SWORD_WIDTH = 40, SWORD_LENGTH = 90;
        public const int STARTING_HEALTH = 3;

        public const int INVENTORY_ROWS = 2, INVENTORY_COLUMNS = 4;

        public const int COLLIDER_SCALE = 3;
        public const int DEFAULT_SIDE_LENGTH = 16;

        public const int LOW_HP = 2, MID_HP = 4, HIGH_HP = 12, MAX_HP = 99999;
        public const int LOW_DMG = 1, MID_DMG = 2, HIGH_DMG = 4, NO_DMG = 0;
        public const int PROJECTILE_SIDE_LENGTH = 8;
    }
}
