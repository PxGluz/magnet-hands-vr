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

    [Header("Check area config")]
    [Tooltip("Offset from button position")]
    [SerializeField] private Vector3 boxCenter;
    [SerializeField] private Vector3 boxSizeHalfs;

    [Header("Press animation config")]
    [SerializeField] private float moveAmount;
    [SerializeField] private Vector3 moveDirection;

    public UnityEvent onPress;
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

        Collider[] pressingColliders = Physics.OverlapBox(transform.position + boxCenter, boxSizeHalfs, transform.rotation, relevantLayers);
        if (pressingColliders.Length >= itemCount)
        {
            print("Button has been pressed");
            onPress.Invoke();
            hasBeenPressed = true;
            StartCoroutine(PressAnimation());
        }
    }

    private IEnumerator PressAnimation()
    {
        float step = moveAmount / 30f;
        // 30 frames for now
        for (int i = 0; i < 30; i++)
        {
            transform.position += moveDirection * step;
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + boxCenter, boxSizeHalfs * 2);
    }
}
