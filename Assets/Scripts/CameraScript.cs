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
    private float xSpace = 0.8f;
    public GameObject elevatorObj;
    public GameObjectTransition eleTransition;
    private ElevatorScript elevatorScript;
    private GameObject lastPerson;
    private PersonScript lastPersonScript;
    private Queue<GameObject>[] travelledUp;
    private List<GameObjectTransition> personTransitionList;
    private List<Event> eventList;
    private List<Event> eleEventList;
    private QueueObj[] eleQueueUp;
    private QueueObj[] eleQueueDown;
    private QueueObj[] doctorQueue;
    private QueueObj[] doctorVisited;
    private float elapsedTime = 0;
    public float yLimit = 3f;
    public int numFloors = 9;
    public float sizeFloor;


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
        sizeFloor = 2 * yLimit / numFloors;
        eleQueueUp = initQueue(-Vector3.right * xSpace);
        eleQueueDown = initQueue(-Vector3.right * xSpace*4);
        doctorQueue = initQueue(Vector3.right * xSpace*4);
        doctorVisited = initQueue(Vector3.right * xSpace * 8);
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
        // Testing only
        if (Input.GetKeyUp(KeyCode.U)) updateQueue(doctorQueue[0], doctorQueue[0].q.Count + 1);
        if (Input.GetKeyUp(KeyCode.L)) updateQueue(doctorQueue[0], 2);
        if (Input.GetKeyUp(KeyCode.R)) {
            updateQueue(doctorQueue[6], doctorQueue[6].q.Count + 4);
        }

    }
    private void transitionHelper()
    {
        personTransitionList.RemoveAll(got => !got.transitionX());
        pushEleTransition();
        eleTransition.transitionY();
        pushPersonTransition();
    }

    private void pushEleTransition()
    {
        if (eleEventList.Count > 1)
        {
            Event prevEvent = eleEventList[0];
            Event curEvent = eleEventList[1];
            if (elapsedTime > prevEvent.time)
            {
                eleTransition.dest = new Vector3(elevatorObj.transform.position.x, -yLimit +
                    sizeFloor * curEvent.floorNum, elevatorObj.transform.position.z);
                eleTransition.transitionTime = curEvent.time - prevEvent.time;
                eleTransition.transitionSpeed = Vector3.Distance(eleTransition.dest, elevatorObj.transform.position) / eleTransition.transitionTime;
                elevatorScript.loadText.text = prevEvent.newVal.ToString();
                if (curEvent.floorNum > prevEvent.floorNum) updateQueue(eleQueueUp[prevEvent.floorNum], 
                    eleQueueUp[prevEvent.floorNum].q.Count - prevEvent.newVal);
                if (curEvent.floorNum < prevEvent.floorNum) updateQueue(eleQueueDown[prevEvent.floorNum],
                    eleQueueUp[prevEvent.floorNum].q.Count - prevEvent.newVal);
                eleEventList.RemoveAt(0);
            }
        }
        if (eleEventList.Count == 1 && elapsedTime > eleEventList[0].time) elevatorScript.loadText.text = eleEventList[0].newVal.ToString();
    }
    private QueueObj[] initQueue(Vector3 xoffSet)
    {
        QueueObj[] q = new QueueObj[elevatorScript.numFloors];
        for (int i = 0; i < elevatorScript.numFloors; i++)
        {
            q[i] = new QueueObj(personPos.position + Vector3.up*(i*sizeFloor)+ xoffSet);
        }
        return q;
    }
    private void updateQueue(QueueObj queueObj, int newSize)
    {
        int sizeDiff = Mathf.Abs(newSize - queueObj.q.Count);
        if (newSize > queueObj.q.Count)
        {
            for (int i = 0; i < sizeDiff; i++)
            {
                GameObject currCicle = Instantiate(person);
                currCicle.SetActive(true);
                currCicle.transform.position = queueObj.offset - Vector3.right * xSpace * queueObj.q.Count;
                PersonScript currCircleScript = currCicle.GetComponent<PersonScript>();
                queueObj.q.Enqueue(currCicle);
            }
        }
        if (newSize < queueObj.q.Count)
        {
            for (int i = 0; i < sizeDiff; i++)
            {
                lastPerson = queueObj.q.Peek();
                lastPersonScript = lastPerson.GetComponent<PersonScript>();
                lastPersonScript.animateDestroy();
                //lastPerson.transform.position += xSpace * Vector3.right;
                //lastPerson.transform.SetParent(elevatorObj.transform);
                queueObj.q.Dequeue();
            }
            
            foreach (GameObject curr in queueObj.q)
            {
                personTransitionList.Add(new GameObjectTransition(curr, curr.transform.position + sizeDiff*Vector3.right * xSpace));
            }
        }
        else return;
    }
    private void pushPersonTransition()
    {
        if (eventList.Count > 0)
        {
            Event curEvent = eventList[0];
            if (curEvent.time < elapsedTime || Mathf.Approximately(curEvent.time, elapsedTime))
            {
                switch (curEvent.eventName)
                {
                    case EventName.hall_queue:
                        if (curEvent.eleDir == EleDirection.UP) updateQueue(eleQueueUp[curEvent.floorNum], curEvent.newVal);
                        else if (curEvent.eleDir == EleDirection.DOWN) updateQueue(eleQueueDown[curEvent.floorNum], curEvent.newVal);
                        break;
                    case EventName.doctor_queue:
                        updateQueue(doctorQueue[curEvent.floorNum], curEvent.newVal);
                        break;
                    case EventName.doctor_visited:
                        updateQueue(doctorVisited[curEvent.floorNum], curEvent.newVal);
                        break;
                }
                eventList.RemoveAt(0);
            }
        }
    }
}
