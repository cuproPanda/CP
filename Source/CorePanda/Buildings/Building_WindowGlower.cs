using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Simulates light coming in through a window
  /// </summary>
  public class Building_WindowGlower : Building {

    public IntVec3 windowPos;
    public IntVec3 outside;

    private CompGlower glowComp;

    private int tickRares = 0;
    private float cachedWindowViewBeauty = 0f;
    private WindowGlow cachedWindowGlow;

    private WindowManager mgr = Find.Map.GetComponent<WindowManager>();

    public float WindowViewBeauty {
      get {
        if (cachedWindowViewBeauty == 0f) {
          GetWindowViewBeauty();
        }
        return cachedWindowViewBeauty;
      }
    }


    /// <summary></summary>
    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref windowPos, "CP_WindowPosition", IntVec3.Invalid);
      Scribe_Values.LookValue(ref outside, "CP_OutsidePosition", IntVec3.Invalid);
      Scribe_Values.LookValue(ref tickRares, "CP_WindowGlowerTickRares", 0);
      Scribe_Values.LookValue(ref cachedWindowViewBeauty, "CP_WindowViewBeauty", 0f);
      Scribe_Values.LookValue(ref cachedWindowGlow, "CP_CachedWindowGlow", new WindowGlow(new ColorInt(1, 1, 2, 0), 1f, 0f));
    }


    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      glowComp = GetComp<CompGlower>();
      cachedWindowGlow = new WindowGlow(new ColorInt(1, 1, 2, 0), 1f, 0f);
      mgr.Register(this);
    }


    /// <summary></summary>
    public override void DeSpawn() {
      mgr.Deregister(this);
      base.DeSpawn();
    }


    /// <summary>
    /// Get the sunlight, find an adjacent window, then update the glow
    /// </summary>
    public override void TickRare() {
      base.TickRare();
      GetAdjacentWindow();

      if (tickRares % 4 == 0) {
        UpdateGlow(cachedWindowGlow);
      }

      if (tickRares == 0 || tickRares % 15 == 0) {
        GetWindowViewBeauty();
      }

      tickRares++;
    }


    /// <summary>
    /// Find an adjacent window
    /// </summary>
    private void GetAdjacentWindow() {
      List<Thing> thingList = windowPos.GetThingList();
      for (int t = 0; t < thingList.Count; t++) {
        if (thingList[t] != null && thingList[t].def == ThingDef.Named("CP_Window")) {
          // If a window was found, do nothing
          return;
        }
      }
      // If a window was not found, destroy this glower
      Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Buildings);
      Find.GlowGrid.DeRegisterGlower(glowComp);
      Destroy();
    }


    private float RoofedBrightnessFactor() {
      List<IntVec3> roofCheck = GetWindowLOS(3);

      int cells = 0;
      int roofs = 0;
      for (int t = 0; t < roofCheck.Count; t++) {
        cells++;
        if (Find.RoofGrid.Roofed(roofCheck[t])) {
          roofs++;
        }
      }
      return (float)(cells - roofs) / cells;
    }


    /// <summary></summary>
    public void UpdateGlow(WindowGlow glow) {
      cachedWindowGlow = glow;

      // Update the CompGlower
      Find.GlowGrid.DeRegisterGlower(glowComp);

      float factor = RoofedBrightnessFactor();

      glowComp.Props.glowColor = glow.color * factor;
      glowComp.Props.glowRadius = glow.radius * factor;
      StatExtension.SetStatBaseValue(def, StatDefOf.Beauty, glow.beauty * factor);
      if (glow.overlit) {
        glowComp.Props.overlightRadius = 2.2f * factor;
      }
      if (!glow.overlit) {
        glowComp.Props.overlightRadius = 0f;
      }

      Find.MapDrawer.MapMeshDirty(Position, MapMeshFlag.Buildings);
      Find.GlowGrid.RegisterGlower(glowComp);
    }


    // This allows for seeing around walls/buildings
    // that would normally not allow seeing around.
    private List<IntVec3> GetWindowLOS(int range = 15) {
      List<IntVec3> tempCells = new List<IntVec3>();
      List<IntVec3> cellsToSearch = new List<IntVec3>();

      // Get first 3 cells
      tempCells.Add(outside);
      tempCells.Add(outside + IntVec3.East.RotatedBy(Rotation));
      tempCells.Add(outside + IntVec3.West.RotatedBy(Rotation));

      // Iterate over the remaining cells
      int x = 2;
      int z = 1;
      while (z <= range) {
        // Add the first cell moving forward
        IntVec3 forward = new IntVec3(0, 0, z).RotatedBy(Rotation);
        tempCells.Add(outside + forward);

        // Add the cells to the left and right
        for (int lt = 1; lt <= x; lt++) {
          IntVec3 left = new IntVec3(-lt, 0, 0).RotatedBy(Rotation);
          tempCells.Add(outside + forward + left);
        }
        for (int rt = 1; rt <= x; rt++) {
          IntVec3 right = new IntVec3(rt, 0, 0).RotatedBy(Rotation);
          tempCells.Add(outside + forward + right);
        }

        // Until 60% the range is reached, expand the width by 1
        if (z < Mathf.FloorToInt(range * 0.6f)) {
          x += 1; 
        }
        // Once more than 60% the range is reached, reduce the width by 1
        if (z > Mathf.FloorToInt(range * 0.6f)) {
          x -= 1;
        }
        // Each time, move forward by 1 until the range is reached
        z++;
      }

      // Only add relevant cells
      // This is done after the previous step to save if statements
      for (int i = 0; i < tempCells.Count; i++) {
        Building edifice = tempCells[i].GetEdifice();
        if (tempCells[i].GetRoom() != null && tempCells[i].GetRoom() == outside.GetRoom() && 
             (edifice == null || (edifice.def != ThingDefOf.Wall && !edifice.def.building.isNaturalRock))) {
          cellsToSearch.Add(tempCells[i]);
        }
      }

      return cellsToSearch;
    }


    private void GetWindowViewBeauty() {
      List<IntVec3> windowView = GetWindowLOS();
      float beauty = 0f;

      for (int t = 0; t < windowView.Count; t++) {
        beauty += BeautyUtility.CellBeauty(windowView[t]);
      }

      cachedWindowViewBeauty = beauty / windowView.Count;
    }
  }
}
