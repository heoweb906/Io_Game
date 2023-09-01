using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMonster : Monster
{
    public bool findAttack; // ���� ã��
    public bool isAttack; // ���� ��
    public bool isJump; // ���� ��
    public bool scan;
    public float rayLength = 2f; // ���� ����

    public BoxCollider2D attackArea; // ���� ����
    Animator anim;

    public float jumpForce = 10f;
    public float jumpDelay = 1f;

    private Rigidbody2D rigid;


    private void Awake()
    {
        anim = GetComponent<Animator>();
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


    private void MoveMonster() // �������� �̵��ϴ� �Լ�
    {
        anim.SetBool("isWalk", true);
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


    private void ScanFrontHouse() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 5f, LayerMask.GetMask("House"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.red); // Ray�� �ð����� ǥ�ø� ���� Debug.DrawRay ���

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("House"))
            {
                anim.SetBool("isWalk", false);
                findAttack = true;
            }
        }
        else
        {
            anim.SetBool("isWalk", true);
            findAttack = false;
        }
    }

    private void ScanFrontPlayer() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength + 3f, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.green); // Ray�� �ð����� ǥ�ø� ���� Debug.DrawRay ���

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



    // #. �������� ���� �մ� ������ ��, �Լ� �ȿ� �ִ� ��ġ���� �����ؼ� ���� �ֱ� ���� ����

    IEnumerator Bress()
    {
        anim.SetBool("isAttack",true);
        isAttack = true;

        float elapsedTime = 0f; // Ÿ�̸� ���� �ʱ�ȭ
        while (elapsedTime < 1f) // 2�ʱ����� �ݺ�  @@ �� ���ڶ� �Ʒ��� ��� �ð� ������Ʈ�� �ð� �¾ƾ� ��
        {
            attackArea.enabled = true; // Collider�� Ȱ��ȭ
            yield return new WaitForSeconds(0.08f);
            attackArea.enabled = false; // Collider�� ��Ȱ��ȭ
            yield return new WaitForSeconds(0.08f);

            elapsedTime += 0.1f; // ��� �ð� ������Ʈ
        }

        // ���⼭�� ���� ���·� ������
        attackArea.enabled = false;

        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(3f);

        
        isAttack = false;
    }






    private void Jump()
    {
        rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        anim.SetBool("isJump",true);
        isJump = false;
    }

    


    IEnumerator Die()
    {
        anim.SetTrigger("doDie");
        dodie = true;

        // �ݶ��̴� ��Ȱ��ȭ
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D ������ ��Ȱ��ȭ
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        yield return new WaitForSeconds(0.1f);
        
    }


    public void SalamanderDestroy()
    {
        Invoke("DelayedSlimeDestroy", 0.7f);
    }

    private void DelayedSlimeDestroy()
    {

        Destroy(gameObject);
    }




}
