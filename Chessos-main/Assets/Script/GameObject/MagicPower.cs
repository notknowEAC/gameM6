using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPower : PowerMP
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        powerMP.Raise();
        Destroy(this.gameObject);
    }
}
