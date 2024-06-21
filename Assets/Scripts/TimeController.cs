using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class TimeController : MonoBehaviour
{
    private const string HttpsTimeIsMoscow = "http://worldtimeapi.org/api/timezone/Europe/Moscow";

    [SerializeField] private Button _getTimeButton;

    [DllImport("__Internal")]
    private static extern void ShowTime(string time);

    private void Awake() => 
        _getTimeButton.onClick.AddListener(GetMoscowTime);

    public async void GetMoscowTime()
    {
        using var request = UnityWebRequest.Get(HttpsTimeIsMoscow);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            JsonData jsonData = JsonUtility.FromJson<JsonData>(jsonResponse);
            string time = jsonData.datetime.Substring(11, 8);
            ShowTime(time);
        }
        else
            Debug.LogError($"Error: {request.error}");
    }
    
    private class JsonData
    {
        public string datetime;
    }
}

