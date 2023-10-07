using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackHolder: MonoBehaviour
{
    public string StackName;

    [SerializeField]
    TMP_Text _text;

    [SerializeField]
    float _blockWidth = 2.5f;
    [SerializeField]
    float _blockHeight = 1;

    Dictionary<BlockType, List<Block>> _categorizedBlocks = new Dictionary<BlockType, List<Block>>();
    public void Init(string name)
    {
        StackName = name;
        if (_text != null) _text.text = name;
    }

    public void BuildStack(Block blockPrefab, List<BlockData> blockData)
    {
        int currentLevelIdx = 0;
        Vector3 right = Vector3.right;
        int j = 0;
        for (int i = 0; i < blockData.Count; i++)
        {
            Block block = Instantiate(blockPrefab, transform);
            block.transform.localPosition =  (j - 1) * right * _blockWidth + currentLevelIdx * Vector3.up * _blockHeight;
            block.transform.transform.forward = right;
            j++;
            block.Init(blockData[i], block.transform.position, block.transform.rotation);
            SaveBlock(block);

            if ((i + 1) % 3 == 0)
            {
                j = 0;
                right = Vector3.Cross(Vector3.up, right);
                currentLevelIdx++;
            }
        }
    }

    void SaveBlock(Block block)
    {
        if (!_categorizedBlocks.ContainsKey(block.Type))
        {
            _categorizedBlocks[block.Type] = new List<Block>();
        }
        _categorizedBlocks[block.Type].Add(block);
    }

    public void TestStack()
    {
        if(_categorizedBlocks.TryGetValue(BlockType.Glass, out var glassBlocks))
        {
            foreach(Block block in glassBlocks)
            {
                block.gameObject.SetActive(false);
            }
        }

        if (_categorizedBlocks.TryGetValue(BlockType.Wood, out var woodBlocks))
        {
            foreach (Block block in woodBlocks)
            {
                block.TriggerPhysic();
            }
        }

        if (_categorizedBlocks.TryGetValue(BlockType.Stone, out var stoneBlocks))
        {
            foreach (Block block in stoneBlocks)
            {
                block.TriggerPhysic();
            }
        }
    }

    public void Reset()
    {
        foreach(List<Block> blocks in _categorizedBlocks.Values)
        {
            foreach (Block block in blocks)
            {
                block.gameObject.SetActive(true);
                block.ResetBlock();
            }
        }
    }
}

