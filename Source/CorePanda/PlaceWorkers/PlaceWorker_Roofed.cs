using Verse;

namespace CorePanda {
  /// <summary>
  /// Restrict buildings to being placed under a roof
  /// </summary>
  public class PlaceWorker_Roofed : PlaceWorker {
    /// <summary></summary>
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {

      foreach (IntVec3 current in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size)) {
        if (!Find.RoofGrid.Roofed(current)) {
          return new AcceptanceReport ("CP_NeedsRoof".Translate(new object[] { checkingDef.LabelCap }));
        }
      }

      return true;
    }
  }
}
