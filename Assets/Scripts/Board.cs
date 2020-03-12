using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    //private Tile_Script[,] m_TilesArray = new Tile_Script[6, 6];

    private Dictionary<string, Tile_Script> m_TilesDictionary = new Dictionary<string, Tile_Script>();
    private GameObject m_TilePrefab;

    public int m_Width = 16;
    public int m_Height = 16;

    // Start is called before the first frame update
    void Start()
    {
        // 경로에 있는 파일을 가져온다.
        m_TilePrefab = Resources.Load("Prefabs/Purple") as GameObject;
        CreateTiles();
    }

    /// <summary>
    /// 프리팹을 이용하여 새로운 타일들을 생성한다.
    /// </summary>
    private void CreateTiles()
    {
        for(int y = 0; y < m_Height; ++y)
        {
            for (int x = 0; x < m_Width; ++x)
            {
                string key = x.ToString() + "," + y.ToString();
                Tile_Script tile = Instantiate<Tile_Script>(m_TilePrefab.transform.GetComponent<Tile_Script>());

                tile.transform.SetParent(this.transform);
                tile.transform.position = new Vector3(x, y, 0f);

                m_TilesDictionary.Add(key, tile);
            }
        }
    }
    /// <summary>
    /// Tile을 반환한다.
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns></returns>
    public Tile_Script GetTile(int x, int y)
    {
        string key = x.ToString() + "," + y.ToString();
        return m_TilesDictionary[key];
    }

    /// <summary>
    /// Tile을 반환한다.
    /// </summary>
    /// <param name="xy">좌표</param>
    /// <returns></returns>
    public Tile_Script GetTile(string xy)
    {
        return m_TilesDictionary[xy];
    }
}
