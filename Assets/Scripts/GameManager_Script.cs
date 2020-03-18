using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager_Script : MonoBehaviour
{
    //const int BLUE_ID = 1;
    //const int GREEN_ID = 2;
    //const int ORANGE_ID = 3;
    //const int PURPLE_ID = 4;
    //const int RED_ID = 5;
    //const int YELLOW_ID = 6;
    //const int TOP_ID = 7;
    //const int MAX_X = 6;
    //const int MAX_Y = 12;

    private RaycastHit2D m_TouchBeganHit;
    private RaycastHit2D m_TouchMovedHit;
    private RaycastHit2D m_TouchBeganHit_save;
    private RaycastHit2D m_TouchMovedHit_save;
    private bool m_isTouch = false;

    public Board m_board;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        int count = Input.touchCount;
        if (count == 0) return;
        for (int i = 0; i < count; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                TouchBegan(i);
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                TouchMoved(i);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                m_isTouch = false;
            }
        }
    }

    private RaycastHit2D GetTouchPoint(int count)
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(count).position);
        RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 10.0f);
        return hit;
    }

    private void TouchBegan(int i)
    {
        if (m_isTouch)
        {
            return;
        }

        if (GetTouchPoint(i))
        {
            Debug.Log("Began");
            m_TouchBeganHit = GetTouchPoint(i);
            m_isTouch = true; 
        }
    }

    private void TouchMoved(int i)
    {
        if(GetTouchPoint(i))
        {
            Debug.Log("Moved");
            m_TouchMovedHit = GetTouchPoint(i);
            if(m_isTouch && m_TouchBeganHit.transform.position != m_TouchMovedHit.transform.position)
            {
                EventSwap(m_TouchBeganHit, m_TouchMovedHit);
                Invoke("CheckAnwser", 0.5f);
                //CheckAnwser();
                m_isTouch = false;
            }
        }
    }

    private void EventSwap(RaycastHit2D pos1, RaycastHit2D pos2)
    {
        //Debug.Log("pos1 = (" + pos1.transform.position.x + ", " + pos1.transform.position.y + ")");
        //Debug.Log("pos2 = (" + pos2.transform.position.x + ", " + pos2.transform.position.y + ")");
        if (pos1 && pos2)
        {
            //Debug.Log("SwapStart");
            Vector3 tempPos1 = pos1.transform.position;
            Vector3 tempPos2 = pos2.transform.position;
            pos1.rigidbody.DOMove(tempPos2, 0.5f, false);
            pos2.rigidbody.DOMove(tempPos1, 0.5f, false);

            SwapPos(pos1, pos2);
        }
    }

    private void SwapPos(RaycastHit2D pos1, RaycastHit2D pos2)
    {
        //Tile_Script BeganHitTile = m_TouchBeganHit.transform.GetComponent<Tile_Script>();
        //Tile_Script MovedHitTile = m_TouchMovedHit.transform.GetComponent<Tile_Script>();

        Tile_Script BeganHitTile = pos1.transform.GetComponent<Tile_Script>();
        Tile_Script MovedHitTile = pos2.transform.GetComponent<Tile_Script>();

        m_board.ChangeTilePos(BeganHitTile.m_x, BeganHitTile.m_y, MovedHitTile.m_x, MovedHitTile.m_y);
    }

    private void SwapPos(Board.toys pos1, Board.toys pos2)
    {
        if (pos1 == null && pos2 == null)
        {
            return;
        }

        Tile_Script BeganHitTile = pos1.type_.transform.GetComponent<Tile_Script>();
        Tile_Script MovedHitTile = pos2.type_.transform.GetComponent<Tile_Script>();

        m_board.ChangeTilePos(BeganHitTile.m_x, BeganHitTile.m_y, MovedHitTile.m_x, MovedHitTile.m_y);
    }

    private void EventDrop(Board.toys pos1, Board.toys pos2)
    {
        Debug.Log("Drop");
        //Debug.Log("pos1 = (" + pos1.transform.position.x + ", " + pos1.transform.position.y + ")");
        //Debug.Log("pos2 = (" + pos2.transform.position.x + ", " + pos2.transform.position.y + ")");
        if (pos1 != null && pos2 != null)
        {
            //Debug.Log("SwapStart");
            Vector3 tempPos1 = pos1.type_.transform.position;
            Vector3 tempPos2 = pos2.type_.transform.position;
            pos1.type_.transform.position = tempPos2;
            pos2.type_.transform.position = tempPos1;
            //pos1.transform.GetComponent<Rigidbody2D>().DOMove(tempPos2, 0.1f, false);
            //pos2.transform.GetComponent<Rigidbody2D>().DOMove(tempPos1, 0.1f, false);

            SwapPos(pos1, pos2);
        }
    }

    private void CheckAnwser()
    {
        Tile_Script BeganHitTile = m_TouchBeganHit.transform.GetComponent<Tile_Script>();
        Tile_Script MovedHitTile = m_TouchMovedHit.transform.GetComponent<Tile_Script>();

        int Began_id = BeganHitTile.GetTileID();
        int moved_id = MovedHitTile.GetTileID();

        int Began_left_up = CheckCaseLeftUp(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x - 1, BeganHitTile.m_y - 1, 0)
            + CheckCaseRightDown(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x + 1, BeganHitTile.m_y + 1, 0);

        int Began_right_up = CheckCaseRightUp(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x + 1, BeganHitTile.m_y - 1, 0)
            + CheckCaseLeftDown(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x - 1, BeganHitTile.m_y + 1, 0);

        int Began_center_up = CheckCaseCenterUp(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x, BeganHitTile.m_y - 2, 0)
            + CheckCaseCenterDown(BeganHitTile.m_x, BeganHitTile.m_y, BeganHitTile.m_x, BeganHitTile.m_y + 2, 0);

        int Moved_left_up = CheckCaseLeftUp(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x - 1, MovedHitTile.m_y - 1, 0)
            + CheckCaseRightDown(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x + 1, MovedHitTile.m_y + 1, 0);

        int Moved_right_up = CheckCaseRightUp(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x + 1, MovedHitTile.m_y - 1, 0)
            + CheckCaseLeftDown(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x - 1, MovedHitTile.m_y + 1, 0);

        int Moved_center_up = CheckCaseCenterUp(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x, MovedHitTile.m_y - 2, 0)
           + CheckCaseCenterDown(MovedHitTile.m_x, MovedHitTile.m_y, MovedHitTile.m_x, MovedHitTile.m_y + 2, 0);

        //모든 경우가 맞지 않으면 되돌린다.
        if(isAllWrong(Began_left_up, Began_right_up, Began_center_up, Moved_left_up, Moved_right_up, Moved_center_up))
        {
            Debug.Log("All Wrong");
            EventSwap(m_TouchBeganHit, m_TouchMovedHit);
            return;
        }

        if(Began_left_up >= 2)
        {
            RemoveLeftUp(Began_id, BeganHitTile.m_x, BeganHitTile.m_y);
            RemoveRightDown(Began_id, BeganHitTile.m_x + 1, BeganHitTile.m_y + 1);
        }

        if (Began_right_up >= 2)
        {
            RemoveRightUp(Began_id, BeganHitTile.m_x, BeganHitTile.m_y);
            RemoveLeftDown(Began_id, BeganHitTile.m_x - 1, BeganHitTile.m_y + 1);
        }

        if (Began_center_up >= 2)
        {
            RemoveCenterUp(Began_id, BeganHitTile.m_x, BeganHitTile.m_y);
            RemoveCenterDown(Began_id, BeganHitTile.m_x, BeganHitTile.m_y + 2);
        }

        if (Moved_left_up >= 2)
        {
            RemoveLeftUp(moved_id, MovedHitTile.m_x, MovedHitTile.m_y);
            RemoveRightDown(moved_id, MovedHitTile.m_x + 1, MovedHitTile.m_y + 1);
        }

        if (Moved_right_up >= 2)
        {
            RemoveRightUp(moved_id, MovedHitTile.m_x, MovedHitTile.m_y);
            RemoveLeftDown(moved_id, MovedHitTile.m_x - 1, MovedHitTile.m_y + 1);
        }

        if (Moved_center_up >= 2)
        {
            RemoveCenterUp(moved_id, MovedHitTile.m_x, MovedHitTile.m_y);
            RemoveCenterDown(moved_id, MovedHitTile.m_x, MovedHitTile.m_y + 2);
        }

        StartDrop();

    }

    private void StartDrop()
    {
        for(int i= Param_Script.MAX_X; i>=0; --i)
        {
            for(int j= Param_Script.MAX_Y; j>0; --j)
            {
                if(m_board.GetTileType(i,j) == Param_Script.BLOCK_ID)
                {
                    continue;
                }

                if(m_board.GetTileType(i, j) == Param_Script.NULL_ID)
                {
                    //Debug.Log(i + ", " + j);
                    //m_board.SetTileType(i, j, Random.Range(1, 7));

                    ChangeColor(i, j);

                    //EventDrop(m_board., m_board.GetTileGameObject(i, j - 1));
                    //StartCoroutine(CoroutineDrop(i, j));
                }
            }
        }
    }

    IEnumerator CoroutineDrop(int i, int j)
    {
        yield return new WaitForSeconds(0.1f);
        //EventDrop(m_board.GetTileGameObject(i, j), m_board.GetTileGameObject(i, j - 1));
    }

    private void ChangeColor(int i, int j)
    {
        int change_color = Random.Range(1, 7);
        if (change_color == m_board.GetTileType(i + 1, j + 1) 
            || change_color == m_board.GetTileType(i + 1, j - 1) || change_color == m_board.GetTileType(i, j + 2))
        {
            ChangeColor(i, j);
        }
        m_board.SetTileType(i, j, change_color);
        return;
    }

    private bool isAllWrong(int case1, int case2, int case3, int case4, int case5, int case6)
    {
        Debug.Log("case 1 = " + case1 + " case 2 = " + case2 + " case 3 = " + case3 + " case 4 = " + case4 + " case 5 = " + case5 + " case 6 = " + case6);
        if (case1 < 2 && case2 < 2 && case3 < 2 && case4 < 2 && case5 < 2 && case6 < 2)
        {
            return true;
        }
        return false;
    }

    private void CheckTop(int MatchingPoint_x, int MatchingPoint_y)
    {
        
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x + 1, MatchingPoint_y + 1))
        {
            m_board.SetTileType(MatchingPoint_x + 1, MatchingPoint_y + 1, Param_Script.NULL_ID);
        }
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x + 1, MatchingPoint_y - 1))
        {
            m_board.SetTileType(MatchingPoint_x + 1, MatchingPoint_y - 1, Param_Script.NULL_ID);
        }
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x - 1, MatchingPoint_y + 1))
        {
            m_board.SetTileType(MatchingPoint_x - 1, MatchingPoint_y + 1, Param_Script.NULL_ID);
        }
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x - 1, MatchingPoint_y - 1))
        {
            m_board.SetTileType(MatchingPoint_x - 1, MatchingPoint_y - 1, Param_Script.NULL_ID);
        }
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y + 2))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y + 2, Param_Script.NULL_ID);
        }
        if (Param_Script.ROLLING_TOP_ID == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y - 2))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y - 2, Param_Script.NULL_ID);
        }

        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x + 1, MatchingPoint_y + 1))
        {
            m_board.SetRollingTop(MatchingPoint_x + 1, MatchingPoint_y + 1);
        }
        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x + 1, MatchingPoint_y - 1))
        {
            m_board.SetRollingTop(MatchingPoint_x + 1, MatchingPoint_y - 1);
        }
        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x - 1, MatchingPoint_y + 1))
        {
            m_board.SetRollingTop(MatchingPoint_x - 1, MatchingPoint_y + 1);
        }
        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x - 1, MatchingPoint_y - 1))
        {
            m_board.SetRollingTop(MatchingPoint_x - 1, MatchingPoint_y - 1);
        }
        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y + 2))
        {
            m_board.SetRollingTop(MatchingPoint_x, MatchingPoint_y + 2);
        }
        if (Param_Script.TOP_ID == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y - 2))
        {
            m_board.SetRollingTop(MatchingPoint_x, MatchingPoint_y - 2);
        }
    }

    /// <summary>
    /// 맞는게 하나라도 있었을 시에 그 부분 제거
    /// </summary>
    /// <param name="MatchedPoint_x"></param>
    /// <param name="MatchedPoint_y"></param>
    /// <param name="MatchingPoint_x"></param>
    /// <param name="MatchingPoint_y"></param>

    private void RemoveLeftUp(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveLeftUp(type_id, MatchingPoint_x - 1, MatchingPoint_y - 1);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    private void RemoveRightDown(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveRightDown(type_id, MatchingPoint_x + 1, MatchingPoint_y + 1);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    private void RemoveRightUp(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveRightUp(type_id, MatchingPoint_x + 1, MatchingPoint_y - 1);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    private void RemoveLeftDown(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveLeftDown(type_id, MatchingPoint_x - 1, MatchingPoint_y + 1);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    private void RemoveCenterUp(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveCenterUp(type_id, MatchingPoint_x, MatchingPoint_y - 2);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    private void RemoveCenterDown(int type_id, int MatchingPoint_x, int MatchingPoint_y)
    {
        if (type_id == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y))
        {
            m_board.SetTileType(MatchingPoint_x, MatchingPoint_y, Param_Script.NULL_ID);
            RemoveCenterDown(type_id, MatchingPoint_x, MatchingPoint_y + 2);
            CheckTop(MatchingPoint_x, MatchingPoint_y);
        }
    }

    /// <summary>
    /// 맞는 게 하나라도 있는 지 검증
    /// </summary>
    /// <param name="MatchedPoint"></param>
    /// <param name="MatchingPoint"></param>
    private int CheckCaseLeftUp(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y) 
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("LeftUp");
            count += CheckCaseLeftUp(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x - 1, MatchingPoint_y - 1, count + 1);
        }
        return count;
    }
    
    private int CheckCaseRightDown(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y)
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("RightDown");
            count += CheckCaseRightDown(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x + 1, MatchingPoint_y + 1, count + 1);
        }
        return count;
    }

    private int CheckCaseRightUp(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y)
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("LeftUp");
            count += CheckCaseRightUp(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x + 1, MatchingPoint_y - 1, count + 1);
        }
        return count;
    }

    private int CheckCaseLeftDown(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y)
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("LeftUp");
            count += CheckCaseLeftDown(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x - 1, MatchingPoint_y + 1, count + 1);
        }
        return count;
    }

    private int CheckCaseCenterUp(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y)
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("LeftUp");
            count += CheckCaseCenterUp(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x, MatchingPoint_y - 2, count + 1);
        }
        return count;
    }

    private int CheckCaseCenterDown(int MatchedPoint_x, int MatchedPoint_y, int MatchingPoint_x, int MatchingPoint_y, int count)
    {
        //Debug.Log("pos1 = (" + MatchedPoint_x + ", " + MatchedPoint_y + ")");
        //Debug.Log("pos2 = (" + MatchingPoint_x + ", " + MatchingPoint_y + ")");
        if (m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) == m_board.GetTileType(MatchingPoint_x, MatchingPoint_y)
            && m_board.GetTileType(MatchedPoint_x, MatchedPoint_y) != Param_Script.TOP_ID)
        {
            //Debug.Log("LeftUp");
            count += CheckCaseCenterDown(MatchingPoint_x, MatchingPoint_y, MatchingPoint_x, MatchingPoint_y + 2, count + 1);
        }
        return count;
    }

}
