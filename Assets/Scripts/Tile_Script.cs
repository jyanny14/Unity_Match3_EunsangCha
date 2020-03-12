using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile_Script : MonoBehaviour
{
    public int m_ID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Vector3 strength = new Vector3(0.1f, 0.1f, 0.1f);

        transform.DOPunchScale(strength, 1f);
        Debug.Log("OnMouseDown");
    }
}
