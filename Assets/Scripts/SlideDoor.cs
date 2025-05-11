using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour
{
    public GameObject door;
    public Transform marker;
    public Transform startlocation;
    public Transform endlocation;
    void Start()
    {

    }

    void Update()
    {
        door.transform.position = Vector3.Lerp(door.transform.position, marker.transform.position, 1.5f*Time.deltaTime);

    }
   
    private void OnTriggerEnter(Collider other)
    {
      //  if (other.CompareTag("Customer"))
        {
            Debug.Log("customerChecked");
            marker.position = endlocation.position;
        }

    }
    private void OnTriggerExit(Collider other)
    {
    //    if (other.CompareTag("Customer"))
        {
            marker.position = startlocation.position;
        }
    }

}
