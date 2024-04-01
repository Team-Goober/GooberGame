using Sprint.Levels;
using System;
using System.Runtime.CompilerServices;

namespace Sprint.Events
{
    public delegate void HUDHandler(int number);

    public class HUDUpdate
    {

        static int keyAmount = 0;
        static int gemAmount = 0;
        static int bombAmount = 0;

        public static void UpdateKey(int keys)
        {
            keyAmount = keys;
        }

        public static void UpdateGem(int gem)
        {
            gemAmount = gem;
        }

        public static void UpdateBomb(int bomb)
        {
            bombAmount = bomb;
        }
    }
}

