using UnityEngine;
using System;
using System.Collections.Generic;

public class CreateMap : MonoBehaviour
{
  public static CreateMap Instance;
  private void Awake()
  {
    Instance = this;
  }
  public int level;//level of difficulty
  
  //mapSize
  public int X = 5;
  public int Y = 5;

  public GameObject groundPrefab;
  public GameObject wallPrefab;
  public GameObject wallPrefab1;

  public Floor[,] cubes;
  public Wall[,] walls1;
  public Wall[,] walls2;

  //start poinit
  public Vector2 startPoint;
  public bool isTest;


  public List<Floor> cantMoves = new List<Floor>();
  
  public void Creat()
  {
    Clear();
    Init();
    CreateStep1();
    CreateStep2();
  }

  public void Clear()
  {
    if (cubes != null)
    {
      foreach (var item in cubes)
      {
        if (item != null)
        {
          Destroy(item.gameObject);
        }
      }
    }
    if (walls1 != null)
    {
      foreach (var item in walls1)
      {
        if (item != null)
        {
          Destroy(item.gameObject);
        }
      }
    }
    if (walls2 != null)
    {
      foreach (var item in walls2)
      {
        if (item != null)
        {
          Destroy(item.gameObject);
        }
      }
    }
  }
  public void Init()
  {
    cubes = new Floor[X, Y];
    walls1 = new Wall[X, Y + 1];
    walls2 = new Wall[X + 1, Y];
    CreateGround();
    CreateWall1();
    CreateWall2();
    foreach (var item in cubes)
    {
      item.neighborFloors = new Floor[4];
      item.neighborWalls = new Wall[4];
      item.GetNeighborFloors();
      item.GetNeighborWalls();
    }
  }
  public void CreateGround()
  {
    for (int i = 0; i < X; i++)
    {
      for (int j = 0; j < Y; j++)
      {
        Floor item = Instantiate(groundPrefab).GetComponent<Floor>();
        item.transform.position = new Vector3(i, 0, j);
        cubes[i, j] = item;
        item.index = new Vector2(i, j);
      }
    }
  }
  public void CreateWall1()
  {
    for (int i = 0; i < X; i++)
    {
      for (int j = 0; j < Y + 1; j++)
      {

        Wall item = Instantiate(wallPrefab).GetComponent<Wall>();
        item.transform.position = new Vector3(i, 0.5f, j - 0.5f);
        walls1[i, j] = item;
        if (j == 0 || j == Y)
        {
          item.isBorder = true;
        }
      }
    }
  }
  public void CreateWall2()
  {
    for (int i = 0; i < X + 1; i++)
    {
      for (int j = 0; j < Y; j++)
      {
        Wall item = Instantiate(wallPrefab1).GetComponent<Wall>();
        item.transform.position = new Vector3(i - 0.5f, 0.5f, j);
        walls2[i, j] = item;
        if (i == X || i == 0)
        {
          item.isBorder = true;
        }
      }
    }
  }
  public void CreateStep1()
  {

    List<Vector2> road = cubes[(int)startPoint.x, (int)startPoint.y].
        SetRoad((int)startPoint.x, (int)startPoint.y, new List<Vector2>());
    // This is the test pharse to find the best rouate, delete when finish
    foreach (var item in road)
    {
      if (isTest)
      {
        cubes[(int)item.x, (int)item.y].gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
      }

    }

    for (int i = 0; i < road.Count - 1; i++)
    {
      DestoryWall(road[i], road[i + 1]);
      if (i == 0)
      {
        Destroy(cubes[(int)road[i].x, (int)road[i].y].neighborWalls[1].gameObject);
        cubes[(int)road[i].x, (int)road[i].y].neighborWalls[1] = null;
      }
      if (i == road.Count - 2)
      {
        Destroy(cubes[(int)road[i + 1].x, (int)road[i + 1].y].neighborWalls[0].gameObject);
        cubes[(int)road[i + 1].x, (int)road[i + 1].y].neighborWalls[0] = null;
      }
    }
    DigWall();

    for (int i = 0; i < X; i++)
    {
      for (int j = 0; j < Y; j++)
      {
        cantMoves.Add(cubes[i, j]);
      }
    }
  }
  public void CreateStep2()
  {
    while (cantMoves.Count > 0)
    {
      for (int i = 0; i < 5; i++)
      {
        cantMoves = SelectCantMove();
      }
      Level();

    }
  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Clear();
      Init();
      CreateStep1();
    }

    if (Input.GetKeyDown(KeyCode.A))
    {
      CreateStep2();
    }
    if (Input.GetKeyDown(KeyCode.C))
    {
      Creat();
    }
  }

  public bool NeighborCanMove(Floor item)
  {
    for (int i = 0; i < 4; i++)
    {
      if (item.neighborFloors[i] != null && item.neighborFloors[i].CantMove)
      {
        return true;
      }
    }
    return false;
  }
  /// <summary>
  /// </summary>
  public void DigWall()
  {
    foreach (var item in cubes)
    {
      bool isFull = true;
      for (int i = 0; i < 4; i++)
      {
        if (item.neighborWalls[i] == null)
        {
          isFull = false;
        }
      }
      if (isFull)
      {
        List<Floor> roadNeighbor = new List<Floor>();
        foreach (var cube in item.neighborFloors)
        {
          if (cube != null && cube.CantMove == true)
          {
            roadNeighbor.Add(cube);
          }
        }
        if (roadNeighbor.Count > 0)
        {
          Floor cubeItem = roadNeighbor[UnityEngine.Random.Range(0, roadNeighbor.Count)];
          DestoryWall(item.index, cubeItem.index);
        }
        else
        {
          List<Wall> walls = new List<Wall>();
          foreach (var wall in item.neighborWalls)
          {
            if (!wall.isBorder)
            {
              walls.Add(wall);
            }
          }
          int dir = UnityEngine.Random.Range(0, walls.Count);
          for (int i = 0; i < 4; i++)
          {
            if (item.neighborWalls[i] != null && item.neighborWalls[i] == walls[dir])
            {
              dir = i;
              break;
            }
          }
          switch (dir)
          {
            case 0:
              Destroy(item.neighborWalls[0].gameObject);
              item.neighborWalls[0] = null;
              if (item.neighborFloors[0] != null)
                item.neighborFloors[0].neighborWalls[1] = null;
              break;
            case 1:
              Destroy(item.neighborWalls[1].gameObject);
              item.neighborWalls[1] = null;
              if (item.neighborFloors[1] != null)
                item.neighborFloors[1].neighborWalls[0] = null;
              break;
            case 2:
              Destroy(item.neighborWalls[2].gameObject);
              item.neighborWalls[2] = null;
              if (item.neighborFloors[2] != null)
                item.neighborFloors[2].neighborWalls[3] = null;
              break;
            case 3:
              Destroy(item.neighborWalls[3].gameObject);
              item.neighborWalls[3] = null;
              if (item.neighborFloors[3] != null)
                item.neighborFloors[3].neighborWalls[2] = null;
              break;
            default:
              break;
          }
        }
      }
    }
  }
  public void DestoryWall(Vector2 front, Vector2 back)
  {
    if (Mathf.Abs(front.x - back.x) > 0.1)
    {
      if (front.x - back.x > 0.1)
      {
        Destroy(cubes[(int)front.x, (int)front.y].neighborWalls[2].gameObject);
        cubes[(int)front.x, (int)front.y].neighborWalls[2] = null;
        cubes[(int)back.x, (int)back.y].neighborWalls[3] = null;

      }
      else
      {
        Destroy(cubes[(int)front.x, (int)front.y].neighborWalls[3].gameObject);
        cubes[(int)front.x, (int)front.y].neighborWalls[3] = null;
        cubes[(int)back.x, (int)back.y].neighborWalls[2] = null;
      }
    }
    else
    {
      if (front.y - back.y > 0.1)
      {
        Destroy(cubes[(int)front.x, (int)front.y].neighborWalls[1].gameObject);
        cubes[(int)front.x, (int)front.y].neighborWalls[1] = null;
        cubes[(int)back.x, (int)back.y].neighborWalls[0] = null;

      }
      else
      {

        Destroy(cubes[(int)front.x, (int)front.y].neighborWalls[0].gameObject);
        cubes[(int)front.x, (int)front.y].neighborWalls[0] = null;
        cubes[(int)back.x, (int)back.y].neighborWalls[1] = null;
      }
    }
  }
  public List<Floor> SelectCantMove()
  {
    List<Floor> cantMoveCubes = new List<Floor>();
    for (int i = 0; i < cantMoves.Count; i++)
    {
      if (cantMoves[i].CantMove)
      {
        continue;
      }
      bool canMove = false;
      for (int j = 0; j < 4; j++)
      {
        if (cantMoves[i].neighborWalls[j] == null && cantMoves[i].neighborFloors[j] != null && cantMoves[i].neighborFloors[j].CantMove)
        {
          canMove = true;
          cantMoves[i].CantMove = true;
          if (isTest)
          {
            cantMoves[i].GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
          }
          break;
        }
      }
      if (!canMove)
      {
        cantMoveCubes.Add(cantMoves[i]);
      }
    }
    Debug.Log("cantMoveCubes.Count:" + cantMoveCubes.Count);
    return cantMoveCubes;

  }

  public void Level()
  {
    if (level <= 0)
    {
      level = 1;
    }
    if (level >= 50)
    {
      level = 50;
    }

    for (int i = 0; i < level; i++)
    {
      if (cantMoves.Count > 0)
      {
        Floor item = null;
        while (true)
        {
          int index1 = UnityEngine.Random.Range(0, cantMoves.Count);
          item = cantMoves[index1];
          if (NeighborCanMove(item))
          {
            break;
          }
        };

        List<int> dirs = new List<int>();
        for (int k = 0; k < 4; k++)
        {
          if (item.neighborWalls[k] != null && !item.neighborWalls[k].isBorder)
          {
            dirs.Add(k);
          }
        }
        if (dirs.Count > 0)
        {
          Debug.Log(55);
          int index = UnityEngine.Random.Range(0, dirs.Count);
          Destroy(item.neighborWalls[dirs[index]].gameObject);
          item.neighborWalls[dirs[index]] = null;
          switch (dirs[index])
          {
            case 0:
              item.neighborFloors[dirs[index]].neighborWalls[1] = null;
              break;
            case 1:
              item.neighborFloors[dirs[index]].neighborWalls[0] = null;
              break;
            case 2:
              item.neighborFloors[dirs[index]].neighborWalls[3] = null;
              break;
            case 3:
              item.neighborFloors[dirs[index]].neighborWalls[2] = null;
              break;
            default:
              break;
          }

        }

        //for (int j = 0; j < 5; j++)
        //{
        //    cantMoves = SelectCantMove();
        //}
      }


    }

  }

}
