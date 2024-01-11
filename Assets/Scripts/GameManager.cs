using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Ins;
    public static event Action OnResetLevel = delegate { };

    public bool IsPlaying;
    public Slider HealthBarSlider;
    public TextMeshProUGUI HealthBarText;
    public static int currentHealth = 100;

    [SerializeField] Transform _levelContainer;
    [SerializeField] List<LevelManager> _levelList;
    [SerializeField] CanvasController _canvasController;

    private GameObject _levelPrefab;
    private int _currentLevel = 0;
    private int _health;
    private int _money;

    public int Money
    {
        get { return _money; } set { _money = value; }
    }
    public bool IsWin { get; private set; }

    private void Awake()
    {
        if (Ins)
        {
            Destroy(Ins);
        }
        Ins = this;
        DontDestroyOnLoad(Ins);

        EnemySpawner.OnComplete += EndGame;
    }
    public void StartGame()
    {
        ResetLevel();
    }

    public void EndGame()
    {
        IsPlaying = false;
        IsWin = false;

        if (_health != 0)
        {
            IsWin = true;
            _currentLevel = ++_currentLevel % _levelList.Count;
        }

        if (IsWin)
        {
            AudioController.Ins.PlayWinSFX();
        }
        else
        {
            AudioController.Ins.PlayLoseSFX();
        }

        _canvasController.OpenEndgameCanvas(true);
    }

    public void ResetLevel()
    {
        IsPlaying = true;
        ResetHealth();
        ResetMoney();
        if (_levelPrefab)
        {
            DestroyImmediate(_levelPrefab);
        }
        _levelPrefab = Instantiate(_levelList[_currentLevel].gameObject, _levelContainer, false);
        GameEngine.Ins.InitialGrid();

        OnResetLevel.Invoke();
    }

    private void ResetHealth()
    {
        _health = 100;
        UpdateHealthUI();
    }

    public void UpdateHealth(int damage)
    {
        AudioController.Ins.PlayDamageSFX();
        _health = Mathf.Max(_health - damage, 0);
        UpdateHealthUI();
        if(_health == 0)
        {
            EndGame();
        }
    }

    private void UpdateHealthUI()
    {
        HealthBarSlider.value = _health;
        HealthBarText.SetText(_health.ToString());
    }

    private void ResetMoney()
    {
        _money = _levelList[_currentLevel].initMoney;
    }

    public void AddCurrency(int amount)
    {
        GameManager.Ins._money += amount;
    }
}
