using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FromCSVToScriptableObject
{
    // 上のタブに出るようにする
    [MenuItem("CSV/Scriptable化")]
    private static void AAA()
    {
        // MasterData.csvのパス
        string path = "Assets/MasterData.csv";

        // Fileがあれば読み込みなければエラー吐き出す
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            SetToScriptableObject(data);
        }
        else
        {
            Debug.LogError("No Assets/MasterData.csv!");
        }
    }

    private static void SetToScriptableObject(string csvText)
    {
        // 改行ごとにパース
        string[] afterParse = csvText.Split('\n');

        // ヘッダー行を除いてインポート
        for(int i = 1;i < afterParse.Length; i++)
        {
            string[] parseByComma = afterParse[i].Split(',');

            int column = 0;

            // 先頭の列が空であればその行は読み込まない
            if(parseByComma[column] == "")
            {
                continue;
            }

            // 行数をIDとしてファイルを作成
            string fileName = "ID_" + (i - 1).ToString() + ".asset";
            string path = "Assets/Resources/CardEntityList/" + fileName;

            // cardDataのインスタンスをメモリ上に生成
            var cardData = new CardEntity();

            cardData.cardID = int.Parse(parseByComma[column]);

            column++;
            cardData.name = parseByComma[column];

            column++;
            cardData.cost = int.Parse(parseByComma[column]);

            column++;
            cardData.power = int.Parse(parseByComma[column]);

            column++;
            cardData.hp = int.Parse(parseByComma[column]);

            CardIconEntity cardIconEntity = (CardIconEntity)AssetDatabase.LoadAssetAtPath($"Assets/Images/CardIconEntityList/CardIcon_{cardData.cardID}.asset", typeof(CardIconEntity));
            if(cardIconEntity == null)
            {
                Debug.LogError("Not CardIcon!");
                return;
            }
            cardData.icon = cardIconEntity.icon;

            // インスタンス化したものをアセットとして保存
            var asset = (CardEntity)AssetDatabase.LoadAssetAtPath(path, typeof(CardEntity));
            if(asset == null)
            {
                // 指定のパスにファイルが存在しない場合
                AssetDatabase.CreateAsset(cardData, path);
            }
            else
            {
                // 指定のパスにすでに同名のファイルが存在する場合
                EditorUtility.CopySerialized(cardData, asset);
                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
        }
        Debug.Log("Scriptable化完了");
    }
}
