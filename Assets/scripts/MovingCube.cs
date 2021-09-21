using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField] GameObject triggerObject;
    [SerializeField] float horDist;
    [SerializeField] float vertDist;
    [SerializeField] float speed = 8f;
    [SerializeField] int direction = 1;
    int index = 0;
    private Vector3 toPoint;
    Vector3[] points = new Vector3[4];

    private void Start()
    {
        points[0] = this.transform.position;
        points[1] = points[0] + (direction * horDist * new Vector3(0, 0, 3));
        points[2] = points[1] + (direction * vertDist * new Vector3(3, 0, 0));
        points[3] = points[2] + (direction * horDist * new Vector3(0, 0, -3));
    }

    void Update()
    {
        toPoint = points[index];
        this.transform.position = Vector3.MoveTowards(this.transform.position, toPoint, speed * Time.deltaTime);
        if (Mathf.Abs(this.transform.position.x - toPoint.x) < 0.1f && Mathf.Abs(this.transform.position.z - toPoint.z) < 0.1f)
        {
            this.transform.position = toPoint;
            index = (index + 1) % 4;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = FindObjectOfType<Player>();
        player.GetComponent<Rigidbody>().freezeRotation = true;
        player.GetComponent<BoxCollider>().isTrigger = true;
        FindObjectOfType<GameManager>().EndGame(1f);
    }

}
