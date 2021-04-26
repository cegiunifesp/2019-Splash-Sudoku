using System.Collections;

public class CoroutineExecutor : SingletonBehaviour<CoroutineExecutor>
{
    public static void Start(IEnumerator routine)
    {
        Instance.StartCoroutine(routine);
    }
}
