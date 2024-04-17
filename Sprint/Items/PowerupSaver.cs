using System.Xml;
using Sprint.Items.Effects;
using System.Reflection;
using XMLData;

namespace Sprint.Items
{
    internal class PowerupSaver
    {


        public static void WriteFile()
        {

            PowerupData pup = new()
            {
                Label = "heart",
                Description = "HEART|heals one heart",
                Sprite = "heart",
                Effect = new HealPlayerEffect() { amount = 1 },
                Type = typeof(InstantPowerup).FullName
            };

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create("../../../Content/powerups.xml", settings))
            {
                Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.
                IntermediateSerializer.Serialize(writer, pup, null);
            }
        }
    }
}
