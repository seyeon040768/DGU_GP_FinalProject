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

    public override void Attack()
    {
        GameObject bulletObject = Instantiate(bullet, transform.position, transform.rotation);
        Bullet bulletScript = bulletObject.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.owner = owner;
        }
    }
}
