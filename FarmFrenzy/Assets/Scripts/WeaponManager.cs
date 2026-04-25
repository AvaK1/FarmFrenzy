using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //weaponmanager goes on player, when the player gets a weapon it spawns 1 of that weapon on this gameobject, and stores it in an array. if the weapon is chosen again, it calls the weapon's upgrade method
    //will have a dictionary that keeps track of the possible weapons, with their names as the key
    //will have a serialized list to store all of the possible weapons
    [SerializeField] public List<GameObject> allWeapons = new List<GameObject>();
    public Dictionary<string, GameObject> possibleWeapons = new Dictionary<string, GameObject>();
    protected Weapon[] currentWeapons = new Weapon[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //will set the dictionary to be all the weapons from the pool that the player has access to
        foreach (GameObject weapon in allWeapons)
        {
            possibleWeapons.Add(weapon.name, weapon);
        }
        AddWeapon("Pitchfork"); //will spawn the starting weapon and add it to the dictionary right off the bat - change from Pitchfork to it checking whatever the name of the starting weapon is
    }

    public void AddWeapon(string weaponName)
    {
        int nextIndex = 0;
        bool notFound = true;
        while (nextIndex < currentWeapons.Length && notFound) //gets the index of the next empty weapon slot
        {
            if (currentWeapons[nextIndex] == null)
            {
                notFound = false;
            }
            nextIndex++;
        }
        if (!notFound)
        {
            currentWeapons[nextIndex] = Instantiate(possibleWeapons[weaponName], transform.position, transform.rotation, transform).GetComponent<Weapon>();
        }
    }
}
