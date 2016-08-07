using Verse;

namespace CorePanda {
  /// <summary>
  /// Holds data for a window's brightness color, radius, beauty, and if it has an overlight
  /// </summary>
  public struct WindowGlow {

    public ColorInt color;
    public float radius;
    public float beauty;
    public bool overlit;


    public WindowGlow(ColorInt color, float radius, float beauty, bool overlit = false) {
      this.color = color;
      this.radius = radius;
      this.beauty = beauty;
      this.overlit = overlit;
    }
  }
}
