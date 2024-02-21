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


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
