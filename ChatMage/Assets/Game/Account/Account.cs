using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class Account : MonoPersistent
{
    private const string COINS_KEY = "coins";

    // Coins
    private int _coins = 0;
    public event SimpleEvent onCoinsChange;

    private Purchaser purchaser;
    [SerializeField]
    DataSaver dataSaver;

    public static Account instance;

#if UNITY_ADS
    // ADS
    private const string androidGameId = "1426499", iosGameId = "1426500";
    private const bool testMode = true;
#endif

    void Awake()
    {
        instance = this;
    }

    public override void Init(Action onComplete)
    {
        FetchData();

        // INIT MOBILE TOOLS FOR PLAYER
#if UNITY_ANDROID
        // IN GAME PURCHASE
        //purchaser = new Purchaser();
        //purchaser.Init();

        // ADS
#if UNITY_ADS
        string gameId;
#if UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_IOS
        gameId = iosGameId;
#endif
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode);
        }
#endif
#endif

        onComplete();
    }


    private void FetchData()
    {
        Coins = dataSaver.GetInt(COINS_KEY, 0);
    }

    public void Save()
    {
        dataSaver.Save();
    }
    public void SaveAsync(Action onComplete = null)
    {
        dataSaver.SaveAsync(onComplete);
    }

    public int Coins
    {
        get
        {
            return _coins;
        }
        private set
        {
            _coins = value;
            dataSaver.SetInt(COINS_KEY, _coins);
        }
    }

    /// <summary>
    /// Ajout ou retire un certain montant d'argent au compte du joueur
    /// </summary>
    /// <returns>Retourne si le changement a reussi ou pas</returns>
    public bool AddCoins(int amount, bool saveAfterwards = true)
    {
        int moneyResult = Coins + amount;
        if (moneyResult < 0)
            return false;

        Coins = moneyResult;
        if (onCoinsChange != null)
            onCoinsChange();

        if (saveAfterwards)
            Save();

        return true;
    }

    public bool Command(StorePrice.CommandType commandType, int amount = 1, bool saveAfterwards = true)
    {
        return AddCoins(StorePrice.GetPrice(commandType) * amount, saveAfterwards);
    }

    // IN GAME PURCHASE
#if UNITY_ANDROID
    //public void BuyCoins(int amount)
    //{
    //    purchaser.BuyConsumable(amount);
    //}

    //public void BuyFullGame()
    //{
    //    purchaser.BuyTheGame();
    //}
#endif
}
