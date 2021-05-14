using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public Camera miniMapCamera;
  public GameObject miniMapIcon;
  public Material entranceMaterial, exitMaterial;
  public WaypointMarker entranceMarker, exitMarker;
  public Vector3 playerDefaultPosition, playerDefaultRotation;
  public GameObject pauseButton;
  public PausePanelController pausePanel;

  private bool _isPaused = false;
  private CreateMap _mazeCreator;
  private AvatarController _player;
  private GameObject _entranceIcon, _exitIcon;

  // Start is called before the first frame update
  void Start()
  {
    _mazeCreator = GetComponent<CreateMap>();
    _player = GameObject.FindGameObjectWithTag("Player").GetComponent<AvatarController>();
    _entranceIcon = Instantiate(miniMapIcon, Vector3.zero, miniMapIcon.transform.rotation);
    _entranceIcon.GetComponent<MeshRenderer>().material = entranceMaterial;
    _exitIcon = Instantiate(miniMapIcon, Vector3.zero, miniMapIcon.transform.rotation);
    _exitIcon.GetComponent<MeshRenderer>().material = exitMaterial;
    initializeGame();
    pausePanel.gameObject.SetActive(false);
    // PauseGame();
  }

  void Update()
  {
    // Pause the game with key press
    if (Input.GetButtonDown("Cancel"))
    {
      if (IsPaused()) ResumeGame();
      else PauseGame();
    }
  }

  private void initializeGame()
  {
    // Reposition minimap camera
    miniMapCamera.gameObject.transform.position = new Vector3(_mazeCreator.X / 2 - 0.5f, 40, _mazeCreator.Y / 2 - 0.5f);
    miniMapCamera.orthographicSize = _mazeCreator.X / 2;

    Maze maze = _mazeCreator.Creat();
    // Entrance Icon & Marker
    Vector3 entranceIconPosition = new Vector3(
      maze.entranceFloor.transform.position.x,
      miniMapIcon.transform.position.y,
      maze.entranceFloor.transform.position.z);
    _entranceIcon.transform.position = entranceIconPosition;
    entranceMarker.SetTargetPosition(maze.entranceWall.transform.position);

    // Exit Icon & Marker
    Vector3 exitIconPosition = new Vector3(
      maze.exitFloor.transform.position.x,
      miniMapIcon.transform.position.y,
      maze.exitFloor.transform.position.z);
    _exitIcon.transform.position = exitIconPosition;
    exitMarker.SetTargetPosition(maze.exitWall.transform.position);

    // Hide Entrance Wall
    maze.entranceWall.gameObject.SetActive(true);
    foreach (var item in maze.entranceWall.GetComponentsInChildren<MeshRenderer>())
    {
      item.enabled = false;
    }
    maze.entranceWall.gameObject.layer = 7; // MazeTrigger Layer

    // Hide Exit Wall
    maze.exitWall.gameObject.SetActive(true);
    foreach (var item in maze.exitWall.GetComponentsInChildren<MeshRenderer>())
    {
      item.enabled = false;
    }
    maze.exitWall.gameObject.layer = 7; // MazeTrigger Layer

    // Set Name for Exit Floor
    maze.exitFloor.name = "Exit Floor";

    // Reset Player
    _player.transform.position = new Vector3(playerDefaultPosition.x, _player.transform.position.y, playerDefaultPosition.z);
    _player.transform.rotation = Quaternion.Euler(playerDefaultRotation.x, playerDefaultRotation.y, playerDefaultRotation.z);
  }

  public void RestartGame(int mazeSize, int difficulty)
  {
    _mazeCreator.Clear();
    // Reconfigure the Maze
    _mazeCreator.X = mazeSize;
    _mazeCreator.Y = mazeSize;
    _mazeCreator.level = difficulty;
    initializeGame();
    ResumeGame();
  }

  public void PauseGame()
  {
    _isPaused = true;
    pausePanel.SetTitle("Paused");
    pausePanel.SetSliders(_mazeCreator.X, _mazeCreator.level);
    pausePanel.SetCancelEnable(true);
    pausePanel.gameObject.SetActive(true);
    pauseButton.SetActive(false);
  }

  public void ResumeGame()
  {
    pausePanel.gameObject.SetActive(false);
    pauseButton.SetActive(true);
    _isPaused = false;
  }

  public void Winning()
  {
    PauseGame();
    pausePanel.SetTitle("You Win!");
    pausePanel.SetCancelEnable(false);
  }

  public bool IsPaused()
  {
    return _isPaused;
  }

}
