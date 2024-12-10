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

    private TMP_Text notEnoughCoinsMessage;
    const string NotEnoughCoins = "NotEnoughCoinsMessage";


    public void UpdateCurrentCoin()
    {
        coinText = GameObject.Find(CoinCounter).GetComponent<TMP_Text>();
        currentCoin += 1;
        coinText.text = currentCoin.ToString("D3");
    }

    public bool SpendCoin(int amount) 
    { 
        if (currentCoin >= amount) 
        { 
            currentCoin -= amount; 
            UpdateCoinText(); 
            return true; 
        }
        else
        {
            CoinsMessage();
            return false;
        }

    }
    private void UpdateCoinText() 
    { 
        if (coinText == null) 
        { 
            coinText = GameObject.Find(CoinCounter).GetComponent<TMP_Text>(); 
        } 
        coinText.text = currentCoin.ToString("D3"); 
    }

    private void CoinsMessage() 
    {
        notEnoughCoinsMessage = GameObject.Find(NotEnoughCoins).GetComponent<TMP_Text>();
        notEnoughCoinsMessage.text = "Not enough coins!"; 
        StartCoroutine(ClearMessageAfterDelay()); 
    }

    private IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        notEnoughCoinsMessage.text = "";
    }

    public int GetCurrentCoin() 
    { 
        return currentCoin; 
    }
}
