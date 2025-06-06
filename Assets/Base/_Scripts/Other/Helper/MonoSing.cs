public class MonoSing<T> : UnityEngine.MonoBehaviour where T : MonoSing<T>
{
    private static volatile T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(T)) as T;
            return instance;
        }
    }
}