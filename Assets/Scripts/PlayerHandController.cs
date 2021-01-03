using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    int oldPlayerHandCards;
    int nowPlayerHandCards;

    int leftCardNum = 0;

    bool a = false;

    List<Transform> childTransforms = new List<Transform>();

    [SerializeField]
    GameObject rightButton, leftButton;

    private void Update()
    {
        PlayerHandCheck();
    }

    void PlayerHandCheck()
    {
        childTransforms = transform.Cast<Transform>().ToList();

        nowPlayerHandCards = childTransforms.Count;

        if (oldPlayerHandCards != nowPlayerHandCards)
        {
            if (nowPlayerHandCards < 6)
            {
                a = true;
                rightButton.SetActive(false);
                leftButton.SetActive(false);
            }
            else
            {
                if (a && nowPlayerHandCards == 6)
                {
                    childTransforms[5].gameObject.SetActive(false);
                    a = false;
                }
                HandView();
            }
        }

        ButtonCheck();

        oldPlayerHandCards = nowPlayerHandCards;
    }

    public void ButtonCheck()
    {
        if (nowPlayerHandCards > 5)
        {
            if (leftCardNum >= nowPlayerHandCards - 5)
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

    public int ReturnPlayerHandCards()
    {
        return nowPlayerHandCards;
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
        if(nowPlayerHandCards < 6)
        {
            foreach(var item in childTransforms)
            {
                item.gameObject.SetActive(true);
            }
            return;
        }

        for(int a = 0; a < nowPlayerHandCards; a++)
        {
            if(a < leftCardNum || a > leftCardNum + 4)
            {
                childTransforms[a].gameObject.SetActive(false);
            }
            else
            {
                childTransforms[a].gameObject.SetActive(true);
            }
        }

        if(leftCardNum == nowPlayerHandCards - 4)
        {
            childTransforms[nowPlayerHandCards - 5].gameObject.SetActive(true);
        }
    }

    public void HandView(bool a)
    {
        if (nowPlayerHandCards - 1 < 6)
        {
            foreach (var item in childTransforms)
            {
                item.gameObject.SetActive(true);
            }
        }    
    }
}
