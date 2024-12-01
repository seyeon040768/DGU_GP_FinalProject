using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
    }
    private void Update()
    {
         animator = GetComponent<Animator>();
    }

    public override void Jump()
    {
        throw new System.NotImplementedException();
    }

    public override void Move(float horizontal)
    {
        throw new System.NotImplementedException();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeHit()
    {
        Debug.Log("Hit");
        animator.SetTrigger("TakeHit");
    }

}
