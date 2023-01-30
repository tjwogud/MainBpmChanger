using System.IO;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace MainBpmChanger
{
    public class Settings : UnityModManager.ModSettings
    {
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            string filepath = Path.Combine(modEntry.Path, "Settings.xml");
            using (StreamWriter writer = new StreamWriter(filepath))
                new XmlSerializer(GetType()).Serialize(writer, this);
        }

        public float pitch = 200;
        public bool changeMusic = true;
        public bool multiplyMusic = false;
    }
}
