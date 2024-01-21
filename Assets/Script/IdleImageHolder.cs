using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Image = UnityEngine.UI.Image;
public class IdleImageHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite MostConstantState;
    public Sprite LessCOnstantState;

    public float interval = 0.25f;

    public float Count;
    void Start()
    {
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Count += Time.deltaTime;
        if(Count >= interval){
            int xcount = UnityEngine.Random.Range(0, 10);
            if(xcount >= 9){
                StartCoroutine( Blink());
            }
            this.GetComponent<Image>().sprite = MostConstantState;
            Count = 0f;
        }
    }
    IEnumerator Blink()
    {
        this.GetComponent<Image>().sprite = LessCOnstantState;
        yield return new WaitForSeconds(.05f);
    }

}
