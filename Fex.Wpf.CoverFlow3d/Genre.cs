using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iTunaFish.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class Genre
    {
        [XmlAttribute]
        public string Name { get; set; }

        public Genre(string Name)
        {
            this.Name = Name;
        }

        public Genre()
        {

        }
    }
}
