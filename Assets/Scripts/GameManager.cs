using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 playerCheckpoint;

    public Transform startPosition;

    private void OnEnable()
    {
        SteamVR_Actions._default.OpenMenu.onChange += BackToMenu;
    }

    private void OnDisable()
    {
        SteamVR_Actions._default.OpenMenu.onChange -= BackToMenu;
    }

    public void LevelCompleted()
    {
        PlayerContainer.Instance.timerLogic.StopTimer();
        HighscoreManager.WriteHighscore(SceneManager.GetActiveScene().buildIndex, PlayerContainer.Instance.timerLogic.GetTimer());
        SetPlayerCheckpoint(PlayerContainer.Instance.transform.position);
        Debug.Log("Level Completed!");
    }

    public void ResetPlayer()
    {
        //Debug.Log("Level Failed!");
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

    private void Start()
    {
        SetPlayerCheckpoint(startPosition.position);
        ResetPlayer();
    }

    private void BackToMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        PlayerContainer.Instance.transform.position = Vector3.zero;
        PlayerContainer.Instance.timerLogic.ClearTimerObjects();
        SceneManager.LoadScene(0);
    }
}
