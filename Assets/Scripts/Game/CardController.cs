using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;   // カードの見た目の処理
    public CardModel model; // カードのデータの処理

    CardMovement cardMovement;

    private void Awake()
    {
        view = GetComponent<CardView>();
    }

    public void Init(int cardID, bool isPlayer)
    {
        model = new CardModel(cardID, isPlayer);
        cardMovement = GetComponent<CardMovement>();
        if (model.playerCard == false)
        {
            Destroy(cardMovement);
        }
        view.Show(model);
    }

    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }

    public void Show()
    {
        view.Show(model);
    }

    public void SetDrag(bool flag)
    {
        cardMovement.enabled = flag;
    }
}
