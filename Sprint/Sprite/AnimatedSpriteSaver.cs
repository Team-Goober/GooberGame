using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml;
using XMLData;

namespace Sprint.Sprite
{

    /*
     * This class exists solely to convert hard-coded sprite initializations
     * into XML files. It is written to easily replace those constructors.
     * This class should not live to the final game.
     */

    public class AnimatedSpriteSaver
    {

        private float scale;
        private string initAnim;
        private SpriteData data;

        public AnimatedSpriteSaver(String texture)
        {
            data = new SpriteData();
            data.Texture = texture;
            
            data.Animations = new Dictionary<string, AtlasData>();
        }

        public void WriteXML(string path)
        {
            data.Scale = scale;
            data.InitialAnimation = initAnim;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.
                IntermediateSerializer.Serialize(writer, data, null);
            }
        }

        public void RegisterAnimation(string label, AtlasData atlas)
        {
            data.Animations.Add(label, atlas);
        }

        public void SetAnimation(string label)
        {
            initAnim = label;
        }

        public void SetScale(float scale)
        {
            this.scale = scale;
        }

    }

    public class AutoAtlasSaver
    {

        public AutoAtlasData data;

        public AutoAtlasSaver(Rectangle sheetArea, int rows, int cols, int padding, Vector2 centerPoint, bool loop, float framerate)
        {
            data = new AutoAtlasData();
            data.SheetArea = sheetArea;
            data.Rows = rows;
            data.Cols = cols;
            data.Padding = padding;
            data.CenterPoint = centerPoint;
            data.Loop = loop;
            data.Framerate = framerate;
        }

    }

    public class SingleAtlasSaver
    {
        public SingleAtlasData data;

        public SingleAtlasSaver(Rectangle rectangle, Vector2 centerPoint)
        {
            data = new SingleAtlasData();
            data.Rectangle = rectangle;
            data.CenterPoint = centerPoint;
        }
    }

    public class ManualAtlasSaver
    {
        public ManualAtlasData data;

        public ManualAtlasSaver(Rectangle[] rectangles, Vector2[] centerPoints, float[] durations, bool loop, float framerate)
        {
            data = new ManualAtlasData();
            data.Rectangles = new List<Rectangle>(rectangles);
            data.CenterPoints = new List<Vector2>(centerPoints);
            data.Durations = new List<float>(durations);
            data.Loop = loop;
            data.Framerate = framerate;
        }
    }


}
