using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[AddComponentMenu("ThinhLe/CoinManager")]

public class CoinManager : Singleton<CoinManager>
{
    private TMP_Text coinText;
    private int currentCoin = 0;

    const string CoinCounter = "Coin Counter";

    public void UpdateCurrentCoin()
    {
        currentCoin += 1;

        if(coinText == null)
        {
            coinText = GameObject.Find(CoinCounter).GetComponent<TMP_Text>();

        }

        coinText.text = currentCoin.ToString("D3");
    }
}
