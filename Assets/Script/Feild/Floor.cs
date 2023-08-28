using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBehavior playerBehavior = collision.gameObject.GetComponent<PlayerBehavior>();

        if (playerBehavior != null)
        {
            // isJump 상태를 false로 변경
            playerBehavior.isJump = false;

            // Animator 컴포넌트 가져오기
            Animator animator = playerBehavior.GetComponent<Animator>();
            animator.SetBool("isJump",false);
        }


        Boss1 boss = collision.gameObject.GetComponent<Boss1>();
        if (boss != null)
        {
            // isJump 상태를 false로 변경
            boss.isJump = false;
        }




        JumpMonster jumpMonster = collision.gameObject.GetComponent<JumpMonster>();
        if (jumpMonster != null)
        {
            jumpMonster.scan = false;

            Animator animator = jumpMonster.GetComponent<Animator>();
            animator.SetBool("isJump", false);
        }

    }
}
