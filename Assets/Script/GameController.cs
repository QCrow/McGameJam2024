using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<BasicPiece> currentPlayerPieces;
    public RoundController rc;


    public BasicPiece currentSelectedPiece;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerPieces = rc.getCurrentPlayerPieces();
        checkMouseSelectedPiece();
        checkMouseSelectedPosition();


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
                    if(currentSelectedPiece != bpSelected){
                        currentSelectedPiece = bpSelected;
                        Debug.Log("SelectingPiece");
                        //TODO: indicationg of selected piece
                    }else{
                        currentSelectedPiece = null;
                        Debug.Log("UnSelectingPiece");
                        //TODO: indication of selected piece disabled.
                    }
                }
            }
        }
    }

    private void checkMouseSelectedPosition(){
        if(currentSelectedPiece == null) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask("PositionNode");
        if ( Physics.Raycast (ray,out hit,100.0f, layer_mask)){
            
            Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object

            
            
            //TODO: inplement getPieceAvailable Move
            
            List<Face> availableFaces = currentSelectedPiece.WalkableList;
            List<Face> attackableFaces =  currentSelectedPiece. ReachableList;;
            if(availableFaces.Contains(hit.transform.GetComponent<Face>())){
                Face f = hit.transform.GetComponent<Face>();
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    

                    currentSelectedPiece.MoveTo(f);
                    
                    currentSelectedPiece = null;
                }
            }
            if(attackableFaces.Contains(hit.transform.GetComponent<Face>())){
                Face f = hit.transform.GetComponent<Face>();
                BasicPiece p = Face.getCurrentPiece();
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    currentSelectedPiece.Attack(p);
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
}
