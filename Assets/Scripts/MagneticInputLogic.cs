using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;

public class MagneticInputLogic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private LayerMask magnetismRaycast;
    [SerializeField] private LayerMask sphereCastLayers;
    [SerializeField] private LineRenderer leftHandTrajectory;
    [SerializeField] private LineRenderer rightHandTrajectory;
    [SerializeField] private Material positiveColor;
    [SerializeField] private Material negativeColor;

    [Header("Variables")]
    [SerializeField] private float offset;
    [SerializeField] private float magnetismRange;
    [SerializeField] private float magnetismSphereRadius;
    [SerializeField] private float pullForce;
    [SerializeField] private float pullObjectSpeed;
    [SerializeField] private float rotationSpeed;

    public enum MagnetismType
    {
        Negative,
        Positive,
    }

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SteamVR_Actions._default.NegativeL.onChange += NegativeLogic;
        SteamVR_Actions._default.PositiveL.onChange += PositiveLogic;
        SteamVR_Actions._default.NegativeR.onChange += NegativeLogic;
        SteamVR_Actions._default.PositiveR.onChange += PositiveLogic;
        // SteamVR_Actions._default.Trajectory.onChange += EnableTrajectory;
    }

    private GameObject heldLeft;
    private GameObject heldRight;

    private class SourceObject
    {
        public Transform sourceTransform;
        public MagnetismType magnetismType;
        public SteamVR_Input_Sources inputSource;
    }

    SourceObject sourceObjectL = null;
    SourceObject sourceObjectR = null;
    private void HandleMagnetism(SteamVR_Input_Sources fromSource, bool newState, MagnetismType magnetismType)
    {
        Debug.Log(fromSource + " " + newState + " " + magnetismType);
        if (newState)
        {
            Transform source = fromSource == SteamVR_Input_Sources.LeftHand ? leftHand : rightHand;

            if (fromSource == SteamVR_Input_Sources.LeftHand && heldLeft)
            {
                Rigidbody heldObjectRB = heldLeft.GetComponent<Rigidbody>();
                heldObjectRB.isKinematic = false;
                heldObjectRB.useGravity = true;
                heldLeft.GetComponent<Collider>().enabled = true;
                heldObjectRB.velocity = leftHand.forward * pullForce;
                heldObjectRB.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed;
                heldLeft = null;
                return;
            }
            if (fromSource == SteamVR_Input_Sources.RightHand && heldRight)
            {
                Rigidbody heldObjectRB = heldRight.GetComponent<Rigidbody>();
                heldObjectRB.isKinematic = false;
                heldObjectRB.useGravity = true;
                heldRight.GetComponent<Collider>().enabled = true;
                heldObjectRB.velocity = rightHand.forward * pullForce;
                heldObjectRB.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed;
                heldRight = null;
                return;
            }

            (source == leftHand ? leftHandTrajectory : rightHandTrajectory).gameObject.SetActive(true);
            (source == leftHand ? leftHandTrajectory : rightHandTrajectory).material = magnetismType == MagnetismType.Negative ? negativeColor : positiveColor;
            SourceObject sourceObject = new SourceObject()
            {
                sourceTransform = source,
                magnetismType = magnetismType,
                inputSource = fromSource
            };
            if (fromSource == SteamVR_Input_Sources.LeftHand)
                sourceObjectL = sourceObject;
            else
                sourceObjectR = sourceObject;
        }
        else
        {
            if (heldLeft && fromSource == SteamVR_Input_Sources.LeftHand)
            {
                Debug.Log("let go of " + heldLeft.name);
                Rigidbody heldObjectRB = heldLeft.GetComponent<Rigidbody>();
                heldObjectRB.isKinematic = false;
                heldObjectRB.useGravity = true;
                heldObjectRB.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed;
                heldLeft.GetComponent<Collider>().enabled = true;
                heldLeft = null;
            }
            if (heldRight && fromSource == SteamVR_Input_Sources.RightHand)
            {
                Debug.Log("let go of " + heldRight.name);
                Rigidbody heldObjectRB = heldRight.GetComponent<Rigidbody>();
                heldObjectRB.isKinematic = false;
                heldObjectRB.useGravity = true;
                heldObjectRB.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed;
                heldRight.GetComponent<Collider>().enabled = true;
                heldRight = null;
            }
            (fromSource == SteamVR_Input_Sources.LeftHand ? leftHandTrajectory : rightHandTrajectory).gameObject.SetActive(false);
            if (fromSource == SteamVR_Input_Sources.LeftHand)
                sourceObjectL = null;
            else
                sourceObjectR = null;
        }
    }

    private void NegativeLogic(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        HandleMagnetism(fromAction.activeDevice, newState, MagnetismType.Negative);
    }

    private void PositiveLogic(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        HandleMagnetism(fromAction.activeDevice, newState, MagnetismType.Positive);
    }

    // private void EnableTrajectory(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    // {
    //     if (fromAction.activeDevice == SteamVR_Input_Sources.LeftHand)
    //         leftHandTrajectory.gameObject.SetActive(newState);
    //     else
    //         rightHandTrajectory.gameObject.SetActive(newState);
    // }


    private Vector3 currentLeft;
    private Vector3 currentRight;
    private void MoveHeldObjectsToPoints()
    {
        if (heldLeft)
        {
            heldLeft.transform.position = Vector3.SmoothDamp(heldLeft.transform.position, leftHand.transform.position + leftHand.transform.forward * offset, ref currentLeft, pullObjectSpeed);
            heldLeft.transform.Rotate(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed);
        }
        if (heldRight)
        {
            heldRight.transform.position = Vector3.SmoothDamp(heldRight.transform.position, rightHand.transform.position + rightHand.transform.forward * offset, ref currentRight, pullObjectSpeed);
            heldRight.transform.Rotate(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * rotationSpeed);
        }
    }

    private void HandleHoldingLogic(SourceObject sourceObject)
    {   
        if (sourceObject == null)
            return;
        Transform source = sourceObject.sourceTransform;
        MagnetismType magnetismType = sourceObject.magnetismType;
        SteamVR_Input_Sources fromSource = sourceObject.inputSource;
        if ((fromSource == SteamVR_Input_Sources.LeftHand && heldLeft) || (fromSource == SteamVR_Input_Sources.RightHand && heldRight))
            return;

        RaycastHit hit;
        if (Physics.SphereCast(source.position, magnetismSphereRadius, source.forward, out hit, magnetismRange, sphereCastLayers))
        {
            if (!Helpers.isLayerInMask(hit.collider.gameObject.layer, magnetismRaycast))
                return;
            GameObject hitObject = hit.collider.gameObject;
            MagneticObject magneticObject = hitObject.GetComponent<MagneticObject>();
            Vector3 directionToObject = (hit.point - source.position).normalized;
            if (magneticObject.isPullable)
            {
                if (magneticObject.magneticPole != magnetismType)
                {
                    Debug.Log("started pulling " + hitObject.name);
                    if (fromSource == SteamVR_Input_Sources.LeftHand)
                        heldLeft = hitObject;
                    else
                        heldRight = hitObject;
                    Rigidbody heldObjectRB = hitObject.GetComponent<Rigidbody>();
                    heldObjectRB.isKinematic = true;
                    heldObjectRB.useGravity = false;
                    hitObject.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    hitObject.GetComponent<Rigidbody>().velocity = directionToObject * pullForce;
                }
            }
            else
            {
                if (magneticObject.magneticPole != magnetismType)
                    rb.velocity = directionToObject * pullForce;
                else
                    rb.velocity = -directionToObject * pullForce;
            }
            (source == leftHand ? leftHandTrajectory : rightHandTrajectory).gameObject.SetActive(false);
            if (fromSource == SteamVR_Input_Sources.LeftHand)
                sourceObjectL = null;
            else
                sourceObjectR = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveHeldObjectsToPoints();
        HandleHoldingLogic(sourceObjectL);
        HandleHoldingLogic(sourceObjectR);
    }
}
