using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class AddToBoard : MonoBehaviour
{
    public PieceMove pm;
    public Transform content;

    public List<Image> whiteSlots;
    public List<Text> whiteSText; // White slot texts
    public List<int> whiteSOcc;   // White slot occupier pieces
    private List<int> whiteSOccA; // White slot occupier piece's amount

    public List<Image> blackSlots;
    public List<Text> blackSText; // Black slot texts
    public List<int> blackSOcc;   // Black slot occupier pieces
    private List<int> blackSOccA; // Black slot occupier piece's amount

    public List<string> previousMoveData;
    public List<string> previousPositionData; // Xcord, Ycord  
    public List<GameObject> previousMovedPiece;
    public List<Text> movetexts;
    public GameObject textPrefab;

    public void AddToTheBoard(GameObject Defeated) 
    {
        bool Side = Defeated.GetComponent<PieceData>().pside;
        PieceData defeateddata = Defeated.GetComponent<PieceData>();
        if (Side)
        {
            if (whiteSOcc.Count.Equals(0) || !whiteSOcc.Contains(defeateddata.piecetype))
            {
                whiteSOcc.Add(defeateddata.piecetype);
                int index = whiteSOcc.IndexOf(defeateddata.piecetype);
                Debug.Log(index);
                whiteSlots[index].sprite = defeateddata.msprite;
                whiteSOccA[index] = 1;
                whiteSText[index].text = "x" + 1;
            }
            else
            {
                int index = whiteSOcc.IndexOf(defeateddata.piecetype);
                Text txt = whiteSText[index];
                string temp = "x" + whiteSOccA[index];
                txt.text = temp;
            }
        }

        else if (!Side)
        {
            if (blackSOcc.Count.Equals(0) || !blackSOcc.Contains(defeateddata.piecetype))
            {
                blackSOcc.Add(defeateddata.piecetype);
                int index = blackSOcc.IndexOf(defeateddata.piecetype);
                Debug.Log(index);
                blackSlots[index].sprite = defeateddata.msprite;
                blackSOccA[index] = 1;
                blackSText[index].text = "x" + 1;
            }
            else
            {
                int index = blackSOcc.IndexOf(defeateddata.piecetype);
                Text txt = blackSText[index];
                string temp = "x" + blackSOccA[index];
                txt.text = temp;
            }
        }
        
    }

    public void MoveBoard(String data) //color, type, defeated, row
    {
        previousMoveData.Add(data);
        GameObject txtobj = Instantiate(textPrefab, content);
        Text txt = txtobj.GetComponent<Text>();
        movetexts.Add(txt);

        String temp = "";

        switch (data[0])
        {
            case '0':
                txt.color = UnityEngine.Color.black; break;
            case '1':
                txt.color = UnityEngine.Color.white; break;

        }

        switch (data[1])
        {
            case '0':
                break;
            case '1':
                temp += "R"; break;
            case '2':
                temp += "N"; break;
            case '3':
                temp += "B"; break;
            case '4':
                temp += "K"; break;
            case '5':
                temp += "Q"; break;
            default:
                temp += "unknownpiecetype"; break;
        }

        switch (data[2])
        {
            case '1':
                temp += "x"; break;
            default: break;
        }

        switch (data[3])
        {
            case '1':
                temp += "a"; break;
            case '2':
                temp += "b"; break;
            case '3':
                temp += "c"; break;
            case '4':
                temp += "d"; break;
            case '5':
                temp += "e"; break;
            case '6':
                temp += "f"; break;
            case '7':
                temp += "g"; break;
            case '8':
                temp += "h"; break;
            default:
                temp += "unknowny"; break;
        }

        temp += data[4];
        txt.text = temp;
    }

    public void RevertMove()
    {
        GameObject temppiece = previousMovedPiece[previousMovedPiece.Count - 1];
        PieceData tcmpdata = temppiece.GetComponent<PieceData>();
        String temppositiondata = previousMoveData[previousMoveData.Count - 1];
        previousMoveData.RemoveAt(previousMoveData.Count - 1);
        movetexts.RemoveAt(movetexts.Count - 1);
        //pm.Move(tcmpdata, CharToInt(temppositiondata[0]), CharToInt(temppositiondata[1]));
    }

    private int CharToInt(char ch)
    {
        int bar = ch - '0';
        return bar;
    }
}
