using System.Linq;

using UnityEngine;
using Verse;

namespace CorePanda {

  public enum CpStringReq {
    Label,
    Tooltip
  }

  public enum CpOreReq {
    None,
    Copper,
    Lead,
    Tin,
    Aluminum
  }



  public sealed class CorePandaMod : Mod {

    private bool cxpInstalled = false;
    private bool calInstalled = false;


    public CorePandaMod(ModContentPack content) : base(content) {
      GetSettings<Settings>();
      LongEventHandler.ExecuteWhenFinished(GetDefaultSettings);
    }


    public override string SettingsCategory() {
      return Static.CorePanda;
    }


    private void GetDefaultSettings() {
      if (ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.Name == "Complicated Power")) {
        cxpInstalled = true;
        Settings.cxpInstalled = true;
        Settings.spawnCopper = true;
        Settings.spawnLead = true;
        Settings.spawnTin = true;
      }
      if (ModsConfig.ActiveModsInLoadOrder.Any(mod => mod.Name == "Cupro's Alloys")) {
        calInstalled = true;
        Settings.calInstalled = true;
        Settings.spawnCopper = true;
        Settings.spawnLead = true;
        Settings.spawnTin = true;
        Settings.spawnAluminum = true;
      }
    }


    public override void DoSettingsWindowContents(Rect rect) {
      Listing_Standard list = new Listing_Standard();

      list.ColumnWidth = rect.width;
      list.Begin(rect);
      list.Gap(10);
      {
        Rect firstRowRect = list.GetRect(Text.LineHeight);
        Rect firstLeftRect = firstRowRect.LeftHalf().Rounded();
        Rect firstRightRect = firstRowRect.RightHalf().Rounded();

        if (cxpInstalled || calInstalled) {
          Widgets.Label(firstLeftRect, GetString(CpStringReq.Label, CpOreReq.Copper, true));
          if (Mouse.IsOver(firstLeftRect)) {
            Widgets.DrawHighlight(firstLeftRect);
          }
          TooltipHandler.TipRegion(firstLeftRect, GetString(CpStringReq.Tooltip, CpOreReq.Copper, true));

          Widgets.Label(firstRightRect, GetString(CpStringReq.Label, CpOreReq.Lead, true));
          if (Mouse.IsOver(firstRightRect)) {
            Widgets.DrawHighlight(firstRightRect);
          }
          TooltipHandler.TipRegion(firstRightRect, GetString(CpStringReq.Tooltip, CpOreReq.Lead, true));
        }
        else {
          Widgets.CheckboxLabeled(firstLeftRect, GetString(CpStringReq.Label, CpOreReq.Copper), ref Settings.spawnCopper);
          if (Mouse.IsOver(firstLeftRect)) {
            Widgets.DrawHighlight(firstLeftRect);
          }
          TooltipHandler.TipRegion(firstLeftRect, GetString(CpStringReq.Tooltip, CpOreReq.Copper));

          Widgets.CheckboxLabeled(firstRightRect, GetString(CpStringReq.Label, CpOreReq.Lead), ref Settings.spawnLead);
          if (Mouse.IsOver(firstRightRect)) {
            Widgets.DrawHighlight(firstRightRect);
          }
          TooltipHandler.TipRegion(firstRightRect, GetString(CpStringReq.Tooltip, CpOreReq.Lead));
        }

        list.Gap(25);

        Rect secondRowRect = list.GetRect(Text.LineHeight);
        Rect secondLeftRect = secondRowRect.LeftHalf().Rounded();
        Rect secondRightRect = secondRowRect.RightHalf().Rounded();

        if (cxpInstalled || calInstalled) {
          Widgets.Label(secondLeftRect, GetString(CpStringReq.Label, CpOreReq.Tin, true));
          if (Mouse.IsOver(secondLeftRect)) {
            Widgets.DrawHighlight(secondLeftRect);
          }
          TooltipHandler.TipRegion(secondLeftRect, GetString(CpStringReq.Tooltip, CpOreReq.Tin, true));
        }
        else {
          Widgets.CheckboxLabeled(secondLeftRect, GetString(CpStringReq.Label, CpOreReq.Tin), ref Settings.spawnTin);
          if (Mouse.IsOver(secondLeftRect)) {
            Widgets.DrawHighlight(secondLeftRect);
          }
          TooltipHandler.TipRegion(secondLeftRect, GetString(CpStringReq.Tooltip, CpOreReq.Tin));
        }

        if (calInstalled) {
          Widgets.Label(secondRightRect, GetString(CpStringReq.Label, CpOreReq.Aluminum, true));
          if (Mouse.IsOver(secondRightRect)) {
            Widgets.DrawHighlight(secondRightRect);
          }
          TooltipHandler.TipRegion(secondRightRect, GetString(CpStringReq.Tooltip, CpOreReq.Aluminum, true));
        }
        else {
          Widgets.CheckboxLabeled(secondRightRect, GetString(CpStringReq.Label, CpOreReq.Aluminum), ref Settings.spawnAluminum);
          if (Mouse.IsOver(secondRightRect)) {
            Widgets.DrawHighlight(secondRightRect);
          }
          TooltipHandler.TipRegion(secondRightRect, GetString(CpStringReq.Tooltip, CpOreReq.Aluminum));
        }

        list.Gap(10);

        list.End();
      }
    }


    private string GetString(CpStringReq stringReq, CpOreReq oreReq, bool required = false) {
      if (required) {
        if (oreReq == CpOreReq.Copper) {
          if (stringReq == CpStringReq.Label) {
            return Static.SettingsSpawnCopperRequired;
          }
          return Static.TooltipSpawnCopperRequired;
        }
        if (oreReq == CpOreReq.Lead) {
          if (stringReq == CpStringReq.Label) {
            return Static.SettingsSpawnLeadRequired;
          }
          return Static.TooltipSpawnLeadRequired;
        }
        if (oreReq == CpOreReq.Tin) {
          if (stringReq == CpStringReq.Label) {
            return Static.SettingsSpawnTinRequired;
          }
          return Static.TooltipSpawnTinRequired;
        }
        if (oreReq == CpOreReq.Aluminum) {
          if (stringReq == CpStringReq.Label) {
            return Static.SettingsSpawnAluminumRequired;
          }
          return Static.TooltipSpawnAluminumRequired;
        }
      }

      if (oreReq == CpOreReq.Copper) {
        if (stringReq == CpStringReq.Label) {
          return Static.SettingsSpawnCopperOptional;
        }
        return Static.TooltipSpawnCopperOptional;
      }
      if (oreReq == CpOreReq.Lead) {
        if (stringReq == CpStringReq.Label) {
          return Static.SettingsSpawnLeadOptional;
        }
        return Static.TooltipSpawnLeadOptional;
      }
      if (oreReq == CpOreReq.Tin) {
        if (stringReq == CpStringReq.Label) {
          return Static.SettingsSpawnTinOptional;
        }
        return Static.TooltipSpawnTinOptional;
      }
      if (oreReq == CpOreReq.Aluminum) {
        if (stringReq == CpStringReq.Label) {
          return Static.SettingsSpawnAluminumOptional;
        }
        return Static.TooltipSpawnAluminumOptional;
      }

      Log.Error($"CorePanda:: Error getting {stringReq.ToString()} string for {oreReq.ToString()}.");
      return "null";
    }
  }
}
