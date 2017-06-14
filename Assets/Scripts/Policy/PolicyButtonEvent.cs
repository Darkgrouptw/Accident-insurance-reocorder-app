using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PolicyButtonEvent : MonoBehaviour
{
    [Header("========== 輸入的資訊 ==========")]
    public InputField NameField;
    public InputField NameField1;
    public InputField NIDField;
    public InputField CarInfoField;
    public InputField PolicyTypeField;

    public void TestButtonPressEvent()
    {
        string AccountName = PlayerPrefs.GetString("Account");
        if (AccountName == "abc")
        {
            NameField.text = "吳先生";
            NameField1.text = "吳先生";
            NIDField.text = "A123456789";
            CarInfoField.text = "AD1235";
            PolicyTypeField.text = "第三責任險";
        }
        else if(AccountName == "abcd")
        {
            NameField.text = "林先生";
            NameField1.text = "林先生";
            NIDField.text = "A112233445";
            CarInfoField.text = "WW5487";
            PolicyTypeField.text = "第三責任險";
        }
    }

    public void SubmitButtonPressEvent()
    {
        if (NameField.text != "" && NIDField.text != "")
        {
            PlayerPrefs.SetString("Name", NameField.text);
            PlayerPrefs.SetString("Name1", NameField1.text);
            PlayerPrefs.SetString("NID", NIDField.text);
            PlayerPrefs.SetString("CarInfoField", CarInfoField.text);
            PlayerPrefs.SetString("PolicyTypeField", PolicyTypeField.text);

            SceneManager.LoadSceneAsync (2);
        }
    }
}
