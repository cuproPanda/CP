using Verse;

namespace CorePanda {
  //TODO: Change to SunlightManager, refactor all using classes

  /// <summary>
  /// Handles sunlight based on weather
  /// </summary>
  public class CompSunlight : ThingComp {

    private WeatherDef weatherDef;        // Weather reference
    private float sunStrength;            // Strength of the sun multiplier, affected by weather

    // Used for inspect string weather reporting
    private WeatherLight wLight = WeatherLight.None;  

    /// <summary>
    /// Used for determining if the weather is affecting the current sunlight
    /// </summary>
    public WeatherLight WeatherLight { get { return wLight; } }

    /// <summary>
    /// Returns 0.0f to 1.0f based on time and weather
    /// <para>Requires GetSunlight() to have already been called</para>
    /// </summary>
    public float SimpleFactoredSunlight { // Final sunlight value based on time and weather, returns cached value
      get { return SkyManager.CurSkyGlow * sunStrength; }
    }

    /// <summary>
    /// Gets the current sunlight, then returns 0.0f to 1.0f based on time and weather
    /// </summary>
    public float FactoredSunlight {
      get { GetSunlight();  return SkyManager.CurSkyGlow * sunStrength; }
    }


    /// <summary>
    /// Updates the sunlight based on weather
    /// </summary>
    public virtual void GetSunlight() {
      // Get the current weather
      weatherDef = Find.WeatherManager.curWeather;

      // Clear weather provides the maximum sunlight
      if (weatherDef == WeatherDef.Named("Clear")) {
        wLight = WeatherLight.Bright;
        sunStrength = 1f;
        return;
      }
      // These weathers provide 60% sunlight
      else if (weatherDef == WeatherDef.Named("Fog") ||
               weatherDef == WeatherDef.Named("Rain") ||
               weatherDef == WeatherDef.Named("SnowGentle")) {
        wLight = WeatherLight.Darkened;
        sunStrength = 0.6f;
        return;
      }
      // These weathers get only 25% sunlight
      else if (weatherDef == WeatherDef.Named("FoggyRain") ||
               weatherDef == WeatherDef.Named("SnowHard") ||
               weatherDef == WeatherDef.Named("DryThunderstorm") ||
               weatherDef == WeatherDef.Named("RainyThunderstorm")) {
        wLight = WeatherLight.Dark;
        sunStrength = 0.25f;
        return;
      }
      // Default variable. Prevents issues when other mods add custom weather
      else {
        wLight = WeatherLight.Bright;
        sunStrength = 1f;
      }
    }
  }
}