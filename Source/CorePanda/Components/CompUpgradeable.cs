using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Allows buildings to be upgraded
  /// </summary>
  public class CompUpgradeable : ThingComp {

    /// <summary></summary>
    public CompProperties_Upgradeable Props {
      get { return (CompProperties_Upgradeable)props; }
    }


    /// <summary>
    /// Add a button for upgrading this building
    /// </summary>
    public override IEnumerable<Command> CompGetGizmosExtra() {

      if (Props.researchString == null || (Props.researchString != null && ResearchProjectDef.Named(Props.researchString).IsFinished)) {
        Command_Action upgrade = new Command_Action() {
          icon = ContentFinder<Texture2D>.Get(Props.upgradeTex, false),
          defaultDesc = Props.upgradeDescString,
          defaultLabel = Props.upgradeLabelString,
          activateSound = SoundDef.Named("Click"),
          action = () => { HandlePopup(); },
        };
        yield return upgrade;
      }
      yield break;
    }


    /// <summary>
    /// Creates a popup dialog, letting the player confirm the choice to upgrade
    /// </summary>
    public virtual void HandlePopup() {

      ThingDef BP = ThingDef.Named(Props.blueprintThingDef);

      StringBuilder stringBuilder = new StringBuilder();
      if (BP.costStuffCount != -1) {
        stringBuilder.AppendLine(parent.Stuff.LabelCap + ": " + BP.costStuffCount); 
      }
      if (BP.costList != null) {
        for (int c = 0; c < BP.costList.Count; c++) {
          stringBuilder.AppendLine(BP.costList.ElementAt(c).thingDef.LabelCap + ": " + BP.costList.ElementAt(c).count);
        } 
      }

      string text = "CP_UpgradeConfirmationDialog".Translate(new object[]
      {
        parent.def.LabelCap,
        BP.LabelCap,
        stringBuilder,
        "CP_UpgradeChoiceCancel".Translate()
      });
      DiaNode diaNode = new DiaNode(text);
      DiaOption diaOption = new DiaOption("CP_UpgradeChoiceAccept".Translate());
      diaOption.action = delegate {
        DestroyAndSetBlueprint(Props.blueprintThingDef);
      };
      diaOption.resolveTree = true;
      diaNode.options.Add(diaOption);

      string text2 = "CP_UpgradeChoiceCancel".Translate();
      DiaOption diaOption2 = new DiaOption(text2);
      diaOption2.resolveTree = true;
      diaNode.options.Add(diaOption2);

      Find.WindowStack.Add(new Dialog_NodeTree(diaNode));
    }


    /// <summary>
    /// Destroys the current building, then sets the blueprint for the upgrade
    /// </summary>
    public virtual void DestroyAndSetBlueprint(string TDef) {
      parent.Destroy();
      GenConstruct.PlaceBlueprintForBuild(ThingDef.Named(TDef), parent.Position, parent.Rotation, Faction.OfPlayer, parent.Stuff);
    }
  }
}
