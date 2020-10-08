using System;

namespace Prism.Logging.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] array, int length)
        {
            if (length > array.Length)
                length -= length - array.Length;

            T[] result = new T[length];
            Array.Copy(array, result, length);
            return result;
        }

        public static ArraySegment<T> ToArraySegment<T>(this T[] array) =>
            new ArraySegment<T>(array);
    }
}