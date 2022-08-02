using Firebase.Analytics;
using UnityEngine;

public class BuyItems : MonoBehaviour
{
    SaveAndLoadGameData gameData;

    void Start()
    {
        gameData = GetComponent<SaveAndLoadGameData>();
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
    }

    private void PurchaseManager_OnPurchaseNonConsumable(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        if(args.purchasedProduct.definition.id == "pack_01" && gameData.GetAdsBuy())
        {
            gameData.SetCurrentMoneyPlayer(200000, '+');
            gameData.SetAdsBuy(false);
            FirebaseAnalytics.LogEvent("buypack_01", new Parameter("type", "Buyed!"));
        }
    }

    private void PurchaseManager_OnPurchaseConsumable(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        switch (args.purchasedProduct.definition.id)
        {
            case "coins_75":
                gameData.SetCurrentMoneyPlayer(100000, '+');
                FirebaseAnalytics.LogEvent("buycoins75", new Parameter("type", "Buyed!"));
                break;
            case "coins_349":
                gameData.SetCurrentMoneyPlayer(600000, '+');
                FirebaseAnalytics.LogEvent("buycoins349", new Parameter("type", "Buyed!"));
                break;
            case "coins_749":
                gameData.SetCurrentMoneyPlayer(1500000, '+');
                FirebaseAnalytics.LogEvent("buycoins749", new Parameter("type", "Buyed!"));
                break;
        }
    }
}
