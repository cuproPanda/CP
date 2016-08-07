using System.Collections.Generic;

using RimWorld;
using Verse;

namespace CorePanda {

  internal class Bill_Well : Bill_Production {

    // Setup constructors
    public Bill_Well() {
    }
    public Bill_Well(RecipeDef recipe) : base(recipe)
		{
    }


    public override bool ShouldDoNow() {
      // Setup base checks
      if (suspended) {
        return false;
      }
      if (repeatMode == BillRepeatMode.RepeatCount && repeatCount <= 0) {
        return false;
      }
      if (repeatMode == BillRepeatMode.TargetCount && (recipe.WorkerCounter.CountProducts(this) > targetCount)) {
        return false;
      }

      // Save the bill giver as Building_Well to access its properties
      Building_Well well = billStack.billGiver as Building_Well;

      if (well != null && well.CanWorkHere) {
        return true;
      }

      // Otherwise, return false
      return false;
    }


    public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients) {
      Building_Well well = billStack.billGiver as Building_Well;

      // Tell the well to reduce its water level
      if (well != null) {
        well.Notify_IterationCompleted();
      }

      // Do the base steps
      base.Notify_IterationCompleted(billDoer, ingredients);
    }
  }
}
