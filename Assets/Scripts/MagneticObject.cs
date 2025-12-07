using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    [SerializeField] private Material positiveColor;
    [SerializeField] private Material negativeColor;

    public bool isPullable = false;
    public MagneticInputLogic.MagnetismType magneticPole = MagneticInputLogic.MagnetismType.Negative;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public void RespawnItem()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(initialPosition, initialRotation);
    }

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        GetComponent<MeshRenderer>().material = magneticPole == MagneticInputLogic.MagnetismType.Negative ? negativeColor : positiveColor;
    }
}
