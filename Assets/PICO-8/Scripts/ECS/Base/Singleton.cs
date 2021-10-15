public abstract class Singleton<T> where T : class, new()
{

    private static T _instance;
    private static readonly object _locked = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_locked)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
