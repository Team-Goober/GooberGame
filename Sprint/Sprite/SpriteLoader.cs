
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using XMLData;

namespace Sprint.Sprite
{
    internal class SpriteLoader
    {

        private Dictionary<string, Texture2D> textures;
        private ContentManager content;

        public SpriteLoader(ContentManager content)
        {
            textures = new Dictionary<string, Texture2D>();
        }

        // Loads a new AnimatedSprite from the path
        public ISprite loadSprite(string path)
        {
            // Read XML file into a data object
            XMLData.SpriteData data = content.Load<XMLData.SpriteData>(path);

            // Already loaded textures are saved so they won't be reloaded for every sprite
            Texture2D texture;
            if (textures.ContainsKey(data.Texture))
            {
                texture = textures[data.Texture];
            }
            else
            {
                texture = content.Load<Texture2D>(data.Texture);
                textures.Add(data.Texture, texture);
            }

            ISprite sprite = new AnimatedSprite(texture);

            // Add each animation
            foreach (KeyValuePair<string, XMLData.AtlasData> entry in data.Animations)
            {
                XMLData.AtlasData atlasData = entry.Value;
                IAtlas atlas = null;
                
                // Load atlas based on its type
                if (atlasData is XMLData.AutoAtlasData)
                {
                    XMLData.AutoAtlasData aad = atlasData as XMLData.AutoAtlasData;
                    atlas = new AutoAtlas(
                        aad.SheetArea,
                        aad.Rows,
                        aad.Cols,
                        aad.Padding,
                        aad.CenterPoint,
                        aad.Loop,
                        aad.Framerate
                        );
                }
                else if (atlasData is XMLData.SingleAtlasData)
                {
                    XMLData.SingleAtlasData sad = atlasData as XMLData.SingleAtlasData;
                    atlas = new SingleAtlas(
                        sad.Rectangle,
                        sad.CenterPoint
                        );
                }
                else if (atlasData is XMLData.ManualAtlasData)
                {
                    XMLData.ManualAtlasData mad = atlasData as XMLData.ManualAtlasData;
                    atlas = new ManualAtlas(
                        mad.Rectangles.ToArray(),
                        mad.CenterPoints.ToArray(),
                        mad.Durations.ToArray(),
                        mad.Loop,
                        mad.Framerate
                        );
                }

                // Register newly loaded atlas
                sprite.RegisterAnimation(entry.Key, atlas);

            }

            // Optional initial values
            if (data.InitialAnimation != null)
            {
                sprite.SetAnimation(data.InitialAnimation);
            }
            if (data.Scale != 0.0)
            {
                sprite.SetScale(data.Scale);
            }

            return sprite;

        }


    }
}
