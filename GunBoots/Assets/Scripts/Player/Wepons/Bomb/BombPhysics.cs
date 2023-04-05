using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPhysics : MonoBehaviour
{
    

    private ExplosionHandler explosionHandler;

    private void OnEnable()
    {
        explosionHandler = GetComponent<ExplosionHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        explosionHandler.Explode();
        StartCoroutine(TempAnim());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // set the color of the sphere
        Gizmos.DrawWireSphere(transform.position, explosionHandler.range); // draw the sphere
    }

    private IEnumerator TempAnim()
    {
        transform.localScale = new Vector3(explosionHandler.range * 2, explosionHandler.range * 2, 1);
        yield return new WaitForSeconds(.01f);
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
