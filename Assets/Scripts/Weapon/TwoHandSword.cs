using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : MeleeWeapon
{
    public int weaponNum;
    protected override void Start()
    {
        base.Start();

        weaponNum = 0;
    }

    public override bool Attack()
    {
        weaponNum = (weaponNum + 1) % 2;

        return base.Attack();
    }
}
