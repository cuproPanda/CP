using Verse;

namespace CorePanda {

  public class CompSunlight : ThingComp {

    private WeatherDef weatherDef;
    private float sunStrength;

    // Used for inspect string weather reporting
    private WeatherLight wLight = WeatherLight.None;  

    // Used for determining if the weather is affecting the current sunlight
    public WeatherLight WeatherLight { get { return wLight; } }

    // Returns 0.0f to 1.0f based on time and weather
    public float SimpleFactoredSunlight {
      get { return parent.Map.skyManager.CurSkyGlow * sunStrength; }
    }

    // Gets the current sunlight, then returns 0.0f to 1.0f based on time and weather
    public float FactoredSunlight {
      get { GetSunlight();  return parent.Map.skyManager.CurSkyGlow * sunStrength; }
    }


    // Updates the sunlight based on weather
    public virtual void GetSunlight() {
      // Get the current weather
      weatherDef = parent.Map.weatherManager.curWeather;

      // Clear weather provides the maximum sunlight
      if (weatherDef == CpDefOf.Clear) {
        wLight = WeatherLight.Bright;
        sunStrength = 1f;
        return;
      }
      // These weathers provide 60% sunlight
      else if (weatherDef == CpDefOf.Fog ||
               weatherDef == CpDefOf.Rain ||
               weatherDef == CpDefOf.SnowGentle) {
        wLight = WeatherLight.Darkened;
        sunStrength = 0.6f;
        return;
      }
      // These weathers get only 35% sunlight
      else if (weatherDef == CpDefOf.FoggyRain ||
               weatherDef == CpDefOf.SnowHard ||
               weatherDef == CpDefOf.DryThunderstorm ||
               weatherDef == CpDefOf.RainyThunderstorm) {
        wLight = WeatherLight.Dark;
        sunStrength = 0.35f;
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