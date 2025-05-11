using System.Collections.Generic;
using UnityEngine;

public class NpcQueueManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public List<Transform> queueSpots;
    public DrinkSignalManager drinkSignal;

    public Transform exitPoint;


    private Queue<CustomerBehavior> customerQueue = new Queue<CustomerBehavior>();
    private bool isProcessing = false;

    public float spawnInterval = 5f;
    public int maxQueue = 4;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 0f, spawnInterval);
    }

    void SpawnCustomer()
    {
        if (customerQueue.Count >= maxQueue) return;

        GameObject go = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        CustomerBehavior customer = go.GetComponent<CustomerBehavior>();

        int index = customerQueue.Count;
        customer.AssignQueue(queueSpots, index, this);

        customerQueue.Enqueue(customer);

        customer.exitPoint = exitPoint;
    }

    public void OnCustomerServed()
    {
        customerQueue.Dequeue();

        int i = 0;
        foreach (var cust in customerQueue)
        {
            cust.MoveToQueueSpot(i);
            i++;
        }

        if (customerQueue.Count > 0)
        {
            customerQueue.Peek().TryOrder();
        }
    }
}
