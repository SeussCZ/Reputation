using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using System.Xml.Serialization;


namespace LuxarPL.Reputation
{
    public sealed class Group
    {
        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("NeededRep")]
        public int NeededRep;

        public Group(string name, int neededRep)
        {
            Name = name;
            NeededRep = neededRep;
        }
        public Group()
        {
            Name = "";
            NeededRep = 0;
        }
    }

    public class ReputationConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public int DatabasePort;

        public int DefaultReputation;
        public int ReputationForKill;

        [XmlArrayItem("Group")]
        [XmlArray(ElementName = "Groups")]
        public Group[] Groups;

        public void LoadDefaults()
        {
            DatabaseAddress = "localhost";
            DatabaseUsername = "root";
            DatabasePassword = "unturned123";
            DatabaseName = "unturned";
            DatabaseTableName = "Reputation";
            DatabasePort = 3306;
            DefaultReputation = 5;
            ReputationForKill = 1;

            Groups = new Group[]{
                new Group("Bandit lvl2", -20),
                new Group("Bandit", 0),
                new Group("Civilian", 1),
                new Group("Civilian lvl2", 20)
            };


        }
    }
}
