using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RecorderButtonEvent : MonoBehaviour
{
    [Header("========== Help ==========")]
    public string HelpURL = "http://140.118.9.179/newEvents/help";
    public GameObject HelpPanel;
    public GameObject LoadingTexture;

    public void HelpButtonPressEvent()
    {
        StartCoroutine(HelpCoroutine());
    }

    private IEnumerator HelpCoroutine()
    {
        // 路徑
        string FinalPath = Application.persistentDataPath + "/" + CameraRecorder.FolderName + "/";

        // 更新圖檔的名稱
        if (Directory.Exists(FinalPath + "/Temp"))
            Directory.Delete(FinalPath + "/Temp", true);
        Directory.CreateDirectory(FinalPath + "/Temp");

        // 圖片名稱重新歸檔
        int EndIndex = PlayerPrefs.GetInt("CountSaveIndex");
        bool IsRepeat = PlayerPrefs.GetInt("IsRepeat") == 1 ? true : false;

        // 上傳資訊
        string helpID = PlayerPrefs.GetString("helpID");
        string ID = PlayerPrefs.GetString("NID");
        float latData = PlayerPrefs.GetFloat("lat");
        float longData = PlayerPrefs.GetFloat("long");

        // 新的 Web request 機制
        WWWForm FormData = new WWWForm();
        FormData.AddField("helpID", helpID);
        FormData.AddField("userID", ID);
        FormData.AddField("lat", latData.ToString());
        FormData.AddField("lng", longData.ToString());

        UnityWebRequest req = UnityWebRequest.Post(HelpURL, FormData);
        AsyncOperation requestAsync = req.Send();

        // 產生 Prograss
        while (!requestAsync.isDone)
        {
            Debug.Log("Prograss => " + req.uploadProgress.ToString());
            yield return new WaitForSeconds(1);
        }
        Debug.Log(req.downloadHandler.text);

        // 關閉視窗
        HelpPanel.SetActive(false);
        LoadingTexture.SetActive(false);
    }

    public void RecordToConvert()
    {
        SceneManager.LoadSceneAsync(3);
	}
}
