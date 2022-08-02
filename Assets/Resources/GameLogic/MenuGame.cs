using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGame : MonoBehaviour
{
    [Header("Текст текущего меню.")]
    public Text TextCurrentMenu;
    [Header("Массив кнопок с уровнями")]
    public Button[] LvLButtons;
    int oldLvLsOpen = 0;

    [Header("Покупка пака")]
    public Button packButton;

    [Header("Прокачка персонажа")]
    public Button UpgradeButtonPlayer;
    public Text UpgradeTextPlayer;
    public Text UpgradeTextStatsPlayer;
    float Buy = 0;
    [Range(0, 100)]
    public int UpgradeNextLvLCountMoney; // Cтоимость прокачки следующего уровня.

    [Header("Деньги игрока")]
    public Text MoneyPlayer;

    [Header("Музыкальное сопровождение")]
    public AudioSource Music;

    [Header("Тексты гл.меню")]
    public Text Levels;
    public Text Bledding;
    public Text Tower;
    public Text Shop;

    [Header("Тексты магазин")]
    public Text Buypack01;
    public Text coins_01;
    public Text coins_05;
    public Text coins_10;

    [Header("Текст загрузка")]
    public Text Loading;

    Game game;
    SaveAndLoadGameData gameData;
    Language language;
    void Start()
    {
        game = GetComponent<Game>();
        gameData = GetComponent<SaveAndLoadGameData>();
        language = FindObjectOfType<Language>();
    }

    void FixedUpdate()
    {
        RealtimeLanguageUpdate();
        RealtimeUpdateLvLsOpen();
        RealtimeUpdateTextMoneyPlayer();
        RealtimeUpdateTextUpgradePlayer();
        RealtimeCheckButtonBuyNoAds();
    }

    private void RealtimeLanguageUpdate()
    {
        language.LanguageUpdate(Levels, 1);
        language.LanguageUpdate(Bledding, 2);
        language.LanguageUpdate(Tower, 3);
        language.LanguageUpdate(Shop, 4);
        language.LanguageUpdate(Buypack01, 9);
        language.LanguageUpdate(coins_01, 10);
        language.LanguageUpdate(coins_05, 11);
        language.LanguageUpdate(coins_10, 12);
        language.LanguageUpdate(Loading, 19);
    }

    private void RealtimeUpdateLvLsOpen()
    {
        if(oldLvLsOpen != gameData.GetLvLMaxComplect())
        {
            for(int i = 0; i < LvLButtons.Length; i++)
                LvLButtons[i].interactable = false;
            oldLvLsOpen = gameData.GetLvLMaxComplect();
            for (int i = 0; i < oldLvLsOpen; i++)
                LvLButtons[i].interactable = true;
        }
    }

    public void UpgradePlayerButton()
    {
        gameData.SetCurrentMoneyPlayer(Buy, '-');
        gameData.SetMinDamage(1, '+');
        gameData.SetMaxDamage(10, '+');
        gameData.SetCurrentPlayerStartHealth(10,'+');
        gameData.SetCurrentLvLPlayerUpgrade(1, '+');

        if(gameData.GetCurrentLvLPlayerUpgrade() == 50)
        {
            FirebaseAnalytics.LogEvent("LvL50Upgrade",
            new Parameter("type", "Upgraded"));
        }
    }

    private void RealtimeCheckButtonBuyNoAds()
    {
        packButton.interactable = gameData.GetAdsBuy();
    }

    private void RealtimeUpdateTextUpgradePlayer()
    {
        int Hero_1_LVL = gameData.GetCurrentLvLPlayerUpgrade();
        Buy = (Hero_1_LVL * 10 + Hero_1_LVL * Hero_1_LVL) * Mathf.Ceil(Hero_1_LVL * 0.1f);
        UpgradeButtonPlayer.interactable = gameData.GetCurrentMoneyPlayer() >= Buy;
        UpgradeTextPlayer.text = language.LanguageUpdate(5) + Buy + language.LanguageUpdate(6);
        UpgradeTextStatsPlayer.text = language.LanguageUpdate(7) + gameData.GetCurrentPlayerStartHealth() + language.LanguageUpdate(8) + gameData.GetMinDamage() + " - " + gameData.GetMaxDamage();
    }
    private void RealtimeUpdateTextMoneyPlayer()
    {
        MoneyPlayer.text = Mathf.RoundToInt(gameData.GetCurrentMoneyPlayer()).ToString();
    }
    public void OpenMenu(GameObject openMenu)
    {
        openMenu.SetActive(true);
    }

    public void CloseMenu(GameObject closeMenu)
    {
        closeMenu.SetActive(false);
    }

    public void ChangedTextCurrentMenu(Text text)
    {
        TextCurrentMenu.text = text.text;
    }
    public void ChangedTextCurrentMenu(string text)
    {
        TextCurrentMenu.text = text;
    }

    public void LauchGame(Text text)
    {
        LauchGameMethods(false,int.Parse(text.text));
    }

    public void OpenGoldChest()
    {
        int MoneyPlus = Random.Range(15000, 15999);
        gameData.SetCurrentMoneyPlayer(MoneyPlus, '+');
    }

    public void TowerLauchGame()
    {
        LauchGameMethods(true,1);
    }
    public void LauchGameMethods(bool isTowerDifficuly,int CurrentLvL)
    {
        game.isTowerDifficuly = isTowerDifficuly;
        game.CurrentLvL = CurrentLvL;
        game.LoadGame();
        game.player.LoadPlayerData();
        Music.Play();
        if (game.HUDGameOver.activeSelf)
            game.HUDGameOver.SetActive(false);
        if (game.HUDEnemyDataObject.activeSelf)
            game.HUDEnemyDataObject.SetActive(false);
    }
}
