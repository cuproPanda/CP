using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Allows colonists to dig up dirt, clay, sand, or gravel
  /// </summary>
  internal class Building_ResourcePitSpawner : Building {
    /// <summary></summary>
    public override void SpawnSetup() {
      TerrainDef terrainDef = Find.TerrainGrid.TerrainAt(Position);
      Building_WorkTable pit = null;

      base.SpawnSetup();

      // Destroy the spawner, otherwise every load generates a new pit
      // Also destroys the spawner in the event something went wrong
      Destroy();

      // Determine what pit to spawn based on the terrain
      if (terrainDef == TerrainDef.Named("Sand")){
        pit = ThingMaker.MakeThing(ThingDef.Named("CP_SandPit"), null) as Building_WorkTable;
      }
      if (terrainDef == TerrainDef.Named("WaterShallow") ||
          terrainDef == TerrainDef.Named("MarshyTerrain")) {
        pit = ThingMaker.MakeThing(ThingDef.Named("CP_ClayPit"), null) as Building_WorkTable;
      }
      if (terrainDef == TerrainDef.Named("Mud") ||
          terrainDef == TerrainDef.Named("Soil") ||
          terrainDef == TerrainDef.Named("SoilRich") ||
          terrainDef == TerrainDef.Named("MossyTerrain")){
        pit = ThingMaker.MakeThing(ThingDef.Named("CP_DirtPit"), null) as Building_WorkTable;
      }
      if (terrainDef == TerrainDef.Named("Gravel")) {
        pit = ThingMaker.MakeThing(ThingDef.Named("CP_GravelPit"), null) as Building_WorkTable;
      }

      if (pit != null) {
        GenSpawn.Spawn(pit, Position, Rotation);
        pit.SetFactionDirect(Faction.OfPlayer);
      }
    }
  }
}
