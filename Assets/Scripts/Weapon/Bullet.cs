using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;
    public float damage;
    public float speed;
    public float duration;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform")
            || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            character.Hp -= damage;
            character.TakeHit();

            Player player = owner.GetComponent<Player>();
            if (player != null)
            {
                player.AddCombo();
            }

            Destroy(gameObject);
        }
    }
}
