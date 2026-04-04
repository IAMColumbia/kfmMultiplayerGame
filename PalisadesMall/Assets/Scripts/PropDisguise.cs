using System.Collections.Generic;
using UnityEngine;

public class PropDisguise : MonoBehaviour
{
    [SerializeField] private Transform propVisualRoot;
    [SerializeField] private List<GameObject> propPrefabs = new();

    private GameObject currentPropInstance;
    private PlayerIdentity identity;

    private void Awake()
    {
        identity = GetComponent<PlayerIdentity>();
    }

    private void Start()
    {
        AssignRandomProp();
    }

    public void AssignRandomProp()
    {
        if (propPrefabs.Count == 0)
        {
            Debug.LogWarning("No prop prefabs assigned.");
            return;
        }

        if (currentPropInstance != null)
            Destroy(currentPropInstance);

        int index = Random.Range(0, propPrefabs.Count);
        currentPropInstance = Instantiate(propPrefabs[index], propVisualRoot);

        currentPropInstance.transform.localPosition = Vector3.zero;
        currentPropInstance.transform.localRotation = Quaternion.identity;
        currentPropInstance.transform.localScale = Vector3.one;

        Transform anchor = currentPropInstance.transform.Find("PropAnchor");

        if (anchor != null)
        {
            currentPropInstance.transform.localPosition -= anchor.localPosition;

            FPSController fps = GetComponent<FPSController>();
            if (fps != null)
            {
                currentPropInstance.transform.localPosition -= Vector3.up * fps.hoverHeight;
            }
        }
        else
        {
            Debug.LogWarning($"{currentPropInstance.name} is missing a PropAnchor.");
        }

        TagTarget tagTarget = currentPropInstance.GetComponent<TagTarget>();
        if (tagTarget == null)
            tagTarget = currentPropInstance.AddComponent<TagTarget>();

        tagTarget.Initialize(identity);
        
        if (identity != null)
        {
            string layerName = identity.playerIndex == 0 ? "Player1Prop" : "Player2Prop";
            int layer = LayerMask.NameToLayer(layerName);

            if (layer != -1)
            {
                SetLayerRecursively(currentPropInstance, layer);
            }
            else
            {
                Debug.LogWarning($"Layer '{layerName}' does not exist.");
            }
        }
    }

    public GameObject GetCurrentPropInstance()
    {
        return currentPropInstance;
    }

        private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}