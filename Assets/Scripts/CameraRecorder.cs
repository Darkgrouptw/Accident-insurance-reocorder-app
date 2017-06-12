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
	public int frameNumber = 25;
	public int MaxPictureCount = 10000;
    private int CountSaveIndex = 0;

    private void Start()
    {
        // 創建目錄
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/" + FolderName))
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + FolderName);
        FinalPath = Application.persistentDataPath + "/" + FolderName + "/";
        Debug.Log("SavePath => " + FinalPath);

        // 創建 Texture
		recorder = new WebCamTexture(Screen.height / 4, Screen.width / 4);
        image.texture = recorder;
        recorder.Play();

		StartCoroutine(RecordEvent());
    }

	private IEnumerator RecordEvent()
	{
		while (true) 
		{
			System.GC.Collect();
			Texture2D textureTemp = new Texture2D(recorder.width, recorder.height);
			textureTemp.SetPixels (recorder.GetPixels ());
			textureTemp.Apply ();

			byte[] dataArray = textureTemp.EncodeToPNG();
			System.IO.File.WriteAllBytes(FinalPath + "/" + CountSaveIndex + ".png", dataArray);

			CountSaveIndex++;
			if (CountSaveIndex > MaxPictureCount)
				CountSaveIndex = 0;

			yield return new WaitForSeconds (1.0f / frameNumber);
		}

	}
}
