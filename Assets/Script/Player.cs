using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController gameController;
    public string PlayerName;
    public List<BasicPiece> pieces;
    public RoundController roundController;
    public bool isActive;

    public int defaultActionPoints = 2;
    private int actionPoints;

    void Start(){
        actionPoints = defaultActionPoints;
        foreach(BasicPiece p in pieces){
            p.holder = this;
        }
    }
    // Update is called once per frame
    public void decreaseActionPoint(){
        actionPoints--;

        //check if actions used up
        if(actionPoints == 0){
            roundController.changeSide();
            actionPoints = defaultActionPoints;
        }
    }

    
    public void giveUp(){
        gameController.endGameWithWinner(PlayerName);
    }
}
