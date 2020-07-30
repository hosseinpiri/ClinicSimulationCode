using System.Collections;
using System.Collections.Generic;
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
    private int upSoFar = 0;

    private void Awake()
    {
        personQueue = new Queue<GameObject>();
        elevatorScript = elevatorObj.GetComponent<ElevatorScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
            personQueue.Enqueue(currCicle);
        }
        if (!elevatorScript.isMoving && elevatorScript.isBottom && personQueue.Count > 0)
        {
            lastPerson = personQueue.Peek();
            personQueue.Dequeue();
            upSoFar++;
            foreach (GameObject curr in personQueue) {
                curr.transform.position += Vector3.right * xSpace;
            } 
            elevatorScript.isMoving = true;
        }

        if (!elevatorScript.isBottom && !elevatorScript.isMoving)
        {
            lastPerson.transform.position = new Vector3(-lastPerson.transform.position.x*upSoFar* xSpace, -lastPerson.transform.position.y,
                lastPerson.transform.position.z);
            elevatorScript.isMoving = true;
        }

    }
}
