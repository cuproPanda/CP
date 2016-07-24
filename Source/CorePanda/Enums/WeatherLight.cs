namespace CorePanda {
  /// <summary>
  /// Shows how much the weather is affecting perceived sunlight levels
  /// </summary>
  public enum WeatherLight {
    /// <summary> Weather is not affecting the sunlight at all </summary>
    Bright,
    /// <summary> Weather is blocking a little bit of sunlight </summary>
    Darkened,
    /// <summary> Weather is blocking a lot of sunlight </summary>
    Dark,
    /// <summary> Data has not been processed yet </summary>
    None
  }
}
