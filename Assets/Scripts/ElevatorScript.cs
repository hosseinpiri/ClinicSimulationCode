#if UNITY_EDITOR

#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ElevatorScript : MonoBehaviour
{
    public Boolean isBottom;
    public Boolean isMoving;
    public GameObject cameraObj;
    private CameraScript cameraScript;
    private Queue<GameObject> personQueue;
    public float eleSpeed = 5f;
    public float yLimit = 3f;
    public int numFloors = 9;

    // Start is called before the first frame update

    private void Awake()
    {
        isBottom = true;
        isMoving = false;
        cameraScript = cameraObj.GetComponent<CameraScript>();
        
    }
    void Start()
    {
        personQueue = cameraScript.personQueue;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && isBottom)
        {
            moveUp();
        }


        if (!isBottom && isMoving)
        {
            moveDown();
        }
    }
    
    void moveUp()
    {
        if (transform.position.y < yLimit)
        {
            Vector3 newPos = transform.position + Vector3.up * Time.deltaTime * eleSpeed;
            if (newPos.y <= yLimit) transform.position = newPos;
            else transform.position = new Vector3(0, yLimit, 0);
        }
        else
        {
            isMoving = false;
            isBottom = false;
        }
    }

    void moveDown()
    {
        if (transform.position.y > -yLimit)
        {
            Vector3 newPos = transform.position - Vector3.up * Time.deltaTime * eleSpeed;
            if (newPos.y >= -yLimit) transform.position = newPos;
            else transform.position = new Vector3(0, -yLimit, 0);
        }
        else
        {
            isMoving = false;
            isBottom = true;
        }
    }
}
