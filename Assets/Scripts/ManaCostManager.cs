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
            if(playerDefaultManaCost < 15)
            {
                playerDefaultManaCost++;
            }
            playerManaCost = playerDefaultManaCost;
            
            playerManaCostText.text = $"{playerManaCost} / {playerDefaultManaCost}";
        }
        else
        {
            if (enemyDefaultManaCost < 15)
            {
                enemyDefaultManaCost++;
            }
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
            playerManaCost -= useCost;
            if (playerManaCost < 0)
            {
                playerManaCost = 0;
            }

            playerManaCostText.text = $"{playerManaCost} / {playerDefaultManaCost}";
        }
        else
        {
            enemyManaCost -= useCost;
            if (enemyManaCost < 0)
            {
                enemyManaCost = 0;
            }

            enemyManaCostText.text = $"{enemyManaCost} / {enemyDefaultManaCost}";
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
