using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    int[] Additions = {4,8,12,16,20,24,28,32,32,32,36,40,44,48,52,56,60,64,56,60,64,68,71,74,61,64,67,70,73,73 };
    internal string NameAniamtionLvLStatic = "Static";
    const string NameAnimationLvLGoodLuck = "LvLGoodLuck";
    const int SceneGame = 0;
    [Header("Константа дележа отправки данных аналитики о прохождении уровней")]
    [SerializeField] internal int MultiplityLvL = 10,MultiplityTower = 5;
    [Header("Текущий уровень")]
    [SerializeField] internal int CurrentLvL = 0;
    [SerializeField] internal Text TextCurrentMenu;
    [Header("Спрайты на уровне")]
    [SerializeField] internal Sprite[] AllSprites;

    //-idRoll-
    // -1 - пусто
    // 0 - Выход
    // 1 - сабля(MinMaxDamage) - 50%
    // 2 - нормальный меч меч(MinMaxDamage) - 25%
    // 3 - большой меч(MinMaxDamage) - 75%
    // 4 - кольцо(MaxHealth) - 25%
    // 5 - маленький щит(MaxHealth) - 50%
    // 6 - броня щит(MaxHealth) - 75%
    // 7 - маленький бутылёк(CurrentHealth) - 5%
    // 8 - средний бутылёк(CurrentHealth) - 25%
    // 9 - большой бутылёк(CurrentHealth) - 75%
    // 10 - сундук серебряный
    // 11 - сундук золотой
    // 12 - врагГоблин1
    // 13 - врагГоблин2 
    // 14 - врагГоблин3
    // 15 - врагГоблин4
    // 16 - врагГоблин5
    // 17 - врагЗомби1
    // 18 - врагЗомби2
    // 19 - врагЗомби3
    // 20 - врагЗомби4
    // 21 - врагЗомби5
    // 22 - врагСкелет1
    // 23 - врагСкелет2
    // 24 - врагСкелет3
    // 25 - врагСкелет4
    // 26 - врагСкелет5
    // 27 - монета
    [Header("Сетка уровня")]
    [SerializeField] internal Roll[] rolls;
    [Header("Режим башни")]
    [SerializeField] internal bool isTowerDifficuly = false;
    [Header("Прохождение башни")]
    [SerializeField] internal GameObject TowerAccept;
    [SerializeField] internal GameObject GameUI;
    [Header("Драка с врагом и его HUD")]
    [SerializeField] internal AudioClip hit;
    [SerializeField] internal bool isEnemyAttack = false;
    [SerializeField] internal bool isNextLvL = false;
    [SerializeField] internal GameObject HUDEnemyDataObject;
    [SerializeField] internal GameObject HUDGameOver;
    [SerializeField] internal Image EnemyImg;
    [SerializeField] internal Text TextHealthEnemy, TextDamageEnemy;

    internal Roll EnemyRoll;
    internal SaveAndLoadGameData gameData;
    internal Language language;
    internal Player player;

    int rndChestNumber, rndExitNumber = 0;
    void Start()
    {
        gameData = GetComponent<SaveAndLoadGameData>();
        language = FindObjectOfType<Language>();
        player = GetComponent<Player>();
    }
    internal void LoadGame()
    {
        if (EnemyRoll)
            EnemyRoll = null;
        if (isEnemyAttack)
            isEnemyAttack = false;
        if (!isTowerDifficuly)
            TextCurrentMenu.text =  language.LanguageUpdate(13) + CurrentLvL + language.LanguageUpdate(14);
        else
            TextCurrentMenu.text = language.LanguageUpdate(15) + language.LanguageUpdate(13) + CurrentLvL + language.LanguageUpdate(16);
        TextCurrentMenu.GetComponent<Animator>().Play(NameAnimationLvLGoodLuck,0,0);

        if(!isTowerDifficuly && isNextLvL && player.CurrentHealth > 0)
        {
            player.CurrentHealth = player.MaxHeath;
            isNextLvL = false;
        }
        LoadLvL();
    }

    void LoadLvL()
    {
        GenerationSpawnExitAndChest();
        GenerationMonstersAndItems();
    }

    private void GenerationSpawnExitAndChest()
    {
        for (int i = 0; i < rolls.Length; i++)
        {
            rolls[i].RollStart();
        }

        rndExitNumber = rndChestNumber = Random.Range(0, rolls.Length);
        while (rndChestNumber == rndExitNumber)
            rndChestNumber = Random.Range(0, rolls.Length);
        rolls[rndExitNumber].SetItem(0);
        rolls[rndChestNumber].SetItem(10);
    }

    private void GenerationMonstersAndItems()
    {
        for(int i = 0; i < rolls.Length; i++)
        {
            if(i != rndExitNumber && i != rndChestNumber)
            {
                int rnd = Random.Range(0, Additions[CurrentLvL - 1]);
                if(!isTowerDifficuly)
                    switch (CurrentLvL)
                {
                    case 1:
                        if (rnd >= 0 && rnd <= 2)
                            rolls[i].SetItem(-1);
                        else if (rnd > 2 && rnd <= 4)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        break;
                    case 2:
                        if (rnd >= 0 && rnd <= 5)
                            rolls[i].SetItem(-1);
                        else if(rnd > 5 && rnd <= 6)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if(rnd > 6 && rnd <= 7)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        break;
                    case 3:
                        if (rnd >= 0 && rnd <= 5)
                            rolls[i].SetItem(-1);
                        else if (rnd > 5 && rnd <= 6)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 6 && rnd <= 7)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 7 && rnd <= 10)
                            SetSpawnItem(i, 7);
                        else if(rnd > 10 && rnd <= 11)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        break;
                    case 4:
                        if (rnd >= 0 && rnd <= 5)
                            rolls[i].SetItem(-1);
                        else if (rnd > 5 && rnd <= 6)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 6 && rnd <= 7)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 7 && rnd <= 10)
                            SetSpawnItem(i, 7);
                        else if (rnd > 10 && rnd <= 11)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 11 && rnd <= 14)
                            SetSpawnItem(i, 4);
                        else if(rnd > 14 && rnd <=15)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        break;
                    case 5:
                        if (rnd >= 0 && rnd <= 5)
                            rolls[i].SetItem(-1);
                        else if (rnd > 5 && rnd <= 6)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 6 && rnd <= 7)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 7 && rnd <= 10)
                            SetSpawnItem(i, 7);
                        else if (rnd > 10 && rnd <= 11)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 11 && rnd <= 14)
                            SetSpawnItem(i, 4);
                        else if (rnd > 14 && rnd <= 15)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if(rnd > 15 && rnd <= 18)
                            SetSpawnItem(i, 2);
                        else if(rnd > 18 && rnd <= 19)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        break;
                    case 6:
                        if (rnd >= 0 && rnd <= 7)
                            rolls[i].SetItem(-1);
                        else if (rnd > 7 && rnd <= 8)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 8 && rnd <= 9)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 9 && rnd <= 13)
                            SetSpawnItem(i, 7);
                        else if (rnd > 13 && rnd <= 14)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 14 && rnd <= 17)
                            SetSpawnItem(i, 4);
                        else if (rnd > 17 && rnd <= 18)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 18 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if(rnd > 22 && rnd <= 23)
                            SetSpawnEnemy(i, 23, 50, 5, 30);

                        break;
                    case 7:
                        if (rnd >= 0 && rnd <= 9)
                            rolls[i].SetItem(-1);
                        else if (rnd > 9 && rnd <= 10)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 10 && rnd <= 11)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 11 && rnd <= 15)
                            SetSpawnItem(i, 7);
                        else if (rnd > 15 && rnd <= 16)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 16 && rnd <= 20)
                            SetSpawnItem(i, 4);
                        else if (rnd > 20 && rnd <= 21)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 21 && rnd <= 24)
                            SetSpawnItem(i, 2);
                        else if (rnd > 24 && rnd <= 25)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 25 && rnd <= 26)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if(rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        break;
                    case 8:
                        if (rnd >= 0 && rnd <= 11)
                            rolls[i].SetItem(-1);
                        else if (rnd > 11 && rnd <= 12)
                            SetSpawnEnemy(i, 12, 1, 1, 1);
                        else if (rnd > 12 && rnd <= 13)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 13 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 18)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 18 && rnd <= 22)
                            SetSpawnItem(i, 4);
                        else if (rnd > 22 && rnd <= 23)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 23 && rnd <= 27)
                            SetSpawnItem(i, 2);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if(rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        break;
                    case 9:
                        if (rnd >= 0 && rnd <= 11)
                            rolls[i].SetItem(-1);
                        else if (rnd > 11 && rnd <= 12)
                            SetSpawnEnemy(i, 13, 10, 1, 2);
                        else if (rnd > 12 && rnd <= 16)
                            SetSpawnItem(i, 7);
                        else if (rnd > 16 && rnd <= 17)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 17 && rnd <= 21)
                            SetSpawnItem(i, 4);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 22 && rnd <= 26)
                            SetSpawnItem(i, 2);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if(rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        break;
                    case 10:
                        if (rnd >= 0 && rnd <= 10)
                            rolls[i].SetItem(-1);
                        else if (rnd > 10 && rnd <= 14)
                            SetSpawnItem(i, 7);
                        else if (rnd > 14 && rnd <= 15)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 15 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 20)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 20 && rnd <= 24)
                            SetSpawnItem(i, 2);
                        else if (rnd > 24 && rnd <= 25)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 25 && rnd <= 26)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnItem(i, 8);
                        else if(rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        break;
                    case 11:
                        if (rnd >= 0 && rnd <= 11)
                            rolls[i].SetItem(-1);
                        else if (rnd > 11 && rnd <= 15)
                            SetSpawnItem(i, 7);
                        else if (rnd > 15 && rnd <= 16)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 16 && rnd <= 20)
                            SetSpawnItem(i, 4);
                        else if (rnd > 20 && rnd <= 21)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 2);
                        else if (rnd > 25 && rnd <= 26)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnItem(i, 8);
                        else if(rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if(rnd > 32 && rnd <= 33)
                            SetSpawnItem(i, 5);
                        else if(rnd < 33 && rnd <= 34)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        break;
                    case 12:
                        if (rnd >= 0 && rnd <= 11)
                            rolls[i].SetItem(-1);
                        else if (rnd > 11 && rnd <= 15)
                            SetSpawnItem(i, 7);
                        else if (rnd > 15 && rnd <= 16)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 16 && rnd <= 21)
                            SetSpawnItem(i, 4);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 22 && rnd <= 26)
                            SetSpawnItem(i, 2);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnItem(i, 8);
                        else if (rnd > 32 && rnd <= 35)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 35 && rnd <= 36)
                            SetSpawnItem(i, 5);
                        else if (rnd < 36 && rnd <= 38)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 38 && rnd <= 39)
                            SetSpawnItem(i, 1);
                        else if(rnd > 39 && rnd <= 40)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        break;
                    case 13:
                        if (rnd >= 0 && rnd <= 12)
                            rolls[i].SetItem(-1);
                        else if (rnd > 12 && rnd <= 16)
                            SetSpawnItem(i, 7);
                        else if (rnd > 16 && rnd <= 17)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 17 && rnd <= 21)
                            SetSpawnItem(i, 4);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 22 && rnd <= 26)
                            SetSpawnItem(i, 2);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 31 && rnd <= 33)
                            SetSpawnItem(i, 8);
                        else if (rnd > 33 && rnd <= 38)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 38 && rnd <= 39)
                            SetSpawnItem(i, 5);
                        else if (rnd < 39 && rnd <= 41)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 41 && rnd <= 42)
                            SetSpawnItem(i, 1);
                        else if (rnd > 42 && rnd <= 43)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        break;
                    case 14:
                        if (rnd >= 0 && rnd <= 13)
                            rolls[i].SetItem(-1);
                        else if (rnd > 13 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 18)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 18 && rnd <= 22)
                            SetSpawnItem(i, 4);
                        else if (rnd > 22 && rnd <= 23)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 23 && rnd <= 27)
                            SetSpawnItem(i, 2);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 32 && rnd <= 34)
                            SetSpawnItem(i, 8);
                        else if (rnd > 34 && rnd <= 39)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 39 && rnd <= 41)
                            SetSpawnItem(i, 5);
                        else if (rnd < 41 && rnd <= 45)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 45 && rnd <= 46)
                            SetSpawnItem(i, 1);
                        else if (rnd > 46 && rnd <= 47)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        break;
                    case 15:
                        if (rnd >= 0 && rnd <= 14)
                            rolls[i].SetItem(-1);
                        else if (rnd > 14 && rnd <= 18)
                            SetSpawnItem(i, 7);
                        else if (rnd > 18 && rnd <= 19)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 19 && rnd <= 23)
                            SetSpawnItem(i, 4);
                        else if (rnd > 23 && rnd <= 24)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 24 && rnd <= 28)
                            SetSpawnItem(i, 2);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 33 && rnd <= 35)
                            SetSpawnItem(i, 8);
                        else if (rnd > 35 && rnd <= 40)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 40 && rnd <= 42)
                            SetSpawnItem(i, 5);
                        else if (rnd < 42 && rnd <= 46)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 46 && rnd <= 48)
                            SetSpawnItem(i, 1);
                        else if (rnd > 48 && rnd <= 51)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        break;
                    case 16:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 19)
                            SetSpawnItem(i, 7);
                        else if (rnd > 19 && rnd <= 20)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 20 && rnd <= 24)
                            SetSpawnItem(i, 4);
                        else if (rnd > 24 && rnd <= 25)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 25 && rnd <= 29)
                            SetSpawnItem(i, 2);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 30 && rnd <= 31)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 34 && rnd <= 37)
                            SetSpawnItem(i, 8);
                        else if (rnd > 37 && rnd <= 43)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 43 && rnd <= 45)
                            SetSpawnItem(i, 5);
                        else if (rnd < 45 && rnd <= 49)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 49 && rnd <= 51)
                            SetSpawnItem(i, 1);
                        else if (rnd > 51 && rnd <= 54)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if(rnd > 54 && rnd <= 55)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        break;
                    case 17:
                        if (rnd >= 0 && rnd <= 16)
                            rolls[i].SetItem(-1);
                        else if (rnd > 16 && rnd <= 20)
                            SetSpawnItem(i, 7);
                        else if (rnd > 20 && rnd <= 21)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 4);
                        else if (rnd > 25 && rnd <= 26)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 29 && rnd <= 33)
                            SetSpawnItem(i, 2);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 35 && rnd <= 36)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 36 && rnd <= 37)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 37 && rnd <= 38)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 38 && rnd <= 41)
                            SetSpawnItem(i, 8);
                        else if (rnd > 41 && rnd <= 47)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 47 && rnd <= 50)
                            SetSpawnItem(i, 5);
                        else if (rnd < 50 && rnd <= 55)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 55 && rnd <= 57)
                            SetSpawnItem(i, 1);
                        else if (rnd > 57 && rnd <= 60)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 60 && rnd <= 61)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if(rnd > 61 && rnd <= 62)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        break;
                    case 18:
                        if (rnd >= 0 && rnd <= 17)
                            rolls[i].SetItem(-1);
                        else if (rnd > 17 && rnd <= 21)
                            SetSpawnItem(i, 7);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 17, 20, 1, 5);
                        else if (rnd > 22 && rnd <= 26)
                            SetSpawnItem(i, 4);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 18, 30, 1, 10);
                        else if (rnd > 27 && rnd <= 31)
                            SetSpawnItem(i, 2);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 22, 40, 5, 20);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 35 && rnd <= 36)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 36 && rnd <= 39)
                            SetSpawnItem(i, 8);
                        else if (rnd > 39 && rnd <= 46)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 46 && rnd <= 49)
                            SetSpawnItem(i, 5);
                        else if (rnd < 49 && rnd <= 54)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 54 && rnd <= 57)
                            SetSpawnItem(i, 1);
                        else if (rnd > 57 && rnd <= 61)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 61 && rnd <= 62)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 62 && rnd <= 63)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if(rnd > 63 && rnd <= 64)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 19:
                        if (rnd >= 0 && rnd <= 18)
                            rolls[i].SetItem(-1);
                        else if (rnd > 18 && rnd <= 21)
                            SetSpawnItem(i, 7);
                        else if (rnd > 18 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 20)
                            SetSpawnItem(i, 2);
                        else if (rnd > 20 && rnd <= 21)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 21 && rnd <= 22)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 22 && rnd <= 23)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 23 && rnd <= 24)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 24 && rnd <= 27)
                            SetSpawnItem(i, 8);
                        else if (rnd > 27 && rnd <= 33)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 33 && rnd <= 36)
                            SetSpawnItem(i, 5);
                        else if (rnd < 36 && rnd <= 41)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 41 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 50)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 50 && rnd <= 51)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if(rnd > 51 && rnd <= 52)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 20:
                        if (rnd >= 0 && rnd <= 19)
                            rolls[i].SetItem(-1);
                        else if (rnd > 19 && rnd <= 22)
                            SetSpawnItem(i, 7);
                        else if (rnd > 22 && rnd <= 25)
                            SetSpawnItem(i, 4);
                        else if (rnd > 25 && rnd <= 26)
                            SetSpawnItem(i, 2);
                        else if (rnd > 26 && rnd <= 27)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 27 && rnd <= 28)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 28 && rnd <= 29)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 29 && rnd <= 30)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 30 && rnd <= 33)
                            SetSpawnItem(i, 8);
                        else if (rnd > 33 && rnd <= 39)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 39 && rnd <= 42)
                            SetSpawnItem(i, 5);
                        else if (rnd < 42 && rnd <= 47)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 47 && rnd <= 50)
                            SetSpawnItem(i, 1);
                        else if (rnd > 50 && rnd <= 54)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 54 && rnd <= 56)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 56 && rnd <= 58)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 58 && rnd <= 59)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 21:
                        if (rnd >= 0 && rnd <= 20)
                            rolls[i].SetItem(-1);
                        else if (rnd > 20 && rnd <= 23)
                            SetSpawnItem(i, 7);
                        else if (rnd > 25 && rnd <= 28)
                            SetSpawnItem(i, 4);
                        else if (rnd > 28 && rnd <= 31)
                            SetSpawnItem(i, 2);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 35 && rnd <= 38)
                            SetSpawnItem(i, 8);
                        else if (rnd > 38 && rnd <= 44)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 44 && rnd <= 37)
                            SetSpawnItem(i, 5);
                        else if (rnd < 37 && rnd <= 41)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 41 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 50)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 50 && rnd <= 52)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 52 && rnd <= 54)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 22:
                        if (rnd >= 0 && rnd <= 22)
                            rolls[i].SetItem(-1);
                        else if (rnd > 22 && rnd <= 25)
                            SetSpawnItem(i, 7);
                        else if (rnd > 25 && rnd <= 28)
                            SetSpawnItem(i, 4);
                        else if (rnd > 28 && rnd <= 31)
                            SetSpawnItem(i, 2);
                        else if (rnd > 31 && rnd <= 32)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 35 && rnd <= 39)
                            SetSpawnItem(i, 8);
                        else if (rnd > 39 && rnd <= 45)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 45 && rnd <= 38)
                            SetSpawnItem(i, 5);
                        else if (rnd < 38 && rnd <= 42)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 42 && rnd <= 45)
                            SetSpawnItem(i, 1);
                        else if (rnd > 45 && rnd <= 49)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 49 && rnd <= 52)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 52 && rnd <= 54)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 54 && rnd <= 56)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 23:
                        if (rnd >= 0 && rnd <= 23)
                            rolls[i].SetItem(-1);
                        else if (rnd > 23 && rnd <= 26)
                            SetSpawnItem(i, 7);
                        else if (rnd > 26 && rnd <= 29)
                            SetSpawnItem(i, 4);
                        else if (rnd > 29 && rnd <= 32)
                            SetSpawnItem(i, 2);
                        else if (rnd > 32 && rnd <= 33)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 35 && rnd <= 36)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 36 && rnd <= 40)
                            SetSpawnItem(i, 8);
                        else if (rnd > 40 && rnd <= 46)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 46 && rnd <= 50)
                            SetSpawnItem(i, 5);
                        else if (rnd < 50 && rnd <= 55)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 55 && rnd <= 58)
                            SetSpawnItem(i, 1);
                        else if (rnd > 58 && rnd <= 62)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 62 && rnd <= 65)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 65 && rnd <= 68)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 68 && rnd <= 70)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 24:
                        if (rnd >= 0 && rnd <= 24)
                            rolls[i].SetItem(-1);
                        else if (rnd > 24 && rnd <= 27)
                            SetSpawnItem(i, 7);
                        else if (rnd > 27 && rnd <= 30)
                            SetSpawnItem(i, 4);
                        else if (rnd > 30 && rnd <= 33)
                            SetSpawnItem(i, 2);
                        else if (rnd > 33 && rnd <= 34)
                            SetSpawnEnemy(i, 23, 50, 5, 30);
                        else if (rnd > 34 && rnd <= 35)
                            SetSpawnEnemy(i, 14, 75, 5, 40);
                        else if (rnd > 35 && rnd <= 36)
                            SetSpawnEnemy(i, 19, 100, 5, 50);
                        else if (rnd > 36 && rnd <= 37)
                            SetSpawnEnemy(i, 24, 125, 10, 60);
                        else if (rnd > 37 && rnd <= 41)
                            SetSpawnItem(i, 8);
                        else if (rnd > 41 && rnd <= 47)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 47 && rnd <= 51)
                            SetSpawnItem(i, 5);
                        else if (rnd < 51 && rnd <= 56)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 56 && rnd <= 60)
                            SetSpawnItem(i, 1);
                        else if (rnd > 60 && rnd <= 64)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 64 && rnd <= 67)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 67 && rnd <= 70)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 70 && rnd <= 73)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        break;
                    case 25:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 53)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 53 && rnd <= 56)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 56 && rnd <= 59)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 59 && rnd <= 60)
                            SetSpawnItem(i,9);
                        break;
                    case 26:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 53)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 53 && rnd <= 58)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 58 && rnd <= 61)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 61 && rnd <= 62)
                            SetSpawnItem(i, 9);
                        else if (rnd > 62 && rnd <= 63)
                            SetSpawnItem(i,6);
                        break;
                    case 27:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 53)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 53 && rnd <= 58)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 58 && rnd <= 63)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 63 && rnd <= 64)
                            SetSpawnItem(i, 9);
                        else if (rnd > 64 && rnd <= 65)
                            SetSpawnItem(i, 6);
                        else if (rnd > 65 && rnd <= 66)
                            SetSpawnItem(i, 3);
                        break;
                    case 28:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 56)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 56 && rnd <= 61)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 61 && rnd <= 66)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 66 && rnd <= 67)
                            SetSpawnItem(i, 9);
                        else if (rnd > 67 && rnd <= 68)
                            SetSpawnItem(i, 6);
                        else if (rnd > 68 && rnd <= 69)
                            SetSpawnItem(i, 3);
                        break;
                    case 29:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 56)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 56 && rnd <= 64)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 64 && rnd <= 69)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 69 && rnd <= 70)
                            SetSpawnItem(i, 9);
                        else if (rnd > 70 && rnd <= 71)
                            SetSpawnItem(i, 6);
                        else if (rnd > 71 && rnd <= 72)
                            SetSpawnItem(i, 3);
                        break;
                    case 30:
                        if (rnd >= 0 && rnd <= 15)
                            rolls[i].SetItem(-1);
                        else if (rnd > 15 && rnd <= 17)
                            SetSpawnItem(i, 7);
                        else if (rnd > 17 && rnd <= 19)
                            SetSpawnItem(i, 4);
                        else if (rnd > 19 && rnd <= 21)
                            SetSpawnItem(i, 2);
                        else if (rnd > 21 && rnd <= 25)
                            SetSpawnItem(i, 8);
                        else if (rnd > 25 && rnd <= 31)
                            SetSpawnEnemy(i, 15, 150, 10, 70);
                        else if (rnd > 31 && rnd <= 35)
                            SetSpawnItem(i, 5);
                        else if (rnd < 35 && rnd <= 40)
                            SetSpawnEnemy(i, 20, 175, 10, 80);
                        else if (rnd > 40 && rnd <= 44)
                            SetSpawnItem(i, 1);
                        else if (rnd > 44 && rnd <= 48)
                            SetSpawnEnemy(i, 25, 200, 10, 90);
                        else if (rnd > 48 && rnd <= 56)
                            SetSpawnEnemy(i, 16, 250, 25, 125);
                        else if (rnd > 56 && rnd <= 64)
                            SetSpawnEnemy(i, 21, 300, 25, 150);
                        else if (rnd > 64 && rnd <= 69)
                            SetSpawnEnemy(i, 26, 350, 25, 175);
                        else if (rnd > 69 && rnd <= 70)
                            SetSpawnItem(i, 9);
                        else if (rnd > 70 && rnd <= 71)
                            SetSpawnItem(i, 6);
                        else if (rnd > 71 && rnd <= 72)
                            SetSpawnItem(i, 3);
                        break;
                }
                else
                    switch (CurrentLvL)
                    {
                        case 1:
                            if (rnd >= 0 && rnd <= 2)
                                rolls[i].SetItem(-1);
                            else if (rnd > 2 && rnd <= 4)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            break;
                        case 2:
                            if (rnd >= 0 && rnd <= 5)
                                rolls[i].SetItem(-1);
                            else if (rnd > 5 && rnd <= 6)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 6 && rnd <= 7)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            break;
                        case 3:
                            if (rnd >= 0 && rnd <= 5)
                                rolls[i].SetItem(-1);
                            else if (rnd > 5 && rnd <= 6)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 6 && rnd <= 7)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 7 && rnd <= 10)
                                SetSpawnItem(i, 7);
                            else if (rnd > 10 && rnd <= 11)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            break;
                        case 4:
                            if (rnd >= 0 && rnd <= 5)
                                rolls[i].SetItem(-1);
                            else if (rnd > 5 && rnd <= 6)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 6 && rnd <= 7)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 7 && rnd <= 10)
                                SetSpawnItem(i, 7);
                            else if (rnd > 10 && rnd <= 11)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 11 && rnd <= 14)
                                SetSpawnItem(i, 4);
                            else if (rnd > 14 && rnd <= 15)
                                SetSpawnEnemy(i, 18, 200, 20, 200);
                            break;
                        case 5:
                            if (rnd >= 0 && rnd <= 5)
                                rolls[i].SetItem(-1);
                            else if (rnd > 5 && rnd <= 6)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 6 && rnd <= 7)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 7 && rnd <= 10)
                                SetSpawnItem(i, 7);
                            else if (rnd > 10 && rnd <= 11)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 11 && rnd <= 14)
                                SetSpawnItem(i, 4);
                            else if (rnd > 14 && rnd <= 15)
                                SetSpawnEnemy(i, 18, 200, 20, 200);
                            else if (rnd > 15 && rnd <= 18)
                                SetSpawnItem(i, 2);
                            else if (rnd > 18 && rnd <= 19)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            break;
                        case 6:
                            if (rnd >= 0 && rnd <= 7)
                                rolls[i].SetItem(-1);
                            else if (rnd > 7 && rnd <= 8)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 8 && rnd <= 9)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 9 && rnd <= 13)
                                SetSpawnItem(i, 7);
                            else if (rnd > 13 && rnd <= 14)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 14 && rnd <= 17)
                                SetSpawnItem(i, 4);
                            else if (rnd > 17 && rnd <= 18)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 18 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 22 && rnd <= 23)
                                SetSpawnEnemy(i, 23, 100, 10, 100);

                            break;
                        case 7:
                            if (rnd >= 0 && rnd <= 9)
                                rolls[i].SetItem(-1);
                            else if (rnd > 9 && rnd <= 10)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 10 && rnd <= 11)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 11 && rnd <= 15)
                                SetSpawnItem(i, 7);
                            else if (rnd > 15 && rnd <= 16)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 16 && rnd <= 20)
                                SetSpawnItem(i, 4);
                            else if (rnd > 20 && rnd <= 21)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 21 && rnd <= 24)
                                SetSpawnItem(i, 2);
                            else if (rnd > 24 && rnd <= 25)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 25 && rnd <= 26)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            break;
                        case 8:
                            if (rnd >= 0 && rnd <= 11)
                                rolls[i].SetItem(-1);
                            else if (rnd > 11 && rnd <= 12)
                                SetSpawnEnemy(i, 12, 10, 1, 10);
                            else if (rnd > 12 && rnd <= 13)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 13 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 18)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 18 && rnd <= 22)
                                SetSpawnItem(i, 4);
                            else if (rnd > 22 && rnd <= 23)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 23 && rnd <= 27)
                                SetSpawnItem(i, 2);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            break;
                        case 9:
                            if (rnd >= 0 && rnd <= 11)
                                rolls[i].SetItem(-1);
                            else if (rnd > 11 && rnd <= 12)
                                SetSpawnEnemy(i, 13, 20, 2, 20);
                            else if (rnd > 12 && rnd <= 16)
                                SetSpawnItem(i, 7);
                            else if (rnd > 16 && rnd <= 17)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 17 && rnd <= 21)
                                SetSpawnItem(i, 4);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 22 && rnd <= 26)
                                SetSpawnItem(i, 2);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 14, 75, 5, 40);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 19, 100, 5, 50);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            break;
                        case 10:
                            if (rnd >= 0 && rnd <= 10)
                                rolls[i].SetItem(-1);
                            else if (rnd > 10 && rnd <= 14)
                                SetSpawnItem(i, 7);
                            else if (rnd > 14 && rnd <= 15)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 15 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 20)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 20 && rnd <= 24)
                                SetSpawnItem(i, 2);
                            else if (rnd > 24 && rnd <= 25)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 25 && rnd <= 26)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnItem(i, 8);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            break;
                        case 11:
                            if (rnd >= 0 && rnd <= 11)
                                rolls[i].SetItem(-1);
                            else if (rnd > 11 && rnd <= 15)
                                SetSpawnItem(i, 7);
                            else if (rnd > 15 && rnd <= 16)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 16 && rnd <= 20)
                                SetSpawnItem(i, 4);
                            else if (rnd > 20 && rnd <= 21)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 2);
                            else if (rnd > 25 && rnd <= 26)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnItem(i, 8);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnItem(i, 5);
                            else if (rnd < 33 && rnd <= 34)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            break;
                        case 12:
                            if (rnd >= 0 && rnd <= 11)
                                rolls[i].SetItem(-1);
                            else if (rnd > 11 && rnd <= 15)
                                SetSpawnItem(i, 7);
                            else if (rnd > 15 && rnd <= 16)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 16 && rnd <= 21)
                                SetSpawnItem(i, 4);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 22 && rnd <= 26)
                                SetSpawnItem(i, 2);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnItem(i, 8);
                            else if (rnd > 32 && rnd <= 35)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 35 && rnd <= 36)
                                SetSpawnItem(i, 5);
                            else if (rnd < 36 && rnd <= 38)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 38 && rnd <= 39)
                                SetSpawnItem(i, 1);
                            else if (rnd > 39 && rnd <= 40)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            break;
                        case 13:
                            if (rnd >= 0 && rnd <= 12)
                                rolls[i].SetItem(-1);
                            else if (rnd > 12 && rnd <= 16)
                                SetSpawnItem(i, 7);
                            else if (rnd > 16 && rnd <= 17)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 17 && rnd <= 21)
                                SetSpawnItem(i, 4);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 22 && rnd <= 26)
                                SetSpawnItem(i, 2);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 31 && rnd <= 33)
                                SetSpawnItem(i, 8);
                            else if (rnd > 33 && rnd <= 38)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 38 && rnd <= 39)
                                SetSpawnItem(i, 5);
                            else if (rnd < 39 && rnd <= 41)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 41 && rnd <= 42)
                                SetSpawnItem(i, 1);
                            else if (rnd > 42 && rnd <= 43)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            break;
                        case 14:
                            if (rnd >= 0 && rnd <= 13)
                                rolls[i].SetItem(-1);
                            else if (rnd > 13 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 18)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 18 && rnd <= 22)
                                SetSpawnItem(i, 4);
                            else if (rnd > 22 && rnd <= 23)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 23 && rnd <= 27)
                                SetSpawnItem(i, 2);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 32 && rnd <= 34)
                                SetSpawnItem(i, 8);
                            else if (rnd > 34 && rnd <= 39)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 39 && rnd <= 41)
                                SetSpawnItem(i, 5);
                            else if (rnd < 41 && rnd <= 45)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 45 && rnd <= 46)
                                SetSpawnItem(i, 1);
                            else if (rnd > 46 && rnd <= 47)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            break;
                        case 15:
                            if (rnd >= 0 && rnd <= 14)
                                rolls[i].SetItem(-1);
                            else if (rnd > 14 && rnd <= 18)
                                SetSpawnItem(i, 7);
                            else if (rnd > 18 && rnd <= 19)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 19 && rnd <= 23)
                                SetSpawnItem(i, 4);
                            else if (rnd > 23 && rnd <= 24)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 24 && rnd <= 28)
                                SetSpawnItem(i, 2);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 33 && rnd <= 35)
                                SetSpawnItem(i, 8);
                            else if (rnd > 35 && rnd <= 40)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 40 && rnd <= 42)
                                SetSpawnItem(i, 5);
                            else if (rnd < 42 && rnd <= 46)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 46 && rnd <= 48)
                                SetSpawnItem(i, 1);
                            else if (rnd > 48 && rnd <= 51)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            break;
                        case 16:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 19)
                                SetSpawnItem(i, 7);
                            else if (rnd > 19 && rnd <= 20)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 20 && rnd <= 24)
                                SetSpawnItem(i, 4);
                            else if (rnd > 24 && rnd <= 25)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 25 && rnd <= 29)
                                SetSpawnItem(i, 2);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 30 && rnd <= 31)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 34 && rnd <= 37)
                                SetSpawnItem(i, 8);
                            else if (rnd > 37 && rnd <= 43)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 43 && rnd <= 45)
                                SetSpawnItem(i, 5);
                            else if (rnd < 45 && rnd <= 49)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 49 && rnd <= 51)
                                SetSpawnItem(i, 1);
                            else if (rnd > 51 && rnd <= 54)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 54 && rnd <= 55)
                                SetSpawnEnemy(i, 16, 250, 25, 125);
                            break;
                        case 17:
                            if (rnd >= 0 && rnd <= 16)
                                rolls[i].SetItem(-1);
                            else if (rnd > 16 && rnd <= 20)
                                SetSpawnItem(i, 7);
                            else if (rnd > 20 && rnd <= 21)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 4);
                            else if (rnd > 25 && rnd <= 26)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 29 && rnd <= 33)
                                SetSpawnItem(i, 2);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 35 && rnd <= 36)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 36 && rnd <= 37)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 37 && rnd <= 38)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 38 && rnd <= 41)
                                SetSpawnItem(i, 8);
                            else if (rnd > 41 && rnd <= 47)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 47 && rnd <= 50)
                                SetSpawnItem(i, 5);
                            else if (rnd < 50 && rnd <= 55)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 55 && rnd <= 57)
                                SetSpawnItem(i, 1);
                            else if (rnd > 57 && rnd <= 60)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 60 && rnd <= 61)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 61 && rnd <= 62)
                                SetSpawnEnemy(i, 21, 300, 25, 150);
                            break;
                        case 18:
                            if (rnd >= 0 && rnd <= 17)
                                rolls[i].SetItem(-1);
                            else if (rnd > 17 && rnd <= 21)
                                SetSpawnItem(i, 7);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 17, 30, 3, 30);
                            else if (rnd > 22 && rnd <= 26)
                                SetSpawnItem(i, 4);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 18, 40, 4, 40);
                            else if (rnd > 27 && rnd <= 31)
                                SetSpawnItem(i, 2);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 22, 50, 5, 50);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 35 && rnd <= 36)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 36 && rnd <= 39)
                                SetSpawnItem(i, 8);
                            else if (rnd > 39 && rnd <= 46)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 46 && rnd <= 49)
                                SetSpawnItem(i, 5);
                            else if (rnd < 49 && rnd <= 54)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 54 && rnd <= 57)
                                SetSpawnItem(i, 1);
                            else if (rnd > 57 && rnd <= 61)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 61 && rnd <= 62)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 62 && rnd <= 63)
                                SetSpawnEnemy(i, 21, 300, 25, 150);
                            else if (rnd > 63 && rnd <= 64)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 19:
                            if (rnd >= 0 && rnd <= 18)
                                rolls[i].SetItem(-1);
                            else if (rnd > 18 && rnd <= 21)
                                SetSpawnItem(i, 7);
                            else if (rnd > 18 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 20)
                                SetSpawnItem(i, 2);
                            else if (rnd > 20 && rnd <= 21)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 21 && rnd <= 22)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 22 && rnd <= 23)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 23 && rnd <= 24)
                                SetSpawnEnemy(i, 24, 125, 10, 60);
                            else if (rnd > 24 && rnd <= 27)
                                SetSpawnItem(i, 8);
                            else if (rnd > 27 && rnd <= 33)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 33 && rnd <= 36)
                                SetSpawnItem(i, 5);
                            else if (rnd < 36 && rnd <= 41)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 41 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 50)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 50 && rnd <= 51)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 51 && rnd <= 52)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 20:
                            if (rnd >= 0 && rnd <= 19)
                                rolls[i].SetItem(-1);
                            else if (rnd > 19 && rnd <= 22)
                                SetSpawnItem(i, 7);
                            else if (rnd > 22 && rnd <= 25)
                                SetSpawnItem(i, 4);
                            else if (rnd > 25 && rnd <= 26)
                                SetSpawnItem(i, 2);
                            else if (rnd > 26 && rnd <= 27)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 27 && rnd <= 28)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 28 && rnd <= 29)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 29 && rnd <= 30)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 30 && rnd <= 33)
                                SetSpawnItem(i, 8);
                            else if (rnd > 33 && rnd <= 39)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 39 && rnd <= 42)
                                SetSpawnItem(i, 5);
                            else if (rnd < 42 && rnd <= 47)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 47 && rnd <= 50)
                                SetSpawnItem(i, 1);
                            else if (rnd > 50 && rnd <= 54)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 54 && rnd <= 56)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 56 && rnd <= 58)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 58 && rnd <= 59)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 21:
                            if (rnd >= 0 && rnd <= 20)
                                rolls[i].SetItem(-1);
                            else if (rnd > 20 && rnd <= 23)
                                SetSpawnItem(i, 7);
                            else if (rnd > 25 && rnd <= 28)
                                SetSpawnItem(i, 4);
                            else if (rnd > 28 && rnd <= 31)
                                SetSpawnItem(i, 2);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 35 && rnd <= 38)
                                SetSpawnItem(i, 8);
                            else if (rnd > 38 && rnd <= 44)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 44 && rnd <= 37)
                                SetSpawnItem(i, 5);
                            else if (rnd < 37 && rnd <= 41)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 41 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 50)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 50 && rnd <= 52)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 52 && rnd <= 54)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 22:
                            if (rnd >= 0 && rnd <= 22)
                                rolls[i].SetItem(-1);
                            else if (rnd > 22 && rnd <= 25)
                                SetSpawnItem(i, 7);
                            else if (rnd > 25 && rnd <= 28)
                                SetSpawnItem(i, 4);
                            else if (rnd > 28 && rnd <= 31)
                                SetSpawnItem(i, 2);
                            else if (rnd > 31 && rnd <= 32)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 35 && rnd <= 39)
                                SetSpawnItem(i, 8);
                            else if (rnd > 39 && rnd <= 45)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 45 && rnd <= 38)
                                SetSpawnItem(i, 5);
                            else if (rnd < 38 && rnd <= 42)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 42 && rnd <= 45)
                                SetSpawnItem(i, 1);
                            else if (rnd > 45 && rnd <= 49)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 49 && rnd <= 52)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 52 && rnd <= 54)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 54 && rnd <= 56)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 23:
                            if (rnd >= 0 && rnd <= 23)
                                rolls[i].SetItem(-1);
                            else if (rnd > 23 && rnd <= 26)
                                SetSpawnItem(i, 7);
                            else if (rnd > 26 && rnd <= 29)
                                SetSpawnItem(i, 4);
                            else if (rnd > 29 && rnd <= 32)
                                SetSpawnItem(i, 2);
                            else if (rnd > 32 && rnd <= 33)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 35 && rnd <= 36)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 36 && rnd <= 40)
                                SetSpawnItem(i, 8);
                            else if (rnd > 40 && rnd <= 46)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 46 && rnd <= 50)
                                SetSpawnItem(i, 5);
                            else if (rnd < 50 && rnd <= 55)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 55 && rnd <= 58)
                                SetSpawnItem(i, 1);
                            else if (rnd > 58 && rnd <= 62)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 62 && rnd <= 65)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 65 && rnd <= 68)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 68 && rnd <= 70)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 24:
                            if (rnd >= 0 && rnd <= 24)
                                rolls[i].SetItem(-1);
                            else if (rnd > 24 && rnd <= 27)
                                SetSpawnItem(i, 7);
                            else if (rnd > 27 && rnd <= 30)
                                SetSpawnItem(i, 4);
                            else if (rnd > 30 && rnd <= 33)
                                SetSpawnItem(i, 2);
                            else if (rnd > 33 && rnd <= 34)
                                SetSpawnEnemy(i, 23, 100, 10, 100);
                            else if (rnd > 34 && rnd <= 35)
                                SetSpawnEnemy(i, 14, 200, 20, 200);
                            else if (rnd > 35 && rnd <= 36)
                                SetSpawnEnemy(i, 19, 400, 40, 400);
                            else if (rnd > 36 && rnd <= 37)
                                SetSpawnEnemy(i, 24, 800, 80, 800);
                            else if (rnd > 37 && rnd <= 41)
                                SetSpawnItem(i, 8);
                            else if (rnd > 41 && rnd <= 47)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 47 && rnd <= 51)
                                SetSpawnItem(i, 5);
                            else if (rnd < 51 && rnd <= 56)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 56 && rnd <= 60)
                                SetSpawnItem(i, 1);
                            else if (rnd > 60 && rnd <= 64)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 64 && rnd <= 67)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 67 && rnd <= 70)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 70 && rnd <= 73)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            break;
                        case 25:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 53)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 53 && rnd <= 56)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 56 && rnd <= 59)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 59 && rnd <= 60)
                                SetSpawnItem(i, 9);
                            break;
                        case 26:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 53)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 53 && rnd <= 58)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 58 && rnd <= 61)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 61 && rnd <= 62)
                                SetSpawnItem(i, 9);
                            else if (rnd > 62 && rnd <= 63)
                                SetSpawnItem(i, 6);
                            break;
                        case 27:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 150, 10, 70);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 53)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 53 && rnd <= 58)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 58 && rnd <= 63)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 63 && rnd <= 64)
                                SetSpawnItem(i, 9);
                            else if (rnd > 64 && rnd <= 65)
                                SetSpawnItem(i, 6);
                            else if (rnd > 65 && rnd <= 66)
                                SetSpawnItem(i, 3);
                            break;
                        case 28:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 56)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 56 && rnd <= 61)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 61 && rnd <= 66)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 66 && rnd <= 67)
                                SetSpawnItem(i, 9);
                            else if (rnd > 67 && rnd <= 68)
                                SetSpawnItem(i, 6);
                            else if (rnd > 68 && rnd <= 69)
                                SetSpawnItem(i, 3);
                            break;
                        case 29:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 56)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 56 && rnd <= 64)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 64 && rnd <= 69)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 69 && rnd <= 70)
                                SetSpawnItem(i, 9);
                            else if (rnd > 70 && rnd <= 71)
                                SetSpawnItem(i, 6);
                            else if (rnd > 71 && rnd <= 72)
                                SetSpawnItem(i, 3);
                            break;
                        case 30:
                            if (rnd >= 0 && rnd <= 15)
                                rolls[i].SetItem(-1);
                            else if (rnd > 15 && rnd <= 17)
                                SetSpawnItem(i, 7);
                            else if (rnd > 17 && rnd <= 19)
                                SetSpawnItem(i, 4);
                            else if (rnd > 19 && rnd <= 21)
                                SetSpawnItem(i, 2);
                            else if (rnd > 21 && rnd <= 25)
                                SetSpawnItem(i, 8);
                            else if (rnd > 25 && rnd <= 31)
                                SetSpawnEnemy(i, 15, 1200, 120, 1200);
                            else if (rnd > 31 && rnd <= 35)
                                SetSpawnItem(i, 5);
                            else if (rnd < 35 && rnd <= 40)
                                SetSpawnEnemy(i, 20, 2400, 240, 2400);
                            else if (rnd > 40 && rnd <= 44)
                                SetSpawnItem(i, 1);
                            else if (rnd > 44 && rnd <= 48)
                                SetSpawnEnemy(i, 25, 4800, 480, 4800);
                            else if (rnd > 48 && rnd <= 56)
                                SetSpawnEnemy(i, 16, 10000, 1000, 10000);
                            else if (rnd > 56 && rnd <= 64)
                                SetSpawnEnemy(i, 21, 20000, 2000, 20000);
                            else if (rnd > 64 && rnd <= 69)
                                SetSpawnEnemy(i, 26, 30000, 3000, 30000);
                            else if (rnd > 69 && rnd <= 70)
                                SetSpawnItem(i, 9);
                            else if (rnd > 70 && rnd <= 71)
                                SetSpawnItem(i, 6);
                            else if (rnd > 71 && rnd <= 72)
                                SetSpawnItem(i, 3);
                            break;
                    }
            }
        }
    }

    private void SetSpawnEnemy(int indexRoll,int idEnemy,int CurrentHealthEnemy,int MinDamageEnemy,int MaxDamageEnemy)
    {
        rolls[indexRoll].SetItem(idEnemy);
        rolls[indexRoll].MaxHealthEnemy = rolls[indexRoll].CurrentHealthEnemy = CurrentHealthEnemy;
        rolls[indexRoll].MinDamageEnemy = MinDamageEnemy;
        rolls[indexRoll].MaxDamageEnemy = MaxDamageEnemy;
    }
    private void SetSpawnItem(int indexRoll,int idItem)
    {
        rolls[indexRoll].SetItem(idItem);
    }

    internal void LvLChanged()
    {
        if (player.CurrentHealth > 0 && CurrentLvL != 30)
        {
            if(!isTowerDifficuly)
                player.LoadPlayerData();
            if (!isTowerDifficuly && CurrentLvL == gameData.GetLvLMaxComplect())
            {
                if(CurrentLvL % MultiplityLvL == 0)
                    AnalyticksLvLS();
                gameData.SetLvLMaxComplect(1, '+');
                CurrentLvL = gameData.GetLvLMaxComplect();
            }
            else if (isTowerDifficuly && CurrentLvL == gameData.GetLvLMaxTower())
            {
                if(CurrentLvL % MultiplityTower == 0)
                    AnalyticksLvLS();
                gameData.SetLvLMaxTower(1, '+');
                CurrentLvL = gameData.GetLvLMaxTower();
            }
            else
                CurrentLvL++;
        }
        else if(CurrentLvL == 30)
        {
            if (isTowerDifficuly)
            {
                GetComponent<MenuGame>().OpenMenu(TowerAccept);
                GetComponent<MenuGame>().CloseMenu(GameUI);
                GetComponent<MenuGame>().Music.Stop();
            }
            else
            {
                BackToHome();
            }
        }
        if (!isTowerDifficuly && player.CurrentHealth > 0 && CurrentLvL != 30)
            isNextLvL = true;

        if(CurrentLvL != 30)
        {
            Debug.Log("AD SHOWED IN LEVELS!");
            GetComponent<AdMobRealtek>().AdShow();
            LoadGame();
        }
    }

    private void AnalyticksLvLS()
    {
        if (!isTowerDifficuly)
        {
            Debug.Log("Analyticks isNoTower");

            FirebaseAnalytics.LogEvent("LvLs",
                new Parameter("type", CurrentLvL + " уровень был достигнут!"));
        }
        else
        {
            Debug.Log("Analyticks isTower");

            FirebaseAnalytics.LogEvent("Tower_LvLs",
                 new Parameter("type", CurrentLvL + " уровень в башне был достигнут!"));
        }
        gameData.SaveDataPlayer();
    }

    public void BackToHome()
    {
        gameData.SaveDataPlayer();
        SceneManager.LoadScene(SceneGame);
    }
    public void RestartGame()
    {
        if(!isTowerDifficuly)
            GetComponent<MenuGame>().LauchGameMethods(isTowerDifficuly,CurrentLvL);
        else
            GetComponent<MenuGame>().LauchGameMethods(isTowerDifficuly, 1);
    }

    public void CreateHitSound()
    {
        GameObject audioHit = new GameObject();
        audioHit.AddComponent<AudioSource>();
        audioHit.name = "Hited!";
        audioHit.GetComponent<AudioSource>().clip = hit;
        audioHit.GetComponent<AudioSource>().Play();
        Destroy(audioHit, hit.length);
    }

    void FixedUpdate()
    {
        RealtimeEnemyData();
    }

    private void RealtimeEnemyData()
    {
        HUDEnemyDataObject.SetActive(isEnemyAttack);
        if (isEnemyAttack)
        {
            if (EnemyRoll)
            {
                EnemyImg.sprite = EnemyRoll.ItemSprite.sprite;
                string damage = language.LanguageUpdate(8).Replace(" ", "");
                TextHealthEnemy.text = language.LanguageUpdate(7) + EnemyRoll.CurrentHealthEnemy + " / " + EnemyRoll.MaxHealthEnemy;
                TextDamageEnemy.text = damage + " " + EnemyRoll.MinDamageEnemy + " - " + EnemyRoll.MaxDamageEnemy;
            }
            else
            {
                Debug.LogError("Враг не обнаружен не удалось считать данные об враге!");
            }
        }
    }
}
