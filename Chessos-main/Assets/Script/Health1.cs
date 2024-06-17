using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health1 : MonoBehaviour
{
   [SerializeField] private int health = 100;
   public int MAX_HEALTH = 100;
    
   public void Damage(int amount)
   {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
   }
   public void Die()
   {
        Debug.Log("I am Dead!");
        SceneManager.LoadScene("Cut f 1", LoadSceneMode.Single);
   }
}
