using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>(); // ドラッグしてきた情報からCardMovementを取得
        CardController cardController = card.gameObject.GetComponent<CardController>();
        if (card != null && GetComponentsInChildren<CardController>().Length < 5 && ManaCostManager.Instance.CheckUsingCost(cardController.model.cost, true))    // もしカードがあれば、
        {
            card.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする
            ManaCostManager.Instance.UseManaCostText(card.gameObject.GetComponent<CardController>().model.cost, true);
        }
    }
}
