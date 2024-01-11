using UnityEngine;
using System;

[Serializable] // Set to be visible in [SerializeField] from another script
public class HeroTower {
    public string name;
    public int cost;
    public Sprite heroSprite;
    public GameObject heroPrefab;

    public HeroTower (string _name, int _cost, GameObject _heroPrefab) {
        name = _name;
        cost = _cost;
        heroPrefab = _heroPrefab;
    }
}