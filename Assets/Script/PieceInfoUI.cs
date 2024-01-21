
using System.Collections.Generic;

using UnityEngine;

using Image = UnityEngine.UI.Image;
public class PieceInfoUI : MonoBehaviour
{
    // Start is called before the first frame update

    public BasicPiece bp;
    public List<Sprite> images;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(images == null) return;
        if(bp._endurance >= images.Count || bp._endurance < 0) return;
        if(images[bp._endurance] == null){
            return;
        }

        this.GetComponent<Image>().sprite = images[bp._endurance];
    }
}
