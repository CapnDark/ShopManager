using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Vector3 mouseStartPOS, mouseEndPOS;
    Vector3 direction;
    [SerializeField]
    float speed = 200f;
    float timeToMakeItem = 2.5f;

    Rigidbody rb;
    Animator anim;

    public GameObject characterModel;
    public Transform trayInstaitePoint;
    Image timerImage;

    public int croissantCount = 0;
    public int cupcakeCount = 0;
    public int doughnutCount = 0;

    bool isMakingItem = false;
    bool isCarrying = false;

    List<ItemScript> trayStack = new List<ItemScript>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(isCarrying)
            {
                anim.SetBool("isCarrying", true);
            }
            anim.SetBool("isWalking", true);
            PlayerMove();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void PlayerMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPOS = Input.mousePosition;
            mouseEndPOS = Input.mousePosition;
        }

        if(Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            mouseEndPOS = Input.mousePosition;
        }

        direction = mouseEndPOS - mouseStartPOS;
        direction.z = direction.y;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            rb.velocity = direction.normalized * speed * Time.deltaTime;
            characterModel.transform.forward = direction.normalized;
        }

    }

    void SellItem(int itemId)
    {
        for (int i = trayStack.Count-1; i >= 0; i--)
        {
            ItemScript item = trayStack[i];

            if (item.itemId == itemId && GameManager.openOrder)
            {
                GameManager.openOrder = false;
                trayStack.Remove(item);
                Destroy(item.gameObject);
            }
        }

        int tempPOS = 0;
        foreach (ItemScript item in trayStack)
        {
            item.positionInStack = tempPOS++;
        }

        switch (itemId)
        {
            case 0:
                croissantCount--;
                break;

            case 1:
                cupcakeCount--;
                break;

            case 2:
                doughnutCount--;
                break;
        }

        if(croissantCount+cupcakeCount+doughnutCount <= 0)
        {
            anim.SetBool("isCarrying", false);
            isCarrying = false;
        }
    }

    void TakeItem(int itemID)
    {
        timerImage = GameManager.gm.timers[itemID];

        if (isMakingItem)
        {
            timerImage.gameObject.SetActive(true);

            timeToMakeItem -= Time.deltaTime;
            float fraction = timeToMakeItem / 2.5f;
            Debug.Log(fraction / timeToMakeItem * Time.deltaTime);
            timerImage.fillAmount += (fraction / timeToMakeItem * Time.deltaTime);
            if (timeToMakeItem < 0)
            {
                switch (itemID)
                {
                    case 0:
                        croissantCount++;
                        break;

                    case 1:
                        cupcakeCount++;
                        break;

                    case 2:
                        doughnutCount++;
                        break;
                }

                InstatiateItem(itemID);
                ResetTime();
            }
        }
    }

    void InstatiateItem(int itemID)
    {
        GameObject item = Instantiate(GameManager.gm.itemTrays[itemID], trayInstaitePoint);
        Vector3 temp = item.transform.localPosition;
        temp.y = (croissantCount + cupcakeCount + doughnutCount)-1;
        item.transform.localPosition = temp;

        ItemScript tempItem = item.GetComponent<ItemScript>();
        tempItem.positionInStack = (int)temp.y;
        trayStack.Add(tempItem);

        isCarrying = true;
        anim.SetBool("isCarrying", true);
    }

    void ResetTime()
    {
        timeToMakeItem = 2.5f;
        timerImage.fillAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Croissant") || other.CompareTag("Cupcake") || other.CompareTag("Doughnut"))
        {
            isMakingItem = true;
        }

        if (other.CompareTag("CustomerSlot"))
        {            
            foreach(ItemScript item in trayStack)
            {
                if(item.itemId == other.GetComponent<CustomerRequestScript>().requestedItemId)
                {
                    SellItem(other.GetComponent<CustomerRequestScript>().requestedItemId);
                    other.GetComponent<CustomerRequestScript>().hasRecievedItem = true;
                    return;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Croissant":
                TakeItem(0);
                break;

            case "Cupcake":
                TakeItem(1);
                break;

            case "Doughnut":
                TakeItem(2);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Croissant") || other.CompareTag("Cupcake") || other.CompareTag("Doughnut"))
        {
            isMakingItem = false;
            ResetTime();
            if (timerImage != null)
            {
                timerImage.gameObject.SetActive(false);
            }

        }
    }
}
