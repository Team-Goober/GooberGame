using Microsoft.Xna.Framework;

namespace XMLData
{
    public class SpriteData
    {
        public string Texture;
        public Dictionary<string, AtlasData> Animations;
        public float Scale;
        public string InitialAnimation;
    }

    public abstract class AtlasData
    {

    }

    public class ManualAtlasData : AtlasData
    {
        public List<Rectangle> Rectangles;
        public List<Vector2> CenterPoints;
        public List<float> Durations;
        public bool Loop;
        public float Framerate;
    }

    public class AutoAtlasData : AtlasData
    {
        public Rectangle SheetArea;
        public int Rows;
        public int Cols;
        public int Padding;
        public Vector2 CenterPoint;
        public bool Loop;
        public float Framerate;
    }

    public class SingleAtlasData : AtlasData
    {
        public Rectangle Rectangle;
        public Vector2 CenterPoint;
    }
}
