using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Infantry : BasicPiece
{
    // Start is called before the first frame update
    public override void updateReachableList(){
        
        Face curf = transform.parent.gameObject.GetComponent<Face>();       
        List<Face> availableFace = curf.GetAvailableFaces("horse",2); //bishop or horse or car lol
        ReachableList = new List<Face>();
        foreach (Face f in availableFace){
            if(f.transform.childCount > 0){
                Debug.Log("hasChild On Face");
                if(holder.pieces.Contains(f.transform.GetChild(0).gameObject.GetComponent<BasicPiece>())){
                    Debug.Log("Is same side");
                    continue;
                }

                ReachableList.Add(f);
            }
        }
        //Debug.Log(ReachableList.Count);
        
        
    }


    
    public override void updateWalkableList(){
        
        Face curf = transform.parent.gameObject.GetComponent<Face>();
        
        List<Face> availableFace = curf.GetAvailableFaces("horse",2); //bishop or horse or car lol
        
        WalkableList = new List<Face>();
        foreach (Face f in availableFace){
            if(f.transform.childCount > 0){
                continue;
            }
            WalkableList.Add(f);
        }
        
        
    }




    
}
