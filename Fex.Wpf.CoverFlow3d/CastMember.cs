using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iTunaFish.Library
{
    public class CastMember
    {
        [XmlAttribute]
        public int TmdbId { get; set; }
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Character { get; set; }

        public CastMember()
        {

        }

        public CastMember(int TmdbId, string Type, string Name, string Character)
        {
            this.TmdbId = TmdbId;
            this.Type = Type;
            this.Name = Name;
            this.Character = Character;
        }
    }
}
