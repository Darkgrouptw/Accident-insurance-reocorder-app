using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UploadButtonEvent : MonoBehaviour
{
    public void ButtonCheck(GameObject DownImage)
    {
        DownImage.SetActive(!DownImage.activeInHierarchy);
    }
}
