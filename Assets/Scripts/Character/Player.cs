using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public string[] attackAnimName;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlatformEffector2D currentPlatform;

    public GameManager manager; // ��ȭâ ���� ��
    private int[] attackAnimHash;
    private float jumpRayDistanceThres; // �ٴڿ� ���������� ������ ������Ʈ �߽ɿ��� �ٴ����� ���ϴ� ray�� �ִ� �Ÿ�
    GameObject scanObject;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackAnimHash = new int[attackAnimName.Length];
        for (int i = 0; i < attackAnimName.Length; i++)
        {
            attackAnimHash[i] = Animator.StringToHash(attackAnimName[i]);
        }
        jumpRayDistanceThres = col.bounds.extents.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ObjectData�� �ִ��� Ȯ���Ͽ� scanObject�� ����
        if (collision.GetComponent<ObjectData>() != null)
        {
            scanObject = collision.gameObject;
            Debug.Log("NPC ������: " + scanObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger���� ����� scanObject�� �ʱ�ȭ
        if (collision.gameObject == scanObject)
        {
            scanObject = null;
            Debug.Log("NPC �������� ���");
        }
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground", "Platform"));
        isGrounded = rayHit.collider != null && (rayHit.distance < jumpRayDistanceThres * 1.1f && rayHit.distance > jumpRayDistanceThres * 0.9f);
        isGrounded = isGrounded && rb.velocity.y < 0.1f; // ���� �ö󰡴� ���̸� ���� �Ұ�

        this.Move(horizontal);

        if ((Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && vertical > 0)) 
            && isGrounded) // ����
        {
            this.Jump();
        }

        if ((Input.GetButtonDown("Vertical") && vertical < 0) && currentPlatform != null) // �Ʒ��� ��������
        {
            StartCoroutine(DisablePlatformCollision());
        }

        if (Input.GetButtonDown("Fire1"))
        {
            this.Attack();
        }

        if (rb.velocity.y > 0 && !isGrounded)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isFall", false);
            animator.SetBool("isMove", false);
        }
        else if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
        }

        if (Input.GetKeyDown(KeyCode.Return) && scanObject != null)
        {
            Debug.Log("NPC�� ��ȣ�ۿ� �õ�");
            manager.Action(scanObject);
        }


        Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));

    }

    public override void Jump()
    {
        animator.SetBool("isJump", true);
        animator.SetBool("isFall", false);
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    public override void Move(float horizontal)
    {
        if (horizontal != 0)
        {
            if (horizontal < 0)
            {
                transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
            }
            else
            {
                transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            }
            animator.SetBool("isMove", true);

            transform.position += new Vector3(horizontal, 0, 0) * (Speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    public override void Attack()
    {
        int attackNum = Random.Range(0, attackAnimHash.Length);
        animator.SetTrigger(attackAnimHash[attackNum]);
        weapon.GetComponent<Weapon>().Attack();
    }

    public override void TakeHit()
    {
        throw new System.NotImplementedException();
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
