using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPiece : MonoBehaviour
{

    protected GameObject mainCamera;

    protected Player holder;
    [Header("Attack")]
    [SerializeField]
    protected int _attackDistance;
    [Header("Survival")]
    [SerializeField]
    protected int _endurance;
    protected int _walkDistance;
    protected bool isSelected = false;
    protected List<GameObject> WalkableList;
    protected List<GameObject> ReachableList;
    public abstract void updateReachableList();
    public abstract void updateWalkableList();

    public abstract void Attack(BasicPiece target);


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
        LookAtWithFrozenAxis(mainCamera.transform.position);
    }

    

}
