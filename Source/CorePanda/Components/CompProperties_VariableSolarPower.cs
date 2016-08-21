using RimWorld;

namespace CorePanda {
  /// <summary>
  /// Allows XML-editing of solar power output
  /// </summary>
  public class CompProperties_VariableSolarPower : CompProperties_Power{
    /// Public to allow XML editing
    public float fullSunPower = 1700f;
    /// Public to allow XML editing
    public float nightPower = 0f;

    /// <summary> Connect this to CompProperties_VariableSolarPower </summary>
    public CompProperties_VariableSolarPower() {
      compClass = typeof(CompVariablePowerPlantSolar);
    }
  }
}
