using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    public GameObject bullet;

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.right, new Color(0, 1, 0));
    }

    public override bool Attack()
    {
        float theta = transform.rotation.eulerAngles.z;
        if (owner.transform.localScale.x < 0)
        {
            theta -= 180.0f;
        }
        GameObject bulletObject = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, theta));
        Bullet bulletScript = bulletObject.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.owner = owner;
        }
        if (ownerCharacter.isUltim)
        {
            bulletScript.damage *= coef;
        }

        return false;
    }
}
