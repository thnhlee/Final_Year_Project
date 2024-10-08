using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/Iventory")]

public class Inventory : Singleton<Inventory>
{
    private int activeSlotNum = 0;
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotNum = indexNum;

        foreach(Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if(ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);  
        }

        if (transform.GetChild(activeSlotNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo() == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = transform.GetChild(activeSlotNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        //ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        //newWeapon.transform.parent =ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
