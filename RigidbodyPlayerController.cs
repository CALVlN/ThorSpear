using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6f;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0f, 0.5f)] float mouseSmoothTime = 0.03f;
    // [SerializeField] float jumpForce = 10f;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0f;
    float velocityY = 0f;
    public Rigidbody playerRB;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    void Start() {
        playerRB = GetComponent<Rigidbody>();
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void Update() {
        UpdateMouseLook();
        // UpdateMovement();
        UpdatePlayerMovement();
        // Debug.Log("Y velocity " + playerRB.velocity);
    }

    void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdatePlayerMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * targetDir.y * walkSpeed + transform.right * targetDir.x * walkSpeed + Vector3.up * velocityY);

        Debug.Log(targetDir.y);
        // Debug.Log("currentDir" + currentDir);

        playerRB.AddForce(velocity * Time.deltaTime);
    }

    // void UpdateMovement() {
    //     Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //     targetDir.Normalize();

    //     currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

    //     // // This really doesn't work properly at all but it doesn't not work anymore.
    //     // if ((controller.collisionFlags & CollisionFlags.Below) != 0 || (controller.collisionFlags & CollisionFlags.Above) != 0) {
    //     //     velocityY = 0f;
    //     // }

    //     // Jumping! (for joy?)
    //     if (Input.GetKeyDown("space")) {
    //         if (PlayerIsGrounded()) {
    //             // TODO
    //             // prevent double tapping space for slightly higher jump.
    //             velocityY += jumpForce;
    //         }
    //     }

    //     Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

    //     // controller.Move(velocity * Time.deltaTime);
    //     playerRB.AddForce(velocity * Time.deltaTime);
    // }

    bool PlayerIsGrounded() {
        RaycastHit hit;
        // TODO
        // Send raycasts to the side to let the player jump while on a ledge.
        // Or use a newly discovered SPHERE cast :OOOOOO


        // Does the ray intersect any objects?
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.2f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            return true;
        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1.2f, Color.white);

            return false;
        }
    }
}
