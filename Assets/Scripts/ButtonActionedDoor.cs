using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionedDoor : MonoBehaviour
{
    [Header("Open animation")]
    [SerializeField] private Vector3 openDestination;
    [SerializeField] private float openTime;
    [Header("References")]
    [SerializeField] private ButtonLogic linkedButton;
    [SerializeField] private Collider doorCollider;

    private bool isOpened = false;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        linkedButton.onPress.AddListener(BeginOpeningDoor);
    }

    void Update()
    {
        if (isOpened)
            transform.position = Vector3.SmoothDamp(transform.position, openDestination, ref velocity, openTime);
    }

    private void BeginOpeningDoor()
    {
        isOpened = true;
        doorCollider.enabled = false;
    }
}
