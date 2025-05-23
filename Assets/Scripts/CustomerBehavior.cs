using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    public float threshold = 0.1f;

    private List<Transform> queueSpots;
    private int queueIndex;
    private bool isWaiting = false;
    private bool hasOrdered = false;
    private bool happy = false;

    public Transform exitPoint;
private bool walkingToExit = false;


    private Vector3 target;
    private NpcQueueManager manager;

    public void AssignQueue(List<Transform> spots, int index, NpcQueueManager mgr)
    {
        queueSpots = spots;
        manager = mgr;
        MoveToQueueSpot(index);
    }

    public void MoveToQueueSpot(int index)
    {
        queueIndex = index;

        if (queueSpots != null && queueSpots.Count > index)
        {
            target = queueSpots[index].position;
        }
        else
        {
            Debug.LogError($"❌ QueueSpot index {index} is invalid or not assigned.");
        }

        isWaiting = false;
    }

    public void TryOrder()
    {
        if (!hasOrdered && queueIndex == 0)
        {
            hasOrdered = true;

            var signal = FindObjectOfType<DrinkSignalManager>();
            if (signal != null)
            {
                signal.BeginOrder();
                Debug.Log("🧍 Customer placed an order.");
            }
        }
    }

        void Update()
    {
        if (isWaiting) return;

        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, 1.0f * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target);
        if (distance < threshold)
        {
            if (walkingToExit)
            {
                Destroy(gameObject);
                return;
            }

            if (!hasOrdered && queueIndex == 0)
            {
                isWaiting = true;
                TryOrder();
            }
        }
    }



    public void SetHappy()
{
    if (!happy)
    {
        happy = true;
        isWaiting = false;

        manager.OnCustomerServed();

        if (exitPoint != null)
        {
            target = exitPoint.position; 
            walkingToExit = true;        
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}


}
