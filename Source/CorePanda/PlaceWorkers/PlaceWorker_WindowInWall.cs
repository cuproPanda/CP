using Verse;

namespace CorePanda {
  /// <summary>
  /// Restricts the window to only being placed on a wall tile
  /// </summary>
  public class PlaceWorker_WindowInWall : PlaceWorker {

    /// <summary></summary>
    public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot) {
      IntVec3 adjRelativeNorth = center + IntVec3.North.RotatedBy(rot);
      IntVec3 adjRelativeSouth = center + IntVec3.South.RotatedBy(rot);
      Room room = adjRelativeNorth.GetRoom();
      Room room2 = adjRelativeSouth.GetRoom();

      if (adjRelativeNorth.Impassable() || adjRelativeSouth.Impassable()) {
        return "CP_WindowImpassable".Translate();
      }

      if (room.OpenRoofCount > 1 && room2.OpenRoofCount > 1) {
        return "CP_WindowDoubleOutside".Translate();
      }

      if (room.OpenRoofCount == 0 && room2.OpenRoofCount == 0) {
        return "CP_WindowDoubleInside".Translate();
      }
      return true;
    }
  }
}
