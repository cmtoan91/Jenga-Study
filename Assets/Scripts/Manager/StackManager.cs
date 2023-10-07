using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StackManager : SimpleSingleton<StackManager>
{
    [SerializeField]
    StackHolder _stackPrefab;

    [SerializeField]
    Block _blockPrefab;

    [SerializeField]
    Vector3 _startPosition = Vector3.zero;

    [SerializeField]
    float _distanceBetweenStacks = 10f;

    public SerializedDictionary<BlockType, Material> MaterialLibrary;
    List<StackHolder> _allStacks = new List<StackHolder>();


    public void CreateStacks(List<BlockData> allBlockDatas)
    {
        _allStacks = new List<StackHolder>();
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
            if (!grade.Contains("Grade")) continue;
            List<BlockData> rearranged = blockDataCatalogs[grade].OrderBy(x => x.cluster).ToList();
            rearranged = rearranged.OrderBy(x => x.domain).ToList();
            StackHolder stack = Instantiate(_stackPrefab, currentPos, Quaternion.identity);
            stack.Init(grade);
            stack.BuildStack(_blockPrefab, rearranged);
            currentPos += Vector3.right * _distanceBetweenStacks;
            if (grade == "7th Grade") CameraController.Instance.FocusTarget(stack.transform);
            GlobalPubSub.PublishEvent<StackCreationMessage>(new StackCreationMessage(stack));
            _allStacks.Add(stack);
        }

        StackFunctionalitySetupMessage funcSetupMsg = new StackFunctionalitySetupMessage();
        funcSetupMsg.AllFunctionality.Add("Test Stack", TestMyStack);
        funcSetupMsg.AllFunctionality.Add("Reset", ResetStack);
        GlobalPubSub.PublishEvent<StackFunctionalitySetupMessage>(funcSetupMsg);
    }

    public void SelectStack(StackHolder stack)
    {
        CameraController.Instance.FocusTarget(stack.transform);
    }

    public void TestMyStack()
    {
        foreach(StackHolder stack in _allStacks)
        {
            stack.TestStack();
        }
    }

    public void ResetStack()
    {
        foreach (StackHolder stack in _allStacks)
        {
            stack.Reset();
        }
    }

}
