using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Models;

namespace Fame.Service.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<T[]> PowerSet<T>(this IEnumerable<T> input)
        {
            var inputList = input.ToArray();
            int n = input.Count();
            // Power set contains 2^N subsets.
            int powerSetCount = 1 << n;
            var result = new T[powerSetCount][];

            for (int setMask = 0; setMask < powerSetCount; setMask++)
            {
                var s = new LinkedList<T>();
                for (int i = 0; i < n; i++)
                {
                    // Checking whether i'th element of input collection should go to the current subset.
                    if ((setMask & (1 << i)) > 0)
                    {
                        s.AddLast(inputList[i]);
                    }
                }
                result[setMask] = s.ToArray();
            }

            return result;
        }

        public static bool ContainsAll<T>(this IEnumerable<T> input, IEnumerable<T> items)
        {
            return items.All(item => input.Contains(item));
        }

        public static OptionPrice InCurrency(this IEnumerable<OptionPrice> input, string localisationCode)
        {
            return input.Single(x => x.LocalisationCode == localisationCode);
        }

        public static OptionPrice InAUD(this IEnumerable<OptionPrice> input)
        {
            return input.InCurrency("en-AU");
        }

        public static OptionPrice InUSD(this IEnumerable<OptionPrice> input)
        {
            return input.InCurrency("en-US");
        }

        public static ProductVersionPrice InCurrency(this IEnumerable<ProductVersionPrice> input, string localisationCode)
        {
            return input.Single(x => x.LocalisationCode == localisationCode);
        }

        public static ProductVersionPrice InAUD(this IEnumerable<ProductVersionPrice> input)
        {
            return input.InCurrency("en-AU");
        }

        public static ProductVersionPrice InUSD(this IEnumerable<ProductVersionPrice> input)
        {
            return input.InCurrency("en-US");
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket.Select(x => x);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }

        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] lists)
        {
            return lists.SelectMany(x => x);
        }
    }
}
