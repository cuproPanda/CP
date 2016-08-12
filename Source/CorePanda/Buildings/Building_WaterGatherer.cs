using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {

  internal class Building_WaterGatherer : Building {

    protected float containedWaterInt = 0f;
    protected readonly float bucketVolume = 1200f;
    protected bool hasEnoughWater = false;
    protected float biomeMultiplier;

    protected virtual float maxWater { get; }

    public float ContainedWater {
      get { return containedWaterInt; }
    }


    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref containedWaterInt, "containedWater", 0f, false);
      Scribe_Values.LookValue(ref hasEnoughWater, "hasEnoughWater", false, false);
    }


    public override void SpawnSetup() {
      base.SpawnSetup();

      Log.Warning(def.LabelCap + " spawned at " + Position.ToIntVec2);

      // Get the map's rainfall, at a min of 100 and a max of 1000,
      // then divide by 1000. Values will be somewhere between 0.1f and 1.0f
      biomeMultiplier = Mathf.Min(Mathf.Max(Find.Map.WorldSquare.rainfall, 100), 1000f) / 1000;
    }


    public void BucketSpawned(int buckets = 1) {
      // Make sure there is enough water to remove. Do nothing, but send a warning
      if (containedWaterInt - (bucketVolume * buckets) < 0) {
        Log.Warning("CorePanda:: Drawing more water from the " + def.label + " at " + Position.ToIntVec2 + " than it was supposed to have.");
      }

      // Remove water based on amount of buckets spawned
      AddOrRemoveWater(-bucketVolume * buckets);

      // If there isn't enough water to fill a bucket, note it
      if (containedWaterInt < bucketVolume) {
        hasEnoughWater = false;
      }
    }


    protected void EmptyContainer() {
      Thing bucket = Position.GetThingList().Find(b => b.def == ThingDef.Named("CP_FreshWaterBucket"));

      if (bucket != null && bucket.stackCount >= 10) {
        Messages.Message("CP_TooManyBuckets".Translate(new object[] { def.label }), Position, MessageSound.Negative);
        return;
      }

      if (bucket != null && bucket.stackCount < 10) {
        for (int b = 0; b < (10 - bucket.stackCount); b++) {
          if (hasEnoughWater) {
            bucket.stackCount++;
            BucketSpawned();
          }
        }
        bucket.SetForbidden(true, false);
      }

      if (bucket == null) {
        Thing newBucket = ThingMaker.MakeThing(ThingDef.Named("CP_FreshWaterBucket"));
        newBucket.stackCount = Mathf.Min(Mathf.FloorToInt(containedWaterInt / bucketVolume), newBucket.def.stackLimit);
        GenSpawn.Spawn(newBucket, Position);
        Position.GetThingList().Find(b => b.def == ThingDef.Named("CP_FreshWaterBucket")).SetForbidden(true, false);
        BucketSpawned(newBucket.stackCount);
      }
    }


    protected void AddOrRemoveWater(float amount, bool affectedByBiome = false) {
      if (affectedByBiome) {
        amount *= biomeMultiplier;
      }
      containedWaterInt = Mathf.Clamp(containedWaterInt + amount, 0, maxWater);
    }


    public override string GetInspectString() {
      StringBuilder stringBuilder = new StringBuilder();
      // Get inherited string data
      stringBuilder.Append(base.GetInspectString());

      // Display the water level
      stringBuilder.AppendLine("CP_WaterLevel".Translate() + ": " + containedWaterInt.ToString("#####0"));

      // Display whether there's enough water to fill a bucket
      stringBuilder.AppendLine("CP_Buckets".Translate() + ": " + Mathf.FloorToInt(containedWaterInt / bucketVolume).ToString());

      return stringBuilder.ToString();
    }
  }
}
