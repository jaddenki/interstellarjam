// user input of you going to the different stations
// Need to include how to select different stuff from stations
// need to include how to select which customer to serve at the end

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerInput : MonoBehaviour
{
    public Sussy sussy;
    public int currentStation = 0;
    public int moneyCount = 0;
    public TextMeshProUGUI moolah;
    // placeholder selection options
    public enum Flavor { Taro, Matcha, FruitTea, Thai}
    public enum SugarLevel { ZeroPercent, TwentyFivePercent, FiftyPercent, OneHundredPercent }
    public enum Topping { Tapioca, Pudding, AloeVera, LycheeJelly }

    public Flavor selectedFlavor;
    public SugarLevel selectedSugar;
    public Topping selectedTopping;

    private object[] recentSelections = new object[3];
    private object[] currentOrderArray = new object[3]; // customer order

    // sprites for each station
    public Sprite startSprite;
    public Sprite flavorSprite;
    public Sprite space1Sprite;
    public Sprite iceSugarSprite;
    public Sprite space2Sprite;
    public Sprite toppingsSprite;
    public Sprite trynaServeSprite;

    private SpriteRenderer spriteRenderer;

    public GameObject flavorStationUI;
    public GameObject sugarLevelStationUI;
    public GameObject toppingsStationUI;
    public GameObject Qpc2;
    public GameObject Qpc3;
    public int decSus = -5;
    public int increaseTheSus = 8;
    public GameObject[] qpcImages; // Array of QPC images



    private BoxCollider2D qpc2Collider;
    private BoxCollider2D qpc3Collider;

    public OrderQueue orderQueue;

    // for the delay!!!
    private float inputDelay = 0.2f;  // how long u want
    private float lastInputTime = 0f;

    public DisplayDrink displayDrink;

    public AudioSource move;
    public AudioSource nicejob;
    public AudioSource niceonedumbass;

    void Start()
    {
        // sets initial sprite to the blank one
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = startSprite;

        // order queue
        if (orderQueue == null)
        {
            orderQueue = FindObjectOfType<OrderQueue>();
        }
        //orderQueue.AddRandomOrder(); // first order

        qpc2Collider = Qpc2.GetComponent<BoxCollider2D>();
        qpc3Collider = Qpc3.GetComponent<BoxCollider2D>();

        qpcImages = new GameObject[] { Qpc2, Qpc3};

        displayDrink.LetGo();
        canClickQPC(false);
        moolah.text = moneyCount.ToString();

    }



    void Update()
    {
        // when u wanna switch stations
        if (Time.time - lastInputTime >= inputDelay) // did enough time pass ?!?!
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentStation > 0)
                {
                    currentStation--;
                    lastInputTime = Time.time;
                    move.Play();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentStation < 5)
                {
                    currentStation++;
                    lastInputTime = Time.time;
                    move.Play();
                }
            }
        }

            // selecting stuff at their stations
            switch (currentStation)
         {
            case 0:
               // Debug.Log("Flavor Station");
                // under here we want it to open the station UI and show 
                spriteRenderer.sprite = flavorSprite;
                flavorStationUI.SetActive(true);
                break;
            case 1:
               // Debug.Log("Open Space 1");
                flavorStationUI.SetActive(false);
                sugarLevelStationUI.SetActive(false);
                spriteRenderer.sprite = space1Sprite;
                break;
            case 2:
               // Debug.Log("Sugar Level Station");
                spriteRenderer.sprite = iceSugarSprite;
                sugarLevelStationUI.SetActive(true);
                break;
            case 3:
               // Debug.Log("Open Space 2");
                sugarLevelStationUI.SetActive(false);
                toppingsStationUI.SetActive(false);
                spriteRenderer.sprite = space2Sprite;
                break;
            case 4:
                // Debug.Log("Toppings Station");

                spriteRenderer.sprite = toppingsSprite;
                toppingsStationUI.SetActive(true);
                canClickQPC(false);
                break;
            case 5:
               // Debug.Log("Serve Babe");
                toppingsStationUI.SetActive(false);

                canClickQPC(true);

                spriteRenderer.sprite = trynaServeSprite;
                break;
        }

        // display customer queue
        switch(orderQueue.orderNumber % 2)
        {
            case 1:
                Qpc2.SetActive(true);
                qpcImages[0].SetActive(true);
                break;
            case 0:
                Qpc3.SetActive(true);
                qpcImages[1].SetActive(true);
                break;
            default:
                Qpc2.SetActive(false);
                Qpc3.SetActive(false);
                break;

        }
  
    }



    public void removeQPC(int i)
    {
    switch(i)
        {
            case 0:
                Qpc2.SetActive(false);
                Debug.Log("bye qpc2");
                qpcImages[0].SetActive(false);
                break;
            case 1:
                Qpc3.SetActive(false);
                Debug.Log("bye qpc3");
                qpcImages[1].SetActive(false);
                break;
            default:
                Qpc2.SetActive(false);
                Qpc3.SetActive(false);
                break;

        }
  
    }

    public void ServeOrder(int customerIndex)
    {

        try
        {
            Debug.Log("Now serving customer: " + customerIndex);


            //Order currentOrder = orderQueue.ordersQueue[1 - customerIndex & 0];
            Order currentOrder = orderQueue.ordersQueue[customerIndex % 2];

            Debug.Log("current order " + currentOrder.ToString());
            if (currentOrder != null)
            {

                currentOrderArray[0] = currentOrder.FlavorChoice;
                currentOrderArray[1] = currentOrder.SugarChoice;
                currentOrderArray[2] = currentOrder.ToppingChoice;

                if (CheckOrderMatch(currentOrderArray[0], currentOrderArray[1], currentOrderArray[2]))
                {
                    nicejob.Play();
                    Debug.Log("slayed!");
                    Debug.Log("Order served. Remaining Orders: " + orderQueue.ordersQueue.Count);


                    Debug.Log("Im trying to remove at " + (customerIndex % 2));
                    orderQueue.ordersQueue.RemoveAt((customerIndex % 2));
                    removeQPC(customerIndex % 2);
                    orderQueue.ServeCurrentOrder();
                    displayDrink.LetGo();

                    moneyCount += 6;
                    moolah.text = moneyCount.ToString();
                }
                else
                {
                    niceonedumbass.Play();
                    Debug.Log("WRONGGGGGG.");
                    moneyCount -= 2;
                    moolah.text = moneyCount.ToString();  
                    sussy.IncSus(8);
                                      
                }
            }
            else
            {
                Debug.Log("none");
            }
        }
        catch (NullReferenceException ex) // in case they try to submit with an item not selected
        {
            Debug.Log("WRONGGGGGG. loser");
        }
    }
    
        // update selection array
        public void UpdateSelection(object selection)
        {
        switch (currentStation)
        {
            case 0:
                recentSelections[0] = selection;
                //displayDrink.UpdateMaHand(selection, 0);
                break;
            case 2:
                recentSelections[1] = selection;
                //displayDrink.UpdateMaHand(selection, 1);
                break;
            case 4:
                recentSelections[2] = selection;
                //displayDrink.UpdateMaHand(selection, 2);
                break;
        }

        // print to debug log
        string selectionsLog = "Flavor: " + recentSelections[0];
        selectionsLog += ", Ice Sugar Level: " + recentSelections[1];
        selectionsLog += ", Topping: " + recentSelections[2];
        Debug.Log(selectionsLog);
        }

    private bool CheckOrderMatch(object flav, object sugar, object top)
    {
     object[] super = new object[3]; // customer order
       super[0] = flav;
       super[1] = sugar;
       super[2] = top;
        for (int i = 0; i < recentSelections.Length; i++)
        {
            Debug.Log($"{recentSelections[i]} x {super[i]}");
            if (recentSelections[i].ToString() != super[i].ToString())
            {
                return false;
            }
        }
        return true;
    }

    private void canClickQPC(bool yas)
    {
        qpc2Collider.enabled = yas;
        qpc3Collider.enabled = yas;
    }

}
