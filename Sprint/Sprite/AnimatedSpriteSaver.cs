using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Projectile;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
            // currently set up to generate the projectile animation file

            string itemSheet = "zelda_items";
            string bomb = "Items/Bomb";
            string fireBall = "Items/FireBall";
            string smokeT = "Items/EndArrow";
            string boomerang = "Items/boomerangs";


            // Smoke
            AnimatedSpriteSaver smoke = new AnimatedSpriteSaver(smokeT);
            IAtlasSaver smokeAtlas = new SingleAtlasSaver(new Rectangle(0, 0, 7, 8), new Vector2(3.5f, 4));
            smoke.SetAnimation("smoke");
            smoke.RegisterAnimation("smoke", smokeAtlas);
            smoke.SetScale(4);

            // Arrow
            AnimatedSpriteSaver arrowsprite = new AnimatedSpriteSaver(itemSheet);
            IAtlasSaver arrowright = new SingleAtlasSaver(new Rectangle(0, 45, 16, 5), new Vector2(6, 2.5f));
            arrowsprite.RegisterAnimation("right", arrowright);
            arrowsprite.SetAnimation("right");
            arrowsprite.SetScale(4);

            // Blue Arrow
            AnimatedSpriteSaver bluearrowsprite = new AnimatedSpriteSaver(itemSheet);
            IAtlasSaver bluearrowright = new SingleAtlasSaver(new Rectangle(0, 125, 16, 5), new Vector2(6, 2.5f));
            bluearrowsprite.RegisterAnimation("right", bluearrowright);
            bluearrowsprite.SetAnimation("right");
            bluearrowsprite.SetScale(4);

            // Blue Boomerang
            AnimatedSpriteSaver bboomerangsprite = new AnimatedSpriteSaver(boomerang);
            IAtlasSaver bbatlas = new AutoAtlasSaver(new Rectangle(3, 18, 54, 8), 1, 4, 10, new Vector2(3, 4), true, 50);
            bboomerangsprite.RegisterAnimation("default", bbatlas);
            bboomerangsprite.SetAnimation("default");
            bboomerangsprite.SetScale(4);

            // Bomb
            AnimatedSpriteSaver bombsprite = new AnimatedSpriteSaver(bomb);
            IAtlasSaver bombatlas = new AutoAtlasSaver(new Rectangle(0, 0, 85, 16), 1, 5, 1, new Vector2(8, 8), false, 3);
            bombsprite.RegisterAnimation("default", bombatlas);
            bombsprite.SetAnimation("default");
            bombsprite.SetScale(4);

            // Boomerang
            AnimatedSpriteSaver boomerangsprite = new AnimatedSpriteSaver(boomerang);
            IAtlasSaver atlas = new AutoAtlasSaver(new Rectangle(1, 2, 56, 9), 1, 4, 10, new Vector2(3, 4), true, 18);
            boomerangsprite.RegisterAnimation("boomarang", atlas);
            boomerangsprite.SetAnimation("boomarang");
            boomerangsprite.SetScale(4);

            // Fireball
            AnimatedSpriteSaver firesprite = new AnimatedSpriteSaver(fireBall);
            IAtlasSaver fireatlas = new AutoAtlasSaver(new Rectangle(0, 0, 33, 15), 1, 2, 1, new Vector2(8, 8), true, 5);
            firesprite.RegisterAnimation("fireBall", fireatlas);
            firesprite.SetAnimation("fireBall");
            firesprite.SetScale(3);

            SpriteGroupSaver group = new SpriteGroupSaver();
            group.AddSprite("smoke", smoke.data);
            group.AddSprite("arrow", arrowsprite.data);
            group.AddSprite("bluearrow", bluearrowsprite.data);
            group.AddSprite("blueboomerang", bboomerangsprite.data);
            group.AddSprite("bomb", bombsprite.data);
            group.AddSprite("boomerang", boomerangsprite.data);
            group.AddSprite("fireball", boomerangsprite.data);
            group.WriteXML("projectileAnims.xml");
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
