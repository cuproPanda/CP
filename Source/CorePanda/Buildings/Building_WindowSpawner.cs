using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Destroys itself, then builds a window and a glower
  /// </summary>
  public class Building_WindowSpawner : Building {

    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      // Destroy the spawner first, freeing up room to place the windows
      Destroy();

      // Create the window and the window glower
      // The window spawner is made from stuff, but the spawned window isn't due to transparency
      Building window = ThingMaker.MakeThing(ThingDef.Named("CP_Window")) as Building;
      Building_WindowGlower windowGlower = ThingMaker.MakeThing(ThingDef.Named("CP_WindowGlower")) as Building_WindowGlower;

      // Get transform info
      IntVec3 intVecSouth = Position + IntVec3.South.RotatedBy(Rotation);
      IntVec3 intVecNorth = Position + IntVec3.North.RotatedBy(Rotation);
      Room room = intVecSouth.GetRoom();
      IntVec3 inside = room.UsesOutdoorTemperature ? intVecNorth : intVecSouth;
      IntVec3 outside = room.UsesOutdoorTemperature ? intVecSouth : intVecNorth;

      // Set the ownership of the window and glower (defaults to unowned)
      window.SetFactionDirect(Faction.OfPlayer);
      windowGlower.SetFactionDirect(Faction.OfPlayer);
      // Spawn the window and glower
      GenSpawn.Spawn(window, Position, Rotation);
      GenSpawn.Spawn(windowGlower, inside);
    }
  }
}
