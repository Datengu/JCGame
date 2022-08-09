using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageHUD : MonoBehaviour
{
    public TextMeshProUGUI message;

    public void SetMessage(string text)
    {
        message.SetText(text);
    }
}
