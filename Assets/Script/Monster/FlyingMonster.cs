using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : Monster
{
    public bool findAttack; // 적을 찾음
    public bool isAttack; // 공격 중
    public float rayLength; // 감지 범위
    public float bulletSpeed; // 총알 날아가는 속도

    public GameObject flyingBullet; // 발사할 음파 프리펩
    public Transform flyingTransform; // 발사할 음파 위치
    public GameObject step; // 공중에 떠 있는 느낌을 주기 위한 발판

    //Animator anim;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!dodie)
        {
            if (!isAttack && !findAttack)
            {
                MoveMonster();
            }
            ScanFront();
        }

        if (hp <= 0 && !dodie)
        {
            StartCoroutine(Die());
        }
    }




    private void MoveMonster() // 좌측으로 이동하는 함수
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


    private void ScanFront() // 전방에 플레이어, 혹은 하우스가 있는 확인하는 함수
    {
        float yOffset = -3.0f; // Y 좌표의 오프셋 값

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, yOffset, 0), Vector2.left,
            rayLength, LayerMask.GetMask("Player") | LayerMask.GetMask("House") | LayerMask.GetMask("PlayerOnDamage"));

        Debug.DrawRay(transform.position + new Vector3(0, yOffset, 0), Vector2.left * rayLength, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("House"))
            {
                findAttack = true;
                if (!isAttack)
                {
                    StartCoroutine(FireSoundWave());
                }
            }
        }
        else
        {
            findAttack = false;
        }
    }


    //IEnumerator FireBullet(Vector3 targetPosition)
    //{
    //    //anim.SetBool("isAttack", true);

    //    isAttack = true;
    //    yield return new WaitForSeconds(0.7f);

    //    // 발사할 총알 생성
    //    GameObject bullet = Instantiate(flyingBullet, transform.position, Quaternion.identity);

    //    // 총알의 방향 설정
    //    Vector3 direction = (targetPosition - transform.position).normalized;


    //    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    //    yield return new WaitForSeconds(0.2f);



    //    //anim.SetBool("isAttack", false);
    //    isAttack = false;
    //    //yield return new WaitForSeconds(0.2f); // 공격 종료

    //}


    IEnumerator FireSoundWave()
    {
        isAttack = true;

        while (findAttack)
        {
            // 발사할 총알 생성
            GameObject bullet = Instantiate(flyingBullet, flyingTransform.position, Quaternion.identity);

            // 총알의 방향 설정 (정면 방향)
            Vector3 direction = Vector3.left; // 혹은 원하는 정면 방향으로 설정

            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            yield return new WaitForSeconds(0.2f);

            // 잠시 움직임
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            yield return new WaitForSeconds(0.8f);
        }

        isAttack = false;
    }








    IEnumerator Die()
    {
        dodie = true;
        //anim.SetTrigger("doDie");
        step.SetActive(false);

        // 콜라이더 비활성화
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D 움직임 비활성화
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        yield return new WaitForSeconds(1f);  // 죽음 함수 작동 후, 오브젝트가 삭제되는 시간, 애니메이션 출력 시간이랑 맞춰야 함
        if (Random.Range(0f, 100f) <= 25f)
        {
            int randomIndex = Random.Range(0, gem.Length); // 랜덤한 인덱스 선택
            GameObject newObj = Instantiate(gem[randomIndex], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
