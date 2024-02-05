using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;

namespace Sprint.Sprite
{
    internal class ManualAtlas : TimedAtlas, IAtlas
    {

        private Rectangle[] rects;
        private Vector2[] centers;
        private float[] durs;

        public ManualAtlas(Rectangle[] rectangles, Vector2[] centerPoints, float[] durations, bool loop, float framerate) : base(rectangles.Length, loop, framerate)
        {
            this.rects = rectangles;
            this.centers = centerPoints;
            this.durs = durations;
        }


        override public Vector2 CurrentCenterPoint()
        {
            return centers[frame];
        }

        override public float CurrentDuration()
        {
            return durs[frame];
        }

        override public Rectangle CurrentFrame()
        {
            return rects[frame];
        }

    }
}
