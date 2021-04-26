using UnityEngine;

/// <summary>
/// 拓展类
/// </summary>
public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;

    /// <summary>
    /// 判断玩家是否在前方某个角度内
    /// </summary>
    /// <param name="transform">参照物</param>
    /// <param name="target">玩家</param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
