using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SlideDoor : MonoBehaviour
{
    public GameObject door;
    public Transform marker;
    public Transform startlocation;
    public Transform endlocation;

    [Header("FMOD")]
    [SerializeField]
    private EventReference doorSlideSound;

    private bool isDoorOpen = false;

    void Update()
    {
        door.transform.position = Vector3.Lerp(door.transform.position, marker.transform.position, 1.5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Customer"))
        {
            if (!isDoorOpen)
            {
                isDoorOpen = true;
                marker.position = endlocation.position;

                if (!doorSlideSound.IsNull)
                    RuntimeManager.PlayOneShot(doorSlideSound, door.transform.position);

                Debug.Log("🟢 Door opening sound played.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Customer"))
        {
            if (isDoorOpen)
            {
                isDoorOpen = false;
                marker.position = startlocation.position;

                if (!doorSlideSound.IsNull)
                    RuntimeManager.PlayOneShot(doorSlideSound, door.transform.position);

                Debug.Log("🔴 Door closing sound played.");
            }
        }
    }
}
