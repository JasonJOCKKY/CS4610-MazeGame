using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  private CreateMap _mazeCreator;
  // Start is called before the first frame update
  void Start()
  {
    _mazeCreator = GetComponent<CreateMap>();
    _mazeCreator.Creat();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
