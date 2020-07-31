using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform personPos;
    public GameObject person;
    public Queue<GameObject> personQueue;
    private float xSpace = 1f;
    public GameObject elevatorObj;
    private ElevatorScript elevatorScript;
    private GameObject lastPerson;
    private PersonScript lastPersonScript;
    private int[] upSoFar;
    private List<GameObject>[] travelled;

    private void Awake()
    {
        personQueue = new Queue<GameObject>();
        elevatorScript = elevatorObj.GetComponent<ElevatorScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        upSoFar = new int[elevatorScript.numFloors];
        travelled = new List<GameObject>[elevatorScript.numFloors];
        for (int i = 0; i < elevatorScript.numFloors; i++)
        {
            travelled[i] = new List<GameObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject currCicle = Instantiate(person);
            currCicle.SetActive(true);
            currCicle.transform.position = personPos.transform.position - Vector3.right*xSpace*personQueue.Count;
            currCicle.GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());
            PersonScript currCircleScript = currCicle.GetComponent<PersonScript>();
            currCircleScript.destFloor = (int) (Random.Range(1.0f, 9.0f));
            personQueue.Enqueue(currCicle);
        }
        if (!elevatorScript.isMoving && elevatorScript.currFloor != 0)
        {
            lastPerson.transform.SetParent(transform);
            foreach (GameObject curr in travelled[lastPersonScript.destFloor])
            {
                curr.transform.position += Vector3.right * xSpace;
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
            travelled[lastPersonScript.destFloor].Add(lastPerson);
            foreach (GameObject curr in personQueue) {
                curr.transform.position += Vector3.right * xSpace;
            }
            elevatorScript.deliverToFloor(lastPersonScript.destFloor);
        }

    }
}
