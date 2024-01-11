using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = false;

    private void OnGUI()
    {
        currencyUI.text = GameManager.Ins.Money.ToString();
    }

    public void ToggleMenu() {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("IsMenuOpen", isMenuOpen);
    }

    public void SetSelected() {

    }
}
