using Verse;

namespace CorePanda {
  /// <summary>
  /// Pushes coldness instead of heat
  /// </summary>
  public class CompProperties_ColdPusher : CompProperties {

    /// <summary>
    /// Strength(not temperature) of coldness to output
    /// </summary>
    public float ColdPerSecond;

    /// <summary>
    /// The minimum temperature to push coldness, set to default
    /// </summary>
    public float ColdPushMinTemperature = 0f;

    /// <summary>
    /// Does this constantly push coldness? Also used to toggle on/off state
    /// </summary>
    public bool ConstantPush = false;

    /// <summary></summary>
    public CompProperties_ColdPusher() {
      compClass = typeof(CompColdPusher);
    }
  }
}
