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
        
        List<Face> availableFace = curf.GetAvailableFace(curf,"car");
        CubeState cs = curf.cubeState;
        foreach ( vList<List<(string finalside, int finalx, int finaly)>> l in availableFace){
            
        }
        String name = cs.GetStateDictionary()[];
        List<Face> newRes = new List<Face>();
        foreach (Face f in availableFace){
            if(f.transform.childCount > 0){
                if(holder.pieces.Contains(f.transform.GetChild(0).gameObject.GetComponent<BasicPiece>())){
                    continue;
                }
                newRes.Add(f);
            }
        }
        
        
        //ReachableList = newRes;
    }


    
    public override void updateWalkableList(){
        
        Face curf = transform.parent.gameObject.GetComponent<Face>();
        
        List<Face> availableFace = curf.getAvailableFace(GetType().Name);
        List<Face> newRes = new List<Face>();
        foreach (Face f in availableFace){
            if(f.transform.childCount > 0){
                continue;
            }
            newRes.Add(f);
        }
        
        //WalkableListt = newRes;
    }




    
}
