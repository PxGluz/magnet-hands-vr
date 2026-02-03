using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLogic : MonoBehaviour
{
    [Header("General config")]
    [Tooltip("How many items need to press the button at the same time")]
    [SerializeField] private int itemCount;
    [Tooltip("What layers to consider for items")]
    [SerializeField] private LayerMask relevantLayers;
    [SerializeField] private bool useScale = true;

    [Header("Check area config")]
    [Tooltip("Offset from button position")]
    [SerializeField] private Vector3 boxCenter;
    [SerializeField] private Vector3 boxSizeHalfs;

    [Header("Press animation config")]
    [SerializeField] private Transform animatedObject;

    [HideInInspector] public UnityEvent onPress;
    private bool hasBeenPressed;

    private void OnEnable()
    {
        onPress = new UnityEvent();
        hasBeenPressed = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (hasBeenPressed)
            return;

        Vector3 scaledBoxSizes = useScale ? new Vector3(boxSizeHalfs.x * transform.localScale.x, boxSizeHalfs.y * transform.localScale.y, boxSizeHalfs.z * transform.localScale.z) : boxSizeHalfs;
        Vector3 scaledBoxCenter = useScale ? new Vector3(boxCenter.x * transform.localScale.x, boxCenter.y * transform.localScale.y, boxCenter.z * transform.localScale.z) : boxCenter;

        Collider[] pressingColliders = Physics.OverlapBox(transform.position + scaledBoxCenter, scaledBoxSizes, transform.rotation, relevantLayers);
        if (pressingColliders.Length >= itemCount)
        {
            print("Button has been pressed");
            onPress.Invoke();
            hasBeenPressed = true;
            StartCoroutine(PressAnimation());
            AudioManager.instance.PlaySFXAtPosition("click", gameObject.transform.position);
        }
    }

    private IEnumerator PressAnimation()
    {
        float step = animatedObject.transform.localScale.y / 30f;
        // 30 frames for now
        for (int i = 0; i < 30; i++)
        {
            animatedObject.transform.localScale = new Vector3(animatedObject.transform.localScale.x, animatedObject.transform.localScale.y - step, animatedObject.transform.localScale.z);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 scaledBoxSizes = useScale ? new Vector3(boxSizeHalfs.x * transform.localScale.x, boxSizeHalfs.y * transform.localScale.y, boxSizeHalfs.z * transform.localScale.z) * 2 : boxSizeHalfs * 2;
        Vector3 scaledBoxCenter = useScale ? new Vector3(boxCenter.x * transform.localScale.x, boxCenter.y * transform.localScale.y, boxCenter.z * transform.localScale.z) : boxCenter;

        Gizmos.DrawWireCube(transform.position + scaledBoxCenter, scaledBoxSizes);
    }
}
