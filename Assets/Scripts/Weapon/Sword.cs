using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    [SerializeField] private SFXPool sfxPool;


    protected override void Start()
    {
        base.Start();
    }

    public override bool Attack()
    {
        sfxPool.Play("Sword");
        return base.Attack();
    }

}
