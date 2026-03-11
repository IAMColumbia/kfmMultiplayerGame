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

        TagTarget tagTarget = currentPropInstance.GetComponent<TagTarget>();
        if (tagTarget == null)
            tagTarget = currentPropInstance.AddComponent<TagTarget>();

        tagTarget.Initialize(identity);
    }
}