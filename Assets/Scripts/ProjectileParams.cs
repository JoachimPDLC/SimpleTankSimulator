using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileParams", menuName = "Scriptable Objects/ProjectileParams")]
public class ProjectileParams : ScriptableObject
{
    public float ForwardVelocity;
    public uint NumBounces;
}
