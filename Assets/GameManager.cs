using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    HelpUI Livestext;

    private void Start()
    {
        Livestext = FindObjectOfType<HelpUI>();
    }

    public void LevelCompleted()
    {
        Debug.Log("loading next level");
        Invoke("loadNextLevel", 1f);
    }
    public void loadNextLevel()
    {
        int nextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneNum);
    }
    public void EndGame(float x)
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("ending game");
            Livestext.UpdateLivesNumber();
            Invoke("Restart", x);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
