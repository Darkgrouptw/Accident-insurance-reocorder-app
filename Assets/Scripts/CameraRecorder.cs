using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRecorder : MonoBehaviour
{
    [Header("========== 錄影相關 ==========")]
    public string FolderName = "Recorder";          // 資料夾的
    private string FinalPath;                       // 最後存檔的路徑

    private WebCamTexture recorder;
    public RawImage image;
    private int CountSaveIndex = 0;

    private void Start()
    {
        // 創建目錄
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + FolderName))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + FolderName);
        FinalPath = Application.persistentDataPath + "/" + FolderName + "/";
        Debug.Log("SavePath => " + FinalPath);

        // 創建 Texture
        recorder = new WebCamTexture();
        image.texture = recorder;
        recorder.Play();
    }

    private void Update()
    {
        while(recorder.isPlaying)
        {
            // 錄影
            Texture2D textureTemp = (Texture2D)image.texture;
            byte[] dataArray = textureTemp.EncodeToPNG();
            System.IO.File.WriteAllBytes(FinalPath + "/" + CountSaveIndex + ".png", dataArray);

            CountSaveIndex++;
        }
    }
}
