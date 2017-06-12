using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRecorder : MonoBehaviour
{
    [Header("========== 錄影相關 ==========")]
    public string FolderName = "Recorder";
    private WebCamTexture recorder;
    public RawImage image;
    private int CountSaveIndex = 0;

    private void Start()
    {
        // 創建目錄
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + FolderName))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + FolderName);
        Debug.Log(Application.persistentDataPath + "/" + FolderName);

        // 創建 Texture
        recorder = new WebCamTexture();
        image.texture = recorder;
        recorder.Play();
    }

    private void Update()
    {
        //while(recorder.isPlaying)
        //{
        //    //recorder
        //    tex
        //    CountSaveIndex++;
        //}
    }
}
