using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] internal int CurrentHealth = 10;
    [SerializeField] internal int MaxHeath = 10;
    [SerializeField] internal int MinDamage = 1;
    [SerializeField] internal int MaxDamage = 10;
    [SerializeField] internal int CurrentLvLPlayer = 1;

    SaveAndLoadGameData gameData;
    Language language;

    [Header("HUD игрока")]
    public Text HealthPlayer;
    public Text DamagePlayer;

    void Start()
    {
        gameData = GetComponent<SaveAndLoadGameData>();
        language = FindObjectOfType<Language>();
    }

    void FixedUpdate()
    {
        CheckHealthAndGameOver();
        RealtimeUpdatePlayerHUD();
    }

    private void RealtimeUpdatePlayerHUD()
    {
        HealthPlayer.text = language.LanguageUpdate(7) + MaxHeath + "/" + CurrentHealth;
        string damage = language.LanguageUpdate(8).Replace(" ", "");
        DamagePlayer.text = damage + " " + MinDamage + " - " + MaxDamage;
    }
    internal void LoadPlayerData()
    {
        MaxHeath = CurrentHealth = gameData.GetCurrentPlayerStartHealth();
        MinDamage = gameData.GetMinDamage();
        MaxDamage = gameData.GetMaxDamage();
        CurrentLvLPlayer = gameData.GetCurrentLvLPlayerUpgrade();
    }

    private void CheckHealthAndGameOver()
    {
        if (CurrentHealth > MaxHeath)
            CurrentHealth = MaxHeath;
        else if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            if (GetComponent<Game>().isTowerDifficuly)
            {
                Debug.Log("AD SHOWED IN TOWERS!");
                GetComponent<AdMobRealtek>().AdShow();
            }
            GetComponent<Game>().HUDGameOver.SetActive(true);
            GetComponent<Game>().TextCurrentMenu.text = language.LanguageUpdate(17);
            GetComponent<Game>().TextCurrentMenu.GetComponent<Animator>().Play(GetComponent<Game>().NameAniamtionLvLStatic,0,0);
            GetComponent<MenuGame>().Music.Stop();
        }
    }
   
}
