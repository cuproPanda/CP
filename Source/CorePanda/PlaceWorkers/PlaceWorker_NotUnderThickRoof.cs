using Verse;

namespace CorePanda {
  /// <summary>
  /// Prevents an object from being placed under a thick roof
  /// <para>(Checks the cell behind this object)</para>
  /// </summary>
  public class PlaceWorker_NotUnderThickRoof : PlaceWorker {
    /// <summary>
    /// Whether this can be placed or not
    /// </summary>
    /// <param name="checkingDef">Thing to build</param>
    /// <param name="loc">Location to build</param>
    /// <param name="rot">Rotation of Thing</param>
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {
      RoofDef roofDef = Find.RoofGrid.RoofAt(loc);
      if (roofDef != null && roofDef.isThickRoof) {
        return new AcceptanceReport("CP_MustPlaceUnThickroofed".Translate());
      }
      return true;
    }
  }
}
