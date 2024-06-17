using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
   public Slider MPbar;
    public Inventory1 playerInventory;

   void Start()
   {
    MPbar.maxValue = playerInventory.maxMagic;
    MPbar.value = playerInventory.maxMagic;
    playerInventory.currentMagic = playerInventory.maxMagic;
   }

   public void AddMagic()
   {
    MPbar.value += 1;
    playerInventory.currentMagic += 1;
    if(MPbar.value > MPbar.maxValue)
    {
        MPbar.value = MPbar.maxValue;
        playerInventory.currentMagic = playerInventory.maxMagic;
    }
   }

   public void DecreaseMP()
   {
    MPbar.value -= 1;
    playerInventory.currentMagic -= 1;
    if(MPbar.value < 0)
    {
        MPbar.value = 0;
        playerInventory.currentMagic = 0;
    }
   }
}
