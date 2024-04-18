using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] HealthBar;
    public GameObject[] goAction;
    public Sprite[] spriteAction;
    public string PlayerName { get; set; }
    public int Health { get; set; }
    public PlayerAction[] Action { get; set; }
    
    public enum PlayerAction
    {
        Idle = 0,
        Attack = 1,
        Def = 2,
        Skip = 3
    }
    private void Start()
    {
        InitPlayer();
    }
    public void InitPlayer()
    {
        Health = 6;
        UpdateHealBar();
        Action = new PlayerAction[3] { PlayerAction.Attack, PlayerAction.Attack, PlayerAction.Attack };
    }
    public bool DoAction(PlayerAction action)
    {
        if (action == PlayerAction.Def)
        {
            foreach (var act in Action)
            {
                if (act == PlayerAction.Def)
                {
                    return false;
                }
            }
        }
        for (int i = 0; i < Action.Length - 1; i++)
        {
            Action[i] = Action[i + 1];
        }
        Action[Action.Length - 1] = action;
        UpdateAction();
        return true;

    }
    public bool TakeDame()
    {
        if (Action[2] == PlayerAction.Def)
        {
            return false;
        }
        else
        {
            if (Health > 1)
            {
                Health -= 1;
                UpdateHealBar();

            }
            else if (Health == 1)
            {
                Dead();
            }
            return true;
        }
    }
    void UpdateHealBar()
    {
        for (int i = 6; i > 1; i--)
        {
            if( i > Health)
            {
                HealthBar[i-1].SetActive(false);
                Debug.Log("TakenDame");
            }
        }
    }
    void UpdateAction()
    {
        goAction[0].GetComponent<Image>().sprite = spriteAction[((int)Action[0])-1];
        goAction[1].GetComponent<Image>().sprite = spriteAction[((int)Action[1])-1];
        goAction[2].GetComponent<Image>().sprite = spriteAction[((int)Action[2])-1];
    }
    public void Dead()
    {
        Health = 0;
        Player.SetActive(false);
    }
}
