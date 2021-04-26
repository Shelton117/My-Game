using UnityEngine;

/// <summary>
/// ��չ��
/// </summary>
public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;

    /// <summary>
    /// �ж�����Ƿ���ǰ��ĳ���Ƕ���
    /// </summary>
    /// <param name="transform">������</param>
    /// <param name="target">���</param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
