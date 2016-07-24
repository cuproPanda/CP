using Verse;

namespace CorePanda {
  /// <summary>
  /// Restricts the window to only being placed on a wall tile
  /// </summary>
  public class PlaceWorker_WindowInWall : PlaceWorker {

    /// <summary></summary>
    public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot) {
      IntVec3 loc = center + IntVec3.South.RotatedBy(rot);
      IntVec3 loc2 = center + IntVec3.North.RotatedBy(rot);
      Room room = loc.GetRoom();
      Room room2 = loc2.GetRoom();

      if (loc.Impassable() || loc2.Impassable()) {
        return "CP_WindowImpassable".Translate();
      }
      // Using outdoor temperature is the easiest way I
      // found to check whether a room is considered outside
      if (room.UsesOutdoorTemperature && room2.UsesOutdoorTemperature) {
        return "CP_WindowDoubleOutside".Translate();
      }
      if (!room.UsesOutdoorTemperature && !room2.UsesOutdoorTemperature) {
        return "CP_WindowDoubleInside".Translate();
      }
      return true;
    }
  }
}
