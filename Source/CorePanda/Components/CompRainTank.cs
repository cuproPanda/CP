﻿using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Stores water based on the weather and biome
  /// </summary>
  public class CompRainTank : ThingComp {

    /// <summary>
    /// Getter for production statistics
    /// </summary>
    public float FillRatePerTick {
      get { return Precipitation * biomeMultiplier; }
    }
    /// <summary>
    /// Getter for production statistics
    /// </summary>
    public float FillRatePerHour {
      get { return FillRatePerTick * GenDate.TicksPerHour; }
    }

    /// <summary>
    /// Maximum amount of water to be stored, needs to be set by parent
    /// </summary>
    public float WaterLevelMax { get; set; }

    /// <summary>
    /// Current water level
    /// </summary>
    public float WaterLevel {
      get {
        return waterLevelInt;
      }
    }
    private float waterLevelInt;

    /// <summary>
    /// How much rain/snow is falling
    /// </summary>
    public float Precipitation {
      get {
        return (Find.WeatherManager.RainRate + (Find.WeatherManager.SnowRate * 0.5f));
      }
    }

    // The map biome, used for fine-tuning precipitation based on dryness or freezing
    private BiomeDef biomeDef = Find.Map.Biome;
    private float biomeMultiplierInt = 0f;
    private float biomeMultiplier {
      get {
        if (biomeMultiplierInt == 0f) {
          if (biomeDef == BiomeDefOf.BorealForest ||
              biomeDef == BiomeDefOf.AridShrubland) {
            biomeMultiplierInt = 0.8f;
          }
          else if (biomeDef == BiomeDefOf.Tundra ||
                   biomeDef == BiomeDefOf.Desert) {
            biomeMultiplierInt = 0.6f;
          }
          else if (biomeDef == BiomeDefOf.IceSheet ||
                   biomeDef == BiomeDef.Named("ExtremeDesert")) {
            biomeMultiplierInt = 0.4f;
          }
          else {
            biomeMultiplierInt = 1f;
          }
        }
        return (float)biomeMultiplierInt;
      }
    }


    /// <summary>
    /// Handles loading data
    /// </summary>
    public override void PostExposeData() {
      base.PostExposeData();
      Scribe_Values.LookValue(ref waterLevelInt, "storedWater", 0f, false);
      Scribe_Values.LookValue(ref biomeMultiplierInt, "biomeMultiplier", 0f, false);
    }


    /// <summary>
    /// Add water to the tank based on weather and biome
    /// </summary>
    /// <param name="divisor">125f is a good starting divisor for a 1-cell tank that accepts water every tick</param>
    public virtual void AddWater(float divisor = 125f) {
      // Calculate the water level
      waterLevelInt += (Precipitation * biomeMultiplier) / divisor;
      if (waterLevelInt > WaterLevelMax) {
        waterLevelInt = WaterLevelMax;
      }
    }


    /// <summary>
    /// Consume water from the tank
    /// <para>Make sure there is enough water stored prior to calling this</para>
    /// </summary>
    public virtual void UseWater(float amount) {
      waterLevelInt -= amount;
      if (waterLevelInt < 0f) {
        Log.Error("CP_WaterError".Translate() + this.parent);
        waterLevelInt = 0f;
      }
    }
  }
}