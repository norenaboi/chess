using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PieceData : MonoBehaviour
{
    public PieceMove pm;

    public int xcord;
    public int ycord;
    public Sprite msprite;
    public int piecetype;
    public bool pside;
    public int startxcord;
    public int startycord;

    public bool enpassantable;
    public bool firstmove;

    private float startpointx = -466.5f;
    private float startpointy = -471f;
    private float pixelsmovetoside = 103.6f;
    private float pixelsmovetoup = 104f;

    void Start() 
    { 
        xcord = startxcord; ycord = startycord;
        transform.position = new Vector2((startpointx + pixelsmovetoside * startxcord) / 51.42858f, (startpointy + pixelsmovetoup * startycord)/ 51.42858f);
        gameObject.GetComponent<Image>().sprite = msprite;
        gameObject.GetComponent<Button>().onClick.AddListener(() => pm.MoveType(gameObject));
        if (piecetype == 0) { firstmove = false; enpassantable = false; } else { firstmove = false; }
    }
}
