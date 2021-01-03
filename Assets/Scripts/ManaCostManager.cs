using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaCostManager : SingletonMonoBehaviour<ManaCostManager>
{
    [SerializeField]
    TextMeshProUGUI playerManaCostText, enemyManaCostText;

    int playerDefaultManaCost;
    int playerManaCost;
    int enemyDefaultManaCost;
    int enemyManaCost;

    /// <summary>
    /// 初期設定に使うマナセット
    /// </summary>
    public void SetManaCostText(int manaCost, int defaultManaCost, bool isPlayer)
    {
        if (isPlayer)
        {
            playerManaCost = manaCost;
            playerDefaultManaCost = defaultManaCost;
            playerManaCostText.text = $"{manaCost} / {defaultManaCost}";
        }
        else
        {
            enemyManaCost = manaCost;
            enemyDefaultManaCost = defaultManaCost;
            enemyManaCostText.text = $"{manaCost} / {defaultManaCost}";
        }
    }

    /// <summary>
    /// ターン開始時に使うマナセット
    /// </summary>
    public void SetManaCostText(bool isPlayer)
    {
        if (isPlayer)
        {
            playerDefaultManaCost++;
            playerManaCost = playerDefaultManaCost;
            
            playerManaCostText.text = $"{playerManaCost} / {playerDefaultManaCost}";
        }
        else
        {
            enemyDefaultManaCost++;
            enemyManaCost = enemyDefaultManaCost;

            enemyManaCostText.text = $"{enemyManaCost} / {enemyDefaultManaCost}";
        }
    }

    /// <summary>
    /// コスト使用時に使うマナセット
    /// </summary>
    public void UseManaCostText(int useCost, bool isPlayer)
    {
        if (isPlayer)
        {
            int usedCost = playerManaCost - useCost;
            if (usedCost < 0)
            {
                usedCost = 0;
            }

            playerManaCostText.text = $"{usedCost} / {playerDefaultManaCost}";
        }
        else
        {
            int usedCost = enemyManaCost - useCost;
            if (usedCost < 0)
            {
                usedCost = 0;
            }

            enemyManaCostText.text = $"{usedCost} / {enemyDefaultManaCost}";
        }
    }

    public bool CheckUsingCost(int useCost, bool isPlayer)
    {
        if (isPlayer)
        {
            if (useCost <= playerManaCost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (useCost <= enemyManaCost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
