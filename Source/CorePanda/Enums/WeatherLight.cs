namespace CorePanda {
  /// <summary>
  /// Shows how much the weather is affecting perceived sunlight levels
  /// </summary>
  public enum WeatherLight {
    /// <summary> Data has not been processed yet </summary>
    None = 0,
    /// <summary> Weather is not affecting the sunlight at all </summary>
    Bright = 1,
    /// <summary> Weather is blocking a little bit of sunlight </summary>
    Darkened = 2,
    /// <summary> Weather is blocking a lot of sunlight </summary>
    Dark = 4
  }
}
