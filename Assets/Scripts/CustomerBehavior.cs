using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    public List<Transform> pathMarker;
    [SerializeField]
    int index = 0;
    public int stopIndex;
    public float threshold;
    [SerializeField]
    bool happy =false;

    void Start()
    {

    }

    void Update()
    {

        //walk through array of marker
        Vector3 destination = pathMarker[index].transform.position;
        Vector3 newpos = Vector3.MoveTowards(transform.position, destination, 1.0f * Time.deltaTime);
        transform.position = newpos;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance < threshold)
        {
                if (index == stopIndex &&!happy)
                    return;
            
            if (index < pathMarker.Count - 1)
            {
                index++;
            }
           
            else
            {
                Destroy(gameObject);
            }
        }
        //wait for order to correct then walk away
    }
}
