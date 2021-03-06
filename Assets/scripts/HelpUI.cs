using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpUI : MonoBehaviour
{
    public int lives = 3;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Menu_button()
    {
        SceneManager.LoadScene("Main");
    }

    public void UpdateLivesNumber()
    {
        lives--;
        if (lives >= 0)
        {
            FindObjectOfType<Lives>().UpdateTheLives(lives);
        }
        else
        { 
            lives = 3;
            Invoke("handleLoadingPreviousScene", 1f);
            
        }
    }
    public void handleLoadingPreviousScene()
    {
        Debug.Log(" in hande previous loading");
        int CurrentSceneNum = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(CurrentSceneNum);
        if (CurrentSceneNum > 2)
        {
            CurrentSceneNum -= 2;
        }
        else if (CurrentSceneNum == 2)
        {
            CurrentSceneNum -= 1;
        }
        SceneManager.LoadScene(CurrentSceneNum);
        FindObjectOfType<Lives>().UpdateTheLives(lives);
    }
}
