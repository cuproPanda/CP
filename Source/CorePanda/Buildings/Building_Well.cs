using System.Collections.Generic;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {

  internal class Building_Well : Building_WorkTable {

    private readonly float maxWater = 12000f;
    private float containedWaterInt = 0f;
    private bool hasEnoughWater = false;
    private float biomeMultiplier;
    private CompColdPusher coldpusherComp;

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

      coldpusherComp = GetComp<CompColdPusher>();

      // Get the map's rainfall, at a min of 100 and a max of 1000,
      // then divide by 1000. Values will be somewhere between 0.1f and 1.0f
      biomeMultiplier = Mathf.Min(Mathf.Max(Find.Map.WorldSquare.rainfall, 100), 1000f) / 1000;

      // Add beginning water, so the well doesn't start completely dry
      AddOrRemoveWater(1500 * biomeMultiplier);
    }


    // Allow filling the well in god mode -- for testing
    public override IEnumerable<Gizmo> GetGizmos() {
      if (DebugSettings.godMode) {
        Command_Action fillWell = new Command_Action() {

          icon = BaseContent.BadTex,
          defaultLabel = "Debug: Fill",
          defaultDesc = "Debug: fill well with water",
          activateSound = SoundDef.Named("Click"),
          action = () => { AddOrRemoveWater(99999f); },
        };
        yield return fillWell; 
      }

      if (hasEnoughWater) {
        Command_Action emptyWell = new Command_Action() {

          icon = ContentFinder<Texture2D>.Get("Cupro/Item/Material/WaterBucket"),
          defaultLabel = "CP_EmptyWell".Translate(),
          defaultDesc = "CP_EmptyWellDesc".Translate(),
          activateSound = SoundDef.Named("Click"),
          action = () => { EmptyWell(); },
        };
        yield return emptyWell; 
      }

      if (base.GetGizmos() != null) {
        foreach (Command c in base.GetGizmos()) {
          yield return c;
        }
      }
    }


    public void BucketSpawned(int buckets = 1) {
      // Make sure there is enough water to remove. Do nothing, but send a warning
      if (containedWaterInt - (1200f * buckets) < 0 ) {
        Log.Warning("CorePanda:: Drawing more water from the well than it was supposed to have.");
      }

      // Remove 1200 water per bucket
      AddOrRemoveWater(-1200f * buckets);

      // If there's less than 1200 water remaining, there's not enough water to fill another bucket
      if (containedWaterInt < 1200f) {
        hasEnoughWater = false;
      }
    }


    public override void Tick() {
      base.Tick();

      if (Position.GetTemperature() > coldpusherComp.Props.ColdPushMinTemperature) {
        coldpusherComp.SimplePush(60f / biomeMultiplier);
      }

      if (Find.TickManager.TicksGame % 25 == 0) {
        CalculateWater();
      }
    }


    private void EmptyWell() {
      Thing bucket = Position.GetThingList().Find(b => b.def == ThingDef.Named("CP_FreshWaterBucket"));

      if (bucket != null && bucket.stackCount >= 10) {
        Messages.Message("CP_TooManyBuckets".Translate(), Position, MessageSound.Negative);
        return;
      }

      if (bucket != null && bucket.stackCount < 10) {
        while (hasEnoughWater) {
          bucket.stackCount++;
          BucketSpawned();
        }
        bucket.SetForbidden(true, false);
      }

      if (bucket == null) {
        Thing newBucket = ThingMaker.MakeThing(ThingDef.Named("CP_FreshWaterBucket"));
        newBucket.stackCount = Mathf.FloorToInt(containedWaterInt / 1200f);
        GenSpawn.Spawn(newBucket, Position);
        Position.GetThingList().Find(b => b.def == ThingDef.Named("CP_FreshWaterBucket")).SetForbidden(true, false);
        BucketSpawned(newBucket.stackCount);
      }
    }


    private void CalculateWater() {
      // If there is sufficient precipitation
      if (Find.WeatherManager.RainRate > 0.2f || Find.WeatherManager.SnowRate > 0.7f) {
        // If the well is unroofed, it gets filled directly
        if (!Position.Roofed()) {
          AddOrRemoveWater(2f);
        }
        // If the well is roofed, account for increased ground saturation
        if (Position.Roofed()) {
          AddOrRemoveWater(0.5f * biomeMultiplier);
        }
      }

      // Fill the well a little, simulating ground saturation from the water table
      AddOrRemoveWater(0.8f * biomeMultiplier);

      // If the well has 1200 water or more, there's enough water to fill another bucket
      if (containedWaterInt >= 1200f) {
        hasEnoughWater = true;
      }
    }


    private void AddOrRemoveWater(float amount) {
      containedWaterInt = Mathf.Clamp(containedWaterInt + amount, 0, maxWater);
    }


    /// <summary></summary>
    public override string GetInspectString() {
      StringBuilder stringBuilder = new StringBuilder();
      // Get inherited string data
      stringBuilder.Append(base.GetInspectString());

      // Display the water level
      stringBuilder.AppendLine("CP_WaterLevel".Translate() + ": " + containedWaterInt.ToString("####0"));

      // Display whether there's enough water to fill a bucket
      stringBuilder.Append("CP_Buckets".Translate() + ": ");
      if (hasEnoughWater) {
        stringBuilder.AppendLine(Mathf.FloorToInt(containedWaterInt / 1200).ToString());
      }
      if (!hasEnoughWater) {
        stringBuilder.AppendLine("0");
      }

      return stringBuilder.ToString();
    }
  }
}
