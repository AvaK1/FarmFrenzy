using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //weapons will each have their own attack speed, number of projectiles, knockback, and damage
    //they will keep track of their own sprites and names
    //SOMETHING I HAVEN'T ADDED YET: SPEEDDDDD
    //a gameobject will be spawned, and it will deal damage and knockback to any pest that enters its range. then, it will be destroyed
    //that gameobject will handle the collisions
    [SerializeField] public string weaponName = "Pitchfork";
    [SerializeField] public float attackSpeed = 5;
    [SerializeField] public float weaponAliveTime = 0.2f;
    [SerializeField] public int damage = 5;
    [SerializeField] public int projectileNumber = 1;
    [SerializeField] public int knockback = 1;
    [SerializeField] public GameObject prefab;
    public List<GameObject> weaponInstances = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (projectileNumber <= 0)
        {
            projectileNumber = 1;
        }
        StartCoroutine(SpawnWeapon());
    }

    public virtual IEnumerator SpawnWeapon()
    {
        for (int i = 0; i < projectileNumber; i++)
        {
            weaponInstances.Add(Instantiate(prefab, transform.position, transform.rotation, transform)); //this will be instantiated at the player's position
            yield return StartCoroutine(DestroyWeapon());
        }
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(SpawnWeapon());
    }

    public IEnumerator DestroyWeapon()
    {
        yield return new WaitForSeconds(weaponAliveTime);
        Destroy(weaponInstances[0]);
        weaponInstances.Remove(weaponInstances[0]);
    }
}
