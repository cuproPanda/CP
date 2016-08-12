using RimWorld;
using Verse;

namespace CorePanda {

  internal class MinifiedBarrel : MinifiedThing {

    public override Graphic Graphic {
      get {
        return GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/RainBarrel/RainBarrel_Sealed");
      }
    }
  }
}
