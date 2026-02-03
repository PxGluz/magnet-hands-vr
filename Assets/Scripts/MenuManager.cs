using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject levelSelectorPrefab;
    public GameObject[] extendableWalls;
    public GameObject backWall;
    [Header("Config")]
    public float extendingRate;
    public float sidePlacementFactor;
    public GameObject playerPrefab;

    private Vector3 backWallInitial;

    private void Awake()
    {
        if (PlayerContainer.Instance == null)
            Instantiate(playerPrefab, Vector3.up * 1.05f, Quaternion.identity);
        backWallInitial = backWall.transform.position;
        HighscoreManager.LoadHighscores();
    }

    void Start()
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - 1;
        float totalRate = 2 * extendingRate * ((levelCount + 1) / 2);

        print(levelCount);
        print(totalRate);

        // Update walls
        backWall.transform.position = backWallInitial + new Vector3(0, 0, totalRate);
        foreach (GameObject wall in extendableWalls)
            wall.transform.localScale += new Vector3(0, 0, 4 * totalRate);

        // Spawn level selectors
        for (int level = 0; level < levelCount; level += 2)
        {
            // Left level
            GameObject levelSelector = Instantiate(levelSelectorPrefab, new Vector3(backWallInitial.x - sidePlacementFactor, 0f, backWallInitial.z + extendingRate * level), Quaternion.LookRotation(Vector3.left, Vector3.up));

            levelSelector.GetComponent<LevelSelect>().levelIndex = level + 1;
            levelSelector.GetComponent<LevelSelect>().InitValues();

            // Right level
            if (level + 1 < levelCount)
            {
                levelSelector = Instantiate(levelSelectorPrefab, new Vector3(backWallInitial.x + sidePlacementFactor, 0f, backWallInitial.z + extendingRate * level), Quaternion.LookRotation(Vector3.right, Vector3.up));

                levelSelector.GetComponent<LevelSelect>().levelIndex = level + 2;
                levelSelector.GetComponent<LevelSelect>().InitValues();
            }
        }
    }
}
