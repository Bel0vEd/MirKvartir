using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirKvartir
{
    class ModelsAddres
    {
        public class LocationId
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class SuggestItem
        {
            public LocationId LocationId { get; set; }
            public int LocationType { get; set; }
            public string LocationFullName { get; set; }
            public string LocationFullNameReversed { get; set; }
            public bool IsLeaf { get; set; }
            public int Weight { get; set; }
        }

        public class Location
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class AllAddres
        {
            public SuggestItem SuggestItem { get; set; }
            public bool IsLeaf { get; set; }
            public List<Location> Locations { get; set; }
        }
    }
}
