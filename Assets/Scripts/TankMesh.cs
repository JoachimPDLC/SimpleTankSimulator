using UnityEngine;
using UnityEngine.AddressableAssets;

public class TankMesh : MonoBehaviour
{
    [System.Serializable]
    struct AssetRefWithMat
    {
        public AssetReference AssetRef;
        public Material Mat;
    }

    [SerializeField] private Transform m_turret;
    [SerializeField] private AssetRefWithMat m_core;
    [SerializeField] private AssetRefWithMat m_treads;
    [SerializeField] private AssetRefWithMat m_top;
    [SerializeField] private AssetRefWithMat m_cannon;
    private AssetLoader m_coreLoader = new();
    private AssetLoader m_treadsLoader = new();
    private AssetLoader m_topLoader = new();
    private AssetLoader m_cannonLoader = new();
    private Transform m_transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_transform = transform;

        m_coreLoader.LoadAsset(m_core.AssetRef,
            m_transform, m_core.Mat);
        m_treadsLoader.LoadAsset(m_treads.AssetRef,
            m_transform, m_treads.Mat);
        m_topLoader.LoadAsset(m_top.AssetRef,
            m_turret, m_top.Mat);
        m_cannonLoader.LoadAsset(m_cannon.AssetRef,
            m_turret, m_cannon.Mat);
    }

    private void OnDestroy()
    {
        m_coreLoader.ReleaseAsset();
        m_treadsLoader.ReleaseAsset();
        m_topLoader.ReleaseAsset();
        m_cannonLoader.ReleaseAsset();
    }
}
