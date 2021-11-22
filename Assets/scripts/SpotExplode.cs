using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotExplode : MonoBehaviour {

    public float cubeSize = 0.2f;
    public int cubesInRow = 2;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 5f;
    public float explosionRadius = 0.5f;
    public float explosionUpward = 0.05f;
    private Player player;
    // Use this for initialization
    void Start() {
         player = FindObjectOfType<Player>();
    
        
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

    }

    private void Update() {
        if ((((Mathf.Abs(transform.position.x - player.transform.position.x)) < 0.1f) &&
             ((Mathf.Abs(transform.position.z - player.transform.position.z)) < 0.1f)))
        {
            Invoke("explode",2f);
       
        }
       
     
       
    }
        


    public void explode() {
        bool stillThere = false;
        //make object disappear
        if ((((Mathf.Abs(transform.position.x - player.transform.position.x)) < 0.1f) &&
        ((Mathf.Abs(transform.position.z - player.transform.position.z)) < 0.1f)))
        {
            stillThere = true;
        }
        if (stillThere){
        gameObject.SetActive(false);
        
        //loop 3 times to create 2x2x2 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++) {
            for (int y = 0; y < cubesInRow; y++) {
                for (int z = 0; z < cubesInRow; z++) {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders) {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }

        
             }
    }

    void createPiece(int x, int y, int z) {

        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }

}