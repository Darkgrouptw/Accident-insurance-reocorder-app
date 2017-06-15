using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadManager : MonoBehaviour
{
    [Header("========== 上傳的功能 ==========")]
    public string URL = "http://140.118.9.179/newEvents/";
    private string FinalPath;

    private void Start()
    {
        // 路徑
        FinalPath = Application.persistentDataPath + "/" + CameraRecorder.FolderName + "/";

        // 更新圖檔的名稱
        if (Directory.Exists(FinalPath + "/Temp"))
            Directory.Delete(FinalPath + "/Temp", true);
        Directory.CreateDirectory(FinalPath + "/Temp");

        // 圖片名稱重新歸檔
        int EndIndex = PlayerPrefs.GetInt("CountSaveIndex");
        bool IsRepeat = PlayerPrefs.GetInt("IsRepeat") == 1 ? true: false;
        int StartIndex = 0;

        // 壓縮的 zip 的 list
        List<string> ZipFileList = new List<string>();

        if(IsRepeat)
        {
            StartIndex = (EndIndex + 1) % CameraRecorder.MaxPictureCount;
            
            // 移動位置
            int HandleIndex = 0;
            for (int i = StartIndex; i < CameraRecorder.MaxPictureCount; i++)
            {
                string OrgFormatIndex = string.Format("{0:0000}", i);
                string NewFormatIndex = string.Format("{0:0000}", HandleIndex);
				File.Move(FinalPath + OrgFormatIndex + ".jpg", FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				ZipFileList.Add (FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				HandleIndex++;
            }
            for (int i = 0; i < StartIndex; i++)
            {
                string OrgFormatIndex = string.Format("{0:0000}", i);
                string NewFormatIndex = string.Format("{0:0000}", HandleIndex);
				File.Move(FinalPath + OrgFormatIndex + ".jpg", FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				ZipFileList.Add(FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				HandleIndex++;
            }
        }
        else
        {
            int HandleIndex = 0;
            for (int i = 0; i < EndIndex; i++)
            {
				string OrgFormatIndex = string.Format("{0:0000}", i);
				string NewFormatIndex = string.Format("{0:0000}", HandleIndex);
				File.Move(FinalPath + OrgFormatIndex + ".jpg", FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				ZipFileList.Add(FinalPath + "Temp/" + NewFormatIndex + ".jpg");
				HandleIndex++;
            }
        }

        // 壓縮
        string exportZipPath = FinalPath + "Temp/Video.zip";
        ZipUtil.Zip(exportZipPath, ZipFileList.ToArray());

        // 產生保單的 txt
        string PolicyData = "";
        string PolicyFileName = FinalPath + "Temp/policy.txt";
        PolicyData += "被保人：" + PlayerPrefs.GetString("Name") + "\n";
        PolicyData += "要保人：" + PlayerPrefs.GetString("Name1") + "\n";
        PolicyData += "身分證號：" + PlayerPrefs.GetString("NID") + "\n";
        PolicyData += "車牌：" + PlayerPrefs.GetString("CarInfoField") + "\n";
        PolicyData += "險種：" + PlayerPrefs.GetString("PolicyTypeField") + "\n";
        File.WriteAllText(PolicyFileName, PolicyData);

        // 上傳資訊
        string ID = PlayerPrefs.GetString("NID");
        float latData = PlayerPrefs.GetFloat("lat");
        float longData = PlayerPrefs.GetFloat("long");
        StartCoroutine(UploadFile(ID, latData, longData, exportZipPath, PolicyFileName));
    }


    IEnumerator UploadFile(string ID, float latData, float longData, string ZipFileName, string PolicyFileName)
    {
        byte[] ZipFileData = File.ReadAllBytes(ZipFileName);
        byte[] PolicyData = File.ReadAllBytes(PolicyFileName);
		Debug.Log ("Size =>" + ZipFileData.Length);

        // 新的 Web request 機制
        WWWForm FormData = new WWWForm();
        FormData.AddField("userID", ID);
        FormData.AddField("lat", latData.ToString());
        FormData.AddField("lng", longData.ToString());
        FormData.AddBinaryData("Video.zip", ZipFileData);
        FormData.AddBinaryData("PolicyData.txt", PolicyData);

        UnityWebRequest req = UnityWebRequest.Post(URL, FormData);
		AsyncOperation requestAsync = req.Send();

		// 產生 Prograss
		while (!requestAsync.isDone) 
		{
			Debug.Log ("Prograss => " + req.uploadProgress.ToString());
			yield return new WaitForSeconds (1);
		}
        Debug.Log(req.downloadHandler.text);
    }
}