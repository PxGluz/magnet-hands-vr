using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI highscoreText;

    public int levelIndex = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerContainer.Instance.timerLogic.ResetTimer();
            PlayerContainer.Instance.timerLogic.StartTimer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + levelIndex);
        }
    }

    public void InitValues()
    {
        levelName.text = "Level " + levelIndex;
        float highscore = HighscoreManager.ReadHighscore(levelIndex);
        highscoreText.text = "Highscore:\n" + (highscore == float.MaxValue ? "N/A" : HighscoreManager.SecondsToString(highscore));
    }
}
