using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] public float distanceMultiplier = 1.5f;
    public override IEnumerator SpawnWeapon()
    {
        for (int i = 0; i < projectileNumber; i++)
        {
            Vector3 nearestPestPosition = Vector3.zero;
            if (GameManager.Instance.livingPests.Count > 0)
            {
                float closestPestDistance = 100000;
                int closestPestIndex = -1;
                for (int l = 0; l < GameManager.Instance.livingPests.Count; l++)
                {
                    float currentDistance = Vector3.Distance(transform.position, GameManager.Instance.livingPests[l].transform.position);
                    if (currentDistance < closestPestDistance)
                    {
                        closestPestDistance = currentDistance;
                        closestPestIndex = l;
                    }
                }
                nearestPestPosition = GameManager.Instance.livingPests[closestPestIndex].transform.position;
            }

            //Spawning the weapon
            Vector3 pestDirection = nearestPestPosition - GetComponentInParent<Transform>().position;
            GameObject newInstance = Instantiate(prefab, transform.position + pestDirection.normalized * distanceMultiplier, transform.rotation, transform); //this will be instantiated at the player's position

            Vector3 target = GetComponentInParent<Transform>().position;
            target.z = 0;
            target.x = target.x - nearestPestPosition.x;
            target.y = target.y - nearestPestPosition.y;
            float rotationAngle = (Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg) - 270;
            newInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));

            weaponInstances.Add(newInstance); 
            yield return StartCoroutine(DestroyWeapon());
        }
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(SpawnWeapon());
    }
}
