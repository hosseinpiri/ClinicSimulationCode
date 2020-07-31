using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public GameObject parent;
    public int destFloor;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.SetParent(this.parent.transform);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
