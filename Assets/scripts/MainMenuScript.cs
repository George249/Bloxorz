using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayBloxorz()
    {
        SceneManager.LoadScene("Level1");
    }
    public void QuitBloxorz()
    {
        Application.Quit();
    }
}
