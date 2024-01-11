using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private HeroTower[] heroes;
    public bool isSelectedHero = false;

    private int selectedHero = 0;
    private void Awake() {
        main = this;
        ResetBuild();
        GameManager.OnResetLevel += ResetBuild;
    }
    public void ResetBuild()
    {
        isSelectedHero = false;
        selectedHero = -1;
    }

    public HeroTower GetSelectedHero() {
        return heroes[selectedHero];
    }
    public void SetSelectedTower(int _selectedHero) {
        isSelectedHero = true;
        selectedHero = _selectedHero;
    }

}
