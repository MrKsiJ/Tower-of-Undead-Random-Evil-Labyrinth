using UnityEngine;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    const string NameBlackScreenLoadOff = "BlackScreenLoadOff";

    public Scrollbar LoadingScrollBar;
    SaveAndLoadGameData gameData;
    bool isLoadedGame = false;
    [Header("Менюшные объекты.")]
    public GameObject MoneyAnimation, Menu;
    void Start()
    {
        gameData = Camera.main.GetComponent<SaveAndLoadGameData>();
    }
    public void StartLoad()
    {
        if (!isLoadedGame)
        {
            LoadingScrollBar.size = 0;
            gameData.LoadPlayerDataInLoadGround(LoadingScrollBar);
            LoadingScrollBar.size = 1;
            LoadingScrollBar.gameObject.SetActive(false);
            GetComponent<Animator>().Play(NameBlackScreenLoadOff,0,0);
            isLoadedGame = true;
        }
        else
        {
            MoneyAnimation.SetActive(true);
            Menu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    
}
