using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]
public class SimpleCharacterController : MonoBehaviour {

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float defaultGravity = 20f;
    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;
    private Rigidbody rb;
    public bool active;

    void Start () {
        controller = GetComponent<CharacterController> ();
        rb = GetComponent<Rigidbody> ();
        gravity = defaultGravity;
    }

    public void EnableControl (bool enable) {
        active = enable;
        rb.isKinematic = !enable;
        Debug.Log ("Enable Control: " + enabled);
    }

    void Update () {
        if (active) {
            if (controller.isGrounded) {
                // We are grounded, so recalculate
                // move direction directly from axes

                moveDirection = new Vector3 (0f, 0.0f, Input.GetAxis ("Horizontal"));
                moveDirection *= speed;

                if (Input.GetButton ("Jump")) {
                    moveDirection.y = jumpSpeed;
                }

            } else {
                moveDirection = new Vector3 (0f, moveDirection.y, Input.GetAxis ("Horizontal") * speed);
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            controller.Move (moveDirection * Time.deltaTime);
        }
    }
}