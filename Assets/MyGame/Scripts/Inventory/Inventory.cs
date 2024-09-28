using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/Iventory")]

public class Iventory : MonoBehaviour
{
    private int activeSlotNum = 0;
    private PlayerControls playerControls;

    private void Awake()
    {
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
        Debug.Log(transform.GetChild(activeSlotNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab.name);
    }
}
