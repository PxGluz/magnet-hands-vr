using System.Collections;
using System.Collections.Generic;
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

    private Vector3 backWallInitial;

    private void Awake()
    {
        backWallInitial = backWall.transform.position;
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
        for (int level = 0; level < levelCount + 1; level += 2)
        {
            // Left level
            GameObject levelSelector = Instantiate(levelSelectorPrefab, new Vector3(backWallInitial.x - sidePlacementFactor, 0f, backWallInitial.z + extendingRate * level), Quaternion.LookRotation(Vector3.left, Vector3.up));

            levelSelector.GetComponent<LevelSelect>().levelIndex = level + 1;

            levelSelector.GetComponentInChildren<TMP_Text>().text = "Level " + (level + 1);

            // Right level
            if (level + 1 < levelCount)
            {
                levelSelector = Instantiate(levelSelectorPrefab, new Vector3(backWallInitial.x + sidePlacementFactor, 0f, backWallInitial.z + extendingRate * level), Quaternion.LookRotation(Vector3.right, Vector3.up));

                levelSelector.GetComponent<LevelSelect>().levelIndex = level + 2;

                levelSelector.GetComponentInChildren<TMP_Text>().text = "Level " + (level + 2);
            }
        }
    }
}
