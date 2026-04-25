using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private PolygonCollider2D polygonCollider;
    private List<Collider2D> pestColliders = new List<Collider2D>(); //keeps track of the pests it's already collided with so that it doesn't collide more than once
    [SerializeField] private bool lingeringWeapon;
    private Weapon weaponScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponScript = GetComponentInParent<Weapon>();

        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            Physics2D.OverlapCollider(boxCollider, pestColliders);
        }
        else if (GetComponent<PolygonCollider2D>() != null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            Physics2D.OverlapCollider(polygonCollider, pestColliders);
        }
        else
        {
            circleCollider = GetComponent<CircleCollider2D>();
            Physics2D.OverlapCollider(circleCollider, pestColliders);

        }

        foreach (Collider2D collider in pestColliders)
        {
            if (collider.CompareTag("Pest"))
            {
                collider.GetComponent<Pest>().TakeDamage(weaponScript.damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pest"))
        {
            if (pestColliders == null || !pestColliders.Contains<Collider2D>(collision))
            {
                pestColliders.Add(collision);
                collision.GetComponent<Pest>().TakeDamage(weaponScript.damage);
                if (lingeringWeapon)
                {
                    StartCoroutine(DealLingeringDamage(collision));
                }
            }
        }
    }

    private IEnumerator DealLingeringDamage(Collider2D pestCollider)
    {
        yield return new WaitForSeconds(weaponScript.damageInterval);
        if (pestCollider != null)
        {
            pestCollider.GetComponent<Pest>().TakeDamage(weaponScript.damage);
            StartCoroutine(DealLingeringDamage(pestCollider));
        }
    }
}
