#if UNITY_EDITOR

#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.AI;
using System.Security.Cryptography;

public class ElevatorScript : MonoBehaviour
{
    public Boolean isMoving;
    public Boolean isMovingUp;
    public GameObject cameraObj;
    private CameraScript cameraScript;
    //private Queue<GameObject> personQueue;
    public float eleSpeed = 1f;
    public float yLimit = 3f;
    public int numFloors = 9;
    public int currFloor = 0;
    private TextMeshProUGUI floorNumText;
    public float sizeFloor;
    private int destFloor = 0;
    private GameObjectTransition eleTrans;
    private List<Event> eventsList;

    // Start is called before the first frame update

    private void Awake()
    {
        isMoving = false;
        isMovingUp = false;
        cameraScript = cameraObj.GetComponent<CameraScript>();
        floorNumText = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        eventsList = CSVReader.Read("DataCSV");
    }
    void Start()
    {
        //personQueue = cameraScript.personQueue;
        floorNumText.text = "0";
        sizeFloor = 2 * yLimit / numFloors;
        eleTrans = new GameObjectTransition(gameObject, transform.position, eleSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        updateFloorNum();

        if (isMoving)
        {
            eleTrans.dest = new Vector3(transform.position.x, -yLimit + sizeFloor * destFloor, transform.position.z);
            if (!eleTrans.transitionY()) isMoving = false;
        }
    }
        
    void updateFloorNum()
    {
        currFloor = Convert.ToInt32((yLimit + transform.position.y) / sizeFloor);
        floorNumText.text = currFloor.ToString();
    }

    public void deliverToFloor(int floorNum)
    {
        destFloor = floorNum;
        if (currFloor < floorNum)
        {
            isMoving = true;
            isMovingUp = true;
        }
        if (currFloor > floorNum) {
            isMoving = true;
            isMovingUp = false;
        }
    }
}
