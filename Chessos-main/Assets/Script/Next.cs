using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Next : MonoBehaviour
{   
    void Start()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
