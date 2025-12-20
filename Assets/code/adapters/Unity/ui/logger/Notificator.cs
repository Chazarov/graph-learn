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
                RunNotify("���������� ��������� ��������� ����������� ����� � �����. ���� ����� ����������� �����.", "W");
            }
            if(e is ImpossibleToSetGraphDirection)
            {
                RunNotify("���������� ��������� ��������� ������������ ����� � �����,��� ��� ��� ��������� ����������� �����. ��� ��� ������ ���� ����� �������������� ����������� �����.", "W");
            }
            if(e is NotEulerianGraphException)
            {
                RunNotify("Невозможно выполнить алгоритм. Граф не является эйлеровым", "W");
            }
            if(e is NotHamiltonianGraphException)
            {
                RunNotify("Невозможно выполнить алгоритм. Граф не содержит гамильтонов цикл", "W");
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
