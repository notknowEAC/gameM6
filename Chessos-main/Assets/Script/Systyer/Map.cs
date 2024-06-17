using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static bool MapOpen = false;
    
    public GameObject MapUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapOpen)
            {
                Open();
            } else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        MapUI.SetActive(false);
        MapOpen = false;
    }

    void Close()
    {
        MapUI.SetActive(true);
        MapOpen = true;
    }
}
