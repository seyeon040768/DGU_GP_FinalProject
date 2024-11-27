using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    private BoxCollider2D boxCollider2D;

    protected override void Start()
    {
        base.Start();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        
    }

    public override bool Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == owner)
            {
                continue;
            }

            if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
            {
                Character character = collider.GetComponent<Character>();

                if (ownerCharacter.isUltim)
                {
                    character.Hp -= Damage * coef;
                }
                else
                {
                    character.Hp -= Damage;
                }
                
                character.TakeHit();

                return true;
            }
        }

        return false;
    }
}
