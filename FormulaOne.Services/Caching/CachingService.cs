using FormulaOne.Services.Caching.Interface;
using System.Runtime.Caching;

namespace FormulaOne.Services.Caching;

public class CachingService : ICachingService
{
    private readonly ObjectCache _memoryCache = MemoryCache.Default;

    public T GetData<T>(string key)
    {
        try
        {
            T item = (T)_memoryCache.Get(key);
            return item;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public object? RemoveData(string key)
    {
        try
        {
            if(string.IsNullOrEmpty(key))
            {
                return null;
            }

            var result = _memoryCache.Remove(key);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool SetData<T>(string key, T value)
    {
        try
        {
            if(string.IsNullOrEmpty(key) || value == null)
            {
                return false;
            }

            // Add 10 minutes expiry time
            var expirationTime = DateTimeOffset.Now.AddMinutes(10);

            _memoryCache.Set(key, value, expirationTime);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
