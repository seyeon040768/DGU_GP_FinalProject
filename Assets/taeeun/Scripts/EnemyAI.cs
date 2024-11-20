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
    public int health = 3; // �� ü��

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

    void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
        patrolTarget = startPosition + Vector2.right * patrolDistance; // ���� ��ǥ ����
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        previousPosition = transform.position; // ���� ��ġ �ʱ�ȭ
        screenShake = virtualCamera.GetComponent<ScreenShake>();

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
        previousPosition = transform.position;
    }

    void ChasePlayer()
    {
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // �̵�
        transform.position += (Vector3)direction * chaseSpeed * Time.deltaTime;

    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            movingRight = !movingRight;
            patrolTarget = movingRight
                ? startPosition + Vector2.right * patrolDistance
                : startPosition + Vector2.left * patrolDistance;

            // ���� ������ ���� if������ ���� ����
        }

        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
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
        screenShake.ShakeScreen(4f, 1f); // ���� 4��ŭ 1�ʰ� ī�޶� ��鸲
        Destroy(gameObject, 1f);
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
