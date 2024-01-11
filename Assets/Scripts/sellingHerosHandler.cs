using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class sellingHerosHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mapEvent;
    void OnMouseDown()
    {
        MapEventHandler mapeventhandler = mapEvent.GetComponent<MapEventHandler>();
        mapeventhandler.setActiveSellImage();
        GameManager.Ins.AddCurrency(mapeventhandler.sellCostHeros);
        Destroy(mapeventhandler.heroObj);
        mapeventhandler.heroObj = null;
        GameEngine.Ins.DSUReset();
    }
}
