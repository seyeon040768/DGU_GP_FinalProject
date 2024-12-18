using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool isInvincible;
    public bool isDeath;

    public float maxhp;
    private float hp;
    protected Coroutine blink;
    protected WaitForSeconds delay = new WaitForSeconds(0.1f);
    protected SpriteRenderer sprite;

    public float Hp
    {
        get { return hp; }
        set
        {
            if (isInvincible && value < this.hp)
            { // 무적 상태인 경우 체력 감소 방지
                return;
            }

            this.hp = value;
            if (this.hp <= 0)
            {
                isDeath = true;
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

    public bool isMoving;
    public bool isAttacking;
    public bool isUltim;

    public float attackDuration;
    public float attackCool;

    protected virtual void Start()
    {
        isInvincible = false;
        isDeath = false;

        Hp = maxhp;
        Mp = maxmp;
        Speed = maxspeed;
        JumpForce = maxJumpForce;
        attackCool = 0.0f;
        isAttacking = false;

        isUltim = false;
    }

    public abstract void Jump();
    public abstract void Move(float horizontal);
    public abstract void Attack();
    public abstract void TakeHit();


}
