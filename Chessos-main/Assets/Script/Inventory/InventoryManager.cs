using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
   [Header("Inventroy Information")]
   public PlayerInventory playerInventory;
   [SerializeField] private GameObject blankInventroySlot;
   [SerializeField] private GameObject inventroyPanel;
   [SerializeField] private TextMeshProUGUI descriptionText;
   [SerializeField] private GameObject useButton;

   public void SetTextAndButton(string description, bool buttonActive)
   {
      descriptionText.text = description;
      if (buttonActive)
      {
         useButton.SetActive(true);
      }
      else
      {
         useButton.SetActive(false);
      }

      void MakeInventorySlots()
      {
         if (playerInventory)
         {
            for(int i = 0; i < playerInventory.myInventory.Count; i ++)
            {
               GameObject temp = 
                  Instantiate(blankInventroySlot, 
                  transform.position, Quaternion.identity);
                  temp.transform.SetParent(inventroyPanel.transform);
               InventorySlot newSlot = temp.GetComponent<InventorySlot>();
               if(newSlot)
               {
               newSlot.transform.SetParent(inventroyPanel.transform);
               newSlot.Setup(playerInventory.myInventory[i], this);
               }
            }
         }
      }
      void Start()
      {
         MakeInventorySlots();
         SetTextAndButton("",false);
      }
   }
}
