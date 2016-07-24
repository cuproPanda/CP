using Verse;

namespace CorePanda {
  /// <summary>
  /// Only allows an object to be placed on diggable resources (Dirt, Clay, Sand, or Gravel)
  /// </summary>
  internal class PlaceWorker_DiggableResources : PlaceWorker {
    /// <summary>
    /// Whether this can be placed or not
    /// </summary>
    /// <param name="checkingDef">Thing to build</param>
    /// <param name="loc">Location to build</param>
    /// <param name="rot">Rotation of Thing</param>
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot) {
      TerrainDef terrainDef = Find.TerrainGrid.TerrainAt(loc);
      if (terrainDef == TerrainDef.Named("Sand") ||           // Sand

          terrainDef == TerrainDef.Named("Mud") ||            // Dirt
          terrainDef == TerrainDef.Named("Soil") ||           // Dirt
          terrainDef == TerrainDef.Named("SoilRich") ||       // Dirt
          terrainDef == TerrainDef.Named("MossyTerrain") ||   // Dirt
          terrainDef == TerrainDef.Named("MarshyTerrain") ||  // Dirt/Clay
          terrainDef == TerrainDef.Named("Marsh") ||          // Dirt/Clay
          terrainDef == TerrainDef.Named("WaterShallow") ||   // Dirt/Clay

          terrainDef == TerrainDef.Named("Gravel")) {         // Dirt/Gravel

        return true;
      }
      return new AcceptanceReport("CP_MustPlaceOnDiggableResources".Translate());
    }
  }
}
