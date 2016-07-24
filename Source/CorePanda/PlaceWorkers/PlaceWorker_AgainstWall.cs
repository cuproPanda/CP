using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Only allows an object to be placed against a wall
  /// <para>(Checks the cell behind this object)</para>
  /// </summary>
  public class PlaceWorker_AgainstWall : PlaceWorker {
    /// <summary>
    /// Whether this can be placed or not
    /// </summary>
    /// <param name="checkingDef">Thing to build</param>
    /// <param name="loc">Location to build</param>
    /// <param name="rot">Rotation of Thing</param>
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {

      // Get the tile behind this object
      IntVec3 c = loc - rot.FacingCell;
      // Determine if the tile is an edifice
      Building edifice = c.GetEdifice();
      // Cast checkingDef as ThingDef, allowing CompDefFor to be used
      ThingDef hanger = checkingDef as ThingDef;
      // Reference to CompProperties_Hanger
      CompProperties_Hanger hangerProps = (CompProperties_Hanger)hanger.CompDefFor<CompHanger>();

      // Don't place outside of the map
      if (!c.InBounds()) {
        return false;
      }

      // Only allow placing on a natural or constructed wall
      if (edifice == null || (edifice.def != ThingDefOf.Wall && !edifice.def.building.isNaturalRock)) {
        return new AcceptanceReport("CP_MustBePlacedOnWall".Translate(new object[] { checkingDef.LabelCap }));
      }

      // If the current object can't hang where there is a window
      if (hangerProps != null && hangerProps.WallHeight == WallHeight.High) {
        if (c.GetThingList().Find(window => window.def == ThingDef.Named("CP_Window")) != null) {
          return new AcceptanceReport("CP_WindowBlocksPlacement".Translate(new object[] { checkingDef.LabelCap }));
        }
      }

      // Otherwise, accept placing
      return true;
    }
  }
}

