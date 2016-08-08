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
            if (def == ThingDef.Named("ChanPart_Support_Glass_1x1")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_1x1"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_Quartz_1x1")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_1x1"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
              break;
            }
            break;
          case 2:
            if (def == ThingDef.Named("ChanPart_Support_Glass_2x2")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_2x2"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_Quartz_2x2")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_2x2"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
              break;
            }
            break;
          case 3:
            if (def == ThingDef.Named("ChanPart_Support_Glass_3x3")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_3x3"), ThingDef.Named("CP_Glass")) as Building_ChandelierBowl;
              break;
            }
            if (def == ThingDef.Named("ChanPart_Support_Quartz_3x3")) {
              bowl = ThingMaker.MakeThing(ThingDef.Named("ChanPart_Bowl_3x3"), ThingDef.Named("CP_FusedQuartz")) as Building_ChandelierBowl;
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
