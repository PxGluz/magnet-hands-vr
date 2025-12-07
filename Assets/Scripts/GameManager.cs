using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 playerCheckpoint;

    public void LevelCompleted()
    {
        PlayerContainer.Instance.timerLogic.StopTimer();
        SetPlayerCheckpoint(PlayerContainer.Instance.transform.position);
        Debug.Log("Level Completed!");
    }

    public void LevelFailed()
    {
        Debug.Log("Level Failed!");
        PlayerContainer.Instance.transform.position = playerCheckpoint;
        PlayerContainer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void SetPlayerCheckpoint(Vector3 position)
    {
        playerCheckpoint = position;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
