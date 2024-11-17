using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxhp;
    private float hp;
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

    public float maxmp;
    private float mp;
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

    public float maxspeed;
    private float speed;
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

    public float maxJumpForce;
    private float jumpForce;
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
    public bool isGrounded;

    public bool isAttacking;

    public GameObject weapon;

    protected virtual void Start()
    {
        Hp = maxhp;
        Mp = maxmp;
        Speed = maxspeed;
        JumpForce = maxJumpForce;

        isAttacking = false;
    }

    public abstract void Jump();
    public abstract void Move(float horizontal);
    public abstract void Attack();
    public abstract void TakeHit();
}
