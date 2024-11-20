using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; // 적의 이동 속도
    public float chaseSpeed = 4f; // 플레이어를 추적할 속도
    public float detectionRadius = 5f; // 플레이어 탐지 반경
    public float attackRange = 1f; // 공격 사거리
    public float patrolDistance = 3f; // 순찰 거리
    public Transform player; // 플레이어 Transform
    public Animator animator; // 애니메이터
    public int health = 3; // 적 체력

    private Vector2 startPosition; // 초기 위치
    private Vector2 patrolTarget; // 순찰시 목표 지점
    private bool movingRight = true; // 이동 방향
    private bool isChasing = false; // 추적 상태
    private bool isVisible = true; // 카메라 안에 있는지 여부
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    private ScreenShake screenShake; // ScreenShake 컴포넌트
    private Vector2 previousPosition; // 이전 위치
    private bool isDead = false; // 사망 여부
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        startPosition = transform.position; // 초기 위치 저장
        patrolTarget = startPosition + Vector2.right * patrolDistance; // 순찰 목표 설정
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        previousPosition = transform.position; // 이전 위치 초기화
        screenShake = virtualCamera.GetComponent<ScreenShake>();

    }

    void Update()
    {
        if (isDead) return; // 사망 상태에서는 아무 것도 하지 않음
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

        // 이동
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

            // 추후 가독성 위해 if문으로 수정 예정
        }

        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        animator.SetTrigger("attack"); // 공격 애니메이션
    }

    void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("hit"); // 피격 애니메이션 재생

    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die"); // 사망 애니메이션 재생
        screenShake.ShakeScreen(4f, 1f); // 강도 4만큼 1초간 카메라 흔들림
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
