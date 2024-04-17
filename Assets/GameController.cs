using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Player;
    public GameObject[] ButtonAciton;
    public TextMeshProUGUI PLayerTurn;
    private GameObject CurrentPlayer;

    private int PlayerIndex = 0;
    private bool isDoingAction = false;

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
        PLayerTurn.text = string.Format("Player {0} turn!",PlayerIndex+1);
    }
    public void DoAttackAction()
    {
        isDoingAction = isDoingAction == true ? false : true;
        if (isDoingAction == false)
        {
            for (int i = 0; i < Player.Length; i++)
            {
                if (i != PlayerIndex)
                {
                    Player[i].GetComponent<Animator>().SetTrigger("Attacked");
                }
            }
            ButtonAciton[0].GetComponent<Image>().color = Color.white;
            ButtonAciton[1].GetComponent<Button>().enabled = true;
            ButtonAciton[2].GetComponent<Button>().enabled = true;
            return;
        }
        ButtonAciton[0].GetComponent<Image>().color = Color.red;
        ButtonAciton[1].GetComponent<Button>().enabled = false;
        ButtonAciton[2].GetComponent<Button>().enabled = false;

        for (int i = 0; i < Player.Length; i++)
        {
            if (i != PlayerIndex)
            {
                Player[i].GetComponent<Button>().enabled = true;
                Player[i].GetComponent<Animator>().SetTrigger("CanAttack");
            }
        }
    }
    public void DoAttack(GameObject enemy)
    {
        for (int i = 0; i < Player.Length; i++)
        {
            if (i != PlayerIndex)
            {
                //Player[i].GetComponent<Button>().enabled = true;
                Player[i].GetComponent<Animator>().SetTrigger("Attacked");
            }
        }
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
            ButtonAciton[0].GetComponent<Image>().color = Color.white;
            isDoingAction = false;
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
