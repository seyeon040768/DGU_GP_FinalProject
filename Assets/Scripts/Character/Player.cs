using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int facingWay; // ����(-) ������(+)

    public float dashDistance;
    public float dashRecoveryDuration;
    private float dashRecoveryCool;

    public int maxstamina;
    private int stamina;
    public int Stamina
    {
        get { return stamina; }
        set
        {
            this.stamina = value;
            if (this.stamina < 0)
            {
                this.stamina = 0;
            }
            else if (this.stamina > maxstamina)
            {
                this.stamina = maxstamina;
            }
        }
    }

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

    protected override void Start()
    {
        base.Start();
        Stamina = maxstamina;

        facingWay = (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));

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

    void Update()
    {
        ManageCoolTime();
        ManageAnimation();
        RotateWeapon();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground", "Platform"));
        isGrounded = rayHit.collider != null && (rayHit.distance < jumpRayDistanceThres * 1.1f && rayHit.distance > jumpRayDistanceThres * 0.9f);
        isGrounded = isGrounded && rb.velocity.y < 0.1f; // ���� �ö󰡴� ���̸� ���� �Ұ�

        if (isAttacking)
        {
            return;
        }

        if (isGrounded && Input.GetButtonDown("Fire1")) // ���� ���� ���� ���� �Ұ�
        {
            this.Attack();
            return; // ���� ��ư Ŭ�� �� �̵� ����
        }

        if (isGrounded && stamina > 0 && Input.GetMouseButtonDown(1))
        {
            this.Dash();
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.Return) && scanObject != null)
        {
            manager.Action(scanObject);
        }


        Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));
    }

    private void ManageCoolTime()
    {
        dashRecoveryCool -= Time.deltaTime;
        if (dashRecoveryCool < 0)
        {
            dashRecoveryCool = dashRecoveryDuration;
            ++Stamina;
        }
    }

    private void ManageAnimation()
    {
        if (isAttacking)
        {
            animator.SetBool("isMove", false);
            return;
        }

        if (rb.velocity.y > 0 && !isGrounded) // �����ؼ� ���� �ö󰡴� ����
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isFall", false);
            animator.SetBool("isMove", false);
        }
        else if (rb.velocity.y < 0 && !isGrounded) // �Ʒ��� �������� ����
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
            animator.SetBool("isMove", isMoving);
        }
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
            isMoving = true;
            if (horizontal < 0)
            {
                facingWay = -1;
            }
            else
            {
                facingWay = 1;
            }

            transform.localScale = new Vector3(2.0f * facingWay, 2.0f, 2.0f);
            transform.position += new Vector3(horizontal, 0, 0) * (Speed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

    public void Dash()
    {
        --Stamina;
        transform.position += new Vector3(dashDistance * facingWay, 0.0f, 0.0f);
    }

    public override void Attack()
    {
        isAttacking = true;

        int attackNum = Random.Range(0, attackAnimHash.Length);
        animator.SetTrigger(attackAnimHash[attackNum]);
        weapon.GetComponent<Weapon>().Attack();

        float attackAnimDuration = GetAnimationClipLength(attackAnimName[attackNum]);
        Invoke(nameof(EndAttack), attackAnimDuration);
    }
    private void EndAttack()
    {
        isAttacking = false; // ���� ���� ����
    }

    private void RotateWeapon()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToMouse = mousePos - new Vector2(transform.position.x, transform.position.y);
        float theta = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
        if (facingWay > 0)
        {
            weapon.transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);
        }
        else
        {
            weapon.transform.rotation = Quaternion.AngleAxis(theta - 180.0f, Vector3.forward);
        }
    }

    public override void TakeHit()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ObjectData�� �ִ��� Ȯ���Ͽ� scanObject�� ����
        if (collision.GetComponent<ObjectData>() != null)
        {
            scanObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger���� ����� scanObject�� �ʱ�ȭ
        if (collision.gameObject == scanObject)
        {
            scanObject = null;
        }
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


    // utils
    float GetAnimationClipLength(string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length; // �ִϸ��̼� Ŭ���� ���̸� ��ȯ
            }
        }
        return 0f;
    }
}

