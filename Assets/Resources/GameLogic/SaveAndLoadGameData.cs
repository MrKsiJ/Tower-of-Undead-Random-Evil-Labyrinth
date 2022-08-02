using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RsaEnc
{
    private static string key = "dofkrfacsrdedofkrfaosrdedofsrfao";

    private static string IV = "zxcvbnmdfrasdfgh";

    internal static string Encrypt(string text)
    {
        byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
        aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
        ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
        crypto.Dispose();
        return Convert.ToBase64String(encrypted);
    }

    internal static string Decrypt(string encrypted)
    {
        byte[] encryptedbytes = Convert.FromBase64String(encrypted);
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
        aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;
        ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] secret = crypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
        crypto.Dispose();
        return System.Text.ASCIIEncoding.ASCII.GetString(secret);
    }
}

public class SaveAndLoadGameData : MonoBehaviour
{
    [SerializeField] private SaveDataGame dataPlayer = new SaveDataGame();
    private string pathToSave;
    [SerializeField]
    internal bool isLoad = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pathToSave = Path.Combine(Application.persistentDataPath, "Data.json");
#else
        pathToSave = Path.Combine(Application.dataPath, "Data.json");
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if(pause) SaveDataPlayer();
    }
#endif
    void OnApplicationQuit()
    {
        SaveDataPlayer();
    }

    internal void LoadPlayerDataInLoadGround(Scrollbar loadingBarGame)
    {
        LoadPlayerData(loadingBarGame);
    }

    void LoadPlayerData(Scrollbar loadingBarGame)
    {
        if (loadingBarGame.size < 1)
        loadingBarGame.size += UnityEngine.Random.Range(0.25f, 0.5f);
        if (!File.Exists(pathToSave))
            CreateNewSaveFile();
        else
        {
            if (!isLoad)
            {
                try
                {
                    string Decode = File.ReadAllText(pathToSave, Encoding.Default).Replace("\n", " ");
                    if (Decode.Length > 0)
                    {
                        var plainTetx = RsaEnc.Decrypt(Decode);
                        File.WriteAllText(pathToSave, plainTetx, Encoding.Default);
                        dataPlayer = JsonUtility.FromJson<SaveDataGame>(File.ReadAllText(pathToSave));
                        Debug.Log("DecryptText: \n" + plainTetx);
                        isLoad = true;
                    }
                    else
                        CreateNewSaveFile();
                }
                catch (System.Exception)
                {
                    LoadPlayerData(loadingBarGame);
                }

            }
        }
    }

    internal int GetCurrentPlayerStartHealth()
    {
        return dataPlayer.CurrentPlayerStartHealth;
    }
    internal void SetCurrentPlayerStartHealth(int set,char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.CurrentPlayerStartHealth += set;
                break;
            case '-':
                dataPlayer.CurrentPlayerStartHealth -= set;
                break;
            case '=':
                dataPlayer.CurrentPlayerStartHealth = set;
                break;
        }

    }
    internal int GetMinDamage()
    {
        return dataPlayer.MinDamage;
    }
    internal void SetMinDamage(int set,char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.MinDamage += set;
                break;
            case '-':
                dataPlayer.MinDamage -= set;
                break;
            case '=':
                dataPlayer.MinDamage = set;
                break;
        }
    }
    internal int GetMaxDamage()
    {
        return dataPlayer.MaxDamage;
    }
    internal void SetMaxDamage(int set, char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.MaxDamage += set;
                break;
            case '-':
                dataPlayer.MaxDamage -= set;
                break;
            case '=':
                dataPlayer.MaxDamage = set;
                break;
        }
    }

    internal int GetCurrentLvLPlayerUpgrade()
    {
        return dataPlayer.CurrentLvLPlayerUpgrade;
    }

    internal void SetCurrentLvLPlayerUpgrade(int set,char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.CurrentLvLPlayerUpgrade += set;
                break;
            case '-':
                dataPlayer.CurrentLvLPlayerUpgrade -= set;
                break;
            case '=':
                dataPlayer.CurrentLvLPlayerUpgrade = set;
                break;
        }
    }

    internal bool GetAdsBuy()
    {
        return dataPlayer.AdsBuy;
    }

    internal void SetAdsBuy(bool set)
    {
        dataPlayer.AdsBuy = set;
    }

    internal int GetLvLMaxComplect()
    {
        return dataPlayer.LvLMaxComplect;
    }

    internal void SetLvLMaxComplect(int set,char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.LvLMaxComplect += set;
                break;
            case '-':
                dataPlayer.LvLMaxComplect -= set;
                break;
            case '=':
                dataPlayer.LvLMaxComplect = set;
                break;
        }

    }

    internal int GetLvLMaxTower()
    {
        return dataPlayer.LvLMaxTower;
    }

    internal void SetLvLMaxTower(int set, char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.LvLMaxTower += set;
                break;
            case '-':
                dataPlayer.LvLMaxTower -= set;
                break;
            case '=':
                dataPlayer.LvLMaxTower = set;
                break;
        }
    }

    internal float GetCurrentMoneyPlayer()
    {
        return dataPlayer.CurrentMoneyPlayer;
    }

    internal void SetCurrentMoneyPlayer(float set, char symbol)
    {
        switch (symbol)
        {
            case '+':
                dataPlayer.CurrentMoneyPlayer += set;
                break;
            case '-':
                dataPlayer.CurrentMoneyPlayer -= set;
                break;
            case '=':
                dataPlayer.CurrentMoneyPlayer = set;
                break;
        }

    }


    private void CreateNewSaveFile()
    {
        isLoad = true;
        File.AppendAllText(pathToSave, "");
    }

    internal void SaveDataPlayer()
    {
        File.WriteAllText(pathToSave, JsonUtility.ToJson(dataPlayer));
        var Encode = File.ReadAllText(pathToSave, Encoding.Default).Replace("\n", " ");
        if (Encode != string.Empty)
        {
            var cypher = RsaEnc.Encrypt(Encode);
            UnityEngine.Debug.Log($"Cypher Text: \n {cypher} \n");
            File.WriteAllText(pathToSave, cypher, Encoding.Default);
        }

    }
}

[System.Serializable]
public class SaveDataGame
{
    [SerializeField] internal int CurrentPlayerStartHealth = 10;
    [SerializeField] internal int MinDamage = 1, MaxDamage = 10;
    [SerializeField] internal int CurrentLvLPlayerUpgrade = 1;
    [SerializeField] internal bool AdsBuy = true;
    [SerializeField] internal int LvLMaxComplect = 1;
    [SerializeField] internal int LvLMaxTower = 1;
    [SerializeField] internal float CurrentMoneyPlayer = 10;
}
