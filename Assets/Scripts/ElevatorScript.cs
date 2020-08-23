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
using System.Drawing;

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
    public float sizeFloor;
    private int destFloor = 0;
    private GameObjectTransition eleTrans;
    private int numPeopleX = 3;
    private float paddingFactor = 0.1f;
    private List<GameObject> currLoad;
    public GameObject grid;
    public GameObject floorPf;

    // Start is called before the first frame update

    private void Awake()
    {
        isMoving = false;
        isMovingUp = false;
        cameraScript = cameraObj.GetComponent<CameraScript>();
        sizeFloor = 2 * yLimit / numFloors;
        eleTrans = new GameObjectTransition(gameObject, transform.position, eleSpeed);
        currLoad = new List<GameObject>();

        RectTransform rtEle = gameObject.GetComponent<RectTransform>();
        Vector3[] vEle = new Vector3[4];
        rtEle.GetWorldCorners(vEle);
        float eleHeight = vEle[1].y - vEle[0].y;
        rtEle.localScale = new Vector3(rtEle.lossyScale.x, sizeFloor / eleHeight, rtEle.lossyScale.z);
    }
    void Start()
    {
        renderFloors();
    }

    // Update is called once per frame
    void Update()
    {
        updateFloorNum();

        if (Input.GetKeyDown(KeyCode.R)) renderPeopleInElevator(10);
    }
        
    void updateFloorNum()
    {
        currFloor = Convert.ToInt32((yLimit + transform.position.y) / sizeFloor);
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

    public void renderPeopleInElevator(int numPeople)
    {
        currLoad.RemoveAll(p => p.GetComponent<PersonScript>().animateDestroy());

        RectTransform rtEle = gameObject.GetComponent<RectTransform>();
        Vector3[] vEle = new Vector3[4];
        rtEle.GetWorldCorners(vEle);
        float eleWidth = vEle[2].x - vEle[1].x;

        GameObject person = cameraScript.person;

        RectTransform rtPerson = person.GetComponent<RectTransform>();
        Vector3[] vPerson = new Vector3[4];
        rtPerson.GetWorldCorners(vPerson);
        float personWidth = vPerson[2].x - vPerson[1].x;

        float spaceForEach = eleWidth / numPeopleX;
        float margin = paddingFactor* spaceForEach;
        float scale = (spaceForEach - margin) / personWidth;
        float scaledPersonWidth = personWidth * scale;

        float left = vEle[0].x + margin / 2;
        float currLeft = left;

        float bottom = vEle[0].y + margin / 2 + scaledPersonWidth/ 2;


        for (int i = 0; i < numPeople; i++)
        {
            if (i % numPeopleX == 0) {
                currLeft = left;
                if (i > 0) bottom += scaledPersonWidth + margin;
            }
            Vector3 newPos = new Vector3(currLeft + scaledPersonWidth / 2, bottom, 0);
            GameObject currCircle = Instantiate(person);
            currCircle.transform.localScale = currCircle.transform.lossyScale * scale;
            currCircle.transform.position = newPos;
            currCircle.transform.SetParent(gameObject.transform);
            currCircle.SetActive(true);
            currLeft += margin + scaledPersonWidth;
            currLoad.Add(currCircle);
        }
    }
    private void renderFloors()
    {
        RectTransform rtEle = gameObject.GetComponent<RectTransform>();
        Vector3[] vEle = new Vector3[4];
        rtEle.GetWorldCorners(vEle);
        float eleHeight = vEle[1].y - vEle[0].y;

        for (int i = 0; i < numFloors + 1; i++)
        {
            GameObject currFloor = Instantiate(floorPf);
            currFloor.SetActive(true);
            currFloor.transform.position = new Vector3(0, vEle[0].y + i * eleHeight, transform.position.z);
            if (i < numFloors)
            {
                TextMeshProUGUI floorText = currFloor.GetComponentInChildren<TextMeshProUGUI>();
                if (i == 0) floorText.text = "G";
                else floorText.text = i.ToString();

            }
        }
    }
}
