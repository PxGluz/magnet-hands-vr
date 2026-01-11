using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public static PlayerContainer Instance;

    public PlayerMovement playerMovement;
    public MagneticInputLogic magneticInputLogic;
    public TimerLogic timerLogic;

    private void Awake() {
        timerLogic = GetComponent<TimerLogic>();
        playerMovement = GetComponent<PlayerMovement>();
        magneticInputLogic = GetComponent<MagneticInputLogic>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
