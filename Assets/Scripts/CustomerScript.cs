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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

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
    }

    void MoveCustomer()
    {
        if(!hasReachedPoint)
        {
            anim.SetBool("isWalking", true);
            agent.SetDestination(GameManager.gm.customerSlot.position);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}
