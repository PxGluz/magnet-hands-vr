using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class ButtonActionedDoor : MonoBehaviour
{
    [Header("Open animation")]
    [Tooltip("Offset from door position")]
    [SerializeField] private Vector3 openDestination;
    [SerializeField] private float openTime;
    [Header("References")]
    [SerializeField] private List<ButtonLogic> linkedButtons;
    [SerializeField] private TextMeshProUGUI doorText;

    private bool isOpened = false;
    private int pressedButtons = 0;
    private Vector3 velocity = Vector3.zero;

    private Vector3 destination;

    void OnValidate()
    {
        if (linkedButtons.Count > 1)
        {
            doorText.gameObject.SetActive(true);
            doorText.text = linkedButtons.Count.ToString();
        }
        else
            doorText.gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (ButtonLogic button in linkedButtons)
        {
            if (button != null)
                Gizmos.DrawLine(transform.position, button.transform.position);
        }
    }

    void Start()
    {
        destination = transform.position + openDestination;

        foreach (ButtonLogic button in linkedButtons)
            button.onPress.AddListener(ButtonPressed);
        
        if (linkedButtons.Count > 1)
        {
            doorText.gameObject.SetActive(true);
            doorText.text = linkedButtons.Count.ToString();
        }
        else
            doorText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isOpened)
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, openTime);
    }

    private void ButtonPressed()
    {
        pressedButtons++;
        doorText.text = (linkedButtons.Count - pressedButtons).ToString();
        if (pressedButtons >= linkedButtons.Count)
        {
            isOpened = true;
            doorText.gameObject.SetActive(false);
            AudioManager.instance.PlaySFXAtPosition("door", gameObject.transform.position);
        }
    }
}
