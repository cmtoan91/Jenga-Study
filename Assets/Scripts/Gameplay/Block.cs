using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    TMP_Text[] _texts;

    [SerializeField]
    Rigidbody _rb;

    public BlockType Type;
    public BlockData Data;
    public Vector3 BlockOriginalPosition { get; set; }
    public Quaternion BlockOriginalRotation { get; set; }
    
    public void Init(BlockData data, Vector3 orgPos, Quaternion rotation)
    {
        Type = (BlockType)data.mastery;
        Data = data;
        BlockOriginalPosition = orgPos;
        BlockOriginalRotation = rotation;

        if (_renderer != null)
        {
           if(StackManager.Instance.MaterialLibrary.TryGetValue(Type, out var mat))
           {
                _renderer.material = mat;
           }
        }
        _rb.isKinematic = true;
        string word = "";
        switch(Type)
        {
            case BlockType.Glass:
                word = "Not Learned";
                break;
            case BlockType.Stone:
                word = "Mastered";
                break;
            case BlockType.Wood:
                word = "Learned";
                break;
        }
        foreach(TMP_Text text in _texts)
        {
            text.text = word;
        }
    }

    public void ResetBlock()
    {
        transform.position = BlockOriginalPosition;
        transform.rotation = BlockOriginalRotation;
        _rb.isKinematic = true;
    }

    public void TriggerPhysic()
    {
        _rb.isKinematic = false;
    }

    public void OnSelect(bool isSelected)
    {
        if (isSelected)
            _renderer.material.color = Color.red;
        else
            _renderer.material.color = Color.white;
    }
}

public enum BlockType
{
    Glass, Wood, Stone
}