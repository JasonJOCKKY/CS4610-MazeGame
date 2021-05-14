using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMarker : MonoBehaviour
{
  public Vector3 targetPosition;
  public float minDistance = 2;

  private float _minX, _maxX;
  private float _minY, _maxY;

  void Start()
  {
    Image img = GetComponent<Image>();
    _minX = img.GetPixelAdjustedRect().width / 2;
    _maxX = Screen.width - _minX;
    _minY = img.GetPixelAdjustedRect().height / 2;
    _maxY = Screen.height - _minY;
  }

  // Update is called once per frame
  void Update()
  {
    GetComponent<Image>().enabled = Vector3.Distance(Camera.main.transform.position, targetPosition) > minDistance;
    if (Vector3.Dot(targetPosition - Camera.main.transform.position, Camera.main.transform.forward) > 0)
    {
      Vector3 newPosition = Camera.main.WorldToScreenPoint(targetPosition);
      // newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
      // newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);
      transform.position = newPosition;
    }
  }

  public void SetTargetPosition(Vector3 newTargetPosition)
  {
    targetPosition = newTargetPosition;
  }
}
