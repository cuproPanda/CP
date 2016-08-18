using System.Text;

using UnityEngine;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Dries mudbricks in the sunlight
  /// </summary>
  internal class Building_DryingMudbricks : Building {

    // Sunlight component handles the sunlight based on weather
    private CompSunlight sunlightComp;
    // Allows for setting the color
    private CompColorable colorableComp;
    // The default value for drying
    private int dryingTicksBase = 50;
    // Drying Ticks left. Starts at 50
    private int dryingTicks = 50;

    // Values for different colors as the bricks dry
    private readonly Color WetColor      = new Color(0.274f, 0.196f, 0.118f);  // (70, 50, 30)
    private readonly Color Drying1Color  = new Color(0.372f, 0.294f, 0.216f);
    private readonly Color Drying2Color  = new Color(0.470f, 0.392f, 0.314f);
    private readonly Color Drying3Color  = new Color(0.568f, 0.490f, 0.412f);
    private readonly Color DryColor      = new Color(0.666f, 0.588f, 0.510f);  // (170, 150, 130)

    /// <summary>
    /// Change the color depending on drying progress
    /// </summary>
    public override Color DrawColor {
      get {
        if (dryingTicks >= 30 && dryingTicks < 40) {
          return Drying1Color;
        }
        if (dryingTicks >= 20 && dryingTicks < 30) {
          return Drying2Color;
        }
        if (dryingTicks >= 10 && dryingTicks < 20) {
          return Drying3Color;
        }
        if (dryingTicks < 10) {
          return DryColor;
        }
        return WetColor;
      }

      set {
        base.DrawColor = value;
      }
    }


    /// <summary></summary>
    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref dryingTicks, "dryingTicks", 50);
    }


    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      sunlightComp = GetComp<CompSunlight>();
      colorableComp = GetComp<CompColorable>();
    }


    /// <summary></summary>
    public override void TickRare() {
      base.TickRare();

      // Update the graphic
      if (dryingTicks % 10 == 0) {
        Notify_ColorChanged();
        Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Things); 
      }

      // If it's not raining/snowing, and there is sufficient sunlight
      if (Find.WeatherManager.RainRate == 0f && Find.WeatherManager.SnowRate <= 0.2f) {
        if (sunlightComp.FactoredSunlight >= 0.5f) {
          dryingTicks--;
        }
      }
      // If the bricks got wet
      if (Find.WeatherManager.RainRate > 0f || Find.WeatherManager.SnowRate > 0.2f) {
        dryingTicks = dryingTicksBase;
      }
      // If the bricks are done drying, destroy this and spawn mudbricks
      if (dryingTicks <= 0) {
        Destroy();
        Thing placedProduct = ThingMaker.MakeThing(ThingDef.Named("CP_Mudbrick"));
        placedProduct.stackCount = 5;
        GenPlace.TryPlaceThing(placedProduct, Position, ThingPlaceMode.Direct);
      }
    }


    /// <summary></summary>
    public override string GetInspectString() {
      StringBuilder stringBuilder = new StringBuilder();
      // Get inherited string data
      stringBuilder.Append(base.GetInspectString());

      // Display the drying progress
      stringBuilder.AppendLine("CP_Progress".Translate() + ": (" + (100 - (dryingTicks * 2)) + "%)");

      return stringBuilder.ToString();
    }
  }
}
