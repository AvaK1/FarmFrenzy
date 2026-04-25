using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBoxUI : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject contentBox;
    private List<GameObject> buttons = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //when the weapon box is created, it needs to enable the panel for the weapon choosing and create the weapon buttons
    void Start()
    {
        CreateButtons();
    }

    public void OpenBox()
    {
        ClearButtons();
        CreateButtons();
    }

    private void ClearButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < 3; i++) //create three buttons, set their icons and names to be random of the possible weapons
        {
            Weapon currentWeapon = weaponManager.allWeapons[i].GetComponent<Weapon>();
            GameObject button = Instantiate(buttonPrefab, contentBox.transform);
            ItemButton buttonScript = button.GetComponent<ItemButton>();
            //set the info on the button to be the item's info
            buttonScript.iconImage.sprite = currentWeapon.icon;
            buttonScript.nameText.text = currentWeapon.weaponName;
            buttonScript.descriptionText.text = currentWeapon.weaponDescription;
            //add the event thing
            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                ChooseWeapon(currentWeapon);
            });
            buttons.Add(button);
        }
    }

    public void ChooseWeapon(Weapon weapon) //to handle weapon improvements, could add a new class called weaponstats that just holds stats for the weapons and make an instance of one or whatever that holds the improvements
    {
        weaponManager.AddWeapon(weapon.weaponName);
        GameUIManager.Instance.OpenOrCloseWeaponBox();
    }
}
