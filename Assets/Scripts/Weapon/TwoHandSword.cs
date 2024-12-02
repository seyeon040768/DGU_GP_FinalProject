using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : MeleeWeapon
{
    [SerializeField] private SFXPool sfxPool;
    public int weaponNum;
    protected override void Start()
    {
        base.Start();

        weaponNum = 0;
    }

    public override bool Attack()
    {
        weaponNum = (weaponNum + 1) % 2;
        sfxPool.Play("TwoSword");
        Quaternion rotation = transform.rotation;
        Vector3 position = transform.position;
        int flag = 1;
        if (owner.transform.localScale.x < 0)
        {
            flag = -1;

            position -= transform.right * effectOffset;
        }
        else
        {
            position += transform.right * effectOffset;
        }
        GameObject effectObj = Instantiate(effect, position, rotation);
        effectObj.transform.localScale = new Vector3(flag * effectObj.transform.localScale.x,
            effectObj.transform.localScale.y,
            effectObj.transform.localScale.z);
        Animator animator = effectObj.GetComponent<Animator>();
        attackDuration = GetCurrentAnimationLength(animator);
        Destroy(effectObj, attackDuration);

        return base.Attack();
    }
}
