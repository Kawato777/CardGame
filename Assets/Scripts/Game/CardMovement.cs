using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    Transform wasParent;
    PlayerHandController playerHandController;
    int childNum;

    CardController card;

    void Start()
    {
        playerHandController = GameObject.Find("PlayerHand").GetComponent<PlayerHandController>();
        card = GetComponent<CardController>();
    }

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        cardParent = wasParent =transform.parent;
        childNum = transform.GetSiblingIndex();
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
        playerHandController.HandView(true);
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
        if(wasParent != cardParent)
        {
            transform.SetAsLastSibling();
            GameManager.Instance.CheckUseableHandCard();
            GetComponent<CardController>().view.SetCanAttackPanel(false);   // 速攻は別?
        }
        else
        {
            transform.SetSiblingIndex(childNum);
        }
    }
}
