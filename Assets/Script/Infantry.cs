using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Infantry : BasicPiece
{
    // Start is called before the first frame update
    public override void updateReachableList(){
        /*
        Face curf = transform.parent.gameObject.getComponent<Dace>();
        List<Face> availableFace = curf.getAvailableFace(GetType().Name);
        List<Face> newRes = new List<Face>();
        foreach (Face f in availableFace){
            if(f.isOccupied){
                if(holder.pieces.Conttains(f)){
                    continue;
                }
                newRes.Add(f);
            }
        }
        */
        //ReachableList = newRes;
    }


    
    public override void updateWalkableList(){
        /*
        Face curf = transform.parent.gameObject.getComponent<Dace>();
        List<Face> availableFace = curf.getAvailableFace(GetType().Name);
        List<Face> newRes = new List<Face>();
        foreach (Face f in availableFace){
            if(f.isOccupied){
                continue;
            }
            newRes.Add(f);
        }
        */
        //WalkableListt = newRes;
    }




    
}
