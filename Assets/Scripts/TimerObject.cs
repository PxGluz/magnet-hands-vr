using TMPro;
using UnityEngine;

public class TimerObject : MonoBehaviour
{
    [SerializeField] private bool shouldRotateToPlayer = false;
    [SerializeField] private Transform canvas;
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        PlayerContainer.Instance.timerLogic.InitTimerObject(timerText);
        if (shouldRotateToPlayer)
            target = Camera.main.transform;
    }

    private Transform target;

    private void RotateToPlayer()
    {
        if (!shouldRotateToPlayer) return;
        Vector3 direction = target.position - transform.position;
        canvas.forward = -direction.normalized;
    }

    void Update()
    {
        RotateToPlayer();
    }
}
