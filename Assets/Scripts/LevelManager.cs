using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager main;
    public Transform[] listGate;

    public int initMoney = 100;

    private void Awake() {
        main = this;
    }
}

