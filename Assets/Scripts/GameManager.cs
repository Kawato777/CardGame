using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    Transform playerHand;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cardPrefab, playerHand);    // カード生成
    }

}
