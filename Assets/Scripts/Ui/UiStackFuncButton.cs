using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UiStackFuncButton : MonoBehaviour
{
    [SerializeField]
    TMP_Text _text;

    [SerializeField]
    Button button;

    public void Init(string name, Action func)
    {
        _text.text = name;
        button.onClick.AddListener(() => func());
    }
}
