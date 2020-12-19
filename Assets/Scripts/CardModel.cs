using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    public int cardID;
    public new string name;
    public int cost;
    public int power;
    public int hp;
    public Sprite icon;
    
    public CardModel(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/ID_" + cardID);

        Debug.Log(cardEntity);

        this.cardID = cardEntity.cardID;
        name = cardEntity.name;
        cost = cardEntity.cost;
        power = cardEntity.power;
        hp = cardEntity.hp;
        icon = cardEntity.icon;
    }
}
