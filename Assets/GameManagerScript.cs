using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //îzóÒêÈåæ
    int[] map;


    // Start is called before the first frame update
    void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }
        //ï∂éöÇÃèoóÕ
        Debug.Log(debugText);
    }
    int GetPlayerIndex()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;

    }
    bool MoveNumber(int number,int moveFrom,int moveTo)
    {
        if(moveTo < 0 || moveTo >= map.Length)
        {
            return false;
        }
        if (map[moveTo] == 2) {
            int velocity = moveTo - moveFrom;
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            if(!success)
            {
                return false;
            }
        }
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }
    void Start()
    {
        map= new int[] {0,0,0,1,0,2,0,2,0 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {

        int playerIndex = GetPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveNumber(1, playerIndex, playerIndex + 1);
               PrintArray();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
    }
}
