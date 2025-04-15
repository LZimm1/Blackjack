using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject shuffleIMG;

    private GameObject shuffleIMGRef;
    public GameObject[] cards;
    public GameObject[] shoe;
    public int numOfDecks = 6;
    private int shoeCards;
    private int shoeCardsLeft;

    public static GameManager instance;
    private int randomIndex;
    private GameObject dealtCard;
    public static bool shuffleReady;
    private GameObject cardBackCover;
    public static bool playerTurn;
    public GameObject cardBack;
    public static bool faceUp = false;
    public static bool newHand = true;
    public Vector3 dealerHandPos;
    public Vector3 playerHandPos;
    public static int playerScore = 0;
    public static int dealerScore = 0;
    public static int secretDealerScore;
    public static bool hit;
    public static bool stand;
    public static int aceCount = 0;
    public static int dealerAceCount = 0;
    public static bool flipCard;
    public static bool play;

    public static bool playerBusted = false;
    public static bool playerBlackjack;
    public static bool dealerBlackjack;
    public static bool playerWin;
    public static bool dealerWin;
    public static bool push;

    public static bool loadBettingMenu;
    private int cardSortingIndex = 0;

    public static bool DDavailable;
    public static bool doubleDown;
    public static string memberID;
    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        shoeCards = 52 * numOfDecks; 
        shoeCardsLeft = shoeCards;
        shuffleReady = true;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
    }

    // Update is called once per frame
    void Update()
    {   
        if(play){
            SceneManager.LoadScene("Game");
            DDavailable = true;
            play = false;
            StartCoroutine(Play()); // flipcard = false, playerturn = true.
            dealerHandPos = new Vector3(-1.8f,1.91f,0f);
            playerHandPos = new Vector3(-1.8f,-1.3f,0f);
            stand = false;
            hit = false;
        }
        StartCoroutine(ShuffleCards()); 
        if(playerTurn){
            flipCard = false; 
            FaceDownCard.destroy = false;
        } 
        if(flipCard){
            flipCard = false;
            FaceDownCard.destroy = true;
        }
        if(doubleDown){
            DealCards();
            doubleDown = false;
            stand = true;
        }
        if(aceCount > 0 && playerScore > 21){ 
            playerScore -= 10;
            aceCount -= 1;
        }
        else if(playerScore > 21 && !playerBusted){ 
            StartCoroutine(playerBust());
        } 
        if(hit && playerTurn){ // player did not hit yet
            StartCoroutine(hitFunc());
            hit = false;
        }
        else if(hit){
            hit = false;
        }
        if (shoeCardsLeft < shoeCards/2){ 
            shuffleReady = true;
        } 
        if(stand && playerTurn){ 
            playerTurn = false;
            DDavailable = false;
        } 
        else if(stand){
            stand = false;
        }
        
        if (playerScore == 21)
        {
            playerTurn = false;
            stand = true;
        }
        if(loadBettingMenu){
            SceneManager.LoadScene("Betting Menu");
            loadBettingMenu = false;
        }
        if(secretDealerScore == 21){
            dealerBlackjack = true;
            stand = true;
            playerTurn = false;
        } 
        
        if(UIScript.submitScore){
            LootLockerSDKManager.SubmitScore(memberID, (int)UIScript.money, "15888", (response) =>
            {
                if (response.statusCode == 200) {
                    Debug.Log("Successful");
                } else {
                    Debug.Log("failed: " + response.Error);
                }
            });
            UIScript.submitScore = false;
            UIScript.money = 1000;
        }
    }
    public void DealCards(){
        randomIndex = UnityEngine.Random.Range(0, shoeCards);
        if(shoe[randomIndex]){
            dealtCard = Instantiate(shoe[randomIndex]);
            shoe[randomIndex] = null;
            shoeCardsLeft--;
            cardSortingIndex++;
            dealtCard.transform.position = new Vector3(playerHandPos.x, playerHandPos.y,0f);
            dealtCard.GetComponent<SpriteRenderer>().sortingOrder = cardSortingIndex;
            playerHandPos = new Vector3(playerHandPos.x + 0.4f, playerHandPos.y, 0f);
            if(dealtCard.tag == "2"){
                playerScore += 2;
            }
            else if(dealtCard.tag == "3"){
                playerScore += 3;
            }
            else if(dealtCard.tag == "4"){
                playerScore += 4;
            }
            else if(dealtCard.tag == "5"){
                playerScore += 5;
            }
            else if(dealtCard.tag == "6"){
                playerScore += 6;
            }
            else if(dealtCard.tag == "7"){
                playerScore += 7;
            }
            else if(dealtCard.tag == "8"){
                playerScore += 8;
            }
            else if(dealtCard.tag == "9"){
                playerScore += 9;
            }
            else if(dealtCard.tag == "10"){
                playerScore += 10;
            }
            else{
                aceCount++;
                playerScore += 11;
            }
        }
        else{
            DealCards();
        }

    }
    IEnumerator ShuffleCards(){
            if(shuffleReady && newHand)
            {
                shoeCards = 52 * numOfDecks; 
                shoeCardsLeft = shoeCards;
                for (int i = 0; i < shoeCards; i++)
                {
                    shoe[i] = cards[i % 52];
                }

                shuffleReady = false;
                if(SceneManager.GetActiveScene().name == "Betting Menu" && !UIScript.submitScore){
                    shuffleIMGRef = Instantiate(shuffleIMG);
                    shuffleIMGRef.transform.position = new Vector3 (-2.247545f,-3.022154f,0f);
                    yield return new WaitForSeconds(1);
                    Destroy(shuffleIMGRef);
                }
            }
    }
    public void DealToDealer(bool faceUp){
        if (faceUp){
            randomIndex = UnityEngine.Random.Range(0, shoeCards);
            if(shoe[randomIndex]){
                dealtCard = Instantiate(shoe[randomIndex]);
                shoe[randomIndex] = null;
                shoeCardsLeft--;
                dealtCard.transform.position = new Vector3(dealerHandPos.x,dealerHandPos.y,0f);
                dealerHandPos = new Vector3(dealerHandPos.x+0.4f,dealerHandPos.y,0f);
                if(dealtCard.tag == "2"){
                    dealerScore += 2;
                    secretDealerScore += 2;
                }
                else if(dealtCard.tag == "3"){
                    dealerScore += 3;
                    secretDealerScore +=3;
                }
                else if(dealtCard.tag == "4"){
                    dealerScore += 4;
                    secretDealerScore += 4;
                }
                else if(dealtCard.tag == "5"){
                    dealerScore += 5;
                    secretDealerScore += 5;
                }
                else if(dealtCard.tag == "6"){
                    dealerScore += 6;
                    secretDealerScore += 6;
                }
                else if(dealtCard.tag == "7"){
                    dealerScore += 7;
                    secretDealerScore += 7;
                }
                else if(dealtCard.tag == "8"){
                    dealerScore += 8;
                    secretDealerScore += 8;
                }
                else if(dealtCard.tag == "9"){
                    dealerScore += 9;
                    secretDealerScore += 9;
                }
                else if(dealtCard.tag == "10"){
                    dealerScore += 10;
                    secretDealerScore += 10; 
                }
                else{
                    dealerScore += 11;
                    secretDealerScore += 11; 
                    dealerAceCount++;
                }
            }
            else{
                DealToDealer(faceUp);
            }
        }
        else{
            randomIndex = UnityEngine.Random.Range(0, shoeCards);
            if(shoe[randomIndex]){
                dealtCard = Instantiate(shoe[randomIndex]);
                shoe[randomIndex] = null;
                shoeCardsLeft--;
                dealtCard.transform.position = new Vector3(dealerHandPos.x,dealerHandPos.y,0f);
                dealerHandPos = new Vector3(dealerHandPos.x+0.4f,dealerHandPos.y,0f);
                cardBackCover= Instantiate(cardBack);
                cardBackCover.transform.position = new Vector3(dealtCard.transform.position.x-0.09f,dealtCard.transform.position.y,0f);
                if(dealtCard.tag == "2"){
                    secretDealerScore += 2;
                }
                else if(dealtCard.tag == "3"){
                    secretDealerScore += 3;
                }
                else if(dealtCard.tag == "4"){
                    secretDealerScore += 4;
                }
                else if(dealtCard.tag == "5"){
                    secretDealerScore += 5;
                }
                else if(dealtCard.tag == "6"){
                    secretDealerScore += 6;
                }
                else if(dealtCard.tag == "7"){
                    secretDealerScore += 7;
                }
                else if(dealtCard.tag == "8"){
                    secretDealerScore += 8;
                }
                else if(dealtCard.tag == "9"){
                    secretDealerScore += 9;
                }
                else if(dealtCard.tag == "10"){
                    secretDealerScore += 10;
                }
                else{
                    secretDealerScore += 11;
                    dealerAceCount++;
                }
            
            }
            else{
                DealToDealer(faceUp);
            }
        }
    }
    IEnumerator Play()
    {
        if(newHand){
            dealerScore = 0;
            secretDealerScore = 0;
            playerScore = 0; 
            dealerAceCount = 0;
            aceCount = 0;
            cardSortingIndex = 0;
            newHand = false;
            DDavailable = true;
           
            yield return new WaitForSeconds(0.5f);
            DealCards();
            yield return new WaitForSeconds(0.5f);
            faceUp = true;
            DealToDealer(faceUp);
            yield return new WaitForSeconds(0.5f);
            DealCards();
            yield return new WaitForSeconds(0.5f);
            faceUp = false; 
            if(playerScore == 21){
                playerBlackjack = true;
                stand = true;
                playerTurn = false;
            }
            playerTurn = true;
            DealToDealer(faceUp);
            
        }
    }
    public IEnumerator playerBust(){
        FaceDownCard.destroy = true;
        playerTurn = false;
        playerBusted = true;
        UIScript.showMoneyWon = true;
        UIScript.bet = 0;     
        yield return new WaitForSeconds(4);
        playerScore = 0;
        SceneManager.LoadScene("Betting Menu");
        newHand = true; 
        playerBusted = false;
        if(UIScript.outOfMoney){
            UIScript.outOfMoney = false;
            shuffleReady = true;
            UIScript.money = 1000;
            SceneManager.LoadScene("Main Menu");
        }        
        
    }
    IEnumerator hitFunc(){
        yield return new WaitForSeconds(0.1f);
        DealCards();
        DDavailable = false;
    }
    public void compareScore(){
        if(playerBlackjack && !dealerBlackjack){
            playerWin = true;
            UIScript.money += (int)(Math.Ceiling((UIScript.bet * 2.5f)));
        }
        else if(dealerBlackjack && !playerBlackjack){
            dealerWin = true;
        }
        else if(playerScore == secretDealerScore){
            push= true;
            UIScript.money += UIScript.bet;
        }
        else if(playerScore > secretDealerScore){
            playerWin = true;
            UIScript.money += UIScript.bet *2;
        }
        else if(playerScore < secretDealerScore && secretDealerScore <22){
            dealerWin = true;
        }
        else if(secretDealerScore > 21){
            playerWin = true;
            UIScript.money += UIScript.bet *2;
        }
        
        UIScript.showMoneyWon = true;
        StartCoroutine(backToMenu());
    }
    IEnumerator backToMenu(){
        yield return new WaitForSeconds(4);
        playerBlackjack = false;
        dealerBlackjack = false;
        playerWin = false;
        dealerWin = false;
        push = false;
        SceneManager.LoadScene("Betting Menu");
        newHand = true; 
        UIScript.bet = 0;
        if(UIScript.outOfMoney){
            UIScript.outOfMoney = false;
            shuffleReady = true;
            UIScript.money = 1000;
            SceneManager.LoadScene("Main Menu");
        }
    }


}
