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
    public GameObject Menu;
    public GameObject Winner;
    private GameObject CurrentPlayer;

    private int PlayerIndex = 0;
    private bool isDoingAction = false;

    // Start is called before the first frame update
    void Start()
    {
        Winner.SetActive(false);
        Menu.SetActive(true);
        CurrentPlayer = Player[PlayerIndex];
        CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
    }
    public void NewGame()
    {
        Winner.SetActive(false);
        //Menu.SetActive(true);
        PlayerIndex = 0;
        CurrentPlayer = Player[PlayerIndex];
        CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
        foreach (var player in Player)
        {
            player.GetComponent<PlayerController>().InitPlayer();
            player.gameObject.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
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
            if (IsGameFinish())
            {
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
        }
        else
        {
            PlayerIndex = 0;
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(false);
            CurrentPlayer = Player[PlayerIndex];
            CurrentPlayer.transform.Find("Action").gameObject.SetActive(true);
            if (IsGameFinish())
            {
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
        }
        PLayerTurn.text = string.Format("Player {0} turn!", PlayerIndex + 1);
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
            foreach(var item in ButtonAciton)
            {
                item.GetComponent<Button>().enabled = true;
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
        if (CurrentPlayer.GetComponent<PlayerController>().DoAction(PlayerController.PlayerAction.Def))
            NextPlayer();
    }
    public void DoSkip()
    {
        CurrentPlayer.GetComponent<PlayerController>().DoAction(PlayerController.PlayerAction.Skip);
        NextPlayer();
    }

    [System.Obsolete]
    private bool IsGameFinish()
    {
        int _player_left = 0;
        for (int i = 0; i < Player.Length; i++)
        {
            if (Player[i].gameObject.active == true)
            {
                _player_left += 1;
            }
        }
        if (_player_left <= 1)
        {
            Winner.SetActive(true);
            Winner.GetComponent<TextMeshProUGUI>().text = string.Format("Player {0} is the WINNER!", PlayerIndex + 1);
            Menu.SetActive(true);
            return true;
        }
        return false;
    }
}
