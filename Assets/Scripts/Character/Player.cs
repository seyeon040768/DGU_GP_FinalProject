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

    public int combo;
    public float comboDuration;
    private float comboCool;

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

        combo = 0;

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
        // �ʼ� ���� �ڵ� /////

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground", "Platform"));

        isMoving = (horizontal != 0);
        isGrounded = rayHit.collider != null && (rayHit.distance < jumpRayDistanceThres * 1.1f && rayHit.distance > jumpRayDistanceThres * 0.9f);
        isGrounded = isGrounded && rb.velocity.y < 0.1f; // ���� �ö󰡴� ���̸� ���� �Ұ�
        // isAttacking�� Attack()�� EndAttack()�� �̿��� ���� �ִϸ��̼� �ӵ��� ���� �������� ����

        ManageCoolTime(); // ��Ÿ�� ����

        //////////////////////

        if (isAttacking)
        {

        }
        else if (isGrounded && Input.GetButton("Fire1") && attackCool <= 0.0f) // ���� ���� ���� ���� �Ұ�
        {
            isMoving = false;
            this.Attack();
        }
        else
        {
            if (stamina > 0 && Input.GetMouseButtonDown(1))
            {
                this.Dash();
            }

            if (isMoving)
            {
                this.Move(horizontal);
            }

            if ((Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && vertical > 0))
                && isGrounded) // ����
            {
                this.Jump();
            }

            if ((Input.GetButtonDown("Vertical") && vertical < 0) && currentPlatform != null) // �Ʒ��� ��������
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }

        // ��� ���� �Լ� /////

        RotateWeaponToMouse(); // ���� ���콺 �������� ȸ��
        if (!isMoving && isGrounded)
        { // ���� �����̰� ���� ���� ��쿡�� ���콺 �������� ȸ��
            RotatePlayerToMouse();
        }
        ManageAnimation(); // �ִϸ��̼� ����

        ////////////////////////


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

        attackCool -= Time.deltaTime;
        if (attackCool < 0)
        {
            attackCool = 0.0f;
        }

        comboCool += Time.deltaTime;
        if (comboCool > comboDuration)
        { // �޺��� ������ ���� �ð��� ������ �޺� �ʱ�ȭ
            combo = 0;
            comboCool = 0.0f;
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

    public void Dash()
    {
        --Stamina;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, transform.right * facingWay, dashDistance, LayerMask.GetMask("Ground", "Platform"));
        if (hit.collider != null)
        {
            Debug.Log(hit.distance);
            float eps = hit.distance * 0.1f;
            transform.position += new Vector3((hit.distance - eps) * facingWay, 0.0f, 0.0f);
        }
        else
        {
            transform.position += new Vector3(dashDistance * facingWay, 0.0f, 0.0f);
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        attackCool = attackDuration;

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

    private void UltimateAttack()
    {

    }

    private void RotatePlayerToMouse()
    {
        float theta = GetAngleToMouse(transform.position);

        if (Mathf.Abs(theta) < 90.0f)
        { // ������
            facingWay = 1;
        }
        else
        { // ����
            facingWay = -1;
        }

        transform.localScale = new Vector3(2.0f * facingWay, 2.0f, 2.0f);
    }

    private void RotateWeaponToMouse()
    {
 
        float theta = GetAngleToMouse(weapon.transform.position);

        if(facingWay == -1)
        {
            theta -= 180;
        }
        weapon.transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 weaponToMouse = mousePos - new Vector2(weapon.transform.position.x, weapon.transform.position.y);
        Debug.DrawRay(weapon.transform.position, weaponToMouse, Color.yellow);
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

    float GetAngleToMouse(Vector3 position)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ToMouse = mousePos - new Vector2(position.x, position.y);

        float theta = Mathf.Atan2(ToMouse.y, ToMouse.x) * Mathf.Rad2Deg;

        return theta;
    }
}

