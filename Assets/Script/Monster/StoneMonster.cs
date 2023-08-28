using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneMonster : Monster
{
    public float rayLength; // 감지 범위
    public float bulletSpeed; // 총알 날아가는 속도

    public GameObject bombArray; // 폭발 범위
    public GameObject bombEffect;
    Animator anim;

    public GameObject[] rock;
    public Transform position_rock_1;
    public Transform position_rock_2;
    public Transform position_rock_3;
    public Transform position_rock_4;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp <= 0 && !dodie)
        {
            StartCoroutine(Bomb());
        }
        if (!dodie)
        {
            MoveMonster();
        }
    }

    private void MoveMonster() // 좌측으로 이동하는 함수
    {
        anim.SetBool("isWalk", true);
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


    private IEnumerator Bomb()
    {
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doAttack");

        dodie = true;

        yield return new WaitForSeconds(1f);
        
    }

    public void Die()
    {
        float uprange_Max = 10f;
        float uprange_Min = 7f;

        // 여기에 파편이 튀는 걸 구현해야 함
        float ran = Random.Range(0f, 3f);
        int randomrocknumber = (int)ran;
        GameObject Sculpture_1_ = Instantiate(rock[randomrocknumber], position_rock_1.position, Quaternion.identity);
        Rigidbody2D rb1 = Sculpture_1_.GetComponent<Rigidbody2D>();
        if (rb1 != null)
        {
            Vector2 force = new Vector2(Random.Range(-40f, -30f), Random.Range(uprange_Min, uprange_Max));
            rb1.AddForce(force, ForceMode2D.Impulse);

            float angularVelocity = Random.Range(-180f, 180f); // 랜덤한 회전력
            rb1.angularVelocity = angularVelocity;
        }

        ran = Random.Range(0f, 3f);
        randomrocknumber = (int)ran;
        GameObject Sculpture_2_ = Instantiate(rock[randomrocknumber], position_rock_2.position, Quaternion.identity);
        Rigidbody2D rb2 = Sculpture_2_.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            Vector2 force = new Vector2(Random.Range(-40f, -30f), Random.Range(uprange_Min, uprange_Max)); // 강한 힘으로 수정
            rb2.AddForce(force, ForceMode2D.Impulse);

            float angularVelocity = Random.Range(-180f, 180f); // 랜덤한 회전력
            rb2.angularVelocity = angularVelocity;
        }

        ran = Random.Range(0f, 3f);
        randomrocknumber = (int)ran;
        GameObject Sculpture_3_ = Instantiate(rock[randomrocknumber], position_rock_3.position, Quaternion.identity);
        Rigidbody2D rb3 = Sculpture_3_.GetComponent<Rigidbody2D>();
        if (rb3 != null)
        {
            Vector2 force = new Vector2(Random.Range(30f, 40f), Random.Range(uprange_Min, uprange_Max)); // 강한 힘으로 수정
            rb3.AddForce(force, ForceMode2D.Impulse);

            float angularVelocity = Random.Range(-180f, 180f); // 랜덤한 회전력
            rb3.angularVelocity = angularVelocity;
        }

        ran = Random.Range(0f, 3f);
        randomrocknumber = (int)ran;
        GameObject Sculpture_4_ = Instantiate(rock[randomrocknumber], position_rock_4.position, Quaternion.identity);
        Rigidbody2D rb4 = Sculpture_4_.GetComponent<Rigidbody2D>();
        if (rb4 != null)
        {
            Vector2 force = new Vector2(Random.Range(30f, 40f), Random.Range(uprange_Min, uprange_Max)); // 강한 힘으로 수정
            rb4.AddForce(force, ForceMode2D.Impulse);

            float angularVelocity = Random.Range(-180f, 180f); // 랜덤한 회전력
            rb4.angularVelocity = angularVelocity;
        }


        GameObject bombEffectInstance = Instantiate(bombEffect, transform.position, Quaternion.identity);


        // 젬 드랍
        if (Random.Range(0f, 100f) <= 25f)
        {
            int randomIndex = Random.Range(0, gem.Length); // 랜덤한 인덱스 선택
            GameObject newObj = Instantiate(gem[randomIndex], transform.position, Quaternion.identity);
        }


        // 콜라이더 비활성화
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D 움직임 비활성화
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        anim.SetTrigger("doDie");

        Destroy(bombEffectInstance, 0.5f);
    }

    public void SlimeDestroy()
    {
        Invoke("DelayedSlimeDestroy", 0.7f);
    }

    private void DelayedSlimeDestroy()
    {
        Destroy(gameObject);
    }


}




