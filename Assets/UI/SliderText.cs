using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
  public Slider slider;
  private Text _text;
  // Start is called before the first frame update
  void Start()
  {
    _text = GetComponent<Text>();
    slider.onValueChanged.AddListener(changeValue);
  }

  private void changeValue(float newValue)
  {
    _text.text = ((int)newValue).ToString();
  }
}
