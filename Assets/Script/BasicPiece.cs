using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPiece : MonoBehaviour
{

    

    
    protected Player holder;
    [Header("Attack")]
    [SerializeField]
    protected int _attackDistance;
    [Header("Survival")]
    [SerializeField]
    public int _endurance;

    public bool isUsable = true;
    protected int _walkDistance;
    protected bool isSelected = false;
    public List<GameObject> WalkableList;
    public List<GameObject> ReachableList;
    public abstract void updateReachableList();
    public abstract void updateWalkableList();

    protected void Attack(BasicPiece target){
        target._endurance--;
        if(target._endurance <= 0){
            target.isUsable = false;
            target.gameObject.SetActive(false);
        }
    }


    protected void MoveTo(GameObject targetSquare){
        if(WalkableList.Contains(targetSquare)){
            //self.transform.position = targetSquare.getPosition();
            //transform.rotation = Quaternion.FromToRotation(Vector3.up, targetSquare.getNormal());
            holder.decreaseActionPoint();
        }
    }  
    protected void LookAtWithFrozenAxis(Vector3 target){

        Vector3 relativePos = target - transform.position;
        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    } 
    void Update(){
        LookAtWithFrozenAxis(Camera.main.transform.position);
    }
}
