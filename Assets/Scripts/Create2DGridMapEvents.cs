using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Create2DGridMapEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public static Create2DGridMapEvents Ins;
    public GameObject[] MapEvents;
    private void Awake()
    {
        Ins = this;
        MapEvents = GameObject.FindGameObjectsWithTag("MapEvent")
        .OrderByDescending(go => go.transform.position.y)
        .ThenBy(go => go.transform.position.x)
        .ToArray();
        
    }
    void Start()
    {
        
        
        //for (int i = 0; i < 25; i++)
        //{
        //    MapEvents[i].GetComponent<MapEventHandler>().SetTestHover(false);
        //    Debug.Log(MapEvents[i].transform.position.x+" "+ MapEvents[i].transform.position.y);
        //}
        //Debug.Log("leng"+MapEvents.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
