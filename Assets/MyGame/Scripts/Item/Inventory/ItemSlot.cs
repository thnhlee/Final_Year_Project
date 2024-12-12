using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //===Item Data===//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;

    [SerializeField]
    private int maxNumberOfItems;

    //===Item Slot ===//
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemUseButton;

    //===Item Description Slot ===//
    public Image itemDescriptionImage;
    public Sprite emptySprite;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionInfo;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("UI").GetComponent<InventoryManager>();
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Check to see if the slot is already full
        if (isFull)
            return quantity;

        this.itemName = itemName;

        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        this.itemDescription = itemDescription;

        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;

            // Return leftover items
            return extraItems;
        }

        // Update quantity text
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        itemDescriptionName.text = itemName;
        itemDescriptionInfo.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        itemUseButton.onClick.RemoveAllListeners();
        itemUseButton.onClick.AddListener(UseItem);
    }

    public void UseItem()
    {
        bool usable = inventoryManager.UseItem(itemName);
        if (usable)
        {
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if (this.quantity <= 0)
                EmptySlot();
        }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        quantity = 0;
        isFull = false;
        itemName = "";
        itemDescription = "";
        itemSprite = emptySprite;
        itemDescriptionName.text = "";
        itemDescriptionInfo.text = "";
        itemDescriptionImage.sprite = emptySprite;

    }
}