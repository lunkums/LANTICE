namespace Lunkums.Util
{
    using UnityEngine;

    public static class Vector3Extensions
    {
        public static Vector3 Copy2DComponents(this Vector3 original, Vector3 vectorToCopy)
        {
            return new Vector3(vectorToCopy.x, vectorToCopy.y, original.z);
        }
    }
}
