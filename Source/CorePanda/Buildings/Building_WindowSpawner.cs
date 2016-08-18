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
      Rot4 glowerRot;
      // Rotations are reversed so the glower can scan outside correctly
      if (inside == (Position + IntVec3.North)) {
        glowerRot = Rot4.South;
      }
      else if (inside == (Position + IntVec3.East)) {
        glowerRot = Rot4.West;
      }
      else if (inside == (Position + IntVec3.South)) {
        glowerRot = Rot4.North;
      }
      else {
        glowerRot = Rot4.East;
      }

      // Set the ownership of the window and glower (defaults to unowned)
      window.SetFactionDirect(Faction.OfPlayer);
      windowGlower.SetFactionDirect(Faction.OfPlayer);

      // Spawn the window and glower
      GenSpawn.Spawn(window, Position, Rotation);
      SpawnGlower(windowGlower, inside, glowerRot, Position, outside);
    }


    private Building_WindowGlower SpawnGlower(Building_WindowGlower glower, IntVec3 loc, Rot4 rot, IntVec3 windowPos, IntVec3 outsidePos) {
      if (!loc.InBounds()) {
        Log.Error(string.Concat(new object[]
        {
          "Tried to spawn ",
          glower,
          " out of bounds at ",
          loc,
          "."
        }));
        return null;
      }
      GenSpawn.WipeExistingThings(loc, rot, glower.def, false);

      glower.Rotation = rot;

      glower.SetPositionDirect(IntVec3.Invalid);
      glower.Position = loc;
      glower.windowPos = windowPos;
      glower.outside = outsidePos;
      glower.SpawnSetup();

      return glower;
    }
  }
}
