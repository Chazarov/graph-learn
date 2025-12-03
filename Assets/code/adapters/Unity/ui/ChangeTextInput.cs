using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField numberInput;

    public void Increase()
    {
        if (float.TryParse(numberInput.text, out float num))
        {
            string value = "" + (num + 1);
            numberInput.text = value;
            numberInput.onEndEdit.Invoke(value);
        }
        else
        {
            string value = "" + 0;
            numberInput.text = value;
            numberInput.onEndEdit.Invoke(value);
        }
    }

    public void Reduse()
    {
        if (float.TryParse(numberInput.text, out float num))
        {
            string value = "" + (num - 1);
            numberInput.text = value;
            numberInput.onEndEdit.Invoke(value);
        }
        else
        {
            string value = "" + 0;
            numberInput.text = value;
            numberInput.onEndEdit.Invoke(value);
        }
    }
}
