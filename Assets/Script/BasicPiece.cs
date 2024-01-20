using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPiece : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField]
    protected int _attackDistance;
    [Header("Survival")]
    [SerializeField]
    protected int _endurance;
    protected List<GameObject> WalkableList;
    protected List<GameObject> ReachableList;
    public abstract void updateReachableList();
    public abstract void gupdateReachableList();

    public abstract void Attact();

    public abstract void MoveTo();

}
