using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CustomerScript : MonoBehaviour
{
    int randomItemNo;
    Image spritePlaceHolder;
    Transform targetPos;

    public GameObject thoughtCloud;
    public List<Sprite> itemPic = new List<Sprite>();

    bool hasReachedPoint = false;
    bool hasRecievedItem = false;

    private NavMeshAgent agent;
    private Animator anim;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        randomItemNo = Random.Range(0, 3);
        spritePlaceHolder = thoughtCloud.transform.GetChild(0).GetComponent<Image>();

        targetPos = GameManager.gm.customerSlot;
    }

    // Update is called once per frame
    void Update()
    {
        if(thoughtCloud.activeSelf)
        {
            thoughtCloud.transform.LookAt(Camera.main.transform);
        }

        if(Vector3.Distance(transform.position, targetPos.position) < 0.1f && !hasReachedPoint)
        {
            hasReachedPoint = true;
            RequestRandomItem();

            if (targetPos == GameManager.gm.customerInstantiatePoint)
            {
                GameManager.customersInScene.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        else
        {
            MoveCustomer();
        }

        if(hasRecievedItem)
        {
            targetPos = GameManager.gm.customerInstantiatePoint;
            GameManager.isCustomerReady = false;
        }
    }

    void RequestRandomItem()
    {
        spritePlaceHolder.sprite = itemPic[randomItemNo];
        thoughtCloud.SetActive(true);
        GameManager.openOrder = true;
    }

    void MoveCustomer()
    {
        if(!hasReachedPoint)
        {
            anim.SetBool("isWalking", true);
            agent.SetDestination(targetPos.position);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CustomerSlot"))
        {
            other.GetComponent<CustomerRequestScript>().requestedItemId = randomItemNo;
            other.GetComponent<CustomerRequestScript>().hasRecievedItem = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CustomerSlot") && other.GetComponent<CustomerRequestScript>().hasRecievedItem)
        {
            thoughtCloud.SetActive(false);

            hasRecievedItem = true;
            hasReachedPoint = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CustomerSlot"))
        {
            other.GetComponent<CustomerRequestScript>().requestedItemId = -1;
            other.GetComponent<CustomerRequestScript>().hasRecievedItem = false;
        }
    }
}
