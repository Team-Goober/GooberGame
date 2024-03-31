using Sprint.Levels;
using System;
using System.Runtime.CompilerServices;

namespace Sprint.HUD
{
    public delegate void HUDHandler(int number);

    public class HUDUpdate
    {

        static int keyAmount = 0;

        public static void UpdateKey(int keys)
        {
            keyAmount = keys;
        }

        static int gemAmount = 0;

        public static void UpdateGem(int gem)
        {
            gemAmount = gem;
        }
    }
}

