using GraphMaster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class Notificator : MonoBehaviour
{
    [SerializeField] Animator warningNotifyAnimator;
    [SerializeField] Animator errorNotifyAnimator;

    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] TextMeshProUGUI errorText;

    [SerializeField] RootExceptionHandler rootExceptionHandler;

   
    private void ProcessException(Exception e)
    {
        if(e is GraphMasterException)
        {
            if(e is ImpossibleToSetGraphParralel)
            {
                RunNotify("Невозможно отключить поддержку паралельных ребер в графе. Граф имеет паралельные ребра.", "W");
            }
            if(e is ImpossibleToSetGraphDirection)
            {
                RunNotify("Невозможно отключить поддержку направленных ребер в графе,при том что запрещены паралельные ребра. Так как сейчас граф имеет ненаправленные паралельные ребра.", "W");
            }
            if(e is NotEulerianGraphException)
            {
                RunNotify("Невозможно выполнить алгоритм. Граф не является Эйлеровым", "W");
            }
        }
        else
        {
            RunNotify(e.Message, "E");
        }
    }

    private void RunNotify(string text, string type)
    {
        if(type.ToUpper() == "W" ||  type.ToUpper() == "WARNING")
        {
            RunNotify(warningNotifyAnimator, warningText, text);
        }
        else if(type.ToUpper() == "E" || type.ToUpper() == "ERROR")
        {
            RunNotify(errorNotifyAnimator, errorText, text);
        }
    }

    private void RunNotify(Animator anim, TextMeshProUGUI text, string notifyText)
    {
        text.text = notifyText;
        anim.SetTrigger("Run");
    }
    private void OnEnable()
    {
        rootExceptionHandler.PublishError.AddListener(ProcessException);
    }

    private void OnDisable()
    {
        rootExceptionHandler.PublishError.RemoveListener(ProcessException);
    }

    
}
