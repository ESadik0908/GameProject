using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            return;
        }
        StartCoroutine("DelayedDespawn");
    }


    private IEnumerator DelayedDespawn()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.SetActive(false);
    }
}
