using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaCostPanel : SingletonMonoBehaviour<ManaCostPanel>
{
    [SerializeField]
    TextMeshProUGUI manaCostText;

    int defaultManaCost;
    int manaCost;

    void Start()
    {
        SetManaCostText(manaCost, defaultManaCost);
    }

    void SetManaCostText(int manaCost,int defaultManaCost)
    {
        this.manaCost = manaCost;
        this.defaultManaCost = defaultManaCost;

        manaCostText.text = $"{manaCost} / {defaultManaCost}";
    }

    public void UseManaCost(int useCost)
    {
        int usedCost = manaCost - useCost;
        if(usedCost < 0)
        {
            usedCost = 0;
        }

        manaCostText.text = $"{usedCost} / {defaultManaCost}";
    }

    public bool CheckUsingCost(int useCost)
    {
        if(useCost <= manaCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlusDefaultManaCost() 
    {
        defaultManaCost++;
        manaCost = defaultManaCost;

        manaCostText.text = $"{manaCost} / {defaultManaCost}";
    }
}
