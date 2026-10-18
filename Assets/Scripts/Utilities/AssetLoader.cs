using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader
{
    public GameObject m_objectRef { get; private set; } = null;
    private Action m_onComplete;
    private Material m_material;

    public bool IsAssetLoaded() { return m_objectRef != null; }

    public void LoadAsset(AssetReference assetRef, Transform parent,
         Action onComplete)
    {
        LoadAsset(assetRef, parent, null, onComplete);
    }

    public void LoadAsset(AssetReference assetRef, Transform parent,
        Material material = null, Action onComplete = null)
    {
        if (assetRef.RuntimeKeyIsValid())
        {
            if (m_objectRef != null)
                ReleaseAsset();

            m_material = material;
            m_onComplete = onComplete;
            Addressables.InstantiateAsync(assetRef, parent).Completed += OnAssetInstantiated;
        }
    }

    private void OnAssetInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        m_objectRef = handle.Result;

        if (m_material != null)
        {
            if (m_objectRef.TryGetComponent(
                out MeshRenderer meshRenderer))
            {
                meshRenderer.material = m_material;
            }
        }

        m_onComplete?.Invoke();
    }

    /// <summary>
    /// Also destroys the object!
    /// </summary>
    public void ReleaseAsset()
    {
        if (m_objectRef != null)
        {
            Addressables.ReleaseInstance(m_objectRef);
            UnityEngine.Object.Destroy(m_objectRef);
            m_objectRef = null;
        }
    }
}
