using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    private bool moveLeft;
    private bool moveRight;
    private bool moveForward;
    private bool moveBackward;
    private float horizontalMove;
    private float frontbackMove;

    public float speed = 0.3F;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        moveLeft = false;
        moveRight = false;
        moveForward = false;
        moveBackward = false;

    }

    //when pressing the left button
    public void PointerDownLeft()
    {
        moveLeft = true;

    }
    //when not pressing the left button
    public void PointerUpLeft()
    {
        moveLeft = false;

    }

    //when pressing the right button
    public void PointerDownRight()
    {
        moveRight = true;

    }
    //when not pressing the right button
    public void PointerUpRight()
    {
        moveRight = false;

    }
    //when pressing the forward button
    public void PointerDownForward()
    {
        moveForward = true;

    }

    //when not pressing the forward button
    public void PointerUpForward()
    {
        moveForward = false;

    }
    //when pressing the backward button
    public void PointerDownBackward()
    {
        moveBackward = true;

    }

    //when not pressing the backward button
    public void PointerUpBackward()
    {
        moveBackward = false;

    }

    //When pressing acerleration
    public void AccelerateDown()
    {
        speed = 0.8F;

    }

    //When not pressing acerleration
    public void AccelerateUp()
    {
        speed = 0.3F;

    }

    // Update is called once per frame
    void Update()
    {
        MovementPlayer();
    }


    private void MovementPlayer()
    {
        if (moveLeft)
        {
            horizontalMove = -speed;
        }

        //if i press the right button
        else if (moveRight)
        {
            horizontalMove = speed;
        }

        else if (moveForward)
        {
            frontbackMove = speed;
        }

        else if (moveBackward)
        {
            frontbackMove = -speed;
        }

        //if i am not pressing any button
        else
        {
            horizontalMove = 0;
            frontbackMove = 0;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalMove, rb.velocity.y, frontbackMove);
    }

}
