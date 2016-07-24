namespace CorePanda {
  /// <summary> The height this object is on a wall, used for conflicts </summary>
  public enum WallHeight {
    /// <summary> Data is missing </summary>
    None,
    /// <summary> Interferes with vents </summary>
    Low,
    /// <summary> Interferes with windows </summary>
    High
  }
}
