using RimWorld;
using Verse;

namespace CorePanda {
  
  internal class Building_Chandelier : Building {

    public override void TickRare() {
      base.TickRare();

      Building_ChandelierBowl bowl = Position.GetThingList().Find(b => b is Building_ChandelierBowl) as Building_ChandelierBowl;

      if (Spawned && bowl == null) {
        // Determine what glass bowl to spawn 
        // Allows the chandelier to be MadeFromStuff and have transparency
        if (def.defName == "CP_Chandelier_Glass") {
          bowl = ThingMaker.MakeThing(ThingDef.Named("CP_ChandelierPart_Bowl"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
        }
        if (def.defName == "CP_Chandelier_Quartz") {
          bowl = ThingMaker.MakeThing(ThingDef.Named("CP_ChandelierPart_Bowl"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
        }

        if (bowl != null) {
          GenSpawn.Spawn(bowl, Position, Rotation);
          bowl.SetFactionDirect(Faction.OfPlayer);
        }
      }
    }
  }
}
