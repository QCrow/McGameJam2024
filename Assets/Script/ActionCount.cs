
using System.Collections.Generic;

using UnityEngine;
using Image = UnityEngine.UI.Image;
public class ActionCount : MonoBehaviour
{
    public List<Sprite> images;
    public RoundController rc;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rc.currentActivePlayer == null) return;
        if(rc.currentActivePlayer == player){
            this.GetComponent<Image>().sprite = images[player.actionPoints];
        }
        
    }
}
