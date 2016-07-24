using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {
  // Restricts the vent to only being placed on a wall tile, while
  // still checking the normal vent checks
  internal class PlaceWorker_VentInWall : PlaceWorker {

    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot) {
      IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
      IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
      GenDraw.DrawFieldEdges(new List<IntVec3>
      {
        intVec
      }, Color.white);
      GenDraw.DrawFieldEdges(new List<IntVec3>
      {
        intVec2
      }, Color.white);
      Room room = intVec2.GetRoom();
      Room room2 = intVec.GetRoom();
      if (room != null && room2 != null) {
        if (room == room2 && !room.UsesOutdoorTemperature) {
          GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white);
        }
        else {
          if (!room.UsesOutdoorTemperature) {
            GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), Color.white);
          }
          if (!room2.UsesOutdoorTemperature) {
            GenDraw.DrawFieldEdges(room2.Cells.ToList<IntVec3>(), Color.white);
          }
        }
      }
    }


    public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot) {

      Building edifice = center.GetEdifice();
      IntVec3 c = center + IntVec3.South.RotatedBy(rot);
      IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);

      // Don't place outside of the map
      if (!center.InBounds()) {
        return false;
      }
      // Only allow placing on a constructed wall
      if (edifice == null || edifice.def != ThingDefOf.Wall) {
        return false;
      }
      // Make sure the vent has free areas
      if (c.Impassable() || c2.Impassable()) {
        return "MustPlaceVentWithFreeSpaces".Translate();
      }

      return true;
    }
  }
}
