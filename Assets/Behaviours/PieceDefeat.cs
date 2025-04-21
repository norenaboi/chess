using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PieceDefeat : MonoBehaviour
{
    public PieceMove pm;
    public AddToBoard atb;
    public GeneralBehaviour gb;
    public GameObject tempmoveprefab;
    public Transform canvas;
    public List<GameObject> TemporaryShowDefeatSquares;

    private float startpointx = -466.5f;
    private float startpointy = -471f;
    private float pixelsmovetoside = 103.6f;
    private float pixelsmovetoup = 104f;

    public void ShowDefeat(GameObject Defeater, GameObject Defeated, int xcord, int ycord, bool Side)
    {
        Vector2 vc2 = pm.CreateVector(xcord, ycord);
        GameObject movebutton = Instantiate(tempmoveprefab, vc2, transform.rotation, canvas);
        TemporaryShowDefeatSquares.Add(movebutton);
        movebutton.GetComponent<Button>().onClick.AddListener(() => Defeat(Defeater, Defeated, xcord, ycord, vc2, Side));
    }
    public void CheckDefeatForPawn(GameObject pawn)
    {
        PieceData pawndata = pawn.GetComponent<PieceData>();

        switch (pawndata.pside)
        {
            case true:
                GameObject tempcheck1 = gb.black_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord + 1, pawndata.ycord + 1));
                GameObject tempcheck2 = gb.black_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord - 1, pawndata.ycord + 1));

                if (tempcheck1 != null)
                {
                    PieceData tmpdata1 = tempcheck1.GetComponent<PieceData>();
                    ShowDefeat(pawn, tempcheck1, pawndata.xcord + 1, pawndata.ycord + 1, tmpdata1.pside);
                }

                if (tempcheck2 != null)
                {
                    PieceData tmpdata2 = tempcheck2.GetComponent<PieceData>();
                    ShowDefeat(pawn, tempcheck2, pawndata.xcord - 1, pawndata.ycord + 1, tmpdata2.pside);
                }
                break;
            case false:
                GameObject tempcheck3 = gb.white_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord + 1, pawndata.ycord - 1));
                GameObject tempcheck4 = gb.white_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord - 1, pawndata.ycord - 1));

                if (tempcheck3 != null)
                {
                    PieceData tmpdata3 = tempcheck3.GetComponent<PieceData>();
                    ShowDefeat(pawn, tempcheck3, pawndata.xcord + 1, pawndata.ycord - 1, tmpdata3.pside);
                }

                if (tempcheck4 != null)
                {
                    PieceData tmpdata4 = tempcheck4.GetComponent<PieceData>();
                    ShowDefeat(pawn, tempcheck4, pawndata.xcord - 1, pawndata.ycord - 1, tmpdata4.pside);
                }
                break;
        }
    }
    public void CheckEnPassant(GameObject pawn) 
    {
        PieceData pawndata = pawn.GetComponent<PieceData>();
        switch (pawndata.pside)
        {
            case true:
                GameObject tempcheck1 = gb.black_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord + 1, pawndata.ycord));
                GameObject tempcheck2 = gb.black_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord - 1, pawndata.ycord));

                if (tempcheck1 != null)
                {
                    PieceData tmpdata1 = tempcheck1.GetComponent<PieceData>();
                    if (tmpdata1.enpassantable)
                    {
                        ShowDefeat(pawn, tempcheck1, pawndata.xcord + 1, pawndata.ycord + 1, tmpdata1.pside);
                    }
                }

                if (tempcheck2 != null)
                {
                    PieceData tmpdata2 = tempcheck2.GetComponent<PieceData>();
                    if (tmpdata2.enpassantable)
                    {
                        ShowDefeat(pawn, tempcheck2, pawndata.xcord - 1, pawndata.ycord + 1, tmpdata2.pside);
                    }
                }
                break;
            case false:
                GameObject tempcheck3 = gb.white_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord + 1, pawndata.ycord - 1));
                GameObject tempcheck4 = gb.white_pieces.Find(x => (x.GetComponent<PieceData>().xcord, x.GetComponent<PieceData>().ycord) == (pawndata.xcord - 1, pawndata.ycord - 1));

                if (tempcheck3 != null)
                {
                    PieceData tmpdata3 = tempcheck3.GetComponent<PieceData>();
                    if (tmpdata3.enpassantable)
                    {
                        ShowDefeat(pawn, tempcheck3, pawndata.xcord + 1, pawndata.ycord - 1, tmpdata3.pside);
                    }
                }

                if (tempcheck4 != null)
                {
                    PieceData tmpdata4 = tempcheck4.GetComponent<PieceData>();
                    if (tmpdata4.enpassantable)
                    {
                        ShowDefeat(pawn, tempcheck4, pawndata.xcord - 1, pawndata.ycord - 1, tmpdata4.pside);
                    }
                }
                break;
        }
    }
    private void Defeat(GameObject Defeater, GameObject Defeated, int nextxcord, int nextycord, Vector2 vc2, bool Side)
    {
        string atbdata = "";
        Defeater.GetComponent<PieceData>().xcord = nextxcord;
        Defeater.GetComponent<PieceData>().ycord = nextycord;
        Defeater.GetComponent<Transform>().position = new Vector2((startpointx + pixelsmovetoside * nextxcord) / 51.42858f, (startpointy + pixelsmovetoup * nextycord) / 51.42858f);
        atb.AddToTheBoard(Defeated);
        if (Side) { gb.white_pieces.Remove(Defeated); atbdata = "1"; } else { gb.black_pieces.Remove(Defeated); atbdata = "0"; } gb.pieces.Remove(Defeated);
        Defeated.GetComponent<Button>().interactable = false;
        Defeated.GetComponent<Transform>().position = new Vector2(742f, 160f);
        DeleteTemporaryDefeatSquares();
        pm.DeleteTemporaryMoveSquares();
        gb.SwitchTurns();
        atbdata += Defeater.GetComponent<PieceData>().piecetype + "1" + Defeater.GetComponent<PieceData>().xcord + Defeater.GetComponent<PieceData>().ycord;
        atb.MoveBoard(atbdata);
    }
    public void DeleteTemporaryDefeatSquares()
    {
        foreach (GameObject temporarydefeatsquare in TemporaryShowDefeatSquares) { Destroy(temporarydefeatsquare); }
        TemporaryShowDefeatSquares.Clear();
    }
}
