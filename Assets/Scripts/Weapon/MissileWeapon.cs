using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    public GameObject bullet;
    [SerializeField] private SFXPool sfxPool;

    public int cartridge;
    public int cartridgeMax;

    public float cartridgeCool;
    public float cartridgeDuration;

    private void Update()
    {
        if (cartridgeCool > 0)
        {
            cartridgeCool -= Time.deltaTime;
            if (cartridgeCool < 0.0f)
            {
                cartridgeCool = 0.0f;
                cartridge = cartridgeMax;
            }
        }

        // Debug.DrawRay(transform.position, Vector3.right, new Color(0, 1, 0));
    }

    public override bool Attack()
    {
        if (cartridge > 0)
        {
            float theta = transform.rotation.eulerAngles.z;
            if (owner.transform.localScale.x < 0)
            {
                theta -= 180.0f;
            }
            GameObject bulletObject = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, theta));
            sfxPool.Play("Gun");
            Bullet bulletScript = bulletObject.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.owner = owner;
            }
            if (ownerCharacter.isUltim)
            {
                bulletScript.damage *= coef;
            }

            --cartridge;

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
                effectObj.transform.localScale.z) * 0.5f;
            Animator animator = effectObj.GetComponent<Animator>();
            attackDuration = GetCurrentAnimationLength(animator);
            Destroy(effectObj, attackDuration);
        }

        if (cartridge <= 0 && cartridgeCool <= 0.0f)
        {
            cartridgeCool = cartridgeDuration;

            sfxPool.Play("GunReload");
        }

        return false;
    }
}
