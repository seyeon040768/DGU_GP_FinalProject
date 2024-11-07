using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject owner; // 무기 소유자

    [SerializeField] private float damage;
    private float maxDamage;
    public float Damage
    {
        get { return damage; }
        set
        {
            damage = value;
            if (damage < 0)
            {
                damage = 0;
            }
            else if (damage > maxDamage)
            {
                damage = maxDamage;
            }
        }
    }

    public abstract void Attack();
}
