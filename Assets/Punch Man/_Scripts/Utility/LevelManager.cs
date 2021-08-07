using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public static LevelManager LManager;
    [Header("Game Objects")]
    public GameObject HomeLevel;

    [Header("UIs")]
    public GameObject HomeScreen;
    public GameObject NextScreen;
    public GameObject FailScreen;
    public GameObject LevelScoreS;
    public TextMeshProUGUI scoreText;

    [Header("Levels")]
    public GameObject Levels;
    public GameObject[] gameLevels;

    public int levelNumber = 0;

    private void Start()
    {
        LManager = this;
        Application.targetFrameRate = 60;
    }
    private void Update()
    {


        if (Levels == null)
            return;
    }
    public void spwnLevel()
    {
        HomeLevel.SetActive(false);
        HomeScreen.SetActive(false);
        LevelScoreS.SetActive(true);
        Levels = Instantiate(gameLevels[0], Vector3.zero, Quaternion.identity);
        Levels.transform.parent = transform;
        scoreText.text = (levelNumber + 1).ToString();
    }
    [SerializeField]
    int number;

    public void ScreenHide()
    {
        NextScreen.SetActive(false);
        FailScreen.SetActive(false);
    }
    public void next()
    {
        NextScreen.SetActive(false);
        FailScreen.SetActive(false);

        levelNumber ++;
        Destroy(Levels);
        Levels = null;

        if (Levels == null && levelNumber < 4)
        {
            scoreText.text = (levelNumber + 1).ToString();
            Levels = Instantiate(gameLevels[levelNumber], Vector3.zero, Quaternion.identity);
            Levels.transform.parent = transform;
        }

        if (Levels == null && levelNumber > 3)
        {
            number = Random.Range(0, 4);
            scoreText.text = (levelNumber + 1).ToString();
            Levels = Instantiate(gameLevels[number], Vector3.zero, Quaternion.identity);
            Levels.transform.parent = transform;
        }
    }

}
