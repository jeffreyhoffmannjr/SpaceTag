using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public BuildingManager buildingManager;
    
    public Button buildTreeButton;
    public Button buildTrapButton;
    public Button buildThornButton;
    public Button quickDashButton;

    void Start()
    {
        // Set up button click listeners
        buildTreeButton.onClick.AddListener(() => SelectAction(BuildingManager.BuildingType.Tree));
        buildTrapButton.onClick.AddListener(() => SelectAction(BuildingManager.BuildingType.Trap));
        buildThornButton.onClick.AddListener(() => SelectAction(BuildingManager.BuildingType.Thorn));
        quickDashButton.onClick.AddListener(ActivateQuickDash);
    }

    void SelectAction(BuildingManager.BuildingType buildingType)
    {
        buildingManager.SetBuildingType(buildingType);
    }

    void ActivateQuickDash()
    {
        // We'll implement this later
        Debug.Log("Quick Dash activated!");
        buildingManager.SetBuildingType(BuildingManager.BuildingType.None);
    }
}