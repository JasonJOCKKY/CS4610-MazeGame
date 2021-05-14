using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFog : MonoBehaviour
{
  public MeshRenderer fogRenderer;

  private Floor _floor;

  void Start()
  {
    _floor = GetComponent<Floor>();
  }

  // Called by player to clear the fog
  public void ClearFog()
  {
    DisableFog();
    for (int i = 0; i < _floor.neighborFloors.Length; i++)
    {
      if (_floor.neighborFloors[i] != null && _floor.neighborWalls[i] == null)
      {
        _floor.neighborFloors[i].GetComponent<FloorFog>().DisableFog();
      }
    }
  }

  // Should only be called by self or other FloorFog instance
  public void DisableFog()
  {
    fogRenderer.enabled = false;
  }

}
