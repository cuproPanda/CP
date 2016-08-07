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

    public float ContainedWater {
      get { return containedWaterInt; }
    }

    public bool CanWorkHere {
      get { return hasEnoughWater; }
    }


    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref containedWaterInt, "containedWater", 0f, false);
      Scribe_Values.LookValue(ref hasEnoughWater, "hasEnoughWater", false, false);
    }


    public override void SpawnSetup() {
      base.SpawnSetup();

      // Get the map's rainfall, at a min of 100 and a max of 1000,
      // then divide by 1000. Values will be somewhere between 0.1f and 1.0f
      biomeMultiplier = Mathf.Min(Mathf.Max(Find.Map.WorldSquare.rainfall, 100), 1000f) / 1000;

      // Add beginning water, so the well doesn't start completely dry
      AddOrRemoveWater(500 * biomeMultiplier);
    }


    public void Notify_IterationCompleted() {
      // Make sure there is enough water to remove. Do nothing, but send a warning
      if ((containedWaterInt - 1200f) < 0 ) {
        Log.Warning("CorePanda:: Drawing more water from the well than it was supposed to have.");
      }

      // Remove 1200 water
      AddOrRemoveWater(-1200f);

      // If there's less than 1200 water remaining, there's not enough water to fill another bucket
      if (containedWaterInt < 1200f) {
        hasEnoughWater = false;
      }
    }


    public override void TickRare() {
      base.TickRare();

      // If there is sufficient precipitation
      if (Find.WeatherManager.RainRate > 0.2f || Find.WeatherManager.SnowRate > 0.7f) {
        // If the well is unroofed, it gets filled directly
        if (!Position.Roofed()) {
          AddOrRemoveWater(20f);
        }
        // If the well is roofed, account for increased ground saturation
        if (Position.Roofed()) {
          AddOrRemoveWater(5f * biomeMultiplier);
        }
      }

      // Fill the well a little, simulating ground saturation from the water table
      AddOrRemoveWater(8f * biomeMultiplier);

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
      stringBuilder.AppendLine("CP_WaterLevel".Translate() + ": (" + containedWaterInt.ToString("####0") + ")");

      return stringBuilder.ToString();
    }
  }
}
