using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        
    }

    public override void Attack()
    {
        Debug.Log("Attack");
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
                character.Hp -= Damage;
                character.TakeHit();
            }
        }
    }
}
