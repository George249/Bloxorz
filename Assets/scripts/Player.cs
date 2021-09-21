using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float duration = 0.3f;
    Vector3 scale;
    [SerializeField] private GameObject winSpot;
    //[SerializeField] private GameObject BridgeSpot;
    //[SerializeField] private GameObject followBrirdge;
    public bool isRotating = false;
    float directionX = 0;
    float directionZ = 0;
    float startAngleRad = 0;
    bool won = false;
    Vector3 startPos;
    float rotationTime = 0;
    public bool gotZeroLives = false;
    float radius = 1;
    Quaternion preRotation;
    Quaternion postRotation;

    public bool isGrounded = true;
    void Start()
    {
        scale = transform.lossyScale;
        //Debug.Log("[x, y, z] = [" + scale.x + ", " + scale.y + ", " + scale.z + "]");
        this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, -50, 0);
        Debug.Log(winSpot.transform.position);

    }

    void Update()
    {
        if (!isRotating & isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = 0;
            if (x == 0)
                y = Input.GetAxisRaw("Vertical");

            if ((x != 0 || y != 0) && !isRotating)
            {
                directionX = y;
                directionZ = x;
                startPos = transform.position;
                preRotation = transform.rotation;
                transform.Rotate(directionZ * 90, 0, directionX * 90, Space.World);
                postRotation = transform.rotation;
                transform.rotation = preRotation;
                SetRadius();
                rotationTime = 0;
                isRotating = true;
            }
        }
        else
        {
            this.transform.position += new Vector3(0, 0.1f, 0);
            this.transform.position -= new Vector3(0, 0.1f, 0);
        }
        // check if win - should move to onther scripit
        if ((((Mathf.Abs(transform.position.x - winSpot.transform.position.x)) < 0.1f) &&
        ((Mathf.Abs(transform.position.z - winSpot.transform.position.z)) < 0.1f)))
        {
            Debug.Log("WINNER");
            won = true;
            this.GetComponent<Rigidbody>().freezeRotation = true;
            this.GetComponent<BoxCollider>().isTrigger = true;
            FindObjectOfType<GameManager>().LevelCompleted();
        }
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            rotationTime += Time.fixedDeltaTime;
            float ratio = Mathf.Lerp(0, 1, rotationTime / duration);

            float rotAng = Mathf.Lerp(0, Mathf.PI / 2f, ratio);
            float distanceX = -directionX * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));
            float distanceY = radius * (Mathf.Sin(startAngleRad + rotAng) - Mathf.Sin(startAngleRad));
            float distanceZ = directionZ * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));
            transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);

            transform.rotation = Quaternion.Lerp(preRotation, postRotation, ratio);

            if (ratio == 1)
            {
                isRotating = false;
                directionX = 0;
                directionZ = 0;
                rotationTime = 0;
            }
        }
        if (this.transform.position.y < -1f)
        {
            if (won == false)
            {
                Debug.Log(" Fall from somewhere");
                //FindObjectOfType<GameManager>().EndGame(1f);
            }

        }
    }

    // this method sets the radius of the blocks center to the 4 pivot points, which allows for the motion to work
    void SetRadius()
    {
        Vector3 dirVec = new Vector3(0, 0, 0);
        Vector3 nomVec = Vector3.up;

        if (directionX != 0)
            dirVec = Vector3.right;
        else if (directionZ != 0)
            dirVec = Vector3.forward;

        if (Mathf.Abs(Vector3.Dot(transform.right, dirVec)) > 0.99)
        {                       // moving direction is the same as x of object
            if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {                   // y axis of global is the same as y of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.y, scale.x);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {       // y axis of global is the same as z of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.x / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.x);
            }

        }
        else if (Mathf.Abs(Vector3.Dot(transform.up, dirVec)) > 0.99)
        {                   // moving direction is the same as y of object
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {                   // y of global is the same as x of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.y);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.forward, nomVec)) > 0.99)
            {       // y axis of global is the same as z of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.y / 2f, 2f) + Mathf.Pow(scale.z / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.z, scale.y);
            }
        }
        else if (Mathf.Abs(Vector3.Dot(transform.forward, dirVec)) > 0.99)
        {           // moving direction is the same as z of object
            if (Mathf.Abs(Vector3.Dot(transform.right, nomVec)) > 0.99)
            {                   // y of global is the same as x of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.x / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.x, scale.z);
            }
            else if (Mathf.Abs(Vector3.Dot(transform.up, nomVec)) > 0.99)
            {               // y axis of global is the same as y of object
                radius = Mathf.Sqrt(Mathf.Pow(scale.z / 2f, 2f) + Mathf.Pow(scale.y / 2f, 2f));
                startAngleRad = Mathf.Atan2(scale.y, scale.z);
            }
        }
    }

    // this method checks if the player hit the ground and enables the movement if it did
    void OnCollisionEnter(Collision theCollision)
    {

        string name = theCollision.collider.name;
        Debug.Log(name);
        name = name.Substring(0, 4);
        if (name == "Cube")
            isGrounded = true;
        else if (name == "Deat")
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
            this.GetComponent<BoxCollider>().isTrigger = true;

            if (FindObjectOfType<HelpUI>().lives > 0 )
            {
                Debug.Log("in player in oncollider funduion on lives != 1");
                FindObjectOfType<GameManager>().EndGame(1f);
            }
            else
            {
                Debug.Log("in player in oncollider funduion on else");
                 FindObjectOfType<HelpUI>().UpdateLivesNumber();
            }
        }
    }
}
