using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;
using TMPro;

public class CameraScript : MonoBehaviour
{
    public Transform personPos;
    public GameObject person;
    public Queue<GameObject> personQueue;
    private float xSpace = 1f;
    public GameObject elevatorObj;
    public GameObjectTransition eleTransition;
    private ElevatorScript elevatorScript;
    private GameObject lastPerson;
    private PersonScript lastPersonScript;
    private Queue<GameObject>[] travelledUp;
    private List<GameObjectTransition> personTransitionList;
    private List<Event> eventList;
    private List<Event> eleEventList;
    private Queue<GameObject>[] eleQueueUp;
    private Queue<GameObject>[] eleQueueDown;
    private Queue<GameObject>[] clinicQueue;
    private Queue<GameObject>[] doctorQueue;
    private float elapsedTime = 0;


    private void Awake()
    {
        personQueue = new Queue<GameObject>();
        elevatorScript = elevatorObj.GetComponent<ElevatorScript>();
        eleTransition = new GameObjectTransition(elevatorObj, elevatorObj.transform.position);
        travelledUp = new Queue<GameObject>[elevatorScript.numFloors];
        for (int i = 0; i < elevatorScript.numFloors; i++)
        {
            travelledUp[i] = new Queue<GameObject>();
        }
        personTransitionList = new List<GameObjectTransition>();
        eventList = CSVReader.Read("DataCSV");
        eleEventList = eventList.Where(e => e.eventName == EventName.elevator_load).ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        transitionHelper();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject currCicle = Instantiate(person);
            currCicle.SetActive(true);
            currCicle.transform.position = personPos.transform.position - Vector3.right*xSpace*personQueue.Count;
            currCicle.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
            PersonScript currCircleScript = currCicle.GetComponent<PersonScript>();
            currCircleScript.srcFloor = 0;
            currCircleScript.destFloor = (int) (Random.Range(1.0f, 9.0f));
            personQueue.Enqueue(currCicle);
        }
        if (!elevatorScript.isMoving && elevatorScript.currFloor != 0)
        {
            lastPerson.transform.SetParent(transform);
            foreach (GameObject curr in travelledUp[lastPersonScript.destFloor])
            {
                personTransitionList.Add(new GameObjectTransition(curr, curr.transform.position + Vector3.right * xSpace));
            }
            lastPerson.transform.position = new Vector3(xSpace, -elevatorScript.yLimit + elevatorScript.sizeFloor * lastPersonScript.destFloor, lastPerson.transform.position.z);
            elevatorScript.deliverToFloor(0);
        }
        if (!elevatorScript.isMoving && elevatorScript.currFloor == 0 && personQueue.Count > 0)
        {
            lastPerson = personQueue.Peek();
            lastPersonScript = lastPerson.GetComponent<PersonScript>();
            // Put the person in the elevator
            lastPerson.transform.position += xSpace * Vector3.right;
            lastPerson.transform.SetParent(elevatorObj.transform);
            personQueue.Dequeue();
            travelledUp[lastPersonScript.destFloor].Enqueue(lastPerson);
            foreach (GameObject curr in personQueue) {
                personTransitionList.Add(new GameObjectTransition(curr, curr.transform.position + Vector3.right * xSpace));
            }
            elevatorScript.deliverToFloor(lastPersonScript.destFloor);
        }

    }
    private void transitionHelper()
    {
        personTransitionList.RemoveAll(got => !got.transitionX());
        pushEleTransition();
        eleTransition.transitionY();
    }

    private void pushEleTransition()
    {
        if (eleEventList.Count > 1)
        {
            Event prevEvent = eleEventList[0];
            Event curEvent = eleEventList[1];
            if (elapsedTime > prevEvent.time)
            {
                eleTransition.dest = new Vector3(elevatorObj.transform.position.x, -elevatorScript.yLimit +
                    elevatorScript.sizeFloor * curEvent.floorNum, elevatorObj.transform.position.z);
                eleTransition.transitionTime = curEvent.time - prevEvent.time;
                eleTransition.transitionSpeed = Vector3.Distance(eleTransition.dest, elevatorObj.transform.position) / eleTransition.transitionTime;
                eleEventList.RemoveAt(0);
            }
        }
    }
}
