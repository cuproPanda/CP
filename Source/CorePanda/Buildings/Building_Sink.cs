using System.Collections.Generic;

using RimWorld;
using Verse;

namespace CorePanda {
  //  The sink building. Boosts room cleanliness and add a washed thought to colonists within range
  internal class Building_Sink : Building {

    private CompRefuelable refuelableComp;
    private Room room;
    private int probableCleanliness;


    public override void SpawnSetup() {
      base.SpawnSetup();
      refuelableComp = GetComp<CompRefuelable>();
      probableCleanliness = 0;
      // Find the current room
      room = RoomQuery.RoomAt(Position);
    }


    public override void Tick() {
      base.Tick();

      // Find the current room. This prevents issues when rooms have changed
      room = RoomQuery.RoomAt(Position);

      // If there isn't any fresh water in the sink, it doesn't provide cleanliness
      if (!refuelableComp.HasFuel && probableCleanliness == 50) {
        def.SetStatBaseValue(StatDefOf.Cleanliness, 0f);
        // Force the room to recheck its stats
        room.Notify_TerrainChanged();
        room.GetStatScoreStage(RoomStatDefOf.Cleanliness);
        probableCleanliness = 0;
      }

      // If the sink has fresh water, it provides cleanliness
      if (refuelableComp.HasFuel) {
        if (probableCleanliness == 0) {
          def.SetStatBaseValue(StatDefOf.Cleanliness, 50f);
          room.Notify_TerrainChanged();
          room.GetStatScoreStage(RoomStatDefOf.Cleanliness);
          probableCleanliness = 50;
        }

        // Every TickLong, try to give a pawn a cleaned thought
        if (Find.TickManager.TicksGame % 2000 == 0) {
          ImplyWashedAtSink();
        }
      }
    }


    // Finds a pawn and tries to give the WashedInSink thought
    public void ImplyWashedAtSink() {
      IntVec3 sink = Position;
      Region region = sink.GetRegion();
      float radius = 8f;
      if (region == null) {
        return;
      }
      RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.portal == null || r.portal.Open, delegate (Region r) {
        List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
        for (int i = 0; i < list.Count; i++) {
          Pawn pawn = list[i] as Pawn;
          if (pawn.Position.InHorDistOf(sink, radius) && pawn.needs.mood != null &&
              pawn.RaceProps.Humanlike && (pawn.IsColonist || pawn.IsPrisonerOfColony) && pawn.Awake()) {
            pawn.needs.mood.thoughts.memories.TryGainMemoryThought(ThoughtDef.Named("CP_WashedInSink"));
          }
        }
        return false;
      }, 9);
    }
  }
}
