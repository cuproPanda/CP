using Verse;

namespace CorePanda {

  public abstract class ItemSpawner : Thing {

    // Spawns a random quantity of a ThingDef
    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn, IntVec3 pos, Map map, ThingPlaceMode mode, bool spawningDestroys = true) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      SpawnExactQuantity(TDef, stack, pos, map, mode, spawningDestroys);
    }


    // Spawns an exact quantity of a ThingDef
    public void SpawnExactQuantity(ThingDef TDef, int NumToSpawn, IntVec3 pos, Map map, ThingPlaceMode mode, bool spawningDestroys = true) {
      if (spawningDestroys) {
        Destroy();
      }

      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = NumToSpawn;
      GenPlace.TryPlaceThing(placedProduct, pos, map, mode);
    }
  }
}
