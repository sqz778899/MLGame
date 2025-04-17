using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProbabilityService
{
    static Dictionary<string, PseudoRandomBucket<string>> _buckets = new();

    /// <summary>
    /// 基于 key 和概率字典返回一个伪随机结果
    /// </summary>
    public static string Draw(string key, Dictionary<string, int> weights, int seed)
    {
        if (!_buckets.TryGetValue(key, out var bucket))
        {
            bucket = new PseudoRandomBucket<string>(weights, seed);
            _buckets[key] = bucket;
        }

        return bucket.Roll();
    }
    
    public static void Reset(string key) =>  _buckets.Remove(key);
}

public class PseudoRandomBucket<T>
{
    public class Entry
    {
        public T Value;
        public int Weight;
        public float Accumulator;

        public Entry(T value, int weight, float accumulator)
        {
            Value = value;
            Weight = weight;
            Accumulator = accumulator;
        }
    }

    private readonly List<Entry> _entries;
    private readonly float _totalWeight;

    public PseudoRandomBucket(Dictionary<T, int> weights, int seed)
    {
        System.Random rng = new System.Random(seed); //可控随机数生成器

        _entries = weights.Select(pair =>
                new Entry(pair.Key, pair.Value, (float)rng.NextDouble()) //独立初始值
        ).ToList();

        _totalWeight = _entries.Sum(e => e.Weight);
    }

    public T Roll()
    {
        foreach (var entry in _entries)
        {
            entry.Accumulator += entry.Weight / _totalWeight;
        }

        var selected = _entries.OrderByDescending(e => e.Accumulator).First();
        selected.Accumulator -= 1f;
        return selected.Value;
    }
}


