namespace Lunkums.Util
{
    public static class ArrayExtensions
    {
        public static void Default<T>(this T[] array, T defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
        }
    }
}
