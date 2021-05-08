using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Demo.MemoryCache
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var jwtString = TokenService.GetJwtToken(5);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(jwtString) as JwtSecurityToken;

            if (token?.ValidTo != null)
            {
                DateTime tokenExpiryDate = token.ValidTo;

                Console.WriteLine($"Token Exp Time: {tokenExpiryDate.ToLongTimeString()}, Current UTC: {DateTime.UtcNow.ToLongTimeString()}\n");
            
                ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(tokenExpiryDate),
                    RemovedCallback = CacheRemovedCallback
                };

                cache.Set("token", token, policy);

                await Task.Delay(3000);

                var cachedToken = cache.Get("token");
                
                if (cachedToken is JwtSecurityToken t)
                {
                    foreach (var claim in t.Claims)
                    {
                        Print($"- {claim.Type}: {claim.Value}", ConsoleColor.Cyan);
                    }
                }

                await Task.Delay(20000);
            }
        }

        private static void CacheRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            var cacheItem = arguments.CacheItem;
            var removedReason = arguments.RemovedReason;

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
