using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private List<Collider2D> pestColliders; //keeps track of the pests it's already collided with so that it doesn't collide more than once

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        pestColliders = Physics2D.OverlapBoxAll(boxCollider.transform.position, boxCollider.size, 0).ToList<Collider2D>();

        foreach (Collider2D collider in pestColliders)
        {
            if (collider.CompareTag("Pest"))
            {
                collider.GetComponent<Pest>().TakeDamage(GetComponentInParent<Weapon>().damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pest"))
        {
            if (!pestColliders.Contains<Collider2D>(collision))
            {
                pestColliders.Add(collision);
                collision.GetComponent<Pest>().TakeDamage(GetComponentInParent<Weapon>().damage);
            }
        }
    }
}
