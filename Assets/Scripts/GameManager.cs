using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    Transform playerHand, playerField, enemyHand, enemyField, playerLeader, canvas;
    [SerializeField]
    TextMeshProUGUI playerLeaderHPText, enemyLeaderHpText;
    [SerializeField]
    List<Button> buttons = new List<Button>(3);

    PlayerHandController playerHandController;
    EnemyHandController enemyHandController;


    bool isPlayerTurn = true;   // ターン変数
    List<int> playerDeck = new List<int>() { 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 2, 0, 1, 2, 0 };   // プレイヤーデッキ
    List<int> enemyDeck = new List<int>() { 0, 1, 2, 2, 2, 0, 1, 1, 2, 2, 1, 1, 0, 0, 2 };

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
        enemyHandController = enemyHand.GetComponent<EnemyHandController>();

        // 初期手札を配る
        SetStartHand();

        // コストテキスト設定
        ManaCostManager.Instance.SetManaCostText(0, 0, true);
        ManaCostManager.Instance.SetManaCostText(0, 0, false);

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

    void DrowCard(bool isPlayer)   // カードを引く
    {
        if (isPlayer)
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
                CreateCard(cardID, playerHand);
            }
        }
        else
        {
            if (enemyDeck.Count == 0)
            {
                return;
            }

            if (enemyHandController.ReturnEnemyHandCards() < 9)
            {
                // デッキの一番上のカードを抜き取り、手札に加える
                int cardID = enemyDeck[0];
                enemyDeck.RemoveAt(0);
                CreateCard(cardID, enemyHand);
            }
        } 
    }

    void SetStartHand() // 手札を3枚加える
    {
        for(int i = 0; i < 3; i++)
        {
            DrowCard(true);
            DrowCard(false);
        }
    }

    void TurnCalc() // ターンを管理する
    {
        SetButton(isPlayerTurn);
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(EnemyTurn());
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
        /* マナデフォルトセット */
        ManaCostManager.Instance.SetManaCostText(true);
        
        /* 使えるカードにガイド表示 */
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        SetUseableHandCard(playerHandCardList, true);
        
        /* 手札を一枚加える */
        DrowCard(true);   
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemyのターン");
        
        /* エネミーカード参照 */
        CardController[] enemyHandCardList = enemyHandController.ReturnCardController();
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        SetAttackableFieldCard(enemyFieldCardList, true);
        
        /* マナデフォルトセット */
        ManaCostManager.Instance.SetManaCostText(false);
        
        /* 手札を一枚加える */
        DrowCard(false);
        yield return new WaitForSeconds(1f);
        
        /* カード召喚 */
        foreach(var item in enemyHandCardList)
        {
            if(enemyFieldCardList.Length < 5 && ManaCostManager.Instance.CheckUsingCost(item.model.cost, false))
            {
                while(enemyHandController.leftCardNum + 4 < item.transform.GetSiblingIndex())
                {
                    enemyHandController.RightButton();
                    yield return new WaitForSeconds(1f);
                }

                while(enemyHandController.leftCardNum > item.transform.GetSiblingIndex())
                {
                    enemyHandController.LeftButton();
                    yield return new WaitForSeconds(1f);
                }
        
                item.transform.SetParent(canvas);
                item.transform.DOMove(enemyField.position, 1f);
                yield return new WaitForSeconds(1f);
                item.transform.SetParent(enemyField);   // enemyFieldに召喚
                item.transform.SetAsLastSibling();
                
                ManaCostManager.Instance.UseManaCostText(item.model.cost, false);
                enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();  // enemyfield更新
            } 
        }

        yield return new WaitForSeconds(1f);

        /* 選んで攻撃 */
        foreach(CardController card in enemyFieldCardList)
        {
            if (card.model.canAttack)
            {
                CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
                if(playerFieldCardList.Length > 0)
                {
                    yield return StartCoroutine(CardBattle(card, playerFieldCardList[0], false));
                }
                else
                {
                    Vector3 cardPos = card.transform.position;
                    card.transform.SetParent(canvas);
                    card.transform.DOMove(playerLeader.position, 1f);
                    yield return new WaitForSeconds(1f);
                    AttackToLeader(card, false);
                    card.transform.DOMove(cardPos, 1f);
                    yield return new WaitForSeconds(1f);
                    card.transform.SetParent(enemyField);
                }
            }
        }

        ChangeTurn();   // ターンエンドする
    }

    public IEnumerator CardBattle(CardController attackCard, CardController defenceCard, bool isPlayer)
    {
        if(attackCard.model.canAttack == false)
        {
            yield break;
        }

        if(attackCard.model.playerCard == defenceCard.model.playerCard)
        {
            yield break;
        }

        Vector3 attackCardPos = attackCard.transform.position;

        if(!isPlayer)
        {
            attackCard.transform.DOLocalMove(defenceCard.transform.position, 1f);
            yield return new WaitForSeconds(1f);
        }

        attackCard.model.hp -= defenceCard.model.power;
        defenceCard.model.hp -= attackCard.model.power;

        attackCard.Show();
        defenceCard.Show();

        bool attackCardalive = true;

        if (attackCard.model.hp <= 0)
        {
            attackCard.DestroyCard(attackCard);
            attackCardalive = false;
        }

        if(defenceCard.model.hp <= 0)
        {
            defenceCard.DestroyCard(defenceCard);
        }

        if (!isPlayer && attackCardalive)
        {
            attackCard.transform.DOMove(attackCardPos, 1f);
            yield return new WaitForSeconds(1f);
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

    void SetUseableHandCard(CardController[] cardList, bool flag)
    {
        foreach(CardController card in cardList)
        {
            if (flag)
            {
                if (ManaCostManager.Instance.CheckUsingCost(card.model.cost, true))
                {
                    card.view.SetCanAttackPanel(true);
                }
            }
            else
            {
                card.view.SetCanAttackPanel(false);
            }       
        }
    }

    public void CheckUseableHandCard()
    {
        foreach(CardController card in playerHand.GetComponentsInChildren<CardController>())
        {
            card.view.SetCanAttackPanel(ManaCostManager.Instance.CheckUsingCost(card.model.cost, true));
        }
    }

    public void AttackToLeader(CardController attackcard, bool isPlayer)
    {
        if(attackcard.model.canAttack == false)
        {
            return;
        }

        if (isPlayer)
        {
            enemyLeaderHP -= attackcard.model.power;
        }
        else
        {
            playerLeaderHP -= attackcard.model.power;
        }
        attackcard.model.canAttack = false;
        attackcard.view.SetCanAttackPanel(false);
        if (isPlayer)
        {
            Debug.Log($"敵のHP:{enemyLeaderHP}");
        }
        else
        {
            Debug.Log($"自分のHP:{playerLeaderHP}");
        }
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

    void SetButton(bool flag)
    {
        foreach(var item in buttons)
        {
            item.interactable = flag;
        }
    }
}
