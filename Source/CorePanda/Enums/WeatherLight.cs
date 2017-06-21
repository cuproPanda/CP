namespace CorePanda {

  public enum WeatherLight {
    // Data has not been processed yet
    None = 0,
    // Weather is not affecting the sunlight at all
    Bright = 1,
    // Weather is blocking a little bit of sunlight
    Darkened = 2,
    //Weather is blocking a lot of sunlight 
    Dark = 4
  }
}
