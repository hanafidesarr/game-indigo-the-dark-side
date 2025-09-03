using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour {

    [Header("Settings")]
    [Tooltip("Slider to controll volume")]
    public Slider volumeSlider;
    [Tooltip("Slider to controll sensitivity")]
    public Slider sensitivitySlider;
    public GameObject m_lockImageGameObject;

    public Button m_playButton;
    public Button m_backButton;
    public Button m_prevLevel;
    public Button m_nextLevel;
    public Button m_prevEnemy;
    public Button m_nextEnemy;
    public Button m_prevDifficulty;
    public Button m_nextDifficulty;
    
    public GameObject m_startGameWindow;
    public GameObject m_setupGameWindow;
    public GameObject m_returnedFormGameWindow;
    public Text m_plus_Xp_text;
    public Text m_collectedPicturesCountText;

    [Header("Game Setup")]
    public string m_loadingSceneName;
    public LevelSetup[] m_levels;
    public EnemySetup[] m_enemys;
    public DifficultSetup[] m_difficults;
    int levelID = 0;
    int enemyID = 0;
    int difficultID = 0;

    [Header("XP Settings")]
    public Text m_xp_text;
    private static int m_XP;

    [Header("Level Settings")]
    public Text m_levelNameText;
    public Text m_levelDescriptionText;
    public GameObject m_buyLevelIconGO;
    public Text m_levelPriceText;
    public GameObject m_level_NotEnoughXP;
    bool m_selectedLevelIsLocked;


    [Header("Enemy Settings")]
    public Text m_enemyNameText;
    public Text m_enemyDescriptionText;
    public GameObject m_buyEnemyIconGO;
    public Text m_enemyPriceText;
    public GameObject m_enemy_NotEnoughXP;
    bool m_selectedEnemyIsLocked;


    [Header("Difficult Settings")]
    public Text m_difficultNameText;
    public Text m_difficultDescriptionText;


    [Header("Asset Bundle")]
    private AssetBundle assetBundle;
    private string assetBundlePath;
    private long serverFileSize;

    public Text labelText;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("ReturnFromGame"))
        {
            int m_RFG = PlayerPrefs.GetInt("ReturnFromGame");
            if (m_RFG == 1)
            {

                m_returnedFormGameWindow.SetActive(true);
                m_startGameWindow.SetActive(false);

                if (PlayerPrefs.HasKey("PLUS_XP"))
                {
                 
                    int m_plus_XP = PlayerPrefs.GetInt("PLUS_XP");
                    m_XP += m_plus_XP;
                    PlayerPrefs.SetInt("XP", m_XP);
                    m_plus_Xp_text.text = "+ " + m_plus_XP.ToString();

                }else
                {
                    m_XP += 0;
                    PlayerPrefs.SetInt("XP", m_XP);
                    m_plus_Xp_text.text = "+ " + 0;
                }

                if(PlayerPrefs.HasKey("CollectedPictures"))
                {
                   m_collectedPicturesCountText.text = "You Collect " + PlayerPrefs.GetInt("CollectedPictures") + " Pictures!";
                }

                PlayerPrefs.SetInt("ReturnFromGame", 0);
                PlayerPrefs.SetInt("CollectedPictures", 0);
            }
        }


        if (PlayerPrefs.HasKey("XP"))
        {
            m_XP = PlayerPrefs.GetInt("XP");
        }else
        {
            m_XP = 0;
        }
        m_xp_text.text = m_XP.ToString() + " XP";


        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            AudioListener.volume = 1f;
            volumeSlider.value = 1f;
        }

        if (PlayerPrefs.HasKey("Sensitivity"))
        {         
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            sensitivitySlider.value = sensitivitySlider.maxValue / 2f;
        }

        GameSetup();
    }

    private void GameSetup()
    {
        m_levelNameText.text = m_levels[levelID].name;
        m_levelDescriptionText.text = m_levels[levelID].m_description;

        m_enemyNameText.text = m_enemys[enemyID].name;
        m_enemyDescriptionText.text = m_enemys[enemyID].m_description;

        m_difficultNameText.text = m_difficults[difficultID].name;
        m_difficultDescriptionText.text = m_difficults[difficultID].m_description;

        m_levels[levelID].m_levelShowGameobject.SetActive(true);

        for (int i = 0; i < m_enemys[enemyID].m_enemyShowGameobject.Length; i++)
        {
            m_enemys[enemyID].m_enemyShowGameobject[i].SetActive(true);
        }


        UpdateGameInfo();
    }

    public void ChangeLevel(int value)
    {
        

        if (value > 0)
        {
            if(levelID < m_levels.Length -1)
            {
                m_levels[levelID].m_levelShowGameobject.SetActive(false);
                levelID += 1;
            }
        }

        if (value < 0)
        {
            if (levelID > 0)
            {
                m_levels[levelID].m_levelShowGameobject.SetActive(false);
                levelID -= 1;
            }
        }


        m_levelNameText.text = m_levels[levelID].name;
        m_levelDescriptionText.text = m_levels[levelID].m_description;

        m_levels[levelID].m_levelShowGameobject.SetActive(true);

        UpdateGameInfo();
    }

    public void ChangeEnemy(int value)
    {

        if (value > 0)
        {
            if (enemyID < m_enemys.Length - 1)
            {
                for (int i = 0; i < m_enemys[enemyID].m_enemyShowGameobject.Length; i++)
                {
                    m_enemys[enemyID].m_enemyShowGameobject[i].SetActive(false);
                }
                enemyID += 1;
            }
        }

        if (value < 0)
        {
            if (enemyID > 0)
            {
                for (int i = 0; i < m_enemys[enemyID].m_enemyShowGameobject.Length; i++)
                {
                    m_enemys[enemyID].m_enemyShowGameobject[i].SetActive(false);
                }
                enemyID -= 1;
            }
        }

        for (int i = 0; i < m_enemys[enemyID].m_enemyShowGameobject.Length; i++)
        {
            m_enemys[enemyID].m_enemyShowGameobject[i].SetActive(true);
        }
        m_enemyNameText.text = m_enemys[enemyID].name;
        m_enemyDescriptionText.text = m_enemys[enemyID].m_description;
        UpdateGameInfo();
    }

    public void ChangeDifficulty(int value)
    {


        if (value > 0)
        {
            if (difficultID < m_difficults.Length - 1)
            {
                difficultID += 1;
            }
        }

        if (value < 0)
        {
            if (difficultID > 0)
            {
                difficultID -= 1;
            }
        }

        m_difficultNameText.text = m_difficults[difficultID].name;
        m_difficultDescriptionText.text = m_difficults[difficultID].m_description;

    }

    public void BuyMode(int modeID) /// buy 1 - level, 2 - enemy
    {
        if(modeID == 1)
        {
            if(m_selectedLevelIsLocked)
            {
                if(m_XP >= m_levels[levelID].m_price)
                {
                    m_XP -= m_levels[levelID].m_price;
                    m_levels[levelID].m_lockState = 1;
                    PlayerPrefs.SetInt(m_levels[levelID].m_sceneName + "LockState", 1);
                    PlayerPrefs.SetInt("XP",m_XP);
                    m_setupGameWindow.SetActive(true);
                    UpdateGameInfo();
                }else
                {
                    m_level_NotEnoughXP.SetActive(true);
                }
            }
        }

        if (modeID == 2)
        {
            if (m_selectedEnemyIsLocked)
            {
                if (m_XP >= m_enemys[enemyID].m_price)
                {
                    m_XP -= m_enemys[enemyID].m_price;
                    m_enemys[enemyID].m_lockState = 1;
                    PlayerPrefs.SetInt("Enemy" + m_enemys[enemyID].m_enemyModeId + "LockState", 1);
                    PlayerPrefs.SetInt("XP", m_XP);
                    m_setupGameWindow.SetActive(true);
                    UpdateGameInfo();
                }
                else
                {
                    m_enemy_NotEnoughXP.SetActive(true);
                }
            }
        }
    }

    private void UpdateGameInfo()
    {
        int m_levelLockState = 0;
        int m_enemyLockState = 0;

        m_xp_text.text = m_XP.ToString() + " XP";

        if (PlayerPrefs.HasKey(m_levels[levelID].m_sceneName + "LockState"))
        {
            m_levelLockState = PlayerPrefs.GetInt(m_levels[levelID].m_sceneName + "LockState");
            m_levels[levelID].m_lockState = m_levelLockState;

            switch (m_levelLockState)
            {
                case 0:
                    m_buyLevelIconGO.SetActive(true);
                    m_selectedLevelIsLocked = true;
                    break;
                case 1:
                    m_buyLevelIconGO.SetActive(false);
                    m_selectedLevelIsLocked = false;
                    break;
            }
        }
        else
        {
            if (m_levels[levelID].m_lockByDefault)
            {
                m_selectedLevelIsLocked = true;
                m_buyLevelIconGO.SetActive(true);
            }
            else
            {
                m_buyLevelIconGO.SetActive(false);
                m_selectedLevelIsLocked = false;
            }
        }



        if (PlayerPrefs.HasKey("Enemy" + m_enemys[enemyID].m_enemyModeId + "LockState"))
        {
            m_enemyLockState = PlayerPrefs.GetInt("Enemy" + m_enemys[enemyID].m_enemyModeId + "LockState");
            m_enemys[enemyID].m_lockState = m_enemyLockState;
            switch (m_enemyLockState)
            {
                case 0:
                    m_buyEnemyIconGO.SetActive(true);
                    m_selectedEnemyIsLocked = true;
                    break;
                case 1:
                    m_buyEnemyIconGO.SetActive(false);
                    m_selectedEnemyIsLocked = false;
                    break;
            }
        }
        else
        {
            if (m_enemys[enemyID].m_lockByDefault)
            {
                m_buyEnemyIconGO.SetActive(true);
                m_selectedEnemyIsLocked = true;
            }
            else
            {
                m_selectedEnemyIsLocked = false;
                m_buyEnemyIconGO.SetActive(false);
            }
        }

        if (m_selectedLevelIsLocked || m_selectedEnemyIsLocked)
        {
            m_lockImageGameObject.SetActive(true);
            m_playButton.interactable = false;
        }else
        {
            m_lockImageGameObject.SetActive(false);
            m_playButton.interactable = true;
        }

        m_levelPriceText.text = m_levels[levelID].m_price.ToString();
        m_enemyPriceText.text = m_enemys[enemyID].m_price.ToString();
    }

    public void ApplyConfig()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
     
    }

    public void ClearAllSaves()
    {
        m_XP = 0;
        m_xp_text.text = m_XP.ToString();
        PlayerPrefs.DeleteAll();
        UpdateGameInfo();
    }

    // public void StartGame()
    // {
    //     if (!m_selectedEnemyIsLocked && !m_selectedLevelIsLocked)
    //     {
    //         PlayerPrefs.SetInt("EnemyMode", m_enemys[enemyID].m_enemyModeId);
    //         PlayerPrefs.SetString("GameLevel", m_levels[levelID].m_sceneName);
    //         PlayerPrefs.SetInt("GameDifficulty", m_difficults[difficultID].m_difficultId);

    //         // SceneManager.LoadScene(m_loadingSceneName, LoadSceneMode.Single);
    //     }
    // }

    public void StartGame()
    {
        labelText.text = "Prepare...";
        if (!m_selectedEnemyIsLocked && !m_selectedLevelIsLocked)
        {

            m_playButton.interactable = false;
            m_backButton.interactable = false;
            m_prevLevel.interactable = false;
            m_nextLevel.interactable = false;
            m_prevEnemy.interactable = false;
            m_nextEnemy.interactable = false;
            m_prevDifficulty.interactable = false;
            m_nextDifficulty.interactable = false;

            PlayerPrefs.SetInt("EnemyMode", m_enemys[enemyID].m_enemyModeId);
            PlayerPrefs.SetString("GameLevel", m_levels[levelID].m_sceneName);
            PlayerPrefs.SetInt("GameDifficulty", m_difficults[difficultID].m_difficultId);

            // Set path here before starting coroutine
            assetBundlePath = Path.Combine(Application.persistentDataPath, m_levels[levelID].m_sceneName + ".bundle");

            Debug.Log("Scene sudah ada di Build Settings → langsung load === " + m_levels[levelID].m_sceneName);
            if (SceneExistsInBuild(m_levels[levelID].m_sceneName))
            {
                Debug.Log("Scene sudah ada di Build Settings → langsung load");
                LoadNextScene();
            }
            else if (IsConnectedToInternet())
            {
                Debug.Log("KONEK TO INTERNET");
                StartCoroutine(CheckAndUpdateAssetBundle());
            }
            else
            {
                Debug.Log("TIDAK KONEK TO INTERNET");
                labelText.text = "Failed Connect to server for update";
                m_playButton.interactable = true;
                m_backButton.interactable = true;
                m_prevLevel.interactable = true;
                m_nextLevel.interactable = true;
                m_prevEnemy.interactable = true;
                m_nextEnemy.interactable = true;
                m_prevDifficulty.interactable = true;
                m_nextDifficulty.interactable = true;
                Invoke("LoadAssetBundle", 0f);
            }
        }
    }


    IEnumerator CheckAndUpdateAssetBundle() {

        // function return serverFileSize
        yield return StartCoroutine(GetServerFileSize());

        if (File.Exists(assetBundlePath)) {

            long localFileSize = new FileInfo(assetBundlePath).Length;

            if (localFileSize == serverFileSize) {
                Debug.Log("TIDAK PERLU DOWNLOAD");
                LoadAssetBundle();
            } else {
                Debug.Log("PERLU DOWNLOAD");
                yield return StartCoroutine(DownloadFiles());
            }
        } else {
            yield return StartCoroutine(DownloadFiles());
        }
    }

    IEnumerator GetServerFileSize() {
        UnityWebRequest request = UnityWebRequest.Head(m_levels[levelID].url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Failed to check file size: " + request.error);
            yield break;
        }

        string contentLength = request.GetResponseHeader("Content-Length");

        if (!string.IsNullOrEmpty(contentLength)) {
            serverFileSize = Convert.ToInt64(contentLength);
        }
    }



    IEnumerator DownloadFiles()
    {
        using (WWW www = new WWW(m_levels[levelID].url))
        {
            Debug.Log("Downloading asset bundle...");
            while (!www.isDone) {
                Debug.Log("Download progress: " + Mathf.RoundToInt(www.progress * 100) + "%");
                labelText.text = "Checking Assets: " + Mathf.RoundToInt(www.progress * 100) + "%";
                yield return null;
            }

            yield return www;
            if (!string.IsNullOrEmpty(www.error)) {

                Debug.LogError("EWEUH COY");
                Debug.LogError(www.error);
                SceneManager.LoadScene(m_levels[levelID].m_sceneName);
                yield break;
            }

            File.WriteAllBytes(assetBundlePath, www.bytes);
            // PlayButton.SetActive(true);
            // labelText.text = "Game Ready";

            LoadAssetBundle();
        }
    }

    void LoadAssetBundle()
    {
        Debug.Log("assetBundle : " + assetBundle);

        // Jika sebelumnya sudah ada bundle, unload terlebih dahulu
        if (assetBundle != null)
        {
            Debug.Log("Unloading existing asset bundle...");
            assetBundle.Unload(true);
            assetBundle = null;
            Resources.UnloadUnusedAssets(); // membersihkan cache memory
        }

        byte[] bytes = File.ReadAllBytes(assetBundlePath);
        assetBundle = AssetBundle.LoadFromMemory(bytes);

        Debug.Log("Asset bundle loaded from: " + assetBundlePath);

        // Setelah bundle dimuat, langsung load scene
        // SceneManager.LoadScene(m_levels[levelID].m_sceneName);
        ShowInterstitialOrLoadScene();

        Debug.Log("Clicked on button to play scene");
    }


    void ShowInterstitialOrLoadScene()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            Debug.Log("Interstitial READY - Showing...");
            IronSource.Agent.showInterstitial();
        }
        else
        {
            Debug.Log("Interstitial NOT ready - Loading scene directly");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        Debug.Log("Loading scene: " + m_levels[levelID].m_sceneName);
        SceneManager.LoadScene(m_levels[levelID].m_sceneName);
    }

    void OnEnable()
    {
        IronSourceInterstitialEvents.onAdClosedEvent += OnInterstitialClosed;
    }
    void OnDisable()
    {
        IronSourceInterstitialEvents.onAdClosedEvent -= OnInterstitialClosed;
    }

    void OnInterstitialClosed(IronSourceAdInfo adInfo)
    {
        Debug.Log("Interstitial closed - Now load scene");
        LoadNextScene();

        // Load interstitial baru untuk nanti
        IronSource.Agent.loadInterstitial();
    }

    // IEnumerator PreloadScenes()
    //      {

    //              // Load the scene in the background
    //              AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_levels[levelID].m_sceneName);

    //              // Wait until the asynchronous scene fully loads
    //              while (!asyncLoad.isDone)
    //              {
    //                      Debug.Log("HOHOHOHOO");
    //                      yield return null;
    //              }
    //              // All scenes are preloaded, you can start the game now
    //              Debug.Log("Preloading complete. Game can start.");
    //      }

    public bool IsConnectedToInternet() {
        if (Application.internetReachability != NetworkReachability.NotReachable){
            return true;
        }
        else {
            return false;
        }
    }

    public void QuitGame()
    {      
        Application.Quit();
    }

    private bool SceneExistsInBuild(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

   
}

