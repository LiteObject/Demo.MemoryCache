using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Caching;

namespace Demo.MemoryCache
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            string jwtString = TokenService.GetJwtToken(5);

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken? token = handler.ReadToken(jwtString) as JwtSecurityToken;

            if (token?.ValidTo != null)
            {
                DateTime tokenExpiryDate = token.ValidTo;

                Console.WriteLine($"Token Exp Time: {tokenExpiryDate.ToLongTimeString()}, Current UTC: {DateTime.UtcNow.ToLongTimeString()}\n");

                ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

                CacheItemPolicy policy = new()
                {
                    AbsoluteExpiration = new DateTimeOffset(tokenExpiryDate),
                    RemovedCallback = CacheRemovedCallback
                };

                cache.Set("token", token, policy);

                await Task.Delay(3000);

                object cachedToken = cache.Get("token");

                if (cachedToken is JwtSecurityToken t)
                {
                    foreach (System.Security.Claims.Claim? claim in t.Claims)
                    {
                        Print($"- {claim.Type}: {claim.Value}", ConsoleColor.Cyan);
                    }
                }

                await Task.Delay(20000);
            }
        }

        private static void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            CacheItem cacheItem = arguments.CacheItem;
            CacheEntryRemovedReason removedReason = arguments.RemovedReason;

            Print($"\n[UTC Time: {DateTime.UtcNow.ToLongTimeString()}] Cached item \"{cacheItem.Key}\" has been removed. Reason: {removedReason.ToString()}", ConsoleColor.Yellow);
        }

        private static void Print(string value, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
