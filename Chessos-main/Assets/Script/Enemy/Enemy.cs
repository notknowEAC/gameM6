using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public EnemyBase Base { get; set;}
    public int Level { get; set; }

    public int HP { get; set; }
    public List<Move> Moves { get; set;}

    public bool HpChanged { get; set; }
    public event System.Action OnHPChanged;
    public Enemy(EnemyBase pBase,int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;
        
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }


    public int Attack{
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) +5; }
    }

    public int Defense{
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) +5; }
    }

    public int SpAttack{
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) +5; }
    }

    public int SpDefense{
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) +5; }
    }
    
    public int Speed{
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) +5; }
    }

    public int MaxHp{
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) +10; }
    }

    public int MaxMp{
        get { return Mathf.FloorToInt((Base.MaxMp * Level) / 100f) +15; }
    }

    public DamageDetails TakeDamage(Move move, Enemy attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        var damageDetails = new DamageDetails()
        {
            Critical = critical,
            Fainted = false
        };
        
        float modifiers = Random.Range(0.85f, 1f) * critical;
        float a = ( 2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers );

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }


        return damageDetails;
    }
    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
}
