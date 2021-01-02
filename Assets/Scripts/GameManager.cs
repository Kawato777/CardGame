using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    Transform playerHand, playerField, enemyField;
    [SerializeField]
    TextMeshProUGUI playerLeaderHPText, enemyLeaderHpText;

    PlayerHandController playerHandController;

    bool isPlayerTurn = true;   // ターン変数
    List<int> playerDeck = new List<int>() { 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 2, 0, 1, 2, 0 };   // プレイヤーデッキ
    List<int> enemyDeck = new List<int>() { 2, 2, 1, 1, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 2 };

    int enemyLeaderHP,playerLeaderHP;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()    // 初期値の設定
    {
        enemyLeaderHP = playerLeaderHP = 15;
        ShowLeaderHP();

        // プレイヤー手札コントローラーをセット
        playerHandController = playerHand.GetComponent<PlayerHandController>();

        // 初期手札を配る
        SetStartHand();

        // ターンの決定
        TurnCalc();
    }

    void CreateCard(int cardID,Transform place)
    {
        CardController card = Instantiate(cardPrefab, place).GetComponent<CardController>();
        if(place == playerField || place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }
        
    }

    void DrowCard(Transform hand)   // カードを引く
    {
        if(playerDeck.Count == 0)
        {
            return;
        }

        if(playerHandController.ReturnPlayerHandCards() < 9)
        {
            // デッキの一番上のカードを抜き取り、手札に加える
            int cardID = playerDeck[0];
            playerDeck.RemoveAt(0);
            CreateCard(cardID, hand);
        }
    }

    void SetStartHand() // 手札を3枚加える
    {
        for(int i = 0; i < 3; i++)
        {
            DrowCard(playerHand);
        }
    }

    void TurnCalc() // ターンを管理する
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }

    public void ChangeTurn()   // ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn;   // ターンを逆にする
        TurnCalc(); // ターンを相手に回す
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrowCard(playerHand);   // 手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        
        if(enemyFieldCardList.Length < 5)
        {
            int cardID = enemyDeck[0];
            enemyDeck.RemoveAt(0);
            CreateCard(cardID, enemyField);  // カードを召喚
        }

        ChangeTurn();   // ターンエンドする
    }

    public void CardBattle(CardController attackCard,CardController defenceCard)
    {
        if(attackCard.model.canAttack == false)
        {
            return;
        }

        if(attackCard.model.playerCard == defenceCard.model.playerCard)
        {
            return;
        }

        attackCard.model.hp -= defenceCard.model.power;
        defenceCard.model.hp -= attackCard.model.power;

        attackCard.Show();
        defenceCard.Show();

        if (attackCard.model.hp < 0)
        {
            attackCard.DestroyCard(attackCard);
        }

        if(defenceCard.model.hp < 0)
        {
            defenceCard.DestroyCard(defenceCard);
        }

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
    }

    void SetAttackableFieldCard(CardController[] cardList, bool canAttack)
    {
        foreach(CardController card in cardList)
        {
            card.model.canAttack = canAttack;
            card.view.SetCanAttackPanel(canAttack);
        }
    }

    public void AttackToLeader(CardController attackcard,bool isPlayerCard)
    {
        if(attackcard.model.canAttack == false)
        {
            return;
        }

        enemyLeaderHP -= attackcard.model.power;

        attackcard.model.canAttack = false;
        attackcard.view.SetCanAttackPanel(false);
        Debug.Log($"敵のHP:{enemyLeaderHP}");
        ShowLeaderHP();
    }

    public void ShowLeaderHP()
    {
        if(playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }

        if(enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }

        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHpText.text = enemyLeaderHP.ToString();
    }
}
