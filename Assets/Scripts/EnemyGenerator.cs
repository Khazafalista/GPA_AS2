using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemy2Prefab;
    public GameObject knightHealthBar;
    public GameObject knightHealth;

    private GameObject gameController;

    //random generate parameters
    public float radius1 = 30.0f;
    public float radius2 = 50.0f;
    public int number1 = 1;
    public int number2 = 3;
    public int time1 = 120;
    public int time2 = 300;

    float radius;
    float degree;
    int number;
    int time;
    int counter;

    //catch the generated prefab
    GameObject enemy;
    GameObject enemy2;

    bool generate;

    private void Start()
    {
        counter = 0;
        time = 1;

        if (PlayerPrefs.GetInt("Stage") == 2)
        {
            generate = true;
            Debug.Log("Knight generation is enabled");
        }
        else
        {
            generate = false;
        }
    }

    private void Awake()
    {
        gameController = GameObject.Find("GameController");
        knightHealthBar.GetComponent<Image>().enabled = false;
        knightHealth.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetComponent<GameController>().GetStage() == 2)
        {
            if (generate)
            {
                enemy2 = Instantiate(enemy2Prefab, new Vector3(0, 0, 40.0f), Quaternion.LookRotation(new Vector3(0, 0, -1)));
                enemy2.GetComponent<EnemysController>().SetHealth(1000);
                knightHealthBar.GetComponent<Image>().enabled = true;
                knightHealth.GetComponent<Image>().enabled = true;

                generate = false;
            }
        }
        else
        {
            if (counter == time)
            {
                number = Random.Range(number1, number2);
                for (int i = 0; i < number; i++)
                {
                    radius = Random.Range(radius1, radius2);
                    degree = Random.Range(0.0f, 360.0f);
                    enemy = Instantiate(enemyPrefab, new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * degree), 
                                        0.0015f,
                                        radius * Mathf.Sin(Mathf.Deg2Rad * degree)), Quaternion.Euler(new Vector3(0, Random.Range(0.0f, 360f), 0)));
                    enemy.GetComponent<EnemysController>().SetHealth(100);
                }
                time = Random.Range(time1, time2);
                counter = 0;
            }
            else
            {
                counter++;
            }
        }
    }
}
