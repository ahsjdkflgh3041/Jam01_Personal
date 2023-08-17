using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public List<GameObject> destGrounds = new List<GameObject>();

    public GameObject mushroom2;


    void Start()
    {
        mushroom2?.SetActive(false);

    }

    void Update()
    {

        if(destGrounds != null)
        {
            if ( CheckDestroyedAll() )
            {
                mushroom2.SetActive(true);
            }
        }



    }

    bool CheckDestroyedAll()
    {
        foreach(GameObject go in destGrounds)
        {
            if( go.activeSelf ){
                return false;
            }
        }

        return true;
    }

}
