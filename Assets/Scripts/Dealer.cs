using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public GameManager gameManagerRef;
    public static bool reenterDealFn = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerRef = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.playerTurn == false && !GameManager.playerBusted){
            GameManager.flipCard = true;
        }
        if(GameManager.stand && FaceDownCard.destroy == false){
            StartCoroutine(dealerPlay());
            GameManager.stand = false;
        }
    }
    public IEnumerator dealerPlay(){
        if(GameManager.secretDealerScore == 21){
            GameManager.dealerBlackjack = true;
        } 
        while(GameManager.secretDealerScore < 17  && !GameManager.playerBlackjack ){
            yield return new WaitForSeconds(1f);
            GameManager.faceUp = false;
            gameManagerRef.DealToDealer(GameManager.faceUp);
   
        }
        if(GameManager.dealerAceCount > 0 && GameManager.secretDealerScore > 21){
            GameManager.secretDealerScore -= 10;
            GameManager.dealerAceCount -= 1;
            reenterDealFn = true;
        }
        if(reenterDealFn){
            reenterDealFn = false;
            StartCoroutine(dealerPlay());
        }
        else{
            gameManagerRef.compareScore();
        }

    }
    
}
