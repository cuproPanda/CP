using RimWorld;
using Verse;

namespace CorePanda {
  
  internal class Building_Chandelier : Building {

    public override void TickRare() {
      base.TickRare();

      Building_ChandelierBowl bowl = Position.GetThingList().Find(b => b is Building_ChandelierBowl) as Building_ChandelierBowl;

      if (Spawned && bowl == null) {
        // Determine what glass bowl to spawn 
        // Allows the chandelier to be MadeFromStuff and have transparent MadeFromStuff for the bowl
        int size = def.Size.x;
        switch (size) {
          case 1:
            if (def == ThingDef.Named("ChanPart_Support_1x1_Glass")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_1x1_Bowl"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_1x1_Quartz")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_1x1_Bowl"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
              break;
            }
            break;
          case 2:
            if (def == ThingDef.Named("ChanPart_Support_2x2_Glass")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_2x2_Bowl"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_2x2_Quartz")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_2x2_Bowl"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
              break;
            }
            break;
          case 3:
            if (def == ThingDef.Named("ChanPart_Support_3x3_Glass")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_3x3_Bowl"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_3x3_Quartz")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_3x3_Bowl"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
              break;
            }
            break;
          default:
            Log.Warning("CorePanda:: Could not determine what chandelier bowl to spawn.");
            break;
        }

        if (bowl != null) {
          bowl.SetFactionDirect(Faction.OfPlayer);
          GenSpawn.Spawn(bowl, Position, Rotation);
        }
      }
    }
  }
}
