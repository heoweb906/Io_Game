using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior playerBehavior = collision.gameObject.GetComponent<PlayerBehavior>();
        House house = collision.gameObject.GetComponent<House>();

        if (playerBehavior != null)
        {
            playerBehavior.DecreaseHp(damage);
        }

        if (house != null)
        {
            house.TakeDamage(damage);
        }
    }
}
