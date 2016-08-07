using RimWorld;
using Verse;
using Verse.AI;

namespace CorePanda {
  /// <summary> Allow pawns to work at the well </summary>
  public class WorkGiver_DoBillWell : WorkGiver_Scanner {

    /// <summary> Request a well </summary>
    public override ThingRequest PotentialWorkThingRequest {
      get {
        return ThingRequest.ForDef(ThingDef.Named("CP_Well"));
      }
    }

    /// <summary> Set the path end to the interaction cell </summary>
    public override PathEndMode PathEndMode {
      get {
        return PathEndMode.InteractionCell;
      }
    }


    /// <summary> Make sure the pawn can work here </summary>
    public override bool HasJobOnThing(Pawn pawn, Thing t) {
      return CanGetWater(pawn, t);
    }


    private bool CanGetWater(Pawn pawn, Thing t) {
      // Save Thing t as Building_Well to access its properties
      Building_Well well = t as Building_Well;

      // Make sure the worker can interact with the well
      if (well == null || well.IsForbidden(pawn) || !pawn.CanReserveAndReach(well, PathEndMode, pawn.NormalMaxDanger())) {
        return false;
      }

      // Make sure the well has enough water
      if (well.CanWorkHere) {
        return true;
      }

      // Otherwise, return false
      return false;
    }


    /// <summary> Setup the job to use </summary>
    public override Job JobOnThing(Pawn pawn, Thing t) {
      return new Job(DefDatabase<JobDef>.GetNamed("CP_Job_GatherWater"), t);
    }
  }
}
