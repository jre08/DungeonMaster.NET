using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class ViAltairAlcove : Alcove
    {
        public ViAltairAlcove(IEnumerable<IGrabableItem> items) : base(items) {}
    }
}