using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StackManager : MonoBehaviour
{
    public static StackManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField]
    StackHolder _stackPrefab;

    [SerializeField]
    Block _blockPrefab;

    [SerializeField]
    Vector3 _startPosition = Vector3.zero;

    [SerializeField]
    float _distanceBetweenStacks = 10f;

    public SerializedDictionary<BlockType, Material> MaterialLibrary;

    public void CreateStacks(List<BlockData> allBlockDatas)
    {
        Dictionary<string, List<BlockData>> blockDataCatalogs = new Dictionary<string, List<BlockData>>();
        foreach(BlockData blockData in allBlockDatas)
        {
            if(!blockDataCatalogs.ContainsKey(blockData.grade))
            {
                blockDataCatalogs[blockData.grade] = new List<BlockData>();
            }
            blockDataCatalogs[blockData.grade].Add(blockData);
        }

        Vector3 currentPos = _startPosition;

        foreach(string grade in blockDataCatalogs.Keys)
        {
            List<BlockData> rearranged = blockDataCatalogs[grade].OrderBy(x => x.grade).ToList();
            StackHolder stack = Instantiate(_stackPrefab, currentPos, Quaternion.identity);
            stack.Init(grade);
            stack.BuildStack(_blockPrefab, rearranged);
            currentPos += Vector3.right * _distanceBetweenStacks;
        }
    }


}
