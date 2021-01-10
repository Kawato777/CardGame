using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHandController : MonoBehaviour
{
    int oldEnemyHandCards;
    int nowEnemyHandCards;

    public int leftCardNum = 0;

    bool a = false;

    List<Transform> childTransforms = new List<Transform>();

    [SerializeField]
    GameObject rightButton, leftButton;

    private void Update()
    {
        EnemyHandCheck();
    }

    void EnemyHandCheck()
    {
        childTransforms = transform.Cast<Transform>().ToList();

        nowEnemyHandCards = childTransforms.Count;

        if (oldEnemyHandCards != nowEnemyHandCards)
        {
            if (nowEnemyHandCards < 6)
            {
                a = true;
                rightButton.SetActive(false);
                leftButton.SetActive(false);
            }
            else
            {
                if (a && nowEnemyHandCards == 6)
                {
                    childTransforms[5].gameObject.SetActive(false);
                    a = false;
                }
                HandView();
            }
        }

        ButtonCheck();

        oldEnemyHandCards = nowEnemyHandCards;
    }

    public void ButtonCheck()
    {
        if (nowEnemyHandCards > 5)
        {
            if (leftCardNum >= nowEnemyHandCards - 5)
            { 
                rightButton.SetActive(false);
                leftButton.SetActive(true);
            }
            else if (leftCardNum == 0)
            {
                rightButton.SetActive(true);
                leftButton.SetActive(false);
            }
            else
            {
                rightButton.SetActive(true);
                leftButton.SetActive(true);
            }
        }
    }

    public int ReturnEnemyHandCards()
    {
        return nowEnemyHandCards;
    }

    public void RightButton()
    {
        leftCardNum++;
        HandView();
        ButtonCheck();
    }

    public void LeftButton()
    {
        leftCardNum--;
        HandView();
        ButtonCheck();
    }

    public void HandView()
    {
        if (nowEnemyHandCards < 6)
        {
            foreach (var item in childTransforms)
            {
                item.gameObject.SetActive(true);
            }
            return;
        }

        for (int a = 0; a < nowEnemyHandCards; a++)
        {
            if (a < leftCardNum || a > leftCardNum + 4)
            {
                childTransforms[a].gameObject.SetActive(false);
            }
            else
            {
                childTransforms[a].gameObject.SetActive(true);
            }
        }

        if (leftCardNum == nowEnemyHandCards - 4)
        {
            childTransforms[nowEnemyHandCards - 5].gameObject.SetActive(true);
        }
    }

    public void HandView(bool a)
    {
        if (nowEnemyHandCards - 1 < 6)
        {
            foreach (var item in childTransforms)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public CardController[] ReturnCardController()
    {
        List<CardController> p = new List<CardController>(nowEnemyHandCards);
        foreach(var item in childTransforms)
        {
            p.Add(item.GetComponent<CardController>());
        }

        return p.ToArray();
    }
}
