using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<BasicPiece> currentPlayerPieces;
    public RoundController rc;

    public Indicator WalkableIndicatorPrefab;
    public Indicator ReachableIndicatorPrefab;
    public List<Indicator> generatedIndicators;
    public BasicPiece currentSelectedPiece;

    public static bool KeySelected = false;
    void Start()
    {
        generatedIndicators = new List<Indicator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(RubikControl.Instance.isCubeRotating) return;
        if(FaceControl.Instance.isDragging) return;
        currentPlayerPieces = rc.getCurrentPlayerPieces();
        checkMouseSelectedPiece();
        checkMouseSelectedPosition();
        checkPlayerLose();

    }

    private void checkPlayerLose(){
        if(rc.FirstPlayer.pieces.Count <=0){
            //rc.FirstPlayer.giveUp();
            endGameWithWinner(rc.SecondPlayer.PlayerName);
            return;

        }
        if(rc.SecondPlayer.pieces.Count <=0){
            //rc.FirstPlayer.giveUp();
            endGameWithWinner(rc.FirstPlayer.PlayerName);
        }
    }
    private void checkMouseSelectedPiece(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask("Piece","PositionalNode");
        if ( Physics.Raycast (ray,out hit,100.0f, layer_mask)) {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("PositionalNode")){
                Debug.Log("NotDirectlyOnChessPiece");
                return;
            }
            Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object  
            //CHeck for input to select piece
            if(currentPlayerPieces.Contains(hit.transform.GetComponent<BasicPiece>()) && hit.transform.GetComponent<BasicPiece>().isUsable){
                BasicPiece bpSelected = hit.transform.GetComponent<BasicPiece>();
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    KeySelected = true;
                    if(currentSelectedPiece != bpSelected){
                        KeySelected = true;
                        DestryIndicators();
                        currentSelectedPiece = bpSelected;
                        currentSelectedPiece.updateReachableList();
                        currentSelectedPiece.updateWalkableList();
                        
                        Debug.Log("SelectingPiece");
                        //print(bpSelected.ReachableList);
                        GenerateIndicatorOnReachableFaces(bpSelected.ReachableList);
                        //print(bpSelected.WalkableList);
                        GenerateIndicatorOnWalkableFaces(bpSelected.WalkableList);

                        //TODO: indicationg of selected piece
                    }else{
                        KeySelected = false;
                        currentSelectedPiece = null;
                        Debug.Log("UnSelectingPiece");
                        
                        DestryIndicators();
                        
                    }
                }
            }
        }
    }

    
    public void GenerateIndicatorOnWalkableFaces(List<Face> faces){
        Debug.Log(faces.Count);
        foreach(Face f in faces){
            Indicator g = Instantiate(WalkableIndicatorPrefab, f.transform.position, Quaternion.identity);
            g.generator = f;
            
            generatedIndicators.Add(g);
        }
    }
    public void GenerateIndicatorOnReachableFaces(List<Face> faces){
        foreach(Face f in faces){
            Indicator g = Instantiate(ReachableIndicatorPrefab, f.transform.position, Quaternion.identity);
            //Debug.Log(g);
            g.generator = f;
            generatedIndicators.Add(g);
        }
    }

    public void DestryIndicators(){
        for(int i = 0; i < generatedIndicators.Count; i ++){
            Indicator ind = generatedIndicators[i];
            DestroyImmediate(ind.gameObject);
            
        }
        generatedIndicators = new List<Indicator>();
        
    }
    private void checkMouseSelectedPosition(){

        if(currentSelectedPiece == null) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask("Square");
        if ( Physics.Raycast (ray,out hit,100.0f, layer_mask)){
            
            Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object

            
            
            //TODO: inplement getPieceAvailable Move


            List<Face> availableFaces = currentSelectedPiece.WalkableList;
            List<Face> attackableFaces =  currentSelectedPiece. ReachableList;;
            if(availableFaces.Contains(hit.transform.GetComponent<Face>())){
                Face f = hit.transform.GetComponent<Face>();
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    

                    currentSelectedPiece.MoveTo(f);
                    DestryIndicators();
                    KeySelected = false;
                    notifyActionMade();
                    currentSelectedPiece = null;        
                }
            }
            if(attackableFaces.Contains(hit.transform.GetComponent<Face>())){
                Face f = hit.transform.GetComponent<Face>();
                BasicPiece p = f.getCurrentPiece();
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    currentSelectedPiece.Attack(p);
                    DestryIndicators();
                    KeySelected = false;
                    notifyActionMade();
                    currentSelectedPiece = null;
                }
            }
            
        }
    }

    public void endGameWithWinner(string winnerName){
        print("Player : " + winnerName + " Has Won !");

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }

    public void notifyActionMade(){
        rc.decreaseCurrentPlayerActionPoint();
    }
}
