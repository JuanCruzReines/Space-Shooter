using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int HazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    private int Score;

    //public Text restartText;
    public Text gameOverText;
    public GameObject restartButton;

    private bool gameOverFlag;
    private bool restartFlag;

    void Start()
    {
       StartCoroutine("SpawnWaves");
        Score = 0;
        UpdateScore();
        gameOverFlag = false;
        restartFlag = false;
        //restartText.text = "";
        restartButton.SetActive(false);
        gameOverText.text = "";
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
                //restartText.text = "Presiona 'R' para empezar de nuevo";
                restartButton.SetActive(true);
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

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
