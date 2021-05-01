using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public Camera miniMapCamera;
  private CreateMap _mazeCreator;
  // Start is called before the first frame update
  void Start()
  {
    _mazeCreator = GetComponent<CreateMap>();
    _mazeCreator.Creat();
    miniMapCamera.gameObject.transform.position = new Vector3(_mazeCreator.X / 2 - 0.5f, 40, _mazeCreator.Y / 2 - 0.5f);
    miniMapCamera.orthographicSize = _mazeCreator.X / 2;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
