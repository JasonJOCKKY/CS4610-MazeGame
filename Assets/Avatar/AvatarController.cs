using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
  private Animator animator;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.W))
    {
      animator.SetBool("isRunning", true);
    }

    if (Input.GetKeyUp(KeyCode.W))
    {
      animator.SetBool("isRunning", false);
    }
  }
}
