using System;
using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Adds comps to the vent
  /// </summary>
  public class Building_Vent : RimWorld.Building_Vent {

    private bool ventOpen = true;   // Is the vent open?
    private List<ThingComp> comps = new List<ThingComp>();

    private Texture2D tex {
      get {
        if (ventOpen) {
          return ContentFinder<Texture2D>.Get("Cupro/UI/Designators/VentOpen", false);
        }
        else {
          return ContentFinder<Texture2D>.Get("Cupro/UI/Designators/VentClosed", false);
        }
      }
    }


    /// <summary></summary>
    public override void SpawnSetup() {
      base.SpawnSetup();
      InitializeComps();
    }


    /// <summary> Add a button for opening and closing the vent </summary>
    public override IEnumerable<Gizmo> GetGizmos() {
      Command_Toggle ventStatus = new Command_Toggle() {

        icon = tex,
        defaultDesc = "CP_ToggleVent".Translate(),
        hotKey = KeyBindingDefOf.CommandTogglePower,
        activateSound = SoundDef.Named("Click"),
        isActive = () => ventOpen,
        toggleAction = () => { ventOpen = !ventOpen; },
      };
      yield return ventStatus;

      if (base.GetGizmos() != null) {
        foreach (Command c in base.GetGizmos()) {
          yield return c;
        }
      }
    }


    /// <summary> Add all the comps listed in xml </summary>
    public void InitializeComps() {
      for (int ic = 0; ic < def.comps.Count; ic++) {
        ThingComp thingComp = (ThingComp)Activator.CreateInstance(def.comps[ic].compClass);
        thingComp.parent = this;
        comps.Add(thingComp);
        thingComp.Initialize(def.comps[ic]);
      }
    }


    /// <summary> Equalize temperature and tick each comp </summary>
    public override void TickRare() {

      if (ventOpen) {
        GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f); 
      }

      for (int c = 0; c < comps.Count; c++) {
        comps[c].CompTickRare();
      }
    }


    /// <summary> Handle loading data </summary>
    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.LookValue(ref ventOpen, "ventOpen", true);
    }
  }
}
