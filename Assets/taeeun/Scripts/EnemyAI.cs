using UnityEngine;

public class EnemyAI : Character
{
    public float detectionRadius = 5f; // 플레이어 탐지 반경
    public float attackRange = 1f; // 공격 사거리
    public float patrolDistance = 3f; // 순찰 거리
    public Transform player; // 플레이어 Transform
    public Animator animator; // 애니메이터
    public float damage = 1; // 공격력
    public float AttackDelay = 1f; // 공격 딜레이
    private Collider2D col;
    private Rigidbody2D rigid;


    private Vector2 startPosition; // 초기 위치
    private Vector2 patrolTarget; // 순찰시 목표 지점
    private bool movingRight = true; // 이동 방향
    private bool isChasing = false; // 추적 상태
    private bool isVisible = true; // 카메라 안에 있는지 여부
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러
    private ScreenShake screenShake; // ScreenShake 컴포넌트
    private Vector2 previousPosition; // 이전 위치
    private bool isDead = false; // 사망 여부
    private float jumpRayDistanceThres; // 바닥에 도착했음을 인정할 오브젝트 중심에서 바닥으로 향하는 ray의 최대 거리

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // 초기 위치 저장
        patrolTarget = startPosition + Vector2.right * patrolDistance; // 순찰 목표 설정
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        previousPosition = transform.position; // 이전 위치 초기화
        screenShake = virtualCamera.GetComponent<ScreenShake>();
    }


    void FixedUpdate()
    {
        if (isDead) return; // 사망 상태에서는 아무 것도 하지 않음
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
        // 플레이어를 향해 자연스러운 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;

        // 목표 위치 계산
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;

        // 현재 위치 갱신
        rigid.MovePosition(targetPosition);

        // 추격 애니메이션 설정
        animator.SetBool("isChasing", true);
    }

    void Patrol()
    {
        // 순찰 목표에 가까워지면 방향 변경
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            movingRight = !movingRight;
            patrolTarget = movingRight
                ? startPosition + Vector2.right * patrolDistance
                : startPosition + Vector2.left * patrolDistance;
        }

        // 목표 지점을 향한 방향 계산
        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;

        // 목표 위치로 이동
        Vector2 targetPosition = transform.position + (Vector3)direction * Speed * Time.deltaTime;
        rigid.MovePosition(targetPosition);

        // 순찰 애니메이션 설정
        animator.SetBool("isChasing", false);
    }


    // ChasePlayer와 Patrol을 사용해서 Move 함수를 구현


    public override void Attack()
    {
        animator.SetTrigger("attack"); // 공격 애니메이션
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
        animator.SetTrigger("hit"); // 피격 애니메이션 재생

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("die"); // 사망 애니메이션 재생
        screenShake.ShakeScreen(4f, 1f); // 강도 4만큼 1초간 카메라 흔들림
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
        // EnemyAI는 점프를 사용하지 않으므로 비워둠
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