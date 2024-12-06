using UnityEngine;

public class EnemyAI : Character
{
    [SerializeField] private SFXPool sfxPool;
    public float detectionRadius = 5f; // 플레이어 탐지 반경
    public float attackRange = 1f; // 공격 사거리
    public float patrolDistance = 3f; // 순찰 거리
    public Transform player; // 플레이어 Transform
    public Animator animator; // 애니메이터
    public float damage = 1; // 공격력
    public float AttackDelay = 3f; // 공격 딜레이
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
            FindObjectOfType<SceneManage>().OnEnemyDefeated();
            Destroy(gameObject);
            Die();
        }

        // 시야 범위에서 벗어나면 애니메이션 비활성화
        if (!isVisible)
        {
            animator.enabled = false;
            return;
        }
        else
        {
            animator.enabled = true;
        }

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플랫폼 위에 있는지 확인
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
            // 플랫폼에 없을 경우 정지
            rigid.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0); // 이동 애니메이션 정지
        }
    }

    // 플랫폼 위에 있는지 확인하는 메서드
    bool IsOnPlatform()
    {
        Vector2 frontVec = new Vector2(transform.position.x, transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0)); // 디버그 용도
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
        // EnemyAI는 점프를 사용하지 않으므로 비워둠
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
