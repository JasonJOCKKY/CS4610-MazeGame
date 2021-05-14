using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
  public Slider difficultySlider, sizeSlider;
  public GameController gameController;

  public void ApplyAndReatart()
  {
    gameController.RestartGame((int)sizeSlider.value, (int)difficultySlider.value);
  }
}
