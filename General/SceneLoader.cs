using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class SceneLoader : MonoBehaviour {

    
    public bool isStartScene;
    [HideInInspector]
    public string sceneName;
    [Tooltip("Name of Main Menu scene")]
    public string mainMenuSceneName;
    public Image loadingBar;

    [Header("Asset Bundle")]
    public string m_sceneName;
    private AssetBundle assetBundle;
    private string assetBundlePath;
    private long serverFileSize;
    
    public Text labelText;
    public string url;

    private void Awake()
    {
        if (!isStartScene)
        {
            sceneName = PlayerPrefs.GetString("GameLevel");
        }else
        {
            sceneName = mainMenuSceneName;
        }
    }

    private void Start()
    {
        StartCoroutine(SceneLoad());       
    }

    IEnumerator SceneLoad()
    {
        
        assetBundlePath = Path.Combine(Application.persistentDataPath, m_sceneName + ".bundle");
        if (IsConnectedToInternet())
        {
            Debug.Log("KONEK TO INTERNET");
            yield return StartCoroutine(CheckAndUpdateAssetBundle());
        }
        else
        {
            Debug.Log("TIDAK KONEK TO INTERNET");
            labelText.text = "Failed Connect to server for update";
            Invoke("LoadAssetBundle", 0f);
            yield break;
        }
    }


    
    IEnumerator CheckAndUpdateAssetBundle() {

        // function return serverFileSize
        yield return StartCoroutine(GetServerFileSize());

        if (File.Exists(assetBundlePath)) {

            long localFileSize = new FileInfo(assetBundlePath).Length;

            if (localFileSize == serverFileSize) {
                Debug.Log("TIDAK PERLU DOWNLOAD");
                yield return StartCoroutine(LoadAssetBundle());
            } else {
                Debug.Log("PERLU DOWNLOAD");
                yield return StartCoroutine(DownloadFiles());
            }
        } else {
            yield return StartCoroutine(DownloadFiles());
        }
    }

    IEnumerator GetServerFileSize() {
        UnityWebRequest request = UnityWebRequest.Head(url);
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
        using (WWW www = new WWW(url))
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
                SceneManager.LoadScene(m_sceneName);
                yield break;
            }

            File.WriteAllBytes(assetBundlePath, www.bytes);
            // PlayButton.SetActive(true);
            // labelText.text = "Game Ready";

            yield return StartCoroutine(LoadAssetBundle());
        }
    }

    IEnumerator LoadAssetBundle()
    {
        Debug.Log("assetBundle : " + assetBundle);

        if (assetBundle != null)
        {
            Debug.Log("Unloading existing asset bundle...");
            assetBundle.Unload(true);
            assetBundle = null;
            Resources.UnloadUnusedAssets();
        }

        byte[] bytes = File.ReadAllBytes(assetBundlePath);
        assetBundle = AssetBundle.LoadFromMemory(bytes);

        Debug.Log("Asset bundle loaded from: " + assetBundlePath);

        AsyncOperation operation = SceneManager.LoadSceneAsync(m_sceneName);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            loadingBar.fillAmount = progress;
            yield return null;
        }
    }


    public bool IsConnectedToInternet() {
        if (Application.internetReachability != NetworkReachability.NotReachable){
            return true;
        }
        else {
            return false;
        }
    }


}

