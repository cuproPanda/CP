using System.Collections.Generic;

using RimWorld;
using Verse;

namespace CorePanda {

  internal class PlaceWorker_Chandelier : PlaceWorker {

    List<IntVec3> occupiedCellsTemp = new List<IntVec3>();


    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {

      occupiedCellsTemp.Clear();

      foreach (IntVec3 current in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size)) {
        occupiedCellsTemp.Add(current);
      }

      // Don't allow placing on big things
      for (int i = 0; i < occupiedCellsTemp.Count; i++) {
        IntVec3 c = occupiedCellsTemp[i];
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
