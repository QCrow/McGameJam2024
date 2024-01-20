using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{

    public Player currentActivePlayer;
    public Player FirstPlayer;

    public Player SecondPlayer;


    // Start is called before the first frame update
    void Start()
    {
        currentActivePlayer = FirstPlayer;
    }

    void Update(){
        
    }

    public void changeSide(){
        if (currentActivePlayer == FirstPlayer){
            currentActivePlayer = SecondPlayer;
        }else{
            currentActivePlayer = FirstPlayer;
        }
    }
    // Update is called once per frame
}
