using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RootErrorsWarningsHandler", menuName = "Events/RootErrorsWarningsHandler")]
public class RootExceptionHandler : ScriptableObject
{

    public UnityEvent<Exception> PublishError;  

}