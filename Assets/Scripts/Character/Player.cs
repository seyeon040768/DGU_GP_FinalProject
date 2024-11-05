using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Rigidbody2D rb;
    private Collider2D col;
    private PlatformEffector2D currentPlatform;

    private float jumpRayDistanceThres; // 바닥에 도착했음을 인정할 오브젝트 중심에서 바닥으로 향하는 ray의 최대 거리


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        jumpRayDistanceThres = col.bounds.extents.y;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        this.Move(horizontal);

        if (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && vertical > 0)) // 점프
        {
            this.Jump();
        }

        if (vertical < 0 && currentPlatform != null) // 아래로 내려가기
        {
            StartCoroutine(DisablePlatformCollision());
        }

        Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
    }

    public override void Jump()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground"));

        print(rayHit.distance);
        isGrounded = rayHit.collider != null && (rayHit.distance < jumpRayDistanceThres * 1.1f && rayHit.distance > jumpRayDistanceThres * 0.9f);
        if (!this.isGrounded)
        {
            Debug.Log("Player Jump(ignored)");
            return;
        }
        Debug.Log("Player Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Move(float horzontal)
    {
        transform.position += new Vector3(horzontal, 0, 0) * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            currentPlatform = collision.gameObject.GetComponent<PlatformEffector2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCollider, true);
        yield return new WaitForSeconds(1.0f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCollider, false);
    }
}
