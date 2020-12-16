using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaCostPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI manaCostText;

    public int defaultManaCost;
    public int manaCost;

    void Start()
    {
        SetManaCostText(manaCost, defaultManaCost);
    }

    public void SetManaCostText(int manaCost,int defaultManaCost)
    {
        manaCostText.text = $"{manaCost} / {defaultManaCost}";
    }
}
