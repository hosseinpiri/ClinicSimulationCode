using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueObj
{
    public Vector3 offset;
    public Queue<GameObject> q;

    public QueueObj(Vector3 offset)
    {
        this.offset = offset;
        q = new Queue<GameObject>();
    }
}
