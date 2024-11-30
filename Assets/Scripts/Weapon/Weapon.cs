using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject owner; // 무기 소유자
    protected Character ownerCharacter;

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

    public float attackDuration;
    public float coef;


    protected virtual void Start()
    {
        ownerCharacter = owner.GetComponent<Character>();
    }

    public abstract bool Attack();
    protected virtual void isPlayerDead()
    {
        if (ownerCharacter.Hp == 0)
        {
            gameObject.SetActive(false);
        }
    }


}
