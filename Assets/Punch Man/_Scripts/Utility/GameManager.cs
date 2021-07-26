using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GManager;
    public List<Transform> waypoint = new List<Transform>();

    public int EnemyCountTOKill;
    [HideInInspector]public int EnemyKilled;

    [HideInInspector]public int moveTowardPoint;

    public int Level = 0;

    public float waitTime;

    public bool Win;
    void Start()
    {
        GManager = this;
    }

    void Update()
    {
        StartCoroutine(WaitForTime(waitTime));
    }

    IEnumerator WaitForTime(float t)
    {
        if (Level == 1)
        {
            if (EnemyKilled == 0)
            {
                moveTowardPoint = 0;
            }
            if (EnemyKilled == 3)
            {
                yield return new WaitForSeconds(t);
                moveTowardPoint = 1;
            }
            if(EnemyKilled == EnemyCountTOKill)
            {
                yield return new WaitForSeconds(t);
                moveTowardPoint = 2;
            }
        }

    }
}
