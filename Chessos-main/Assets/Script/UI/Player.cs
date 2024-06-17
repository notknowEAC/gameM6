using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Slider Health;
    [SerializeField] private Slider MP;

    void Start()
    {
        DontDestroyOnLoad(Health);
        DontDestroyOnLoad(MP);
    }
}