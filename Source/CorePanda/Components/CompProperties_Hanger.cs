using Verse;

namespace CorePanda {

  public class CompProperties_Hanger : CompProperties {

    public HangingType hangingType = HangingType.None;


    public CompProperties_Hanger() {
      compClass = typeof(CompHanger);
    }
  }
}
