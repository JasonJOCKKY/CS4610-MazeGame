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

  // Start is called before the first frame update
  void Start()
  {
    _animator = GetComponentInChildren<Animator>();
    _charController = GetComponent<CharacterController>();
    _joystick = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<Joystick>();
  }

  // Update is called once per frame
  void Update()
  {
    // Get the horizontal and vertical axis.
    // By default they are mapped to the arrow keys.
    // The value is in the range -1 to 1
    float translation =
      (Mathf.Abs(Input.GetAxis("Vertical")) > moveThreshold ? Input.GetAxis("Vertical") : 0 +
      Mathf.Abs(_joystick.Vertical) > moveThreshold ? _joystick.Vertical : 0)
      * runningSpeed;
    float rotation =
      (Mathf.Abs(Input.GetAxis("Horizontal")) > moveThreshold ? Input.GetAxis("Horizontal") : 0 +
      Mathf.Abs(_joystick.Horizontal) > moveThreshold ? _joystick.Horizontal : 0)
      * turningSpeed;

    _animator.SetFloat("runningVelocity", translation);
    _animator.SetFloat("turningAngle", rotation);

    // Make it move 10 meters per second instead of 10 meters per frame...
    // translation *= Time.deltaTime;
    rotation *= Time.deltaTime;

    // Move translation along the object's z-axis
    _charController.SimpleMove(transform.forward * translation);

    // Rotate around our y-axis
    transform.Rotate(0, rotation, 0);

  }
}
