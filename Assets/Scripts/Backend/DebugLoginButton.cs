using UnityEngine;

public class DebugLoginButton : MonoBehaviour
{
    public string testTan = "12345678";
    public string testPwd = "9999";

    [ContextMenu("Test Login")]
    public void TestLogin()
    {
        StartCoroutine(SheetsService.Instance.LoginAsync(testTan, testPwd, res =>
        {
            if (!res.ok) Debug.LogWarning("Login FAIL: " + res.error);
            else Debug.Log("Login OK. Language=" + res.value.language);
        }));
    }
}
