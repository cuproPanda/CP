using Verse;

namespace CorePanda {

  internal class PlaceWorker_Chandelier : PlaceWorker {

    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {
      // Don't allow placing on big things
      foreach (IntVec3 c in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size)) {
        if (c.GetEdifice() != null) {
          if (c.GetEdifice().def.blockWind == true || c.GetEdifice().def.holdsRoof == true) {
            return new AcceptanceReport("CP_ObjectTooTall".Translate(new object[] { c.GetEdifice().LabelCap, checkingDef.LabelCap }));
          }
        }
        if (c.GetThingList().Find(ch => ch is Building_Chandelier) != null) {
          return new AcceptanceReport("IdenticalThingExists".Translate());
        }
      }

      // Otherwise, accept placing
      return true;
    }
  }
}
