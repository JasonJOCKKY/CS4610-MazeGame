using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
  public float runningSpeed = 5;
  public float turningSpeed = 200;
  public float moveThreshold = 0.3f;

  private Joystick _joystick;
  private Animator _animator;
  private CharacterController _charController;
  private GameController _gameController;

  // Start is called before the first frame update
  void Start()
  {
    _animator = GetComponentInChildren<Animator>();
    _charController = GetComponent<CharacterController>();
    _joystick = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<Joystick>();
    _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (_joystick.gameObject.activeInHierarchy != !_gameController.IsPaused()) _joystick.gameObject.SetActive(!_gameController.IsPaused());
    if (_charController.enabled != !_gameController.IsPaused())  _charController.enabled = !_gameController.IsPaused();
    // Get the horizontal and vertical axis.
    // By default they are mapped to the arrow keys.
    // The value is in the range -1 to 1
    float translation =
      (Mathf.Abs(Input.GetAxis("Vertical")) > moveThreshold ? Input.GetAxis("Vertical") : 0 +
      Mathf.Abs(_joystick.Vertical) > moveThreshold ? _joystick.Vertical : 0)
      * runningSpeed;
    translation = _gameController.IsPaused() ? 0 : translation;
    float rotation =
      (Mathf.Abs(Input.GetAxis("Horizontal")) > moveThreshold ? Input.GetAxis("Horizontal") : 0 +
      Mathf.Abs(_joystick.Horizontal) > moveThreshold ? _joystick.Horizontal : 0)
      * turningSpeed;
    rotation = _gameController.IsPaused() ? 0 : rotation;

    _animator.SetFloat("runningVelocity", translation);
    _animator.SetFloat("turningAngle", rotation);

    // Make it move 10 meters per second instead of 10 meters per frame...
    rotation *= Time.deltaTime;

    // Move translation along the object's z-axis
    if (!_gameController.IsPaused()) _charController.SimpleMove(transform.forward * translation);

    // Rotate around our y-axis
    transform.Rotate(0, rotation, 0);
  }

  public void SetPauseState()
  {

  }

  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if (hit.gameObject.CompareTag("Floor"))
    {
      hit.gameObject.GetComponent<FloorFog>().ClearFog();
      if (hit.gameObject.name == "Exit Floor") _gameController.Winning();
    }
  }
}
