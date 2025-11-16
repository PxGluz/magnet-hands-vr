using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private GameObject mainCamera;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.gameObject;
        SteamVR_Actions._default.Move.onChange += PlayerMovementLogic;
    }

    private void PlayerMovementLogic(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        Vector3 intermMove = mainCamera.transform.right * axis.x + mainCamera.transform.forward * axis.y;
        Vector3 finalMove = new Vector3(intermMove.x, 0, intermMove.z).normalized * speed;
        rb.velocity = new Vector3(Mathf.Abs(finalMove.x) > Mathf.Abs(rb.velocity.x) ? finalMove.x : rb.velocity.x,
                                  rb.velocity.y,
                                  Mathf.Abs(finalMove.z) > Mathf.Abs(rb.velocity.z) ? finalMove.z : rb.velocity.z);
        // rb.AddForce(finalMove);
    }
}
