using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedLeader : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

        GameManager.Instance.AttackToLeader(attackCard, true);
    }
}
