using Verse;

namespace CorePanda {

  public class Settings : ModSettings {

    internal static bool spawnCopper = false;
    internal static bool spawnLead = false;
    internal static bool spawnTin = false;
    internal static bool spawnAluminum = false;
    internal static bool cxpInstalled = false;
    internal static bool calInstalled = false;

    public static bool ShouldSpawnCopper {
      get { return spawnCopper; }
    }

    public static bool ShouldSpawnLead {
      get { return spawnLead; }
    }

    public static bool ShouldSpawnTin {
      get { return spawnTin; }
    }

    public static bool ShouldSpawnAluminum {
      get { return spawnAluminum; }
    }


    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.Look(ref spawnCopper, "CP_spawnCopper", false);
      Scribe_Values.Look(ref spawnLead, "CP_spawnLead", false);
      Scribe_Values.Look(ref spawnTin, "CP_spawnTin", false);
      Scribe_Values.Look(ref spawnAluminum, "CP_spawnAluminum", false);
      Scribe_Values.Look(ref cxpInstalled, "CP_cxpInstalled", false);
      Scribe_Values.Look(ref calInstalled, "CP_calInstalled", false);
    }
  }
}
