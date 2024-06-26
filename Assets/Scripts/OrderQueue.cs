// this will be a script to manage the queue of orders
// view list of orders ?

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderQueue : MonoBehaviour
{
    public List<Order> ordersQueue = new List<Order>();
    public int maxOrders = 2;
    public float orderInterval = 4f; // 20 seconds?!?!?!?
    public int orderNumber = 0;
    public PlayerInput playerInput;
    //public QPCDisplayManager qpcDisplayManager;
    public QPCToppings qpcToppings;
    public QPCFlavors qpcFlavors;
    public QPCSugar qpcSugar;

    public Sussy sussy;

    public AudioSource damnbro;

    void Start()
    {
        StartCoroutine(AddOrdersAtIntervals());
        StartCoroutine(CheckOrderExpirations());
    }

    void Update()
    {
        Clock clock = FindObjectOfType<Clock>();
        orderInterval = 4f * (1 / clock.difficulty);
    }

    private IEnumerator AddOrdersAtIntervals()
    {
        while (true) // infinite loop for continuous order generation
        {
            if (ordersQueue.Count < maxOrders)
            {
                AddRandomOrder();
                Debug.Log("added order! total orders: " + ordersQueue.Count);
                //qpcDisplayManager.UpdateQPCDisplays(ordersQueue);
                qpcToppings.UpdateToppingDisplays(ordersQueue);
                qpcFlavors.UpdateFlavorDisplays(ordersQueue);
                qpcSugar.UpdateSugarDisplays(ordersQueue);
            }
            else
            {
                Debug.Log("dis many in queue:  " + ordersQueue.Count);
                Debug.Log("ququ full");
            }
            yield return new WaitForSeconds(orderInterval);
           
        }
    }

    // order expire
    private IEnumerator CheckOrderExpirations()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); 

            for (int i = ordersQueue.Count - 1; i >= 0; i--)
            {
                if (ordersQueue[i].IsExpired())
                {
                    damnbro.Play();
                    Debug.Log("BITCH IM IMPATIENT." + ordersQueue[i] + "IS REMOVED FROM QUEUE.");

                    ordersQueue.RemoveAt(i);
                    playerInput.removeQPC(i);
                    sussy.susLvl += 10;
                }
            }
        }
    }

    void DisplayOrders()
    {
        Debug.Log("Orders:");
        foreach (Order order in ordersQueue)
        {
            Debug.Log(order);
        }
    }

    public Order GetCurrentOrder()
    {
        if (ordersQueue.Count > 0)
        {
            return ordersQueue[0]; // first order
        }
        return null; // none!
    }

    // heheheh
    public void ServeCurrentOrder()
    {
        if (ordersQueue.Count > 0)
        {
            //orderNumber--;
            //qpcDisplayManager.UpdateQPCDisplays(ordersQueue);
            //qpcToppings.UpdateToppingDisplays(ordersQueue);
            //qpcFlavors.UpdateFlavorDisplays(ordersQueue);
            //qpcSugar.UpdateSugarDisplays(ordersQueue);
        }
    }

    // new order

        public void AddRandomOrder()
        {

            Order newOrder = Order.GenerateRandomOrder();
            ordersQueue.Add(newOrder);
            orderNumber++;

            Debug.Log("Added new order: " + newOrder);
            DisplayOrders();
        //qpcDisplayManager.UpdateQPCDisplays(ordersQueue);
         qpcToppings.UpdateToppingDisplays(ordersQueue);
         qpcFlavors.UpdateFlavorDisplays(ordersQueue);
         qpcSugar.UpdateSugarDisplays(ordersQueue);
    }
}
