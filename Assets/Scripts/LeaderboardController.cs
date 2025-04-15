using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
public class LeaderboardController : MonoBehaviour
{
    public Text[] entries;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(showScores());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator showScores(){
        while(true){
            
            LootLockerSDKManager.GetScoreList("15888", 10, 0, (response) =>
            {
                if (response.statusCode == 200) {
                    LootLockerLeaderboardMember[] scores = response.items;
                    for(int i = 0; i < scores.Length; i++){
                        entries[i].text = scores[i].rank + ". $" +  (string.Format("{0:n0}", scores[i].score));
                    }
                    if(scores.Length < 10){
                        for(int i = scores.Length;i<10; i++){
                            entries[i].text = "";
                        }
                    }
                }
            });
            yield return new WaitForSeconds(1);
        }
    }
}
