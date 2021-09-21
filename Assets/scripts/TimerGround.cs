using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGround : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    private Player player;
    void OnCollisionEnter(Collision theCollision)
    {

        string name = theCollision.collider.name;
        name = name.Substring(0, 4);
        if (name == "Play")
        {
            Debug.Log("Rasmyyyyyyyyyyyyyyyyyy");
            player = FindObjectOfType<Player>();
            Debug.Log(player.transform.position);
            if ((((Mathf.Abs(transform.position.x - player.transform.position.x)) < 0.1f) &&
                ((Mathf.Abs(transform.position.z - player.transform.position.z)) < 0.1f)))
            {
                Debug.Log("find that the 2 spots are close");
                Invoke("StartFall", 5f);
            }
        }
    }
    void StartFall()
    {
        Debug.Log("starttttttttttttttttttt fall");
        float y = this.transform.position.y;

        this.transform.position += new Vector3(0, Math.Abs(y) * -100, 0);
    }
}
    */
    [SerializeField] ParticleSystem _particleSystem;
    private Player player;
    void Start()
    {
        //player = bridge.GetComponent<BridgeMove>();
        player = FindObjectOfType<Player>();
    }
    private void FixedUpdate()
    {
        if ((((Mathf.Abs(transform.position.x - player.transform.position.x)) < 0.1f) &&
             ((Mathf.Abs(transform.position.z - player.transform.position.z)) < 0.1f)))
        {
            Invoke("StartFall", 2f);
        }
    }
    void StartFall()
    {
        bool stillThere = false;
        Debug.Log("starttttttttttttttttttt fall");
        float y = this.transform.position.y;
        _particleSystem.Play();
        if ((((Mathf.Abs(transform.position.x - player.transform.position.x)) < 0.1f) &&
        ((Mathf.Abs(transform.position.z - player.transform.position.z)) < 0.1f)))
        {
            stillThere = true;
        }
        this.transform.position += new Vector3(0, -100, 0);
        if (stillThere)
        {
            player.GetComponent<Rigidbody>().freezeRotation = true;
            player.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
