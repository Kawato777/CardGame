using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    Transform playerHand, playerField, enemyField;

    bool isPlayerTurn = true;   // ターン変数
    List<int> deck = new List<int>() { 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 2, 0, 1, 2, 0 };   // デッキ

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()    // 初期値の設定
    {
        // 初期手札を配る
        SetStartHand();

        // ターンの決定
        TurnCalc();
    }

    void CreateCard(int cardID,Transform place)
    {
        CardController card = Instantiate(cardPrefab, place).GetComponent<CardController>();
        card.Init(cardID);
    }

    void DrowCard(Transform hand)   // カードを引く
    {
        if(deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        if(playerHandCardList.Length < 9)
        {
            // デッキの一番上のカードを抜き取り、手札に加える
            int cardID = deck[0];
            deck.RemoveAt(0);
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

        DrowCard(playerHand);   // 手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        
        if(enemyFieldCardList.Length < 5)
        {
            CreateCard(0, enemyField);  // カードを召喚
        }

        ChangeTurn();   // ターンエンドする
    }
}
