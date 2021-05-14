using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
  public Text titleText;
  public Slider sizeSlider, difficultySlider;
  public Button cancelButton;

  public void SetTitle(string newTitle)
  {
    titleText.text = newTitle;
  }

  public void SetSliders(int size, int difficulty)
  {
    sizeSlider.value = size;
    difficultySlider.value = difficulty;
  }

  public void SetCancelEnable(bool enable)
  {
    cancelButton.gameObject.SetActive(enable);
  }
}
