using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiControlPanel : MonoBehaviour
{
    [SerializeField]
    UiStackSelectButton _stackSelectButton;

    [SerializeField]
    Transform _stackSelectPanel;

    [SerializeField]
    UiStackFuncButton _stackFunctionButton;

    [SerializeField]
    Transform _stackFunctionalityPanel;

    [SerializeField]
    TMP_Text _blockInfoDisplay;

    Block _currentSelectedBlock;

    private void Awake()
    {
        GlobalPubSub.SubcribeEvent<StackCreationMessage>(OnStackCreation);
        GlobalPubSub.SubcribeEvent<StackFunctionalitySetupMessage>(OnStackFunctionDeploy);
        GlobalPubSub.SubcribeEvent<OnBlockSelectMessage>(OnBlockSelected);
    }

    private void OnDestroy()
    {
        GlobalPubSub.UnsubcribeEvent<StackCreationMessage>(OnStackCreation);
        GlobalPubSub.UnsubcribeEvent<StackFunctionalitySetupMessage>(OnStackFunctionDeploy);
        GlobalPubSub.UnsubcribeEvent<OnBlockSelectMessage>(OnBlockSelected);
    }

    void OnStackCreation(StackCreationMessage msg)
    {
        UiStackSelectButton button = Instantiate(_stackSelectButton, _stackSelectPanel);
        button.Init(msg.Stack.StackName, msg.Stack);
    }

    void OnStackFunctionDeploy(StackFunctionalitySetupMessage msg)
    {
        foreach (string name in msg.AllFunctionality.Keys)
        {
            UiStackFuncButton button = Instantiate(_stackFunctionButton, _stackFunctionalityPanel);
            button.Init(name, msg.AllFunctionality[name]); 
        }
    }

    void OnBlockSelected(OnBlockSelectMessage msg)
    {
        if (_currentSelectedBlock != null) _currentSelectedBlock.OnSelect(false);
        _currentSelectedBlock = msg.SelectedBlock;
        _currentSelectedBlock.OnSelect(true);
        BlockData data = _currentSelectedBlock.Data;
        _blockInfoDisplay.text = string.Format("[Grade level]: {0} \n[Domain]: {1} \n[Cluster]: {2} \n [Standard ID]: {3} \n [Standard Description]: {4}", data.grade, data.domain, data.cluster, data.standardid, data.standarddescription);
    }
}
