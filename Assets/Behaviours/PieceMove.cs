using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class PieceMove : MonoBehaviour
{
    public AddToBoard atb;
    public GeneralBehaviour gb;
    public PieceDefeat pd;
    public GameObject tempmoveprefab;
    public GameObject checkprefab;
    public List<GameObject> TemporaryShowMoveSquares;
    private Image tempimage; // temporary move sprite
    private GameObject tempcurrentmovingpiece;
    public Transform canvas;
    public bool notoccupied;

    // (1,1) on the board is (-362.9, -367) pixels

    public void MoveType(GameObject currentmovingpiece)
    {
        notoccupied = true;
        DeleteTemporaryMoveSquares(); pd.DeleteTemporaryDefeatSquares();
        tempcurrentmovingpiece = currentmovingpiece;
        PieceData tcmpdata = currentmovingpiece.GetComponent<PieceData>(); // tempcurrentmovingpiece's Piece Data

        switch (tcmpdata.piecetype)
        {
            case 0: 
                switch (tcmpdata.pside)
                {
                    case true:
                        Check(tcmpdata.xcord, tcmpdata.ycord + 1);
                        if (tcmpdata.ycord == 2 && notoccupied) { Check(tcmpdata.xcord, tcmpdata.ycord + 2); }
                        pd.CheckDefeatForPawn(currentmovingpiece);
                        pd.CheckEnPassant(currentmovingpiece);
                        break;
                    case false:
                        Check(tcmpdata.xcord, tcmpdata.ycord - 1);
                        if (tcmpdata.ycord == 7 && notoccupied) { Check(tcmpdata.xcord, tcmpdata.ycord - 2); }
                        pd.CheckDefeatForPawn(currentmovingpiece);
                        pd.CheckEnPassant(currentmovingpiece);
                        break;
                }
                break; //pawn
            case 1:
                Rook(tcmpdata.xcord, tcmpdata.ycord);
                break; //rook
            case 2:
                Check(tcmpdata.xcord + 1, tcmpdata.ycord + 2);
                Check(tcmpdata.xcord + 1, tcmpdata.ycord - 2);
                Check(tcmpdata.xcord - 1, tcmpdata.ycord - 2);
                Check(tcmpdata.xcord - 1, tcmpdata.ycord + 2);
                Check(tcmpdata.xcord + 2, tcmpdata.ycord + 1);
                Check(tcmpdata.xcord + 2, tcmpdata.ycord - 1);
                Check(tcmpdata.xcord - 2, tcmpdata.ycord + 1);
                Check(tcmpdata.xcord - 2, tcmpdata.ycord - 1);
                break; //horse
            case 3:
                Bishop(tcmpdata.xcord, tcmpdata.ycord);
                break; //bishop
            case 4:
                Check(tcmpdata.xcord + 1, tcmpdata.ycord);
                Check(tcmpdata.xcord, tcmpdata.ycord + 1);
                Check(tcmpdata.xcord + 1, tcmpdata.ycord + 1);

                Check(tcmpdata.xcord - 1, tcmpdata.ycord);
                Check(tcmpdata.xcord, tcmpdata.ycord - 1);
                Check(tcmpdata.xcord - 1, tcmpdata.ycord - 1);

                Check(tcmpdata.xcord + 1, tcmpdata.ycord - 1);
                Check(tcmpdata.xcord - 1, tcmpdata.ycord + 1);
                break; //king
            case 5:
                Bishop(tcmpdata.xcord, tcmpdata.ycord);
                Rook(tcmpdata.xcord, tcmpdata.ycord);
                break; //queen
        }
    }
    private void Check(int xcord, int ycord) 
    {
        if (1 <= xcord && xcord <= 8 && 1 <= ycord && ycord <= 8) 
        {
            Vector2 point = CreateVector(xcord, ycord);
            GameObject tempcheck = gb.pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (xcord, ycord));
            
            if (tempcheck != null) 
            { 
                notoccupied = false;
                PieceData tcmpdata = tempcurrentmovingpiece.GetComponent<PieceData>();
                PieceData tmpdata = tempcheck.GetComponent<PieceData>();

                if (tcmpdata.pside != tmpdata.pside && tcmpdata.piecetype != 0) { pd.ShowDefeat(tempcurrentmovingpiece, tempcheck, xcord, ycord, tmpdata.pside); }
            } 

            else 
            {
                GameObject movebutton = Instantiate(tempmoveprefab, CreateVector(xcord, ycord), transform.rotation, canvas);
                TemporaryShowMoveSquares.Add(movebutton);
                movebutton.GetComponent<Button>().onClick.AddListener(() => Move(tempcurrentmovingpiece.GetComponent<PieceData>(), xcord, ycord, point)); 
            }
        }
    }
    public void Move(PieceData tcmpdata, int nextxcord, int nextycord, Vector2 vc2)
    {
        if (tcmpdata.piecetype == 0)
        {
            switch (tcmpdata.pside)
            {
                case true:
                    if (nextycord == tcmpdata.ycord + 2) { tcmpdata.enpassantable = true; }
                    break;

                case false:
                    if (nextycord == tcmpdata.ycord - 2) { tcmpdata.enpassantable = true; }
                    break;
            }
        }

        tcmpdata.xcord = nextxcord;
        tcmpdata.ycord = nextycord;
        tempcurrentmovingpiece.GetComponent<Transform>().position = CreateVector(nextxcord, nextycord);
        DeleteTemporaryMoveSquares();
        pd.DeleteTemporaryDefeatSquares();
        gb.SwitchTurns();
        string atbdata;
        if (tcmpdata.pside) { atbdata = "1"; } else { atbdata = "0"; }
        atbdata += tcmpdata.piecetype + "0" + tcmpdata.xcord + tcmpdata.ycord;
        atb.MoveBoard(atbdata);
    }
    public void DeleteTemporaryMoveSquares()
    {
        foreach (GameObject temporarymovesquare in TemporaryShowMoveSquares) { Destroy(temporarymovesquare); }
        TemporaryShowMoveSquares.Clear();
    }
    private void MoveEnpassantable(PieceData tcmpdata, int nextxcord, int nextycord, Vector2 vc2)
    {
        tcmpdata.xcord = nextxcord;
        tcmpdata.ycord = nextycord;
        tcmpdata.enpassantable = true;
        tempcurrentmovingpiece.GetComponent<Transform>().position = CreateVector(nextxcord, nextycord);
        DeleteTemporaryMoveSquares();
        pd.DeleteTemporaryDefeatSquares();
        gb.SwitchTurns();
        string atbdata;
        if (tcmpdata.pside) { atbdata = "1"; } else { atbdata = "0"; } 
        atbdata += tcmpdata.piecetype + "0" + tcmpdata.xcord + tcmpdata.ycord;
        atb.MoveBoard(atbdata);
    }
    private void Bishop(int xx, int yy)
    {
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx + ttimer, yy + ttimer); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx + ttimer, yy - ttimer); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx - ttimer, yy + ttimer); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx - ttimer, yy - ttimer); }
    }
    private void Rook(int xx, int yy)
    {
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx, yy + ttimer); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx + ttimer, yy); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx - ttimer, yy); }
        notoccupied = true; for (int ttimer = 1; notoccupied && ttimer <= 8; ttimer++) { Check(xx, yy - ttimer); }
    }

    public Vector2 CreateVector(int xcord, int ycord)
    {
        float startpointx = -466.5f;
        float startpointy = -471f;
        float pixelsmovetoside = 103.6f;
        float pixelsmovetoup = 104f;
        Vector2 point = new Vector2((startpointx + pixelsmovetoside * xcord) / 51.42858f, (startpointy + pixelsmovetoup * ycord) / 51.42858f);
        return point;
    }
}
