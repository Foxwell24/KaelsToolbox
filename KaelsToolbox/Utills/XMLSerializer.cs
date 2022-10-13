using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace KaelsToolbox.Utills
{
    /// <summary>
    /// Package to manage XML serialization
    /// </summary>
    /// <typeparam name="T">Type to save/load data as. must be [Serializable]</typeparam>
    public class XMLSerializer<T>
    {
        XmlSerializer serializer;
        string basePath;

        #region Constructors        
        /// <summary></summary>
        /// <param name="basePath">Location to place files</param>
        public XMLSerializer(string basePath)
        {
            this.basePath = basePath;
            serializer = new XmlSerializer(typeof(T));
        }

        #endregion

        public void SetPath(string basePath)
        {
            this.basePath = basePath;
        }
        /// <summary>
        /// Save an object into a file named <paramref name="name"/>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="allowOverwrite"></param>
        /// <returns>True if success, else false</returns>
        public bool Save(object item, string name, bool allowOverwrite)
        {
            if (item.GetType() != typeof(T)) throw new Exception($"{item} is not of Type {typeof(T)}");
            Directory.CreateDirectory(basePath);
            string path = $"{basePath}\\{name}.xml";

            if (!File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path);
                serializer.Serialize(tw, item);
                tw.Close();
                return true;
            }
            else if (allowOverwrite)
            {
                File.Delete(path);
                TextWriter tw = new StreamWriter(path);
                serializer.Serialize(tw, item);
                tw.Close();
                return true;
            }
            else if (!allowOverwrite && File.Exists(path))
            {
                return true;
            }
            return false;
        }
        public object Load(string name)
        {
            string path = $"{basePath}\\{name}.xml";

            if (!File.Exists(path))
                return null;

            using (var sr = new StreamReader(path))
            {
                return serializer.Deserialize(sr);
            }
        }
    }
}
