using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 mouseStartPOS, mouseEndPOS;
    Vector3 direction;
    [SerializeField]
    float speed = 200f;

    Rigidbody rb;
    Animator anim;

    public GameObject characterModel;

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
}
