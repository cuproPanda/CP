using UnityEngine;
using Verse;

namespace CorePanda {
  [StaticConstructorOnStartup]
  public static class Static {

    public static Texture2D texHarvestSecondary = ContentFinder<Texture2D>.Get("Cupro/UI/Designations/HarvestSecondary");

    public static DesignationDef DesignationHarvestSecondary = DefDatabase<DesignationDef>.GetNamed("CP_Designator_PlantsHarvestSecondary");

    public static string CorePanda = "CorePanda".Translate();
    public static string SettingsSpawnCopperOptional = "CP_SettingsSpawnCopperOptional".Translate();
    public static string SettingsSpawnLeadOptional = "CP_SettingsSpawnLeadOptional".Translate();
    public static string SettingsSpawnTinOptional = "CP_SettingsSpawnTinOptional".Translate();
    public static string SettingsSpawnAluminumOptional = "CP_SettingsSpawnAluminumOptional".Translate();
    public static string TooltipSpawnCopperOptional = "CP_TooltipSpawnCopperOptional".Translate();
    public static string TooltipSpawnLeadOptional = "CP_TooltipSpawnLeadOptional".Translate();
    public static string TooltipSpawnTinOptional = "CP_TooltipSpawnTinOptional".Translate();
    public static string TooltipSpawnAluminumOptional = "CP_TooltipSpawnAluminumOptional".Translate();

    public static string SettingsSpawnCopperRequired = "CP_SettingsSpawnCopperRequired".Translate();
    public static string SettingsSpawnLeadRequired = "CP_SettingsSpawnLeadRequired".Translate();
    public static string SettingsSpawnTinRequired = "CP_SettingsSpawnTinRequired".Translate();
    public static string SettingsSpawnAluminumRequired = "CP_SettingsSpawnAluminumRequired".Translate();
    public static string TooltipSpawnCopperRequired = "CP_TooltipSpawnCopperRequired".Translate();
    public static string TooltipSpawnLeadRequired = "CP_TooltipSpawnLeadRequired".Translate();
    public static string TooltipSpawnTinRequired = "CP_TooltipSpawnTinRequired".Translate();
    public static string TooltipSpawnAluminumRequired = "CP_TooltipSpawnAluminumRequired".Translate();

    public static string WeatherReportBright = "CP_Bright".Translate();
    public static string WeatherReportDarkened = "CP_Darkened".Translate();
    public static string WeatherReportDark = "CP_Dark".Translate();
    public static string InspectSunlightLevel = "CP_SunlightLevel".Translate();
    public static string LabelHarvestSecondary = "CP_DesignatorHarvestSecondary".Translate();
    public static string DescriptionHarvestSecondary = "CP_DesignatorHarvestSecondaryDesc".Translate();
    public static string ReportDesignatePlantsWithSecondary = "CP_MustDesignatePlantsWithSecondary".Translate();
    public static string ReportDesignateHarvestableSecondary = "CP_MustDesignateHarvestableSecondary".Translate();
    public static string ReportBadSeason = "CP_CannotGrowBadSeason".Translate();
  }
}
