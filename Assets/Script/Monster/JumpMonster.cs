using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMonster : Monster
{
    public bool findAttack; // 적을 찾음
    public bool isAttack; // 공격 중
    public bool isJump; // 점프 중
    public bool scan;
    public float rayLength = 2f; // 감지 범위

    public BoxCollider2D attackArea; // 공격 범위


    public float jumpForce = 5f;
    public float jumpDelay = 1f;

    private Rigidbody2D rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (hp <= 0 && !dodie)
        {
            StartCoroutine(Die());
        }
        if (!dodie)
        {
            if (!isAttack)
            {
                if (!findAttack)
                {
                    if(!isJump)
                    {
                        MoveMonster();
                    }
                    if(!scan)
                    {
                        ScanFrontPlayer();
                    }
                }
                else
                {
                    StartCoroutine(Bress());
                }
            }

            ScanFrontHouse();
        }
    }


    private void MoveMonster() // 좌측으로 이동하는 함수
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


    private void ScanFrontHouse() // 전방에 플레이어, 혹은 하우스가 있는 확인하는 함수
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("House"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.red); // Ray의 시각적인 표시를 위한 Debug.DrawRay 사용

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("House"))
            {
                findAttack = true;
            }
        }
        else
        {
            findAttack = false;
        }
    }

    private void ScanFrontPlayer() // 전방에 플레이어, 혹은 하우스가 있는 확인하는 함수
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength + 2f, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.green); // Ray의 시각적인 표시를 위한 Debug.DrawRay 사용

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (isJump == false)
                {
                    Invoke("Jump", 1f);
                }
                isJump = true;
                scan = true;
            }
        }
    }

    // #. 제일 처음 기본 공격 함수

    //IEnumerator Attack()
    //{
    //    isAttack = true;

    //    yield return new WaitForSeconds(1.5f);
    //    attackArea.enabled = true;

    //    yield return new WaitForSeconds(0.3f);

    //    attackArea.enabled = false;

    //    isAttack = false;
    //}



    // #. 데미지가 불을 뿜는 서서히 들어감, 함수 안에 있는 수치들을 조정해서 공격 주기 조절 가능

    IEnumerator Bress()
    {
        isAttack = true;

        float elapsedTime = 0f; // 타이머 변수 초기화
        while (elapsedTime < 1f) // 2초까지만 반복  @@ 이 숫자랑 아래에 경과 시간 업데이트랑 시간 맞아야 함
        {
            attackArea.enabled = true; // Collider를 활성화
            yield return new WaitForSeconds(0.08f);
            attackArea.enabled = false; // Collider를 비활성화
            yield return new WaitForSeconds(0.08f);

            elapsedTime += 0.1f; // 경과 시간 업데이트
        }

        // 여기서는 꺼짐 상태로 마무리
        attackArea.enabled = false;


        yield return new WaitForSeconds(3f);


        isAttack = false;
    }




    //private IEnumerator Bress()
    //{
    //    isAttack = true;

    //    bressArray.SetActive(true);

    //    float elapsedTime = 0f; // 타이머 변수 초기화
    //    while (elapsedTime < 2f) // 2초까지만 반복  @@ 이 숫자랑 아래에 경과 시간 업데이트랑 시간 맞아야 함
    //    {
    //        bressArrayCollider.enabled = true; // Collider를 활성화
    //        yield return new WaitForSeconds(0.08f);
    //        bressArrayCollider.enabled = false; // Collider를 비활성화
    //        yield return new WaitForSeconds(0.08f);
    //        elapsedTime += 0.1f; // 경과 시간 업데이트
    //    }

    //    isAttack = false;
    //}





    private void Jump()
    {
        rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isJump = false;
    }


    IEnumerator Die()
    {
        dodie = true;

        // 콜라이더 비활성화
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D 움직임 비활성화
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        // 오브젝트 색상 변경
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(1.15f);
        if (Random.Range(0f, 100f) <= 25f)
        {
            int randomIndex = Random.Range(0, gem.Length); // 랜덤한 인덱스 선택
            GameObject newObj = Instantiate(gem[randomIndex], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }


   
}
