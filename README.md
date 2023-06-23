# Enhance Performance with MemoryCache in C#


## Introduction:
In modern software development, optimizing performance is crucial for delivering efficient and responsive applications. Caching is a powerful technique that can significantly improve performance by storing frequently accessed data in memory. In this article, we will explore the MemoryCache class in C# and see how it can be used to enhance performance in your applications.

## What is MemoryCache?
MemoryCache is a caching mechanism provided by the .NET framework that allows you to store data in memory for quick access. It provides an in-memory key-value store that can be used to cache frequently accessed data, such as database query results, API responses, or expensive computation results. The MemoryCache class provides methods for adding, retrieving, and removing items from the cache.

## Example Usage:
Let's look at a simple example to understand how MemoryCache works in practice:

```csharp
using System;
using System.Runtime.Caching;

class Program
{
    static void Main()
    {
        // Create a new MemoryCache instance
        MemoryCache cache = MemoryCache.Default;

        // Add an item to the cache
        cache.Add("myKey", "myValue", DateTimeOffset.Now.AddMinutes(10));

        // Retrieve the item from the cache
        string value = cache.Get("myKey") as string;
        Console.WriteLine(value);

        // Remove the item from the cache
        cache.Remove("myKey");

        // Check if the item exists in the cache
        bool exists = cache.Contains("myKey");
        Console.WriteLine(exists);
    }
}

```

In this example, we first create a new instance of the MemoryCache class using the `MemoryCache.Default` property. We then add an item to the cache using the `Add` method, specifying a key, value, and expiration time. In this case, the item will be valid for 10 minutes.

To retrieve the item from the cache, we use the `Get` method, which returns the value associated with the specified key. We can cast the retrieved value to the appropriate type.

To remove an item from the cache, we use the `Remove` method, passing the key as the argument. We can also check if an item exists in the cache using the `Contains` method, which returns a boolean indicating whether the key is present.

## Conclusion:
MemoryCache is a valuable tool for enhancing performance in your C# applications. By caching frequently accessed data in memory, you can reduce the need for expensive computations or frequent database queries, resulting in improved response times and a smoother user experience. The MemoryCache class provides a simple and efficient way to implement caching in your applications. Remember to use appropriate cache expiration times and consider cache invalidation strategies to ensure your cached data remains up-to-date.

---

## About the demo app:
A demo to show how to set expiration in .NET MemoryCache object matching JWT exp

---