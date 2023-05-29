using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{    public GameObject playerPrefab;
     public GameObject BlockPrefab;
     public GameObject claerPrefab;
     public GameObject WallPrefab;
    public GameObject ParticlePrefab;
     public GameObject ClaerText;
    //配列宣言
    int[,] map;//レベルデザイン用
    GameObject[,] field;//ゲーム管理用

    

    // Start is called before the first frame update
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null){ continue; }
                if (field[y, x].tag == "Player") 
                {
                    return new Vector2Int(x, y); 
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
    bool MoveObject(string tag,Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= map.GetLength(1))
        {
            return false;
        }
        if (field[moveTo.y,moveTo.x]!=null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveObject(tag, moveTo, moveTo + velocity);
            if (!success) {return false; }
        }
        if (field[moveTo.y,moveTo.x]!=null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            return false;
        }
        Instantiate(ParticlePrefab, new Vector3(moveFrom.x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - moveFrom.y, 0), Quaternion.identity);
        Instantiate(ParticlePrefab, new Vector3(moveFrom.x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - moveFrom.y, 0), Quaternion.identity);
        Instantiate(ParticlePrefab, new Vector3(moveFrom.x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - moveFrom.y, 0), Quaternion.identity);
        field[moveFrom.y, moveFrom.x].transform.position =new Vector3(moveTo.x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - moveTo.y, 0);
        field[moveTo.y,moveTo.x] = field[moveFrom.y,moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
 
        return true;
    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int>goals= new List<Vector2Int>();
        //要素数を
        for(int y=0;y<map.GetLength(0);y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        //要素数はgoals.Countで取得
        for(int i=0;i<goals.Count;i++)
        {
            GameObject f = field[goals[i].y,goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //1つでも箱がなければ条件未達成
                return false;
            }
        }

        return true;
      
    }
    void Reset()
    {   
        field = new GameObject[map.GetLength(0), map.GetLength(1)];
        
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(BlockPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(claerPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(WallPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
            }
        }
    }
    void Start()
    {
        Screen.SetResolution(1980, 1080, false);

        map = new int[,] {
       {4,4,4,4,4,4,4,4,4,4,4,4},
       {4,0,1,0,3,0,0,2,0,0,0,4},
       {4,0,0,3,2,0,0,2,0,3,0,4},
       {4,0,0,2,4,4,4,4,4,4,3,4},
       {4,0,2,0,0,0,0,3,0,4,0,4},
       {4,0,0,0,0,0,0,0,0,0,0,4},
       {4,4,4,4,4,4,4,4,4,4,4,4},
        };

        field = new GameObject[map.GetLength(0), map.GetLength(1)];
        for (int y=0; y<map.GetLength(0);y++)
        {
            for(int x=0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0),Quaternion.identity);

                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(BlockPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(claerPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(WallPrefab, new Vector3(x - (map.GetLength(1) / 2), (map.GetLength(0) / 2) - y, 0), Quaternion.identity);
                }
            }
        }
     }

    // Update is called once per frame
    void Update()
    {

        Vector2Int playerIndex = GetPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveObject("Box", playerIndex, playerIndex + new Vector2Int(1, 0));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveObject("Box", playerIndex, playerIndex + new Vector2Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveObject("Box", playerIndex, playerIndex + new Vector2Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveObject("Box", playerIndex, playerIndex + new Vector2Int(0, 1));
        }
        //リセット
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Reset();
        //}
        //もしクリアしていたら
        if (IsCleard())
        {
            //テキストを有効にする
            ClaerText.SetActive(true);
        }
       
    }
}
