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
                    StartCoroutine(Attack());
                }
            }

            ScanFrontHouse();
        }
    }


    private void MoveMonster() // �������� �̵��ϴ� �Լ�
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


    private void ScanFrontHouse() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength, LayerMask.GetMask("House"));

        Debug.DrawRay(transform.position, Vector2.left * rayLength, Color.red); // Ray�� �ð����� ǥ�ø� ���� Debug.DrawRay ���

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

    private void ScanFrontPlayer() // ���濡 �÷��̾�, Ȥ�� �Ͽ콺�� �ִ� Ȯ���ϴ� �Լ�
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, rayLength + 2f, LayerMask.GetMask("Player"));

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

    IEnumerator Attack()
    {
        isAttack = true;

        yield return new WaitForSeconds(1.5f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.3f);

        attackArea.enabled = false;

        isAttack = false;
    }

    private void Jump()
    {
        rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isJump = false;
    }


    IEnumerator Die()
    {
        dodie = true;

        // �ݶ��̴� ��Ȱ��ȭ
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // Rigidbody2D ������ ��Ȱ��ȭ
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        rigidbody.simulated = false;

        // ������Ʈ ���� ����
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(1.15f);
        Destroy(gameObject);
    }


   
}