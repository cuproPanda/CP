using System.Collections.Generic;

using UnityEngine;
using Verse;

namespace CorePanda {
  [StaticConstructorOnStartup]
  internal class Building_RainBarrel : Building_WaterGatherer {

    private static readonly Graphic S_BarrelSealed  = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Sealed");
    private static readonly Graphic S_BarrelEmpty   = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Empty");
    private static readonly Graphic S_BarrelPartial = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Partial");
    private static readonly Graphic S_BarrelFull    = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Full");
    private bool isSealed = false;

    protected override float MaxWater {
      get { return bucketVolume * 10; }
    }

    public override Graphic Graphic {
      get {
        if (isSealed) {
          return S_BarrelSealed;
        }
        if (ContainedWater > (MaxWater * 0.7f)) {
          return S_BarrelFull;
        }
        if (ContainedWater > (MaxWater * 0.3f)) {
          return S_BarrelPartial;
        }
        return S_BarrelEmpty;
      }
    }


    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref isSealed, "CP_RainBarrel_isSealed", false);
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

      if (!isSealed) {
        Command_Action sealBarrel = new Command_Action() {
          icon = ContentFinder<Texture2D>.Get("Cupro/Object/Utility/RainBarrel/RainBarrel_Sealed", false),
          defaultLabel = "CP_SealBarrel".Translate(),
          defaultDesc = "CP_SealBarrelDesc".Translate(),
          activateSound = SoundDef.Named("Click"),
          action = () => { isSealed = true; },
        };
        yield return sealBarrel; 
      }
      if (isSealed) {
        Command_Action unsealBarrel = new Command_Action() {
          icon = ContentFinder<Texture2D>.Get("Cupro/Object/Utility/RainBarrel/RainBarrel_Full", false),
          defaultLabel = "CP_UnsealBarrel".Translate(),
          defaultDesc = "CP_UnsealBarrelDesc".Translate(),
          activateSound = SoundDef.Named("Click"),
          action = () => { isSealed = false; },
        };
        yield return unsealBarrel;
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
        if (!isSealed) {
          CalculateWater(); 
        }

        // Manually update the graphic
        Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Things);
      }
    }


    private void CalculateWater() {
      // If there is sufficient precipitation
      if ((Find.WeatherManager.RainRate > 0.2f || Find.WeatherManager.SnowRate > 0.7f) && !Position.Roofed()) {
        AddOrRemoveWater(2f);
      }

      // Simulate evaporation
      float temp = GenTemperature.GetTemperatureForCell(Position);
      float evaporation = 0.0001f;
      if (temp >= 20f) {
        evaporation = (temp - 19.9f) / 100f;
      }
      AddOrRemoveWater(-evaporation);

      // If the well has 1200 water or more, there's enough water to fill another bucket
      if (containedWaterInt >= bucketVolume) {
        hasEnoughWater = true;
      }
    }
  }
}
