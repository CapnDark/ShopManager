using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform customerSlot;
    public Transform customerInstantiatePoint;

    public static bool isCustomerReady;
    public static bool openOrder = true;
    public static GameManager gm;

    public List<Image> timers = new List<Image>();
    public List<GameObject> itemTrays = new List<GameObject>();

    public GameObject customerPrefab;
    public GameObject tutorialBox;

    public static List<GameObject> customersInScene = new List<GameObject>();
    public List<GameObject> customerSlotsInScene = new List<GameObject>();
    bool canStartGame = false;

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(this);
        }
        else
        {
            gm = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if(PlayerPrefs.GetInt("Tutorial") == 0)
            {
                tutorialBox.SetActive(true);
            }
            else
            {
                canStartGame = true;
            }
        }
        else
        {
            tutorialBox.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(canStartGame && Random.Range(0,10) > 5)
        {
            if(customersInScene.Count < customerSlotsInScene.Count)
            {
                GameObject newCustomer = Instantiate(customerPrefab);
                customersInScene.Add(newCustomer);
            }
        }

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Instantiate(customerPrefab);
        //}
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        tutorialBox.SetActive(false);
        canStartGame = true;
    }
}
