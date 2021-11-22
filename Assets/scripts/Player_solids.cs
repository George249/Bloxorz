using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_solids : MonoBehaviour
{
    [SerializeField] bool useUniformDistance;
    [SerializeField] float tileScale;
    [SerializeField] Transform ground;
    [SerializeField] Mesh groundMesh;
    [SerializeField] Mesh mesh;
    Vector3 targetPos;
    Quaternion rotation;


    GameObject previewObj;

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

    Vector3 defaultAnchorPoint;

    Vector3 GetHighestVertex (Transform tr, Mesh msh) {
        float maxZ = -Mathf.Infinity;
        Vector3 highest = new Vector3();
        foreach (Vector3 vertex in msh.vertices) {
            Vector3 transformed = tr.TransformPoint(vertex);
            if (transformed.z > maxZ) {
                maxZ = transformed.z;
                highest = vertex;
            }
        }

        return highest;
    }

    Vector3 GetClosestVertex (Vector3 pos, Transform tr, Mesh msh) { 
        float minDist = Mathf.Infinity;
        Vector3 closest = new Vector3();
        foreach (Vector3 vertex in msh.vertices) {
            Vector3 transformed = tr.TransformPoint(vertex);
            float dist = Vector3.Distance(pos, transformed);
            if (dist < minDist) {
                minDist = dist;
                closest = vertex;
            }
        }
        return closest;
    }

    void Start()
    {
        previewObj = new GameObject();
        previewObj.transform.localScale = transform.localScale;

        if (winSpot == null)
            winSpot = GameObject.Find("WinSpot");

        foreach (MeshFilter mf in GetComponentsInChildren<MeshFilter>()) {
            mf.sharedMesh = mesh;    
        }
        GetComponent<MeshCollider>().sharedMesh = mesh;

        scale = transform.lossyScale;
        //Debug.Log("[x, y, z] = [" + scale.x + ", " + scale.y + ", " + scale.z + "]");
        this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, -50, 0);
        Debug.Log(winSpot.transform.position);
    }

    Transform GetNearestTile(Vector3 position, Vector3 direction)
    {
        position.y = ground.GetChild(0).position.y;
        RaycastHit hit;
        if (Physics.Raycast(position, direction.normalized, out hit)) {
            if (hit.transform.tag == "Ground") {
                return hit.transform;
            }
        }

        return null;
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
                print(directionX);
                print(directionZ);
                startPos = transform.position;
                preRotation = transform.rotation;
                postRotation = GetRotation();
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
        if ((((Mathf.Abs(transform.position.x - winSpot.transform.position.x)) < 0.6f) &&
        ((Mathf.Abs(transform.position.z - winSpot.transform.position.z)) < 0.6f))&& isRotating==false)
        {
            Debug.Log("WINNER");
            won = true;
            this.GetComponent<Rigidbody>().freezeRotation = true;
            this.GetComponent<MeshCollider>().isTrigger = true;
            FindObjectOfType<GameManager>().LevelCompleted();
        }
    }

    Quaternion GetRotation () {
        //Step 1: Get the rotation needed to move the appropriate face flat.
        Vector3 desiredNormal = new Vector3(-directionX, 0, directionZ);
        Vector3 chosenNormal = transform.TransformDirection(mesh.normals[0]);
        foreach (Vector3 normal in mesh.normals) {
            Vector3 localNormal = transform.TransformDirection(normal);
            Debug.DrawRay(transform.position, localNormal, Color.white, 5f);
            if (Vector3.Distance(localNormal, desiredNormal) < Vector3.Distance(chosenNormal, desiredNormal)) {
                chosenNormal = localNormal;
            }
        }

        Debug.DrawRay(transform.position, desiredNormal, Color.blue, 5f);
        Debug.DrawRay(transform.position, chosenNormal, Color.green, 5f);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 5f);

        Quaternion flatRot = FromToRotation(chosenNormal, Vector3.down) * transform.rotation;
        previewObj.transform.rotation = flatRot;

        Vector3 directionVector = new Vector3(-directionX * tileScale, 0, directionZ * tileScale);
        Transform targetTile = GetNearestTile(transform.position, directionVector);

        if (targetTile != null) {
            targetPos = targetTile.position;
            targetPos.y = transform.position.y;
            previewObj.transform.position = targetPos;

            //Step 2: Align one of the vertices on the player mesh to the base mesh
            Vector3 anchorPoint = previewObj.transform.TransformPoint(GetHighestVertex(previewObj.transform, mesh));
            Vector3 pointOnTile = targetTile.TransformPoint(GetClosestVertex(anchorPoint, targetTile, groundMesh));
            Debug.DrawRay(anchorPoint, Vector3.up * 10, Color.red, 10f);
            Debug.DrawRay(pointOnTile, Vector3.up * 10, Color.blue, 10f);

            //Step 3: Adjust target position so it lines up
            targetPos += pointOnTile - anchorPoint;

            //Step 4: Rotate the player so that at least one edge aligns with the base mesh
            Vector3 tileVert = targetTile.InverseTransformPoint(pointOnTile);
            Vector3 neighbour = targetTile.TransformPoint(GetNeighbouringVertex(tileVert, groundMesh));
            
            Debug.DrawLine(neighbour, pointOnTile, Color.blue, 5f);
            
            Vector3 dirTile = neighbour - pointOnTile;
            Vector3 dirPlayer = previewObj.transform.TransformPoint(GetClosestVertex(neighbour, previewObj.transform, mesh)) - anchorPoint;

            Debug.DrawLine(previewObj.transform.TransformPoint(GetClosestVertex(neighbour, previewObj.transform, mesh)), anchorPoint, Color.red, 5f);
            
            previewObj.transform.rotation = FromToRotation(dirPlayer, dirTile) * previewObj.transform.rotation;
        } else {
            targetPos = transform.position + directionVector;
        }

        return previewObj.transform.rotation;
    }

    Vector3 GetNeighbouringVertex (Vector3 currentVertex, Mesh msh) {
        foreach (Vector3 vert in msh.vertices) {
            if (vert != currentVertex) {
                if (vert.y == currentVertex.y) {
                    return vert;
                }
            }
        }

        return currentVertex;
    }

    public static Quaternion FromToRotation(Vector3 aFrom, Vector3 aTo)
    {
        Vector3 axis = Vector3.Cross(aFrom, aTo);
        float angle = Vector3.Angle(aFrom, aTo);
        return Quaternion.AngleAxis(angle, axis.normalized);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().isKinematic = isRotating;

        if (isRotating)
        {
            rotationTime += Time.fixedDeltaTime;
            float ratio = Mathf.Lerp(0, 1, rotationTime / duration);

            float rotAng = Mathf.Lerp(0, Mathf.PI / 2f, ratio);
            float distanceX = -directionX * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));
            float distanceY = radius * (Mathf.Sin(startAngleRad + rotAng) - Mathf.Sin(startAngleRad));
            float distanceZ = directionZ * radius * (Mathf.Cos(startAngleRad) - Mathf.Cos(startAngleRad + rotAng));

            if(!useUniformDistance) {
                transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, startPos.z + distanceZ);
            } else {
                Vector3 adjustedTargetPos = targetPos;
                adjustedTargetPos.y += distanceY;
                transform.position = Vector3.Lerp(startPos, adjustedTargetPos, ratio);
            }

            transform.rotation = Quaternion.Lerp(preRotation, postRotation, ratio);

            if (ratio == 1)
            {
                transform.rotation = postRotation;
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

        string tag = theCollision.collider.tag;
        string name = theCollision.collider.name;
        //Debug.Log(name);
        if (tag == "Ground" || name.Substring(0,4) == "Cube")
            isGrounded = true;
        else if (name == "Deat")
        {
            this.GetComponent<Rigidbody>().freezeRotation = true;
            this.GetComponent<MeshCollider>().isTrigger = true;

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
