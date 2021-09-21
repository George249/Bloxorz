using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    // Start is called before the first frame update


    TextMesh text;
    GameManager gameManager;
    HelpUI livess;

    // Use this for initialization
    void Start()
    {

        text = gameObject.GetComponent("TextMesh") as TextMesh;
        livess = FindObjectOfType<HelpUI>();
        text.text = "Lives: " + livess.lives.ToString();

    }
    public void UpdateTheLives(int x)
    {
        text.text = "Lives: " + x.ToString();
    }
}
