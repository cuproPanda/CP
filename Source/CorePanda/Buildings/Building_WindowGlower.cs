using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Simulates light coming in through a window
  /// </summary>
  public class Building_WindowGlower : Building {
    // Comps used
    private CompGlower glowComp;
    private CompSunlight sunlightComp;

    // The color/brightness of the light. Used for more realistic light
    private ColorInt brightness;
    // The current radius of light, used for twilight
    private float radius;
    // The beauty value to set, based off incoming light 
    private float beauty;

    private List<IntVec3> cachedAdjCellsCardinal;

    private List<IntVec3> AdjCellsCardinalInBounds {
      get {
        if (cachedAdjCellsCardinal == null) {
          cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
                                    where c.InBounds()
                                    select c).ToList();
        }
        return cachedAdjCellsCardinal;
      }
    }


    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      glowComp = GetComp<CompGlower>();
      sunlightComp = GetComp<CompSunlight>();
      UpdateGlow();
    }


    /// <summary>
    /// Get the sunlight, find an adjacent window, then update the glow
    /// </summary>
    public override void TickRare() {
      base.TickRare();
      GetAdjacentWindow();
      UpdateGlow();
    }


    /// <summary>
    /// Find an adjacent window
    /// </summary>
    private void GetAdjacentWindow() {
      for (int i = 0; i < AdjCellsCardinalInBounds.Count; i++) {
        IntVec3 c = AdjCellsCardinalInBounds[i];
        List<Thing> thingList = c.GetThingList();
        for (int t = 0; t < thingList.Count; t++) {
          if (thingList[t] != null && thingList[t].def == ThingDef.Named("CP_Window")) {
            // If a window was found, do nothing
            return;
          }
        }
      }
      // If a window was not found, destroy this glower
      Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Buildings);
      Find.GlowGrid.DeRegisterGlower(glowComp);
      Destroy();
    }


    /// <summary></summary>
    private void UpdateGlow() {
      // Set the stats based on the current sunlight
      if (sunlightComp.FactoredSunlight >= 0.9f) {
        brightness = new ColorInt(175, 175, 165, 0);
        glowComp.Props.overlightRadius = 2.2f;
        radius = 8f;
        beauty = 25f;
      }
      else if (sunlightComp.FactoredSunlight >= 0.72f && sunlightComp.FactoredSunlight < 0.90f) {
        brightness = new ColorInt(150, 150, 140, 0);
        radius = 8f;
        beauty = 20f;
      }
      else if (sunlightComp.FactoredSunlight >= 0.54f && sunlightComp.FactoredSunlight < 0.72f) {
        brightness = new ColorInt(125, 125, 120, 0);
        glowComp.Props.overlightRadius = 0f;
        radius = 8f;
        beauty = 15f;
      }
      else if (sunlightComp.FactoredSunlight >= 0.36f && sunlightComp.FactoredSunlight < 0.54f) {
        brightness = new ColorInt(105, 105, 100, 0);
        glowComp.Props.overlightRadius = 0f;
        radius = 6f;
        beauty = 10f;
      }
      else if (sunlightComp.FactoredSunlight >= 0.18f && sunlightComp.FactoredSunlight < 0.36f) {
        brightness = new ColorInt(80, 80, 95, 0);
        glowComp.Props.overlightRadius = 0f;
        radius = 4f;
        beauty = 5f;
        ;
      }
      else if (sunlightComp.FactoredSunlight >= 0.05f && sunlightComp.FactoredSunlight < 0.18f) {
        brightness = new ColorInt(60, 53, 75, 0);
        glowComp.Props.overlightRadius = 0f;
        radius = 3f;
        beauty = 1f;
      }
      else {
        brightness = new ColorInt(0, 0, 0, 0);
        glowComp.Props.overlightRadius = 0f;
        radius = 1f;
        beauty = 0f;
      }

      // Update the CompGlower
      Find.GlowGrid.DeRegisterGlower(glowComp);
      glowComp.Props.glowRadius = radius;
      glowComp.Props.glowColor = brightness;
      def.SetStatBaseValue(StatDefOf.Beauty, beauty);
      Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Buildings);
      Find.GlowGrid.RegisterGlower(glowComp);
    }
  }
}
