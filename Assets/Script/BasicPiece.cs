using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public abstract class BasicPiece : MonoBehaviour
{

    

    
    public Player holder;
    [Header("Attack")]
    [SerializeField]
    protected int _attackDistance;
    [Header("Survival")]
    [SerializeField]
    public int _endurance;

    public bool isUsable = true;
    public int _walkDistance;
    protected bool isSelected = false;
    public List<Face> WalkableList;
    public List<Face> ReachableList;
    public abstract void updateReachableList();
    public abstract void updateWalkableList();

    public void Attack(BasicPiece target){
        target._endurance--;
        if(target._endurance <= 0){
            target.isUsable = false;
            target.gameObject.SetActive(false);
        }
        updateReachableList();
        updateWalkableList();
    }


    public void MoveTo(Face targetSquare){
        if(WalkableList.Contains(targetSquare)){
            transform.SetParent(targetSquare.transform);
            transform.localPosition = new Vector3(0,1,0);
            transform.localEulerAngles = new Vector3(0,0,0);
            holder.decreaseActionPoint();
            updateReachableList();
            updateWalkableList();
        }
    }  
    protected void LookAtWithFrozenAxis(Vector3 target){
        Vector3 up = transform.up;
        //Debug.Log(transform.up);
        Vector3 Direction = target - transform.position;
        Vector3 diff = Vector3.Project(Direction, transform.up);
        Vector3 newDir = Direction - diff;
        float angle = Vector3.SignedAngle(newDir, transform.forward,Vector3.up);
        //Debug.Log(angle);
        if( transform.up.y > 0 ){
            transform.localEulerAngles =  transform.localEulerAngles - new Vector3(0f,angle,0f);
        }else{
            transform.localEulerAngles =  transform.localEulerAngles + new Vector3(0f,angle,0f);
        }
        
        
    } 
    void Update(){
        LookAtWithFrozenAxis(Camera.main.transform.position);
    }
}
