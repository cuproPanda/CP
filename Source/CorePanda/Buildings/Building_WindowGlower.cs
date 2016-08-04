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
      
      //// TODO List:
      // Calculate outside roof -- 4 TickRares
      // Scan for outside beauty based on trees, corpses, walls, etc -- 20 TickRares
      // JoyGiver, JoyGainRate based on beauty
      // TickRareTracker++
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
    public void UpdateGlow(WindowGlow glow) {
      // Update the CompGlower
      Find.GlowGrid.DeRegisterGlower(glowComp);

      if (glow.overlit && glowComp.Props.overlightRadius != 2.2f) {
        glowComp.Props.overlightRadius = 2.2f;
      }
      if (!glow.overlit && glowComp.Props.overlightRadius != 0f) {
        glowComp.Props.overlightRadius = 0f;
      }

      glowComp.Props.glowRadius = glow.radius;
      glowComp.Props.glowColor = glow.color;
      StatExtension.SetStatBaseValue(def, StatDefOf.Beauty, glow.beauty);

      Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Buildings);
      Find.GlowGrid.RegisterGlower(glowComp);
    }
  }
}
