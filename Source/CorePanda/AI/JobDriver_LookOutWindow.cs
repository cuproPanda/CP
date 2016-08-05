using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;

namespace CorePanda {

  internal class JobDriver_LookOutWindow : JobDriver_WatchBuilding {

    // Get adjacent cells
    private List<IntVec3> cachedAdjCellsCardinal;
    private List<IntVec3> AdjCellsCardinalInBounds {
      get {
        if (cachedAdjCellsCardinal == null) {
          cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(TargetA.Thing)
                                    where c.InBounds()
                                    select c).ToList();
        }
        return cachedAdjCellsCardinal;
      }
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
            // If a window was found, save it
            TargetThingB = thingList[t];
          }
        }
      }
    }

    protected override IEnumerable<Toil> MakeNewToils() {

      Pawn actor = GetActor();

      // Get an adjacent window first, so there is something to face
      GetAdjacentWindow();

      // TargetIndex.A is the window glower
      // TargetIndex.B is the window

      Building_WindowGlower glower = TargetA.Thing as Building_WindowGlower;

      // Set fail conditions
      this.FailOnDespawnedOrNull(TargetIndex.A);
      this.FailOnDestroyedOrNull(TargetIndex.B);

      // Reserve the window glower
      yield return Toils_Reserve.Reserve(TargetIndex.A);

      // Go to the window
      yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);

      // Look out the window
      Toil low = new Toil();
      JoyKindDef joyKind = DefDatabase<JobDef>.GetNamed("CP_Job_LookOutWindow").joyKind;
      low.socialMode = RandomSocialMode.Normal;
      low.tickAction = () => {
        base.WatchTickAction();
        if (glower != null) {
          actor.needs.joy.GainJoy(Mathf.Max(glower.WindowViewBeauty / 5, 0f) * 0.000144f, joyKind);
        }
        actor.Drawer.rotator.FaceCell(TargetB.Cell);
      };
      low.defaultCompleteMode = ToilCompleteMode.Delay;
      low.defaultDuration = CurJob.def.joyDuration;
      low.AddFinishAction(() => {
        Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDef.Named("CP_LookedOutWindowRegular"));
        Thought cabinFever = actor.needs.mood.thoughts.Thoughts.Find((Thought t) => t.def == ThoughtDef.Named("CabinFever"));

        if (cabinFever != null) {
          thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDef.Named("CP_LookedOutWindowCabinFever"));
          
          if (cabinFever.CurStageIndex == 1) {
            thought_Memory.moodPowerFactor = 1.5f;
          }
        }

        pawn.needs.mood.thoughts.memories.TryGainMemoryThought(thought_Memory);
      });
      yield return low;
    }


    public override string GetReport() {
      if (Find.MapConditionManager.ConditionIsActive(MapConditionDefOf.ToxicFallout)) {
        return "CP_WatchingToxicFallout".Translate();
      }
      if (Find.MapConditionManager.ConditionIsActive(MapConditionDefOf.VolcanicWinter)) {
        return "CP_WatchingVolcanicWinter".Translate();
      }
      float rain = Find.WeatherManager.RainRate;
      if (rain > 0.1f) {
        return "CP_WatchingRain".Translate();
      }
      float snow = Find.WeatherManager.SnowRate;
      if (snow > 0.1f) {
        return "CP_WatchingSnow".Translate();
      }
      return base.GetReport();
    }
  }
}
