using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float hp;
    public float maxhp;

    public float mp;
    public float maxmp;

    public float speed;
    public float maxspeed;

    public float jumpForce;
    public float maxJumpForce;
    public bool isGrounded;

    void Start()
    {
        maxhp = hp;
        maxmp = mp;
        maxspeed = speed;
        maxJumpForce = jumpForce;
    }

    void Update()
    {
        
    }

    public abstract void Jump();
    public abstract void Move(float horizontal);
}
