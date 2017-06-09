using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectPhoneShake : MonoBehaviour
{
    [Header("========== 偵測手機震動參數 ==========")]
    public Text DebugText;


    public float accelerometerUpdateInterval = 1.0f / 60.0f;
    public float lowPassKernelWidthInSeconds = 1.0f;
    public float shakeDetectionThreshold = 2.0f;

    public float lowPassFilterFactor;
    private Vector3 lowPassValue;

    private void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    private void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            DebugText.text += "Shake => " + deltaAcceleration.sqrMagnitude;
    }
}
