using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPhysics : MonoBehaviour
{
    

    private ExplosionHandler explosionHandler;

    private void OnEnable()
    {
        explosionHandler = GetComponent<ExplosionHandler>();
        Vector3 euler = transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        explosionHandler.Explode();
        StartCoroutine(TempAnim());
    }

    private IEnumerator TempAnim()
    {
        transform.localScale = new Vector3(explosionHandler.range * 2, explosionHandler.range * 2, 1);
        yield return new WaitForSeconds(.01f);
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
