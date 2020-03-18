using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile_Script : MonoBehaviour
{
    public int m_ID;
    public int m_x;
    public int m_y;
    public Sprite m_blue;
    public Sprite m_green;
    public Sprite m_orange;
    public Sprite m_purple;
    public Sprite m_red;
    public Sprite m_yellow;
    public Sprite m_top;

    public void SetTilePos(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public int GetTileID()
    {
        return m_ID;
    }

    public void SetTileID(int id)
    {
        //Debug.Log("Set tile id");
        m_ID = id;
        switch(m_ID)
        {
            case Param_Script.BLUE_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_blue;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.RED_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_red;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.GREEN_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_green;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.ORANGE_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_orange;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.PURPLE_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_purple;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.YELLOW_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_yellow;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.TOP_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_top;
                transform.GetComponent<Animation>().Stop();
                break;
            case Param_Script.ROLLING_TOP_ID:
                transform.GetComponent<SpriteRenderer>().sprite = m_top;
                transform.GetComponent<Animation>().Play();
                break;
            default:
                transform.GetComponent<SpriteRenderer>().sprite = null;
                transform.GetComponent<Animation>().Stop();
                break;
        }
    }
}
