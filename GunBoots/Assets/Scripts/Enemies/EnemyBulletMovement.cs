using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private EnemyProjectileStats stats;



    private void Start()
    {
        stats = GetComponent<EnemyProjectileStats>();
    }

    private void OnEnable()
    {
        stats = GetComponent<EnemyProjectileStats>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "PlayerBullet")
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {

            GameObject player = collision.gameObject;
            if (player.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.Damage(stats.contactDamage);
                Debug.Log(stats.contactDamage);
            }
        }
        gameObject.SetActive(false);
    }
}
