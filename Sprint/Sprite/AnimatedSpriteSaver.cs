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
            // currently set up to generate the enemy animation file

            string textureName = "zelda_enemies"; // Using the same texture as JellyfishEnemy
            int scale = 2;

            // Define directional atlases for animations
            IAtlasSaver upFacing = new SingleAtlasSaver(new Rectangle(180, 270, 16, 16), new Vector2(8, 8));
            IAtlasSaver leftFacing = new SingleAtlasSaver(new Rectangle(152, 270, 16, 16), new Vector2(8, 8));
            IAtlasSaver downFacing = new SingleAtlasSaver(new Rectangle(120, 270, 16, 16), new Vector2(8, 8));
            IAtlasSaver rightFacing = new SingleAtlasSaver(new Rectangle(210, 270, 16, 16), new Vector2(8, 8));

            AnimatedSpriteSaver bluebubbleEnemy = new AnimatedSpriteSaver(textureName);

            // Register directional animations
            bluebubbleEnemy.RegisterAnimation("upFacing", upFacing);
            bluebubbleEnemy.RegisterAnimation("leftFacing", leftFacing);
            bluebubbleEnemy.RegisterAnimation("downFacing", downFacing);
            bluebubbleEnemy.RegisterAnimation("rightFacing", rightFacing);

            // Set the default animation and scale
            bluebubbleEnemy.SetAnimation("upFacing");
            bluebubbleEnemy.SetScale(scale);



            Vector2 center = new Vector2(8, 8);

            // Define directional atlases for animations
            IAtlasSaver upFacing2 = new SingleAtlasSaver(new Rectangle(0, 0, 16, 16), new Vector2(8, 8));
            IAtlasSaver leftFacing2 = new SingleAtlasSaver(new Rectangle(88, 0, 16, 16), new Vector2(8, 8));
            IAtlasSaver downFacing2 = new SingleAtlasSaver(new Rectangle(60, 0, 16, 16), new Vector2(8, 8));
            IAtlasSaver rightFacing2 = new SingleAtlasSaver(new Rectangle(32, 0, 16, 16), new Vector2(8, 8));

            AnimatedSpriteSaver jellyfishEnemy = new AnimatedSpriteSaver(textureName);

            // Register directional animations
            jellyfishEnemy.RegisterAnimation("upFacing", upFacing2);
            jellyfishEnemy.RegisterAnimation("leftFacing", leftFacing2);
            jellyfishEnemy.RegisterAnimation("downFacing", downFacing2);
            jellyfishEnemy.RegisterAnimation("rightFacing", rightFacing2);

            // Set the default animation and scale
            jellyfishEnemy.SetAnimation("upFacing");
            jellyfishEnemy.SetScale(scale);




            // Define auto atlases for animations
            IAtlasSaver moveAnimation = new AutoAtlasSaver(new Rectangle(420, 120, 15, 46), 2, 1, 16, new Vector2(7.5f, 8), true, 10);

            AnimatedSpriteSaver skeletonEnemy = new AnimatedSpriteSaver(textureName);

            // Register directional animations
            skeletonEnemy.RegisterAnimation("moving", moveAnimation);



            // Set the default animation and scale
            skeletonEnemy.SetAnimation("moving");
            skeletonEnemy.SetScale(scale);


            SpriteGroupSaver group = new SpriteGroupSaver();
            group.AddSprite("bluebubble", bluebubbleEnemy.data);
            group.AddSprite("jellyfish", jellyfishEnemy.data);
            group.AddSprite("skeleton", skeletonEnemy.data);
            group.WriteXML("enemyAnims.xml");
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
