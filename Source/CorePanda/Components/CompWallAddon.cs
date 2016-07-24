using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Allows objects to die when a wall is destroyed
  /// </summary>
  public class CompWallAddon : ThingComp {
    /// <summary></summary>
    public override void CompTickRare() {

      Building wall = parent.Position.GetEdifice();

      if (wall == null || wall.def != ThingDefOf.Wall) {
        parent.Destroy(DestroyMode.Kill);
      }
    }
  }
}
