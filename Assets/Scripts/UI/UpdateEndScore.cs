using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateEndScore : MonoBehaviour
{
    public TextMeshProUGUI deathText, coinText;
    public IntScriptable totalDeath, totalCoins;


    private void Start()
    {
        coinText.text = totalCoins.Value.ToString();
        deathText.text = totalDeath.Value.ToString();
    }
}
