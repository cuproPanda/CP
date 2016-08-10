using System.Collections.Generic;

using UnityEngine;
using Verse;
using Verse.AI;

namespace CorePanda {
  
  internal class JobDriver_DoBillWell : JobDriver {

    protected override IEnumerable<Toil> MakeNewToils() {
      // TargetIndex.A is the well

      // Setup references
      Pawn actor = GetActor();
      RecipeDef recipe = DefDatabase<RecipeDef>.GetNamed("CP_GatherWater");
      TargetIndex wellIndex = TargetIndex.A;
      Building_Well well = TargetA.Thing as Building_Well;

      // Set fail conditions
      this.FailOnDespawnedOrNull(TargetIndex.A);

      // Reserve the well
      yield return Toils_Reserve.Reserve(wellIndex);

      // Go to the well
      yield return Toils_Goto.GotoThing(wellIndex, PathEndMode.InteractionCell);

      // Wait before spawning water
      yield return Toils_General.Wait(1000).WithProgressBarToilDelay(wellIndex);

      // Spawn water
      Toil gather = new Toil();
      gather.initAction = () => {
        Thing bucket = ThingMaker.MakeThing(ThingDef.Named("CP_FreshWaterBucket"));
        GenPlace.TryPlaceThing(bucket, actor.Position, ThingPlaceMode.Near);
        well.Notify_IterationCompleted();
        pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
      };
      gather.defaultCompleteMode = ToilCompleteMode.Instant;
      yield return gather;

      // Release the reservation on the well
      yield return Toils_Reserve.Release(wellIndex);
    }
  }
}
