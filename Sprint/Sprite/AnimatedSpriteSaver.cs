using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Projectile;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Xml;
using XMLData;
using static System.Formats.Asn1.AsnWriter;

namespace Sprint.Sprite
{

    /*
     * These classes exist solely to convert hard-coded sprite initializations
     * into XML files. They are written to easily replace those constructors.
     * This file should not live to the final game.
     */

    public class SpriteGroupSaver
    {

        Dictionary<string, SpriteData> dict;
        public SpriteGroupSaver()
        {
            dict = new Dictionary<string, SpriteData>();
        }

        public void AddSprite(string label, SpriteData data)
        {
            dict.Add(label, data);
        }

        public void WriteXML(string path)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.
                IntermediateSerializer.Serialize(writer, dict, null);
            }
        }

        public static void WriteFile()
        {
            // currently set up to generate the tile animation file

            SpriteGroupSaver group = new SpriteGroupSaver();

            CreateTile(group, "flat", "tiles", 0, 0, 16, 16, new Vector2(8, 8), 2);   // Tile 1
            CreateTile(group, "bevel", "tiles", 17, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 2
            CreateTile(group, "fish", "tiles", 34, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 3
            CreateTile(group, "dragon", "tiles", 52, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 4

            CreateTile(group, "dark", "tiles", 0, 17, 16, 16, new Vector2(8, 8), 2);   // Tile 5
            CreateTile(group, "sand", "tiles", 17, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 6
            CreateTile(group, "light", "tiles", 34, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 7
            CreateTile(group, "stairs", "tiles", 52, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 8

            CreateTile(group, "bricks", "tiles", 0, 34, 16, 16, new Vector2(8, 8), 2);   // Tile 9
            CreateTile(group, "slats", "tiles", 17, 34, 16, 16, new Vector2(8, 8), 2);  // Tile 10


            group.WriteXML("tileAnims.xml");
        }

        private static void CreateTile(SpriteGroupSaver group, string label, string textureName, int x, int y, int width, int height, Vector2 center, int scale)
        {
            AnimatedSpriteSaver tileSprite = new AnimatedSpriteSaver(textureName);
            IAtlasSaver tileAtlas = new SingleAtlasSaver(new Rectangle(x, y, width, height), center);
            tileSprite.RegisterAnimation("default", tileAtlas);
            tileSprite.SetAnimation("default");
            tileSprite.SetScale(scale);

            group.AddSprite(label, tileSprite.data);
        }

    }

    public class AnimatedSpriteSaver
    {

        public SpriteData data;

        public AnimatedSpriteSaver(String texture)
        {
            data = new SpriteData();
            data.Texture = texture;
            
            data.Animations = new Dictionary<string, AtlasData>();
        }

        public void RegisterAnimation(string label, IAtlasSaver atlas)
        {
            data.Animations.Add(label, atlas.GetData());
        }

        public void SetAnimation(string label)
        {
            data.InitialAnimation = label;
        }

        public void SetScale(float scale)
        {
            data.Scale = scale;
        }

    }

    public interface IAtlasSaver
    {
        public AtlasData GetData();
    }

    public class AutoAtlasSaver : IAtlasSaver
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

        public AtlasData GetData()
        {
            return data;
        }

    }

    public class SingleAtlasSaver : IAtlasSaver
    {
        public SingleAtlasData data;

        public SingleAtlasSaver(Rectangle rectangle, Vector2 centerPoint)
        {
            data = new SingleAtlasData();
            data.Rectangle = rectangle;
            data.CenterPoint = centerPoint;
        }
        public AtlasData GetData()
        {
            return data;
        }

    }

    public class ManualAtlasSaver : IAtlasSaver
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
        public AtlasData GetData()
        {
            return data;
        }
    }


}
