using RimWorld;
using Verse;
using Verse.Sound;

namespace CorePanda {
  /// <summary>
  /// Minifies a minifiable object if the thing it's hanging on is missing
  /// </summary>
  public class CompHanger : ThingComp {

    /// <summary></summary>
    public CompProperties_Hanger Props {
      get { return (CompProperties_Hanger)props; }
    }


    /// <summary> Checks for a disconnection, then minifies an object if needed</summary>
    public override void CompTickRare() {
      base.CompTickRare();

      if (Props.HangingType == HangingType.None) {
        Log.Warning(parent.def.defName + " doesn't have a HangingType defined, please notify the mod author.");
      }

      if (Props.HangingType == HangingType.Wall) {
        // Get the tile behind this object
        IntVec3 c = parent.Position - parent.Rotation.FacingCell;
        // Minify this if the wall is missing
        if (c.GetEdifice() == null || (c.GetEdifice().def != ThingDefOf.Wall && !c.GetEdifice().def.building.isNaturalRock)) {
          Minify();
        }
        // Minify this if a window is behind this cell
        if (Props.WallHeight == WallHeight.High) {
          if (c.GetThingList().Find(window => window.def.defName == "CP_Window") != null) {
            Minify();
          }
        }
      }

      if (Props.HangingType == HangingType.Ceiling) {
        // Minify this if the ceiling is missing
        int occCells = 0;
        int roofCells = 0;
        foreach (IntVec3 current in parent.OccupiedRect()) {
          occCells++;
          if (!Find.RoofGrid.Roofed(current)) {
            roofCells++;
          }
        }
        if (((float)(occCells - roofCells) / occCells) < 0.5f) {
          Minify();
        }
      }
    }


    /// <summary> Minifies the package</summary>
    public virtual void Minify() {
      MinifiedThing package = parent.MakeMinified();
      GenPlace.TryPlaceThing(package, parent.Position, ThingPlaceMode.Near);
      SoundDef.Named("ThingUninstalled").PlayOneShot(parent.Position);
    }
  }
}
