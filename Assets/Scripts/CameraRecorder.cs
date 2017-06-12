using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraRecorder : MonoBehaviour
{
    [Header("========== 錄影相關 ==========")]
    public string FolderName = "Recorder";          // 資料夾的
    private string FinalPath;                       // 最後存檔的路徑
	public GameObject Panel;						// 警告視窗

    private WebCamTexture recorder;					// 錄影的貼圖

	public RawImage image;							// 顯示的圖片
	public int frameNumber = 25;					// 每一個 Frame 的數目
	public int MaxPictureCount = 10000;				// 存最大的上限

	private bool IsRepeat = false;
    private int CountSaveIndex = 0;

	private Coroutine RecordCoroutine;
    private void Start()
	{
        // 創建目錄
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/" + FolderName))
			System.IO.Directory.Delete(Application.persistentDataPath + "/" + FolderName, true);
		System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + FolderName);
		FinalPath = Application.persistentDataPath + "/" + FolderName + "/";
        Debug.Log("SavePath => " + FinalPath);

        // 創建 Texture
		recorder = new WebCamTexture(Screen.height / 4, Screen.width / 4);
        image.texture = recorder;
        recorder.Play();

		RecordCoroutine = StartCoroutine(RecordEvent());
    }

	private IEnumerator RecordEvent()
	{
		while (true) 
		{
			// 清空記憶體
			if(CountSaveIndex % frameNumber == 0)
				System.GC.Collect();
			
			Texture2D textureTemp = new Texture2D(recorder.width, recorder.height);
			textureTemp.SetPixels (recorder.GetPixels ());
			textureTemp.Apply ();

			byte[] dataArray = textureTemp.EncodeToPNG();
			System.IO.File.WriteAllBytes(FinalPath + "/" + CountSaveIndex + ".png", dataArray);

			CountSaveIndex++;
			if (CountSaveIndex > MaxPictureCount) 
			{
				IsRepeat = true;
				CountSaveIndex = 0;
			}

			// 原來這條就解決了ＸＤ
			Destroy (textureTemp);
			yield return new WaitForSeconds (1.0f / frameNumber);
		}
	}

	public void StopRecord()
	{
		if (recorder.isPlaying) 
		{
			Panel.SetActive (true);
			//StopCoroutine (RecordCoroutine);
			StopCoroutine (RecordEvent ());
		}
	}

}
