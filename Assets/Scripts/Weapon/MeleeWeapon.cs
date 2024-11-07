using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    private BoxCollider2D boxCollider2D;
    private Vector2 boxSize;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxSize = boxCollider2D.size;
    }

    public override void Attack()
    {
        Debug.Log("Attack");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == owner)
            {
                continue;
            }

            if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
            {
                collider.GetComponent<Character>().Hp -= Damage;
            }
        }
    }
}
