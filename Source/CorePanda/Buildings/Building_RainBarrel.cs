using System.Collections.Generic;

using UnityEngine;
using Verse;

namespace CorePanda {

  internal class Building_RainBarrel : Building_WaterGatherer {

    private Graphic BarrelEmpty;
    private Graphic BarrelPartial;
    private Graphic BarrelFull;

    protected override float maxWater {
      get { return bucketVolume * 10; }
    }

    public override Graphic Graphic {
      get {
        if (ContainedWater > (maxWater * 0.6f)) {
          return BarrelFull;
        }
        if (ContainedWater > (maxWater * 0.3f)) {
          return BarrelPartial;
        }
        return BarrelEmpty;
      }
    }


    public override void SpawnSetup() {
      base.SpawnSetup();

      BarrelEmpty = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Empty");
      BarrelPartial = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Partial");
      BarrelFull = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Full");
  }


    // Allow filling the barrel in god mode -- for testing
    public override IEnumerable<Gizmo> GetGizmos() {
      if (DebugSettings.godMode) {
        Command_Action fillBarrel = new Command_Action() {

          icon = BaseContent.BadTex,
          defaultLabel = "Debug: Fill",
          defaultDesc = "Debug: fill barrel with water",
          activateSound = SoundDef.Named("Click"),
          action = () => { AddOrRemoveWater(99999f); },
        };
        yield return fillBarrel;
      }

      if (hasEnoughWater) {
        Command_Action emptyBarrel = new Command_Action() {

          icon = ContentFinder<Texture2D>.Get("Cupro/Item/Material/WaterBucket"),
          defaultLabel = "CP_EmptyBarrel".Translate(),
          defaultDesc = "CP_EmptyTankDesc".Translate(),
          activateSound = SoundDef.Named("Click"),
          action = () => { EmptyContainer(); },
        };
        yield return emptyBarrel;
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
    }


    private void CalculateWater() {
      // If there is sufficient precipitation
      if ((Find.WeatherManager.RainRate > 0.2f || Find.WeatherManager.SnowRate > 0.7f) && !Position.Roofed()) {
        AddOrRemoveWater(1.8f);
      }

      // If the well has 1200 water or more, there's enough water to fill another bucket
      if (containedWaterInt >= bucketVolume) {
        hasEnoughWater = true;
      }
    }
  }
}
