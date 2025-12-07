using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionedDoor : MonoBehaviour
{
    [Header("Open animation")]
    [Tooltip("Offset from door position")]
    [SerializeField] private Vector3 openDestination;
    [SerializeField] private float openTime;
    [Header("References")]
    [SerializeField] private List<ButtonLogic> linkedButtons;
    [SerializeField] private Collider doorCollider;

    private bool isOpened = false;
    private int pressedButtons = 0;
    private Vector3 velocity = Vector3.zero;

    private Vector3 destination;
    void Start()
    {
        destination = transform.position + openDestination;

        foreach (ButtonLogic button in linkedButtons)
            button.onPress.AddListener(ButtonPressed);
    }

    void Update()
    {
        if (isOpened)
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, openTime);
    }

    private void ButtonPressed()
    {
        pressedButtons++;
        if (pressedButtons >= linkedButtons.Count)
        {
            isOpened = true;
            doorCollider.enabled = false;
        }
    }
}
