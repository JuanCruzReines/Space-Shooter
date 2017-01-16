using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int HazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    private int Score;

    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOverFlag;
    private bool restartFlag;

    void Start()
    {
       StartCoroutine("SpawnWaves");
        Score = 0;
        UpdateScore();
        gameOverFlag = false;
        restartFlag = false;
        restartText.text = "";
        gameOverText.text = "";
    }

    void Update()
    {
        if (restartFlag)
        {
            if (Input.GetKeyDown(KeyCode.R))
                Application.LoadLevel(Application.loadedLevel);
              
        }

    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (!gameOverFlag){
            for (int i = 0; i < HazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);

            if(gameOverFlag)
            {
                restartText.text = "Presiona 'R' para empezar de nuevo";
                restartFlag = true;
                break;
            }
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + Score;
    }

    public void AddScore(int newScore)
    {
        Score += newScore;
        UpdateScore();
    }

    public void gameOver()
    {
        gameOverText.text = "Game Over";
        gameOverFlag = true;
    }

}
