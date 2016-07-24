using System;

using RimWorld;
using Verse;
using Verse.AI;

namespace CorePanda {
  /// <summary>
  /// Finds the inside of a nearby window and issues a job to look out it
  /// </summary>
  internal class JoyGiver_LookOutWindow : JoyGiver {


    public override Job TryGiveJob(Pawn pawn) {
      Building_WindowGlower glower;
      // Find the closest window glower, given the following parameters
      Predicate<Thing> validator = (Thing t) => (!t.IsForbidden(pawn)) && t.Position.Standable() && pawn.CanReserve(t, 1);
      glower = GenClosest.ClosestThingReachable(pawn.Position, ThingRequest.ForDef(ThingDef.Named("CP_WindowGlower")), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.None, TraverseMode.ByPawn, false), 25f, validator) as Building_WindowGlower;

      if (glower == null) {
        return null;
      }

      return new Job(def.jobDef, glower);
    }
  }
}
