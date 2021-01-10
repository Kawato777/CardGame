using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, powerText, costText, hpText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel;

    public void Show(CardModel model)
    {
        nameText.text = model.name;
        powerText.text = model.power.ToString();
        costText.text = model.cost.ToString();
        hpText.text = model.hp.ToString();
        iconImage.sprite = model.icon;
    }

    public void SetCanAttackPanel(bool flag)
    {
        canAttackPanel.SetActive(flag);
    }
}
