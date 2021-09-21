using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeClick : MonoBehaviour
{
    [SerializeField] private GameObject followBrirdge;
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
            float y = followBrirdge.transform.position.y;
            followBrirdge.transform.position += new Vector3(0, -y, 0);
        }
    }
}
