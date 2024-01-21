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

    public List<BasicPiece> getCurrentPlayerPieces(){
        return currentActivePlayer.pieces;
    }
    // Update is called once per frame
    
    public void decreaseCurrentPlayerActionPoint(){
        currentActivePlayer.decreaseActionPoint();
        if (currentActivePlayer.actionPoints <= 0){
            changeSide();
        }
    }
}
