using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml;
using XMLData;

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
            // currently set up to generate the player animation file

            AnimatedSpriteSaver sprite = new AnimatedSpriteSaver("zelda_links");

            Vector2 centerOffset = new Vector2(8, 8);

            AutoAtlasSaver downAtlas = new AutoAtlasSaver(new Rectangle(0, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("down", downAtlas);

            IAtlasSaver leftAtlas = new AutoAtlasSaver(new Rectangle(30, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("left", leftAtlas);

            IAtlasSaver rightAtlas = new AutoAtlasSaver(new Rectangle(90, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("right", rightAtlas);

            IAtlasSaver upAtlas = new AutoAtlasSaver(new Rectangle(60, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("up", upAtlas);

            IAtlasSaver stillAtlas = new SingleAtlasSaver(new Rectangle(0, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("still", stillAtlas);

            IAtlasSaver downStill = new SingleAtlasSaver(new Rectangle(0, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("downStill", downStill);

            IAtlasSaver leftStill = new SingleAtlasSaver(new Rectangle(30, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("leftStill", leftStill);

            IAtlasSaver upStill = new SingleAtlasSaver(new Rectangle(60, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("upStill", upStill);

            IAtlasSaver rightStill = new SingleAtlasSaver(new Rectangle(90, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("rightStill", rightStill);

            sprite.SetAnimation("still");
            sprite.SetScale(3);

            //Set up damage atlas
            IAtlasSaver damage = new SingleAtlasSaver(new Rectangle(0, 150, 16, 16), centerOffset);
            sprite.RegisterAnimation("damage", damage);

            // sword animations RIGHT 
            IAtlasSaver swordRightAtlas = new SingleAtlasSaver(new Rectangle(84, 90, 27, 15), new Vector2(9, 7));
            sprite.RegisterAnimation("swordRight", swordRightAtlas);

            // sword animations LEFT 
            IAtlasSaver swordLeftAtlas = new SingleAtlasSaver(new Rectangle(24, 90, 27, 15), new Vector2(18, 7));
            sprite.RegisterAnimation("swordLeft", swordLeftAtlas);

            // sword animations UP 
            IAtlasSaver swordUpAtlas = new SingleAtlasSaver(new Rectangle(60, 84, 16, 28), new Vector2(8, 21));
            sprite.RegisterAnimation("swordUp", swordUpAtlas);

            // sword animations DOWN 
            IAtlasSaver swordDownAtlas = new SingleAtlasSaver(new Rectangle(0, 84, 16, 27), new Vector2(8, 7));
            sprite.RegisterAnimation("swordDown", swordDownAtlas);

            // casting animations
            IAtlasSaver castRightAtlas = new SingleAtlasSaver(new Rectangle(90, 60, 16, 16), centerOffset);
            sprite.RegisterAnimation("castRight", castRightAtlas);

            IAtlasSaver castLeftAtlas = new SingleAtlasSaver(new Rectangle(30, 60, 16, 16), centerOffset);
            sprite.RegisterAnimation("castLeft", castLeftAtlas);

            IAtlasSaver castUpAtlas = new SingleAtlasSaver(new Rectangle(60, 60, 16, 16), centerOffset);
            sprite.RegisterAnimation("castUp", castUpAtlas);

            IAtlasSaver castDownAtlas = new SingleAtlasSaver(new Rectangle(0, 60, 16, 16), centerOffset);
            sprite.RegisterAnimation("castDown", castDownAtlas);

            SpriteGroupSaver group = new SpriteGroupSaver();
            group.AddSprite("player", sprite.data);
            group.WriteXML("playerAnims.xml");
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
