using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadManager : MonoBehaviour
{
    [Header("========== 上傳的功能 ==========")]
    public string URL = "http://140.118.9.179/";
    private string FinalPath;

    [Header("========== 測試的 UI ==========")]
    public Text Status;

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
                File.Move(FinalPath + OrgFormatIndex + ".png", FinalPath + "Temp/" + NewFormatIndex + ".png");
                ZipFileList.Add(FinalPath + "Temp/" + NewFormatIndex + ".png");
            }
            for (int i = 0; i < StartIndex; i++)
            {
                string OrgFormatIndex = string.Format("{0:0000}", i);
                string NewFormatIndex = string.Format("{0:0000}", HandleIndex);
                File.Move(FinalPath + OrgFormatIndex + ".png", FinalPath + "Temp/" + NewFormatIndex + ".png");
                ZipFileList.Add(FinalPath + "Temp/" + NewFormatIndex + ".png");
            }
        }
        else
        {
            int HandleIndex = 0;
            for (int i = 0; i <= EndIndex; i++)
            {
                string OrgFormatIndex = string.Format("0:0000", i);
                string NewFormatIndex = string.Format("0:0000", HandleIndex);
                File.Move(FinalPath + OrgFormatIndex + ".png", FinalPath + "Temp/" + NewFormatIndex + ".png");
                ZipFileList.Add(FinalPath + "Temp/" + NewFormatIndex + ".png");
            }
        }
        Status.text = "Zip file";

        // 壓縮
        string exportZipPath = FinalPath + "Temp/test.zip";
        ZipUtil.Zip(exportZipPath, ZipFileList.ToArray());
        Status.text = "Zip Complete\n" + Status.text;

        // 定位資料
        float latData = PlayerPrefs.GetFloat("lat");
        float longData = PlayerPrefs.GetFloat("long"); 
        //StartCoroutine(UploadFile());
    }


    //IEnumerator UploadFile()
    //{
    //    // 新的 Web request 機制
    //    List<IMultipartFormSection> FormData = new List<IMultipartFormSection>();
    //    FormData.Add(new MultipartFormDataSection(""))
    //}
}