using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
    //private const float SIZE_X = 0.96f;
    //private const float SIZE_Y = -0.64f;
    //private const float ZERO_X = -2.88f;
    //private const float ZERO_Y = 4.48f;

    [Serializable]
    public class toys
    {
        public int x_;
        public int y_;
        public GameObject type_;
        private Tile_Script tile_;

        public toys() { }
        public toys(int x, int y, GameObject type)
        {
            x_ = x;
            y_ = y;
            type_ = type;
        }
        public void SetTileScript(Tile_Script t)
        {
            tile_ = t;
        }
        public Tile_Script GetTileScript()
        {
            return tile_;
        }
    };
    public toys[] m_BoardList;

    void Start()
    {
        CreateTiles();
    }

    private void CreateTiles()
    {
        foreach(var s in m_BoardList)
        {
            Tile_Script tile = Instantiate<Tile_Script>(s.type_.transform.GetComponent<Tile_Script>());
            tile.transform.SetParent(this.transform);
            tile.transform.position = new Vector3(Param_Script.ZERO_X + (s.x_ * Param_Script.SIZE_X), Param_Script.ZERO_Y + (s.y_ * Param_Script.SIZE_Y), 0f);
            tile.SetTilePos(s.x_, s.y_);
            s.SetTileScript(tile);
        }
    }

    public void SetTileType(int x, int y, int change_type)
    {
        //Debug.Log("Set tile tpye is " + change_type);
        toys obj = Array.Find(m_BoardList, p => (p.x_ == x && p.y_ == y));
        obj.GetTileScript().SetTileID(change_type);
    }

    public void SetRollingTop(int x, int y)
    {
        toys obj = Array.Find(m_BoardList, p => (p.x_ == x && p.y_ == y));
        obj.GetTileScript().SetTileID(Param_Script.ROLLING_TOP_ID);
    }


    public int GetTileType(int x, int y)
    {
        toys obj = Array.Find(m_BoardList, p => (p.x_ == x && p.y_ == y));
        if(obj != null)
        {
            //Debug.Log("x = " + x + " y = " + y + " id = " + obj.GetTileScript().GetTileID());
            return obj.GetTileScript().GetTileID();
        }
        //Debug.Log("x = " + x + " y = " + y + " GetTileType is Block");
        return 0;
    }

    public GameObject GetTileGameObject(int x, int y)
    {
        toys obj = Array.Find(m_BoardList, p => (p.x_ == x && p.y_ == y));
        if (obj != null)
        {
            return obj.type_;
        }
        //Debug.Log("GetTileScript is Null");
        return null;
    }

    public Tile_Script GetTileScript(int x, int y)
    {
        toys obj = Array.Find(m_BoardList, p => (p.x_ == x && p.y_ == y));
        if(obj != null)
        {
            return obj.GetTileScript();
        }
        //Debug.Log("GetTileScript is Null");
        return null;
    }

    public void ChangeTilePos(int pos1_x, int pos1_y, int pos2_x, int pos2_y)
    {
        toys obj1 = Array.Find(m_BoardList, p => (p.x_ == pos1_x && p.y_ == pos1_y));
        toys obj2 = Array.Find(m_BoardList, p => (p.x_ == pos2_x && p.y_ == pos2_y));
        if (obj1 != null && obj2 != null)
        {
            int temp_x = obj1.GetTileScript().m_x;
            int temp_y = obj1.GetTileScript().m_y;

            obj1.GetTileScript().SetTilePos(obj2.GetTileScript().m_x, obj2.GetTileScript().m_y);
            obj2.GetTileScript().SetTilePos(temp_x, temp_y);

            obj1.x_ = obj2.x_;
            obj1.y_ = obj2.y_;
            obj2.x_ = temp_x;
            obj2.y_ = temp_y;
        }
        //Debug.Log("ChangeTilePos is Null");
    }
}
