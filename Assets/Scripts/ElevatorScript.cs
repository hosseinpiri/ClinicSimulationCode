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
    private int numPeopleX = 3;
    private float paddingFactor = 0.1f;
    private List<GameObject> currLoad;

    // Start is called before the first frame update

    private void Awake()
    {
        isMoving = false;
        isMovingUp = false;
        cameraScript = cameraObj.GetComponent<CameraScript>();
        floorNumText = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        floorNumText.text = "0";
        sizeFloor = 2 * yLimit / numFloors;
        eleTrans = new GameObjectTransition(gameObject, transform.position, eleSpeed);
        currLoad = new List<GameObject>();
    }
    void Start()
    {
        //personQueue = cameraScript.personQueue;
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
}
