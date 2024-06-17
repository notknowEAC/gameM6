using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
     private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player") == true)
        {
             SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
        }
}
}
