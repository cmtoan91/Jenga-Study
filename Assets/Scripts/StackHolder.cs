using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHolder: MonoBehaviour
{
    public string StackName;

    [SerializeField]
    float _blockWidth = 2.5f;
    [SerializeField]
    float _blockHeight = 1;

    Dictionary<BlockType, List<Block>> _categorizedBlocks = new Dictionary<BlockType, List<Block>>();
    public void Init(string name)
    {
        StackName = name;
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
}

