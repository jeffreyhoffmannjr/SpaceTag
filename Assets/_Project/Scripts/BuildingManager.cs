using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public enum BuildingType
    {
        None,
        Tree,
        Trap,
        Thorn
    }

    [SerializeField] private GameObject treeWallPrefab;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private GameObject thornPrefab;
    [SerializeField] private LayerMask buildableLayer;
    
    private BuildingType selectedBuilding = BuildingType.None;
    private GameObject buildingPreview;

    public void SetBuildingType(BuildingType type)
    {
        selectedBuilding = type;
        DestroyBuildingPreview();
    }

    private GameObject GetPrefabForType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Tree: return treeWallPrefab;
            case BuildingType.Trap: return trapPrefab;
            case BuildingType.Thorn: return thornPrefab;
            default: return null;
        }
    }

    void Update()
    {
        if (selectedBuilding == BuildingType.None) return;

        // Check for touch on right side of screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2)  // Right side of screen
            {
                HandleBuildingPlacement(touch);
            }
        }

        // For testing in editor with mouse
        if (Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width / 2)
        {
            HandleMousePlacement();
        }
    }

    void HandleBuildingPlacement(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayer))
        {
            if (touch.phase == TouchPhase.Began)
            {
                CreateBuildingPreview(hit.point);
            }
            else if (touch.phase == TouchPhase.Moved && buildingPreview != null)
            {
                buildingPreview.transform.position = hit.point;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                PlaceBuilding(hit.point);
                DestroyBuildingPreview();
            }
        }
    }

    void HandleMousePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateBuildingPreview(hit.point);
            }
            if (buildingPreview != null)
            {
                buildingPreview.transform.position = hit.point;
            }
            if (Input.GetMouseButtonUp(0))
            {
                PlaceBuilding(hit.point);
                DestroyBuildingPreview();
            }
        }
    }

    void CreateBuildingPreview(Vector3 position)
    {
        GameObject prefab = GetPrefabForType(selectedBuilding);
        if (prefab != null && buildingPreview == null)
        {
            buildingPreview = Instantiate(prefab, position, Quaternion.identity);
            // Make it semi-transparent
            var renderers = buildingPreview.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                Color color = renderer.material.color;
                color.a = 0.5f;
                renderer.material.color = color;
            }
        }
    }

    void PlaceBuilding(Vector3 position)
    {
        GameObject prefab = GetPrefabForType(selectedBuilding);
        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    void DestroyBuildingPreview()
    {
        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
        }
    }
}