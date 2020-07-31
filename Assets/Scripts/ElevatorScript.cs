﻿#if UNITY_EDITOR

#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ElevatorScript : MonoBehaviour
{
    public Boolean isBottom;
    public Boolean isMoving;
    public GameObject cameraObj;
    private CameraScript cameraScript;
    private Queue<GameObject> personQueue;
    public float eleSpeed = 0.5f;
    public float yLimit = 3f;
    public int numFloors = 9;
    private TextMeshProUGUI floorNum;
    private float sizeFloor;

    // Start is called before the first frame update

    private void Awake()
    {
        isBottom = true;
        isMoving = false;
        cameraScript = cameraObj.GetComponent<CameraScript>();
        floorNum = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();

    }
    void Start()
    {
        personQueue = cameraScript.personQueue;
        floorNum.text = "0";
        sizeFloor = 2 * yLimit / numFloors;
    }

    // Update is called once per frame
    void Update()
    {
        updateFloorNum();


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
    
    void updateFloorNum()
    {
        floorNum.text = Convert.ToInt32(((yLimit + transform.position.y) / sizeFloor)).ToString();
        
    }
}