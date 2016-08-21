using UnityEngine;
using Verse;

namespace CorePanda {
  /// <summary>
  /// Pushes coldness
  /// </summary>
  public class CompColdPusher : ThingComp {
    /// <summary>
    /// How often(ticks) this should push coldness
    /// </summary>
    public int ColdPushInterval {
      get { return 60; }
    }

    /// <summary></summary>
    public CompProperties_ColdPusher Props {
      get { return (CompProperties_ColdPusher)props; }
    }

    /// <summary>
    /// Used with CompProperties_ColdPusher.ConstantPush to constantly push coldness
    /// </summary>
    protected virtual bool ShouldPushColdNow {
      get { return Props.constantPush; }
    }


    /// <summary>
    /// This is specifically for Building_SolarChimney
    /// </summary>
    /// <param name="pos">Where to push coldness</param>
    /// <param name="strength">How much coldness to push</param>
    public virtual void Push(IntVec3 pos, float strength) {
      // If the area to push isn't being blocked
      if (!pos.Impassable()) {
        // Get the outdoor temperature
        float tempOutside = GenTemperature.OutdoorTemp;
        // Get the indoor temperature
        float tempInside = pos.GetTemperature();
        // Control the range of num
        float num = tempOutside - tempInside;
        if (tempOutside - 40f > num) {
          num = tempOutside - 40f;
        }
        // Used for temperature lerping
        float num2 = 1f - num * 0.0076923077f;
        if (num2 < 0f) {
          num2 = 0f;
        }
        float num3 = strength * num2 * 4.16666651f;
        float num4 = GenTemperature.ControlTemperatureTempChange(pos, num3, Props.coldPushMinTemperature);

        if (!Mathf.Approximately(num4, 0f)) {
          // Set the temperature
          pos.GetRoom().Temperature += num4 / 600f;
        }
      }
    }


    /// <summary>
    /// Use data defined in the XML to push coldness
    /// </summary>
    /// <param name="tickDivisor">How much to divide by</param>
    public virtual void SimplePush(float tickDivisor = 60f) {
      CompProperties_ColdPusher props = this.Props;
      float temp = GenTemperature.ControlTemperatureTempChange(parent.Position, (Props.coldPerSecond / tickDivisor), Props.coldPushMinTemperature);
      if (!Mathf.Approximately(temp, 0f)) {
        // Set the temperature
        parent.Position.GetRoom().Temperature += temp / 2;
      }
    }


    /// <summary>
    /// Used to constantly push coldness every 60 ticks, if ShouldPushColdNow == true
    /// </summary>
    public override void CompTick() {
      base.CompTick();
      if (this.parent.IsHashIntervalTick(60) && ShouldPushColdNow) {
        CompProperties_ColdPusher props = this.Props;
        if (this.parent.Position.GetTemperature() > props.coldPushMinTemperature) {
          GenTemperature.ControlTemperatureTempChange(this.parent.Position, Props.coldPerSecond, Props.coldPushMinTemperature);
        }
      }
    }
  }
}
