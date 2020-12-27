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

    PlayerHandController playerHandController;

    bool isPlayerTurn = true;   // ターン変数
    List<int> playerDeck = new List<int>() { 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 2, 0, 1, 2, 0 };   // プレイヤーデッキ
    List<int> enemyDeck = new List<int>() { 2, 2, 1, 1, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 2 };


    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()    // 初期値の設定
    {
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
        card.Init(cardID);
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
}
