using UnityEngine;

public class EnemyAI : Character
{
    [SerializeField] private SFXPool sfxPool;
    public float detectionRadius = 5f; // �÷��̾� Ž�� �ݰ�
    public float attackRange = 1f; // ���� ��Ÿ�
    public float patrolDistance = 3f; // ���� �Ÿ�
    public Transform player; // �÷��̾� Transform
    public Animator animator; // �ִϸ�����
    public float damage = 1; // ���ݷ�
    public float AttackDelay = 3f; // ���� ������
    private Collider2D col;
    private Rigidbody2D rigid;

    private Vector2 startPosition; // �ʱ� ��ġ
    private Vector2 patrolTarget; // ������ ��ǥ ����
    private bool movingRight = true; // �̵� ����
    private bool isChasing = false; // ���� ����
    private bool isVisible = true; // ī�޶� �ȿ� �ִ��� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private ScreenShake screenShake; // ScreenShake ������Ʈ
    private Vector2 previousPosition; // ���� ��ġ
    private bool isDead = false; // ��� ����

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // �ʱ� ��ġ ����
        patrolTarget = startPosition + Vector2.right * patrolDistance; // ���� ��ǥ ����
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        previousPosition = transform.position; // ���� ��ġ �ʱ�ȭ
        screenShake = virtualCamera.GetComponent<ScreenShake>();
    }

    void FixedUpdate()
    {
        if (isDead) return; // ��� ���¿����� �ƹ� �͵� ���� ����
        if (Hp <= 0)
        {
            FindObjectOfType<SceneManage>().OnEnemyDefeated();
            Destroy(gameObject);
            Die();
        }

        // �þ� �������� ����� �ִϸ��̼� ��Ȱ��ȭ
        if (!isVisible)
        {
            animator.enabled = false;
            return;
        }
        else
        {
            animator.enabled = true;
        }

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // �÷��� ���� �ִ��� Ȯ��
        if (IsOnPlatform())
        {
            if (distanceToPlayer <= detectionRadius)
            {
                isChasing = true;

                if (distanceToPlayer > attackRange)
                {
                    ChasePlayer();
                }
                else
                {
                    if (Time.time >= AttackDelay)
                    {
                        Attack();
                        AttackDelay = Time.time + 1f;
                    }
                }
            }
            else
            {
                isChasing = false;
                Patrol();
            }

            FlipSprite();
            previousPosition = transform.position;
        }
        else
        {
            // �÷����� ���� ��� ����
            rigid.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0); // �̵� �ִϸ��̼� ����
        }
    }

    // �÷��� ���� �ִ��� Ȯ���ϴ� �޼���
    bool IsOnPlatform()
    {
        Vector2 frontVec = new Vector2(transform.position.x, transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); // ����� �뵵
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1f, LayerMask.GetMask("Platform", "Ground"));

        return rayHit.collider != null;
    }

    void ChasePlayer()
    {

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;
        rigid.MovePosition(targetPosition);
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            movingRight = !movingRight;
            patrolTarget = movingRight
                ? startPosition + Vector2.right * patrolDistance
                : startPosition + Vector2.left * patrolDistance;
        }

        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;
        rigid.MovePosition(targetPosition);

    }

    public override void Attack()
    {
        sfxPool.Play("EnemyAttack");
        animator.SetTrigger("attack");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<Player>().TakeHit();
                collider.GetComponent<Player>().Hp -= damage;
            }
        }
    }

    public override void TakeHit()
    {
        if (isDead) return;

        Hp -= damage;
        animator.SetTrigger("hit");

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        sfxPool.Play("EnemyDie");
        screenShake.ShakeScreen(4f, 1f);
        Destroy(gameObject, 1f);
    }

    public override void Move(float horizontal)
    {
        if (isDead) return;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        FlipSprite();
    }

    public override void Jump()
    {
        // EnemyAI�� ������ ������� �����Ƿ� �����
    }


    void FlipSprite()
    {
        float movementX = transform.position.x - previousPosition.x;
        if (movementX > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementX < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
    }

    void OnBecameVisible()
    {
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        isVisible = false;
    }
}
