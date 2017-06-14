using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GPSLocationManager : MonoBehaviour
{
    [Header("========== GPS 記錄相關 ==========")]
    public string URL = "http://140.118.9.179/userLocation/";

    private void Start()
    {
        StartCoroutine(GPSRecord());
    }

    private IEnumerator GPSRecord()
    {
        // 開始定位
        Input.location.Start();

        // 等待初始化
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Time Out
        if (maxWait < 1)
        {
            Debug.Log("GPS Location Time out !!");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // latitude             => 緯度 (水平線)
            // longitude            => 經度 (垂直線)
            // altitude             => 海拔多少公尺
            // horizontalAccuracy   => 誤差大概半景多少
            // timestamp            => 時間搓
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            while(true)
            {
                // 設定到背景裡面
                float laitude = Input.location.lastData.latitude;
                float longitude = Input.location.lastData.longitude;
                PlayerPrefs.SetFloat("lat", laitude);
                PlayerPrefs.SetFloat("long", longitude);
				Debug.Log ("Location => " + laitude + "," + longitude);

                // 填寫表單上去
                WWWForm UserLocationData = new WWWForm();
                UserLocationData.AddField("userID", "身分證字號");
                UserLocationData.AddField("lat", laitude.ToString());
                UserLocationData.AddField("lng", longitude.ToString());

                // 送出表單
                UnityWebRequest req = UnityWebRequest.Post(URL, UserLocationData);
                req.Send();

				// 要讓他結束後才跑
				while (!req.isDone) {
					yield return new WaitForSeconds (0.1f);
				};
                Debug.Log(req.downloadHandler.text);

                yield return new WaitForSeconds(20);
            }
        }
    }

    private void OnDisable()
    {
        // 停止定位
        Input.location.Stop();
    }
}
