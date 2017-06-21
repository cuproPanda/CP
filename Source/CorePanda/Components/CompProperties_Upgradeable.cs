using Verse;

namespace CorePanda {

  public class CompProperties_Upgradeable : CompProperties {

    public string researchString;

    public string upgradeTex;

    public string blueprintThingDef;

    public string upgradeDescString;

    public string upgradeLabelString;


    public CompProperties_Upgradeable() {
      compClass = typeof(CompUpgradeable);
    }
  }
}
