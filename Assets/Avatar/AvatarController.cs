using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
  public float runningSpeed = 5;
  public float turningSpeed = 200;
  private Animator _animator;
  private CharacterController _charController;
  // Start is called before the first frame update
  void Start()
  {
    _animator = GetComponent<Animator>();
    _charController = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {
    // Get the horizontal and vertical axis.
    // By default they are mapped to the arrow keys.
    // The value is in the range -1 to 1
    float translation = Input.GetAxis("Vertical") * runningSpeed;
    float rotation = Input.GetAxis("Horizontal") * turningSpeed;

    _animator.SetFloat("runningVelocity", translation);
    _animator.SetFloat("turningAngle", rotation);

    // Make it move 10 meters per second instead of 10 meters per frame...
    translation *= Time.deltaTime;
    rotation *= Time.deltaTime;

    // // Move translation along the object's z-axis
    _charController.Move(transform.forward * translation);

    // Rotate around our y-axis
    transform.Rotate(0, rotation, 0);

  }
}
