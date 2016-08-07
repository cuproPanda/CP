using Verse;

namespace CorePanda {
  /// <summary>
  /// Spawns a random quantity of quartz
  /// </summary>
  internal class Item_QuartzSpawner : ItemSpawner {
    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      SpawnRandomQuantity(ThingDef.Named("CP_Quartz"), 8, 35);
      Destroy();
    }
  }
}
