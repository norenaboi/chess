using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class GeneralBehaviour : MonoBehaviour
{
    public List<GameObject> pieces;
    public List<GameObject> white_pieces;
    public List<GameObject> black_pieces;

    private Image tempimage;
    public Sprite empty;
    public Sprite tempsprite;
    public PieceMove pm;
    public bool playturn; //if playturn is 1, then white. if 2, then black.

    void Start()
    {
        RefreshRate refreshRate = new RefreshRate();
        refreshRate.numerator = 60;
        refreshRate.denominator = 1;

        playturn = false;
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed, refreshRate);
        SwitchTurns();
    }
    public void SwitchTurns()
    {
        playturn = !playturn;
        if (playturn) 
        {
            foreach (GameObject white_piece in white_pieces)
            {
                white_piece.GetComponent<Button>().interactable = true;
            }
            foreach (GameObject black_piece in black_pieces)
            {
                black_piece.GetComponent<Button>().interactable = false;
            }
        }
        else 
        {
            foreach (GameObject black_piece in black_pieces)
            {
                black_piece.GetComponent<Button>().interactable = true;
            }
            foreach (GameObject white_piece in white_pieces)
            {
                white_piece.GetComponent<Button>().interactable = false;
            }
        }
        
    }    
}


