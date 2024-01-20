using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Infantry : BasicPiece
{
    // Start is called before the first frame update
    public override void updateReachableList(){
        //WalkableList = new List<Face>();
        //Face f = transform.parent.gameObject.transform.GetComponent<Face>;
    }


    private void getAdjacentMoves(GameObject f, int remainingMoves){}
    public override void updateWalkableList(){}
}
