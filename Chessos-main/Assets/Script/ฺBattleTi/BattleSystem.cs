using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy , RunningTurn , Item}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHub playerHud;
    [SerializeField] BattleHub enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] Canvas BattleSyste;
    [SerializeField] private UIInventoryPage inventoryUI;

    BattleState state;
    int currentAction;
    int currentMove;

    int escapeAttempts;

    private void Start()
    {
        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.enemy);
        enemyHud.SetData(enemyUnit.enemy);

        dialogBox.SetMoveNames(playerUnit.enemy.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.enemy.Base.Name} appeard.");
        yield return new WaitForSeconds(1f);

        escapeAttempts = 0;
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void Run()
    {
        state = BattleState.Busy;
        StartCoroutine(dialogBox.TypeDialog("run"));
        dialogBox.EnableActionSelector(true);
        SceneManager.LoadSceneAsync(1);
    }

    void Item()
    {
        state = BattleState.Item;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(false);
        BattleSyste.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(true);
    }
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.enemy.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.enemy.Base.Name} used {move.Base.Name}");

        playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();
        var damageDetails = enemyUnit.enemy.TakeDamage(move, playerUnit.enemy);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.enemy.Base.Name} Fainted");
            SceneManager.LoadScene("Obby", LoadSceneMode.Single);
            enemyUnit.PlayFaintAnimtion();
            yield return new WaitForSeconds(2f);
            
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.enemy.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.enemy.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.enemy.TakeDamage(move, playerUnit.enemy);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.enemy.Base.Name} Fainted");
            playerUnit.PlayFaintAnimtion();
            SceneManager.LoadScene("Obby", LoadSceneMode.Single);
            yield return new WaitForSeconds(2f);

        }
        else
        {
           PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!");
    }
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
       if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < playerUnit.enemy.Moves.Count - 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < playerUnit.enemy.Moves.Count - 2)
                currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 1)
                currentAction -= 2;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentAction == 0 )
            {
                //Fight
                PlayerMove();
            }
        
            else if (currentAction == 1)
            {
                //Magic
            }

            else if (currentAction == 2)
            {
                //item
                Item();
            }

            else if (currentAction == 3)
            {
                //Run
                Run();
            }

        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.enemy.Moves.Count - 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.enemy.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.enemy.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
