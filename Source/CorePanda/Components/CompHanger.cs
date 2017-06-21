using RimWorld;
using Verse;
using Verse.Sound;

namespace CorePanda {

  public class CompHanger : ThingComp {

    public CompProperties_Hanger Props {
      get { return (CompProperties_Hanger)props; }
    }


    public override void CompTickRare() {
      base.CompTickRare();

      if (Props.hangingType == HangingType.None) {
        Log.Warning("CorePanda:: " + parent.def.defName + " doesn't have a HangingType defined.");
        parent.AllComps.Remove(this);
      }

      if (Props.hangingType == HangingType.Wall) {
        // Get the tile behind this object
        IntVec3 c = parent.Position - parent.Rotation.FacingCell;
        // Minify this if the wall is missing
        Building edifice = c.GetEdifice(parent.Map);
        if (edifice == null || edifice.def == null || (edifice.def != ThingDefOf.Wall &&
          ((edifice.Faction == null || edifice.Faction != Faction.OfPlayer) ||
          edifice.def.graphicData == null || edifice.def.graphicData.linkFlags == 0 || (LinkFlags.Wall & edifice.def.graphicData.linkFlags) == LinkFlags.None))) {
          Minify();
        }
      }

      if (Props.hangingType == HangingType.Ceiling) {
        // Minify this if the ceiling is missing
        int occCells = 0;
        int unroofedCells = 0;
        foreach (IntVec3 current in parent.OccupiedRect()) {
          occCells++;
          if (!parent.Map.roofGrid.Roofed(current)) {
            unroofedCells++;
          }
          if (current.GetEdifice(parent.Map) == null || current.GetEdifice(parent.Map).def == null) {
            continue;
          }
          if (current.GetEdifice(parent.Map).def.blockWind == true || current.GetEdifice(parent.Map).def.holdsRoof == true) {
            Minify();
          }
        }
        if (((float)(occCells - unroofedCells) / occCells) < 0.5f) {
          Minify();
        }
      }
    }


    public virtual void Minify() {
      MinifiedThing package = parent.MakeMinified();
      GenPlace.TryPlaceThing(package, parent.Position, parent.Map, ThingPlaceMode.Near);
      SoundDef.Named("ThingUninstalled").PlayOneShot(new TargetInfo(parent.Position, parent.Map));
    }
  }
}
