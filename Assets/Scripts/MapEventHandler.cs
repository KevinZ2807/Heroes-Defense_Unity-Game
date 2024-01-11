using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventHandler : MonoBehaviour
{
    [SerializeField]  bool _canHover;
    public static event Action<Vector3,bool> OnHover = delegate { };

    [SerializeField] SpriteRenderer _hoverImage;
    [SerializeField] SpriteRenderer _heroImage;
    [SerializeField] GameObject _sellImage;
    [SerializeField] Color _availableColor;
    [SerializeField] Color _unavailableColor;
    [SerializeField] Color _exitColor;



    public GameObject heroObj=null;
    //public BaseHero tower;

    public int row=1;
    public int col=1;
    public bool isAlignLeft=false;
    public bool isAlignRight=false;

    public int sellCostHeros = 0;
    
    private void CancelSelectedHero() {
        if (BuildManager.main.isSelectedHero)
        {
            _hoverImage.color = _exitColor;
            _heroImage.enabled = false;
        }
    }

    public bool GetCanHover()
    {
        return _canHover;
    }
    public void SetTestHover(bool hover)
    {
        _canHover= hover;
    }
    bool isGate() {
        if (row == 0 || row == 19)
        {
            return (col > 6 && col < 6 + 7) ;
        }
        return false;
    }
    private void OnMouseOver()
    {
        if (BuildManager.main.isSelectedHero && GameManager.Ins.IsPlaying)
        {
            if (Input.GetMouseButtonDown(1)) {
                CancelSelectedHero();
                BuildManager.main.ResetBuild();
                return;
            }
            //_heroImage.color.
            _heroImage.sprite = BuildManager.main.GetSelectedHero().heroSprite;
            _heroImage.enabled = true;
            if (_canHover && !heroObj && GameEngine.Ins.isUnion(row,col) && !isGate())
            {
                _hoverImage.color = _availableColor;
            }
            else
            {
                _hoverImage.color = _unavailableColor;

            }
        }   
    }
    public void OnMouseExit()
    {
        if (BuildManager.main.isSelectedHero)
        {
            _hoverImage.color = _exitColor;
            _heroImage.enabled = false;
        }
    }
    public void setActiveSellImage() {
        _sellImage.SetActive(false);
    }
    void OnMouseDown()
    {
        //if (UIManager.main.IsHoveringUI()) return;
        if (heroObj && !BuildManager.main.isSelectedHero)
        {// Kiem tra neu vi tri da co hero hay chua. Neu co roi se return
            //tower.OpenSellingUI();
         
            _sellImage.SetActive (!_sellImage.activeSelf);
            return;
        }
        if (!_canHover || !BuildManager.main.isSelectedHero || !GameManager.Ins.IsPlaying || !GameEngine.Ins.isUnion(row, col) || EnemySpawner.IsSpawning)
        {
            return;
        }
        //if (!_canHover || !GameManager.Ins.IsPlaying || !BuildManager.main.isSelectedHero) return;
        if (heroObj) return;
        // Neu chua co, se tien hanh dat hero:
        HeroTower heroToBuild = BuildManager.main.GetSelectedHero();

        if (heroToBuild.cost > GameManager.Ins.Money)
        {
            Debug.Log("Cannot build");
            return;
        }
        sellCostHeros = heroToBuild.cost / 2;
        GameManager.Ins.AddCurrency(-heroToBuild.cost);
        
        heroObj = Instantiate(heroToBuild.heroPrefab, transform.position, Quaternion.identity);
        heroObj.transform.SetParent(gameObject.transform);
        GameEngine.Ins.BFSPath();
        //tower = heroObj.GetComponent<BaseHero>();

        AudioController.Ins.PlayPlaceSFX();

        GameEngine.Ins.UnionALL(row, col);
    }
}
