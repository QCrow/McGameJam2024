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
        updateReachableList();
        updateWalkableList();
    }


    protected void MoveTo(GameObject targetSquare){
        if(WalkableList.Contains(targetSquare)){
            //self.transform.position = targetSquare.getPosition();
            //transform.rotation = Quaternion.FromToRotation(Vector3.up, targetSquare.getNormal());
            holder.decreaseActionPoint();
            updateReachableList();
            updateWalkableList();
        }
    }  
    protected void LookAtWithFrozenAxis(Vector3 target){
        Vector3 up = transform.up;
        Debug.Log(transform.up);
        Vector3 Direction = target - transform.position;
        Vector3 diff = Vector3.Project(Direction, transform.up);
        Vector3 newDir = Direction - diff;
        float angle = Vector3.SignedAngle(newDir, transform.forward,Vector3.up);
        Debug.Log(angle);
        if( 90f > angle && angle > -90f ){
            transform.localEulerAngles =  transform.localEulerAngles - new Vector3(0f,angle,0f);
        }else{
            transform.localEulerAngles =  transform.localEulerAngles + new Vector3(0f,angle,0f);
        }
        
        
    } 
    void Update(){
        LookAtWithFrozenAxis(Camera.main.transform.position);
    }
}
