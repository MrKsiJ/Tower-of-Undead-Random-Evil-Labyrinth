using UnityEngine;
using UnityEngine.UI;
public class Roll : MonoBehaviour
{
    const int MoneyItemOpenSilverChestSpawn = 27;

    [SerializeField] internal Sprite EmptySprite,StandardSprtie;
    [SerializeField] internal int idRoll = -1;
    [SerializeField] internal bool isItem = false;
    [SerializeField] internal bool isRollOpened = false;
    [SerializeField] internal bool isExitNextLvL = false;
    [SerializeField] internal bool isChest = false;
    [SerializeField] internal bool isEnemy = false;

    [Header("Если клетка окажется врагом")]
    [SerializeField] internal int MaxHealthEnemy, CurrentHealthEnemy = 0;
    [SerializeField] internal int MaxDamageEnemy, MinDamageEnemy = 0;

    

    Game game;
    Image ImgSprite;
    internal Image ItemSprite;
    public void RollStart()
    {
        idRoll = -1;
        isRollOpened = false;
        isExitNextLvL = false;
        isChest = false;
        game = Camera.main.GetComponent<Game>();
        ImgSprite = GetComponent<Image>();
        ImgSprite.sprite = StandardSprtie;
        ItemSprite = transform.GetChild(0).GetComponent<Image>();
        ItemSprite.enabled = false;
    }

    public void RollObject()
    {
        if(game.player.CurrentHealth > 0)
        {
            if (isRollOpened && isItem)
            {
                switch (idRoll)
                {
                    case 1:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 50 / 100;
                            int MinDamagePlus = game.player.MinDamage * 50 / 100;
                            game.player.MinDamage += MinDamagePlus;
                            game.player.MaxDamage += MaxDamagePlus;
                        }
                        else
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 20 / 100;
                            int MinDamagePlus = game.player.MinDamage * 20 / 100;
                            game.player.MinDamage += MinDamagePlus;
                            game.player.MaxDamage += MaxDamagePlus;
                        }
                        break;
                    case 2:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 25 / 100;
                            int MinDamagePlus = game.player.MinDamage * 25 / 100;
                            game.player.MaxDamage += MaxDamagePlus;
                            game.player.MinDamage += MinDamagePlus;
                        }
                        else
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 10 / 100;
                            int MinDamagePlus = game.player.MinDamage * 10 / 100;
                            game.player.MaxDamage += MaxDamagePlus;
                            game.player.MinDamage += MinDamagePlus;
                        }
                        break;
                    case 3:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 75 / 100;
                            int MinDamagePlus = game.player.MinDamage * 75 / 100;
                            game.player.MaxDamage += MaxDamagePlus;
                            game.player.MinDamage += MinDamagePlus;

                        }
                        else
                        {
                            int MaxDamagePlus = game.player.MaxDamage * 30 / 100;
                            int MinDamagePlus = game.player.MinDamage * 30 / 100;
                            game.player.MaxDamage += MaxDamagePlus;
                            game.player.MinDamage += MinDamagePlus;
                        }
                        break;
                    case 4:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 25 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        else
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 10 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        break;
                    case 5:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 50 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        else
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 20 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        break;
                    case 6:
                        if (!game.isTowerDifficuly)
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 75 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        else
                        {
                            int MaxHealthPlus = game.player.MaxHeath * 30 / 100;
                            game.player.MaxHeath += MaxHealthPlus;
                        }
                        break;
                    case 7:
                        if (!game.isTowerDifficuly)
                        {
                            int HealthPlus = game.player.MaxHeath * 25 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        else
                        {
                            int HealthPlus = game.player.MaxHeath * 10 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        break;
                    case 8:
                        if (!game.isTowerDifficuly)
                        {
                            int HealthPlus = game.player.MaxHeath * 50 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        else
                        {
                            int HealthPlus = game.player.MaxHeath * 20 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        break;
                    case 9:
                        if (!game.isTowerDifficuly)
                        {
                            int HealthPlus = game.player.MaxHeath * 75 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        else
                        {
                            int HealthPlus = game.player.MaxHeath * 30 / 100;
                            game.player.CurrentHealth += HealthPlus;
                        }
                        break;
                    case 27:
                        if (!game.isTowerDifficuly)
                            game.gameData.SetCurrentMoneyPlayer(game.CurrentLvL * Random.Range(1, 10), '+');
                        else
                            game.gameData.SetCurrentMoneyPlayer(game.CurrentLvL * Random.Range(1, 10 * game.CurrentLvL), '+');
                        game.gameData.SaveDataPlayer();
                        break;
                }
                ItemSprite.enabled = false;
                isItem = false;
            }
            if (!game.isEnemyAttack)
            {
                ImgSprite.sprite = EmptySprite;
                if (!isRollOpened)
                {
                    isEnemy = game.isEnemyAttack = idRoll >= 12;
                    if (game.isEnemyAttack)
                        game.EnemyRoll = this;
                    ItemSprite.enabled = idRoll != -1;
                    isRollOpened = true;
                }
                else if (isExitNextLvL)
                {
                    game.LvLChanged();
                }
                else if (isChest)
                {
                    SetItem(MoneyItemOpenSilverChestSpawn);
                    isChest = false;
                }
            }
            else if (isEnemy)
            {
                game.CreateHitSound();
                game.player.CurrentHealth -= Random.Range(MinDamageEnemy, MaxDamageEnemy);
                if (CurrentHealthEnemy >= 0)
                {
                    CurrentHealthEnemy -= Random.Range(game.player.MinDamage, game.player.MaxDamage);
                    if (CurrentHealthEnemy <= 0)
                        EnemyDead();
                }
                else
                    EnemyDead();
            }
        }
    }

    private void EnemyDead()
    {
        game.gameData.SetCurrentMoneyPlayer(1, '+');
        game.gameData.SaveDataPlayer();
        ItemSprite.enabled = false;
        game.isEnemyAttack = false;
        game.EnemyRoll = null;
        isEnemy = false;
    }

    internal void SetItem(int idRoll)
    {
        this.idRoll = idRoll;
        isExitNextLvL = idRoll == 0;
        isChest = idRoll == 10 || idRoll == 11;
        isItem = idRoll >= 1 && idRoll <= 9 || idRoll == 27;
        if (idRoll >= 0 && idRoll < game.AllSprites.Length)
        {
            ItemSprite.sprite = game.AllSprites[idRoll];
            ItemSprite.SetNativeSize();
            Vector2 size = ItemSprite.rectTransform.sizeDelta;
            size = new Vector2(size.x * 2, size.y * 2);
            ItemSprite.rectTransform.sizeDelta = size;
        }

    }
}
