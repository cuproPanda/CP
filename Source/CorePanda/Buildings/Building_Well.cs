using System.Collections.Generic;

using UnityEngine;
using Verse;

namespace CorePanda {

  internal class Building_Well : Building_WaterGatherer {

    private CompColdPusher coldpusherComp;

    protected override float MaxWater {
      get { return bucketVolume * 20; }
    }


    public override void SpawnSetup() {
      base.SpawnSetup();

      coldpusherComp = GetComp<CompColdPusher>();

      // Add beginning water, so the well doesn't start completely dry
      AddOrRemoveWater(bucketVolume * 1.3f, true);
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
          defaultDesc = "CP_EmptyTankDesc".Translate(),
          activateSound = SoundDef.Named("Click"),
          action = () => { EmptyContainer(); },
        };
        yield return emptyWell; 
      }

      if (base.GetGizmos() != null) {
        foreach (Command c in base.GetGizmos()) {
          yield return c;
        }
      }
    }


    public override void Tick() {
      base.Tick();

      if (Find.TickManager.TicksGame % 25 == 0) {
        CalculateWater();
      }

      if (Position.GetTemperature() > coldpusherComp.Props.coldPushMinTemperature) {
        coldpusherComp.SimplePush(60f / biomeMultiplier);
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
          AddOrRemoveWater(0.5f, true);
        }
      }

      // Fill the well a little, simulating ground saturation from the water table
      AddOrRemoveWater(0.8f, true);

      // If the well has 1200 water or more, there's enough water to fill another bucket
      if (containedWaterInt >= bucketVolume) {
        hasEnoughWater = true;
      }
    }
  }
}
