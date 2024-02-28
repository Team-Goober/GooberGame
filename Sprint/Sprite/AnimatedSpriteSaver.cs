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
            // currently set up to generate the item animation file

            string rupyT = "Items/ZeldaSprite5Rupies";
            AnimatedSpriteSaver rupyS = new AnimatedSpriteSaver(rupyT);
            IAtlasSaver rupyA = new SingleAtlasSaver(new Rectangle(0, 0, 8, 16), new Vector2(4, 8));
            rupyS.RegisterAnimation("rupy", rupyA);
            rupyS.SetAnimation("rupy");
            rupyS.SetScale(4);

            string arrowT = "Items/ZeldaSpriteArrow";
            AnimatedSpriteSaver arrowS = new AnimatedSpriteSaver(arrowT);
            IAtlasSaver arrowA = new SingleAtlasSaver(new Rectangle(0, 0, 5, 16), new Vector2(2, 8));
            arrowS.RegisterAnimation("arrow", arrowA);
            arrowS.SetAnimation("arrow");
            arrowS.SetScale(4);

            string heartConT = "Items/ZeldaSpriteHeartContainer";
            AnimatedSpriteSaver heartConS = new AnimatedSpriteSaver(heartConT);
            IAtlasSaver heartConA = new SingleAtlasSaver(new Rectangle(0, 0, 13, 13), new Vector2(5, 5));
            heartConS.RegisterAnimation("heartCon", heartConA);
            heartConS.SetAnimation("heartCon");
            heartConS.SetScale(4);

            string magicalShieldT = "Items/ZeldaSpriteMagicalShield";
            AnimatedSpriteSaver magicalShieldS = new AnimatedSpriteSaver(magicalShieldT);
            IAtlasSaver magicalShieldA = new SingleAtlasSaver(new Rectangle(0, 0, 8, 12), new Vector2(4, 6));
            magicalShieldS.RegisterAnimation("magicalShield", magicalShieldA);
            magicalShieldS.SetAnimation("magicalShield");
            magicalShieldS.SetScale(4);

            string triforceT = "Items/Triforce";
            AnimatedSpriteSaver triforceS = new AnimatedSpriteSaver(triforceT);
            IAtlasSaver triforceA = new AutoAtlasSaver(new Rectangle(0, 0, 24, 10), 1, 2, 4, new Vector2(5, 5), true, 10);
            triforceS.RegisterAnimation("triforce", triforceA);
            triforceS.SetAnimation("triforce");
            triforceS.SetScale(4);

            SpriteGroupSaver group = new SpriteGroupSaver();
            group.AddSprite("rupy", rupyS.data);
            group.AddSprite("arrow", arrowS.data);
            group.AddSprite("heart", heartConS.data);
            group.AddSprite("shield", magicalShieldS.data);
            group.AddSprite("triforce", triforceS.data);
            group.WriteXML("itemAnims.xml");
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
