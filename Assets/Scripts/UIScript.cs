using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviour
{
    
    public Text playerScoreText;
    public Text dealerScoreText;
    public Text moneyText;
    public Text betText;
    public Text winnerText;
    public Text moneyWonText;
    public static int money = 1000;
    public static int bet = 0;
    public GameObject _1chip;
    public GameObject _10chip;
    public GameObject _100chip;
    public GameObject _500chip;
    public GameObject _1kchip;
    public GameObject _10kchip;
    public GameObject _100kchip;
    public GameObject _1mchip;
    public GameObject ddButton;
    public GameObject hitButton;
    public GameObject standButton;
    private Vector3 bet1pos;
    private Vector3 bet10pos;
    private Vector3 bet100pos;
    private Vector3 bet500pos;
    private Vector3 bet1kpos;
    private Vector3 bet10kpos;
    private Vector3 bet100kpos;
    private Vector3 bet1mpos;
    private Vector3 ddPos;
    private Vector3 hitPos;
    private Vector3 standPos;
    public static bool outOfMoney = false;
    public static bool showMoneyWon = false;
    public static bool submitScore;
    // Start is called before the first frame update
    void Start()
    {
        if(_1chip){
            bet1pos = new Vector3(_1chip.transform.position.x, _1chip.transform.position.y,0f);
            bet10pos = new Vector3(_10chip.transform.position.x, _10chip.transform.position.y,0f);
            bet100pos = new Vector3(_100chip.transform.position.x, _100chip.transform.position.y,0f);
            bet500pos = new Vector3(_500chip.transform.position.x, _500chip.transform.position.y,0f);
            bet1kpos = new Vector3(_1kchip.transform.position.x, _1kchip.transform.position.y,0f);
            bet10kpos = new Vector3(_10kchip.transform.position.x, _10kchip.transform.position.y,0f);
            bet100kpos = new Vector3(_100kchip.transform.position.x, _100kchip.transform.position.y,0f);
            bet1mpos = new Vector3(_1mchip.transform.position.x, _1mchip.transform.position.y,0f);
        }
        if(ddButton){
            ddPos = new Vector3 (ddButton.transform.position.x, ddButton.transform.position.y,0f);
            hitPos = new Vector3 (hitButton.transform.position.x,hitButton.transform.position.y,0f);
            standPos = new Vector3 (standButton.transform.position.x,standButton.transform.position.y,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(_1chip)
        {
            if(money == 0){
                if(bet > 0){
                    _1chip.transform.position = new Vector3(10000f,1000f,0f);
                }
                if(bet == 0){
                    _1chip.transform.position = new Vector3(10000f,1000f,0f);
                }
            }
            else{
                _1chip.transform.position = bet1pos;
            }
            if(money < 10){
                _10chip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _10chip.transform.position = bet10pos;
            }
            if(money < 100){
                _100chip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _100chip.transform.position = bet100pos;
            }
            
            if(money < 500){
                _500chip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _500chip.transform.position = bet500pos;
            }
            if(money < 1000){
                _1kchip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _1kchip.transform.position = bet1kpos;
            }
            if(money < 10000){
                _10kchip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _10kchip.transform.position = bet10kpos;
            }
            if(money < 100000){
                _100kchip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _100kchip.transform.position = bet100kpos;
            }
            if(money < 1000000){
                _1mchip.transform.position = new Vector3(10000f,10000f,0f);
            }
            else{
                _1mchip.transform.position = bet1mpos;
            }

        }
        if(playerScoreText){
            playerScoreText.text = GameManager.playerScore.ToString();
            if(GameManager.playerScore == 0){
                playerScoreText.text = "";
            }
        }

        if(dealerScoreText){
            
            if(GameManager.playerTurn){
                dealerScoreText.text = GameManager.dealerScore.ToString();
            }
            else if (GameManager.playerTurn == false ){
                dealerScoreText.text = GameManager.secretDealerScore.ToString();
            }
            if(GameManager.dealerScore == 0)
            {
                dealerScoreText.text = "";
            }
        }
        if(moneyText){
            moneyText.text = "Bankroll: $" + (string.Format("{0:n0}", money));
        }
        if(betText){
            betText.text = "Bet: $" + (string.Format("{0:n0}", bet));
            if(SceneManager.GetActiveScene().name == "Game"){
                betText.text = "At Stake: $" + (string.Format("{0:n0}", bet));
                if(bet == 0){
                    betText.text = "";
                }
            }
        }
        if(winnerText){
            if(GameManager.playerWin){
                winnerText.text = "Player Wins";
            }
            if(GameManager.dealerWin){
                winnerText.text = "Dealer Wins";
            }
            if(GameManager.push){
                winnerText.text = "Push";
            }
            if(GameManager.playerBusted){
                winnerText.text = "Player Busted";
            }
            if(GameManager.playerBlackjack && !GameManager.dealerBlackjack){
                winnerText.text = "Blackjack!";
            }
        }
        if(moneyWonText){
            if(showMoneyWon){
                if(GameManager.playerBlackjack && !GameManager.dealerBlackjack && bet != 0){
                    moneyWonText.text = "+$" + (string.Format("{0:n0}",(Math.Ceiling((UIScript.bet * 2.5f)))));
                    moneyWonText.color = new Color(0,255,0);
                    bet = 0;
                }
                else if(GameManager.playerWin && bet != 0){
                    moneyWonText.text = "+$" + (string.Format("{0:n0}", bet*2));
                    moneyWonText.color = new Color(0,255,0);
                    bet = 0;
                }
                else if(GameManager.dealerWin){
                    moneyWonText.text = "Bet Lost";
                    moneyWonText.color = new Color(255,0,0);
                    bet = 0;
                }
                else if(GameManager.push){
                    moneyWonText.text = "Break Even";
                    bet = 0;
                }
                else if(GameManager.playerBusted){
                    moneyWonText.text = "Bet Lost";
                    moneyWonText.color = new Color(255,0,0);
                    bet = 0;
                }
                
                showMoneyWon = false;
            }
        }
        if(ddButton){
            if(!GameManager.playerTurn){
                hitButton.transform.position = new Vector3(10000f,10000f,0f);
                standButton.transform.position = new Vector3(10000f,10000f,0f);

            }
            else{
                hitButton.transform.position = hitPos;
                standButton.transform.position = standPos;
            }
            if(!GameManager.DDavailable){
                ddButton.transform.position = new Vector3(10000f,10000f,0f);
            }
            else if(money < bet){
                ddButton.transform.position = new Vector3(100000f,10000f,0f);
            }
            else if(GameManager.playerTurn){
                ddButton.transform.position = ddPos;
            }
            else{
                ddButton.transform.position = new Vector3(100000f,10000f,0f);
            }
            
        }
        if(bet == 0 && money == 0){
            outOfMoney = true;
        }
        if(money > 1000000000){
            money = 1000000000;
        }
        if(bet > 1000000000){
            bet = 1000000000;
        }
    }
    public void hit(){
        if(GameManager.playerTurn){
            GameManager.hit = true;
        }
    }
    public void stand(){
        if(GameManager.playerTurn){
            GameManager.stand = true;
        }
    }
    public void play(){
        if(bet > 0){
            GameManager.play = true;
        }
    }
    public void bet1(){
        if(money >= 1){
            bet += 1;
            money -= 1;
        }
    }
    public void bet10(){
        if(money >= 10){
            bet += 10;
            money -= 10;
        }
    }
    public void bet100(){
        if(money >= 100){
            bet += 100;
            money -= 100;
        }
    }
    public void bet500(){
        if(money >= 500){
            bet += 500;
            money -= 500;
        }
    }
    public void bet1k(){
        if(money >= 1000){
            bet += 1000;
            money -= 1000;
        }
    }
    public void bet10k(){
        if(money >= 10000){
            bet += 10000;
            money -= 10000;
        }
    }
    public void bet100k(){
        if(money >= 100000){
            bet += 100000;
            money -= 100000;
        }
    }
    public void bet1m(){
        if(money >= 1000000){
            bet += 1000000;
            money -= 1000000;
        }
    }
    public void clearBet(){
        money += bet;
        bet = 0;
    }
    public void allIn(){
        bet += money;
        money = 0;
    }
    public void goToBettingMenu(){
        GameManager.loadBettingMenu = true;
    }
    public void doubleDown(){
        if(GameManager.playerTurn && money >= bet && GameManager.DDavailable){
            money -= bet;
            bet *= 2;
            GameManager.DDavailable = false;
            GameManager.doubleDown = true;
        }
    }
    public void cashOut(){
        GameManager.shuffleReady = true;
        submitScore = true;
        money += bet;
        bet = 0;
        SceneManager.LoadScene("Main Menu");
    }

}
