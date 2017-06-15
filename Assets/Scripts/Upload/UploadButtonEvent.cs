using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UploadButtonEvent : MonoBehaviour
{
    public GameObject[] PeopleButtonArray;
    private bool[] PressBool;
    public GameObject FinishBlock;
    public void Start()
    {
        PressBool = new bool[PeopleButtonArray.Length];
        for (int i = 0; i < PressBool.Length; i++)
            PressBool[i] = false;
    }
    public void ButtonCheck(GameObject DownImage)
    {
        int number = int.Parse(DownImage.GetComponentInParent<Transform>().gameObject.name.Replace("Check Step ", ""));
        DownImage.SetActive(!DownImage.activeInHierarchy);
        PressBool[number - 1] = DownImage.activeInHierarchy;
    }

    public void CheckIfAllTrue()
    {
        for (int i = 0; i < PressBool.Length; i++)
            if (!PressBool[i])
                return;
        FinishBlock.SetActive(true);
    }
}
