using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; // ���� �̵� �ӵ�
    public float chaseSpeed = 4f; // �÷��̾ ������ �ӵ�
    public float detectionRadius = 5f; // �÷��̾� Ž�� �ݰ�
    public float attackRange = 1f; // ���� ��Ÿ�
    public float patrolDistance = 3f; // ���� �Ÿ�
    public Transform player; // �÷��̾� Transform
    public Animator animator; // �ִϸ�����
    public LayerMask groundLayer; // Ÿ�ϸ��� �ִ� ���̾� ����
    public float groundCheckDistance = 0.1f; // �� �Ʒ��� üũ�� �Ÿ�
    public Transform groundCheck; // �� �� �Ʒ� ��ġ�� Ȯ���ϴ� Transform
    public float health = 3; // �� ü��

    private Vector2 startPosition; // �ʱ� ��ġ
    private Vector2 patrolTarget; // ������ ��ǥ ����
    private bool movingRight = true; // �̵� ����
    private bool isChasing = false; // ���� ����
    private bool isVisible = true; // ī�޶� �ȿ� �ִ��� ����
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������
    private bool isDead = false; // ��� ����

    void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
        patrolTarget = startPosition + Vector2.right * patrolDistance; // ���� ��ǥ ����
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
    }

    void Update()
    {
        if (isDead) return; // ��� ���¿����� �ƹ� �͵� ���� ����
        if (health <= 0)
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

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;

            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        FlipSprite();
    }

    void ChasePlayer()
    {
        if (IsGrounded())
        {
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // �̵�
            transform.position += (Vector3)direction * chaseSpeed * Time.deltaTime;
        }
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

        // �� �Ʒ��� Ÿ���� ���� ��쿡�� �̵�
        if (IsGrounded())
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    void AttackPlayer()
    {
        animator.SetTrigger("attack"); // ���� �ִϸ��̼�
    }

    void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("hit"); // �ǰ� �ִϸ��̼� ���
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die"); // ��� �ִϸ��̼� ���
        Destroy(gameObject, 1f);
    }

    void FlipSprite()
    {
        if (isChasing || !isDead)
        {
            spriteRenderer.flipX = movingRight ? false : true;
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

    bool IsGrounded()
    {
        // �� �Ʒ��� Ÿ�ϸ�(groundLayer)�� �ִ��� Raycast�� Ȯ��
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }
}
