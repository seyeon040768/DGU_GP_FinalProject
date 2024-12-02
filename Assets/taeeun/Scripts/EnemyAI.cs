using UnityEngine;

public class EnemyAI : Character
{
    public float detectionRadius = 5f; // �÷��̾� Ž�� �ݰ�
    public float attackRange = 1f; // ���� ��Ÿ�
    public float patrolDistance = 3f; // ���� �Ÿ�
    public Transform player; // �÷��̾� Transform
    public Animator animator; // �ִϸ�����
    public float damage = 1; // ���ݷ�
    public float AttackDelay = 1f; // ���� ������
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
    private float jumpRayDistanceThres; // �ٴڿ� ���������� ������ ������Ʈ �߽ɿ��� �ٴ����� ���ϴ� ray�� �ִ� �Ÿ�

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
            Die();
        }
        if (!isVisible)
        {
            animator.enabled = false;
            return;
        }
        else
        {
            animator.enabled = true;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isGrounded)
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
                        AttackPlayer();
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
    }

    void ChasePlayer()
    {
        // �÷��̾ ���� �ڿ������� �������� �̵�
        Vector2 direction = (player.position - transform.position).normalized;

        // ��ǥ ��ġ ���
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;

        // ���� ��ġ ����
        rigid.MovePosition(targetPosition);

        // �߰� �ִϸ��̼� ����
        animator.SetBool("isChasing", true);
    }

    void Patrol()
    {
        // ���� ��ǥ�� ��������� ���� ����
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            movingRight = !movingRight;
            patrolTarget = movingRight
                ? startPosition + Vector2.right * patrolDistance
                : startPosition + Vector2.left * patrolDistance;
        }

        // ��ǥ ������ ���� ���� ���
        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;

        // ��ǥ ��ġ�� �̵�
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;
        rigid.MovePosition(targetPosition);

        // ���� �ִϸ��̼� ����
        animator.SetBool("isChasing", false);
    }


    // ChasePlayer�� Patrol�� ����ؼ� Move �Լ��� ����


    public override void Attack()
    {
        animator.SetTrigger("attack"); // ���� �ִϸ��̼�
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
        animator.SetTrigger("hit"); // �ǰ� �ִϸ��̼� ���

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("die"); // ��� �ִϸ��̼� ���
        screenShake.ShakeScreen(4f, 1f); // ���� 4��ŭ 1�ʰ� ī�޶� ��鸲
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

    void AttackPlayer()
    {
        Attack();
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