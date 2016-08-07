using Verse;

namespace CorePanda {
  /// <summary>
  /// Spawns items
  /// </summary>
  public class ItemSpawner : ThingWithComps {

    /// <summary>
    /// Spawns a random quantity of a ThingDef
    /// <para>Don't forget to destroy the spawner!</para>
    /// </summary>
    /// <param name="TDef">ThingDef to spawn. ThingDef.Named("DefName") can be used</param>
    /// <param name="MinToSpawn">The minimum amount to spawn</param>
    /// <param name="MaxToSpawn">The maximum amount to spawn. Inclusive</param>
    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = stack;
      GenPlace.TryPlaceThing(placedProduct, base.Position, ThingPlaceMode.Near);
    }


    /// <summary>
    /// Spawns a random quantity of a Thing
    /// <para>Don't forget to destroy the spawner!</para>
    /// </summary>
    /// <param name="T">Thing to spawn.</param>
    /// <param name="MinToSpawn">The minimum amount to spawn</param>
    /// <param name="MaxToSpawn">The maximum amount to spawn. Inclusive</param>
    public void SpawnRandomQuantity(Thing T, int MinToSpawn, int MaxToSpawn) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      T.stackCount = stack;
      GenPlace.TryPlaceThing(T, base.Position, ThingPlaceMode.Near);
    }
  }
}
