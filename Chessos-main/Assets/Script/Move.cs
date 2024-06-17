using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int MP {get; set; }

    public  Move(MoveBase pBase)
    {
        Base = pBase;
        MP = pBase.MP;  
    }
}
