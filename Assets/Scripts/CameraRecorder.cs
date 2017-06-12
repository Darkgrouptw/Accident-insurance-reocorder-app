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

    private void Start()
    {
        // 創建目錄
        if (!System.IO.Directory.Exists(FolderName))
            System.IO.Directory.CreateDirectory(FolderName);

        // 創建 Texture
        recorder = new WebCamTexture();
        image.texture = recorder;
        recorder.Play();
    }

    private void Update()
    {

    }
}
