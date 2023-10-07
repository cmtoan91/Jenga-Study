using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UiStackSelectButton : MonoBehaviour
{
    [SerializeField]
    TMP_Text _text;

    [SerializeField]
    Button button;

    StackHolder _attachedStack;

    public void Init(string name, StackHolder stack)
    {
        _text.text = name;
        _attachedStack = stack;
        button.onClick.AddListener(SelectStack);
    }

    void SelectStack()
    {
        StackManager.Instance.SelectStack(_attachedStack);
    }
}
