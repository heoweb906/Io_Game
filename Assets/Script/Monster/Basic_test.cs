using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_test : Monster
{
    public bool findAttack; // 적을 찾음
    public bool isAttack; // 공격 중
    public float rayLength = 2f; // 감지 범위

    public float xposition;
    public float yposition;

    public BoxCollider2D attackArea; // 공격 범위

    // # 애니메이션 관련 변수
    Animator anim;
    private bool originalIsWalk;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
                    MoveMonster();
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
            ScanFront();
        }
    }

    private void MoveMonster() // 좌측으로 이동하는 함수
    {
        anim.SetBool("isWalk", true);
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void ScanFront() // 전방에 플레이어, 혹은 하우스가 있는 확인하는 함수
    {
        Vector2 raycastOrigin = new Vector2(transform.position.x - xposition, transform.position.y - yposition);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.left, rayLength, LayerMask.GetMask("Player") | LayerMask.GetMask("House"));

        Debug.DrawRay(raycastOrigin, Vector2.left * rayLength, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("House"))
            {
                anim.SetBool("isWalk", false);
                findAttack = true;
            }
        }
        else
        {
            findAttack = false;
            if(!isAttack)
            {
                anim.SetBool("isWalk", true);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.2f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.1f);

        attackArea.enabled = false;


        yield return new WaitForSeconds(0.55f);

        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(2f);

        isAttack = false;
        
    }


  

    IEnumerator Die()
    {
        dodie = true;
        anim.SetTrigger("doDie");

        // 콜라이더 비활성화
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D 움직임 비활성화
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        yield return new WaitForSeconds(0.7f);
        anim.enabled = false;
        if (Random.Range(0f, 100f) <= 25f) //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        {
            int randomIndex = Random.Range(0, gem.Length); // 랜덤한 인덱스 선택
            GameObject newObj = Instantiate(gem[randomIndex], transform.position, Quaternion.identity);
        }


        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}