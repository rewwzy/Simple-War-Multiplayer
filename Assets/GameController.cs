using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Player;
    private GameObject CurrentPlayer;
    private int PlayerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = Player[PlayerIndex];
        CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
    }

    [System.Obsolete]
    void NextPlayer()
    {
        if (PlayerIndex < Player.Length - 1)
        {
            PlayerIndex += 1;
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(false);
            CurrentPlayer = Player[PlayerIndex];
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
            while (Player[PlayerIndex].active == false)
            {
                if (PlayerIndex < Player.Length - 1)
                {
                    PlayerIndex += 1;
                    CurrentPlayer.transform.Find("Action").gameObject.SetActive(false);
                    CurrentPlayer = Player[PlayerIndex];
                    CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
                }
                else
                {
                    PlayerIndex = 0;
                    CurrentPlayer.transform.Find("Action").gameObject.SetActive(false);
                    CurrentPlayer = Player[PlayerIndex];
                    CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
                }
            }
        }
        else
        {
            PlayerIndex = 0;
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(false);
            CurrentPlayer = Player[PlayerIndex];
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
        }
    }
    public void DoAttackAction()
    {
        for (int i = 0; i < Player.Length; i++)
        {
            if (i != PlayerIndex)
            {
                Player[i].GetComponent<Button>().enabled = true;
            }
        }
    }
    public void DoAttack(GameObject enemy)
    {
        var current = CurrentPlayer.GetComponent<PlayerController>();
        if (current.DoAction(PlayerController.PlayerAction.Attack))
        {
            var target = enemy.GetComponent<PlayerController>();
            if (target.TakeDame())
            {
                Debug.Log("Your enermy taken dame");
            }
            else
            {
                Debug.Log("Your enermy under protect");
            }
            NextPlayer();
        }
        else
        {
            Debug.Log("You cannot do Attack");
        }
    }
    public void DoDef()
    {
        CurrentPlayer.GetComponent<PlayerController>().DoAction(PlayerController.PlayerAction.Def);
        NextPlayer();
    }
    public void DoSkip()
    {
        CurrentPlayer.GetComponent<PlayerController>().DoAction(PlayerController.PlayerAction.Skip);
        NextPlayer();
    }
}
