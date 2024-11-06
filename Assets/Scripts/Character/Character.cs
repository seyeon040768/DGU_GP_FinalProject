using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float hp;
    private float maxhp;

    public float mp;
    private float maxmp;

    public float speed;
    private float maxspeed;

    public float jumpForce;
    private float maxJumpForce;
    public bool isGrounded;

    void Start()
    {
        maxhp = hp;
        maxmp = mp;
        maxspeed = speed;
        maxJumpForce = jumpForce;
    }

    public float Hp
    {
        get { return hp; }
        set
        {
            this.hp = value;
            if (this.hp < 0)
            {
                this.hp = 0;
            }
            else if (this.hp > maxhp)
            {
                this.hp = maxhp;
            }
        }
    }
    public float Mp
    {
        get { return mp; }
        set
        {
            this.mp = value;
            if (this.mp < 0)
            {
                this.mp = 0;
            }
            else if (this.mp > maxmp)
            {
                this.mp = maxmp;
            }
        }
    }
    public float Speed
    {
        get { return speed; }
        set
        {
            this.speed = value;
            if (this.speed < 0)
            {
                this.speed = 0;
            }
            else if (this.speed > maxspeed)
            {
                this.speed = maxspeed;
            }
        }
    }
    public float JumpForce
    {
        get { return jumpForce; }
        set
        {
            this.jumpForce = value;
            if (this.jumpForce < 0)
            {
                this.jumpForce = 0;
            }
            else if (this.jumpForce > maxJumpForce)
            {
                this.jumpForce = maxJumpForce;
            }
        }
    }

    public abstract void Jump();
    public abstract void Move(float horizontal);
}
