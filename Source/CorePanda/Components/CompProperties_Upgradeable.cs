using Verse;

namespace CorePanda {
  /// <summary>
  /// Handles stats for upgrading
  /// </summary>
  public class CompProperties_Upgradeable : CompProperties {
    /// <summary>
    /// The <see cref="ResearchProjectDef"/> DefName, if any, to unlock the upgrade
    /// </summary>
    public string ResearchString;

    /// <summary>
    /// The <see cref="ContentFinder{T}"/> string location of the button texture (Things/UI/Designators/ExampleTexture)
    /// </summary>
    public string UpgradeTex;

    /// <summary>
    /// The <see cref="ThingDef"/> DefName for the upgraded ThingDef (ElectricSmithy)
    /// </summary>
    public string BlueprintThingDef;

    /// <summary>
    /// The complete string for the upgrade button's description
    /// </summary>
    public string UpgradeDescString;

    /// <summary>
    /// The complete string for the upgrade button's label
    /// </summary>
    public string UpgradeLabelString;


    /// <summary></summary>
    public CompProperties_Upgradeable() {
      compClass = typeof(CompUpgradeable);
    }
  }
}
