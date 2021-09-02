using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public  void load()
    {
        Debug.Log("in load");
        Invoke("bah", 2f);

    }
    void bah()
    {
        Debug.Log("in load");
        int nextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneNum);

    }
}
