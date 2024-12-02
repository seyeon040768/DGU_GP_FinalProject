using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private SFXPool sfxPool;
    private int facingWay; // 왼쪽(-) 오른쪽(+)

    public float dashDistance;
    public float dashRecoveryDuration;
    public float dashRecoveryCool;
    public GameObject dashEffect;

    public float ultimDuration;
    public float ultimCool;
    public GameObject ultimEffect;

    public int maxstamina;
    private int stamina;

    public HUD hud;
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

    private float walkSoundCooldown = 0.3f; // 걷기 효과음 재생 간격
    private float lastWalkSoundTime = 0f;


    public GameObject[] weaponsObj;
    public Weapon[] weapons;
    public int weaponNum;

    public string[] attackAnimName;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlatformEffector2D currentPlatform;

    public GameManager manager; // 대화창 띄우기 용
    private int[] attackAnimHash;
    private float jumpRayDistanceThres; // 바닥에 도착했음을 인정할 오브젝트 중심에서 바닥으로 향하는 ray의 최대 거리
    GameObject scanObject;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Stamina = maxstamina;

        facingWay = (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));

        combo = 0;

        weaponNum = 0;
        ActivateWeapon(weaponNum);

        

        attackAnimHash = new int[attackAnimName.Length];
        for (int i = 0; i < attackAnimName.Length; ++i)
        {
            attackAnimHash[i] = Animator.StringToHash(attackAnimName[i]);
        }
        jumpRayDistanceThres = col.bounds.extents.y;
    }

    void Update()
    {
        // 필수 실행 코드 /////

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground", "Platform"));

        isMoving = (horizontal != 0);
        isGrounded = rayHit.collider != null && (rayHit.distance < jumpRayDistanceThres * 1.1f && rayHit.distance > jumpRayDistanceThres * 0.9f);
        isGrounded = isGrounded && rb.velocity.y < 0.1f; // 위로 올라가는 중이면 점프 불가

        // isAttacking은 Attack()과 EndAttack()을 이용해 공격 속도에 따라 동적으로 변경

        ManageCoolTime(); // 쿨타임 관리


        //////////////////////

        if (Input.GetKeyDown(KeyCode.R) && combo >= 20)
        {
            combo = 0;
            Ultimate();
        }

        if (isAttacking)
        {
            
        }
        else if (isGrounded && Input.GetButton("Fire1") && attackCool <= 0.0f) // 점프 중일 때는 공격 불가
        {
            isMoving = false;
            this.Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // 무기 교체
        {
            weaponNum = 0;
            ActivateWeapon(weaponNum);
            attackDuration = weapons[weaponNum].attackDuration;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponNum = 1;
            ActivateWeapon(weaponNum);
            attackDuration = weapons[weaponNum].attackDuration;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponNum = 2;
            ActivateWeapon(weaponNum);
            attackDuration = weapons[weaponNum].attackDuration;
        }
        else
        {
            if (stamina > 0 && Input.GetMouseButtonDown(1))
            {
                this.Dash();
                sfxPool.Play("Dash");
            }

            if (isMoving)
            {
                if (Time.time - lastWalkSoundTime >= walkSoundCooldown)
                {
                    lastWalkSoundTime = Time.time;
                    sfxPool.Play("Walk");
                }
                this.Move(horizontal);

            }
            else
            {
                if(isMoving == false)
                {
                    sfxPool.Stop("Walk");
                }
            }

            if ((Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && vertical > 0))
                && isGrounded) // 점프
            {
                this.Jump();
                sfxPool.Play("Jump");
            }

            if ((Input.GetButtonDown("Vertical") && vertical < 0) && currentPlatform != null) // 아래로 내려가기
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }

        // 출력 관련 함수 /////

        if (!isMoving && isGrounded)
        { // 정지 상태이고 땅에 있을 경우에만 마우스 방향으로 회전
            RotatePlayerToMouse();
        }
        RotateWeaponToMouse(); // 무기 마우스 방향으로 회전
        ManageAnimation(); // 애니메이션 관리

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
            EndAttack();
        }

        comboCool += Time.deltaTime;
        if (combo >= 20)
        {

        }
        else if (comboCool > comboDuration)
        { // 콤보를 넣은지 일정 시간이 지나면 콤보 초기화
            combo = 0;
            comboCool = 0.0f;
        }

        ultimCool -= Time.deltaTime;
        if (ultimCool <= 0.0f)
        {
            ultimCool = 0.0f;
            EndUltimate();
        }
    }

    private void ManageAnimation()
    {
        if (isAttacking)
        {
            animator.SetBool("isMove", false);
            return;
        }

        if (rb.velocity.y > 0 && !isGrounded) // 점프해서 위로 올라가는 상태
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isFall", false);
            animator.SetBool("isMove", false);
        }
        else if (rb.velocity.y < 0 && !isGrounded) // 아래로 떨어지는 상태
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
            float eps = hit.distance * 0.5f;
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

        if (weapons[weaponNum].Attack())
        {
            AddCombo();
        }


        attackCool = weapons[weaponNum].attackDuration;

        // float attackAnimDuration = GetAnimationClipLength(attackAnimName[attackNum]);
        // Invoke(nameof(EndAttack), attackAnimDuration);
    }

    public void AddCombo()
    {
        ++combo;
        comboCool = 0.0f;

        if (combo > 20)
        {
            combo = 20;
        }
    }
    private void EndAttack()
    {
        isAttacking = false; // 공격 상태 해제
    }

    private void Ultimate()
    {
        isUltim = true;
        isInvincible = true;
        if(weaponNum == 0) // 양손검
        {
            sfxPool.Play("SwordUlti");
        }
        if (weaponNum == 1) // 총
        {
            sfxPool.Play("GunUlti");
        }
        if (weaponNum == 2) // 쌍검
        {
            sfxPool.Play("TwoSwordUlti");
        }
        ultimCool = ultimDuration;
    }

    private void EndUltimate()
    {
        isUltim = false;
        isInvincible = false;
    }

    private void RotatePlayerToMouse()
    {
        float theta = GetAngleToMouse(transform.position);

        if (Mathf.Abs(theta) < 90.0f)
        { // 오른쪽
            facingWay = 1;
        }
        else
        { // 왼쪽
            facingWay = -1;
        }

        transform.localScale = new Vector3(2.0f * facingWay, 2.0f, 2.0f);
    }

    private void RotateWeaponToMouse()
    {
        GameObject weapon = weaponsObj[weaponNum];

        float theta = GetAngleToMouse(weapon.transform.position);

        if (facingWay < 0)
        {
            theta += 180.0f;
        }

        weapon.transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 weaponToMouse = mousePos - new Vector2(weapon.transform.position.x, weapon.transform.position.y);
        Debug.DrawRay(weapon.transform.position, weaponToMouse, Color.yellow);
    }

    public override void TakeHit()
    {
        // collider를 사용해 공격을 받았음이 판단되었을때 호출되는 코드, 충돌한 공격 범위에 각각 할당된 데미지를 받아와 Hp를 감소시켜야 함
        sfxPool.Play("PlayerHit");
    }

    private void ActivateWeapon(int weaponNum)
    {
        sfxPool.Play("GunReload");
        hud.OnWeaponChanged(weaponNum);
        for (int i = 0; i < weaponNum; i++)
        {
            weaponsObj[i].SetActive(false);
        }
        weaponsObj[weaponNum].SetActive(true);
        for (int i = weaponNum + 1; i < weaponsObj.Length; i++)
        {
            weaponsObj[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ObjectData가 있는지 확인하여 scanObject에 저장
        if (collision.GetComponent<ObjectData>() != null)
        {
            scanObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger에서 벗어나면 scanObject를 초기화
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
                return clip.length; // 애니메이션 클립의 길이를 반환
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

