using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLocationManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 先確定定位能不能用
        if (!Input.location.isEnabledByUser)
            yield break;

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
                PlayerPrefs.SetFloat("lat", Input.location.lastData.latitude);
                PlayerPrefs.SetFloat("long", Input.location.lastData.longitude);
                yield return new WaitForSeconds(5);
            }
        }
    }

    private void OnDisable()
    {
        // 停止定位
        Input.location.Stop();
    }
}
