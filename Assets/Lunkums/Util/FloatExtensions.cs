namespace Lunkums.Util
{
    using UnityEngine;

    public static class FloatExtensions
    {
        public static bool IsZero(this float num)
        {
            return Mathf.Approximately(num, 0);
        }
    }
}
