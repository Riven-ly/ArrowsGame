using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerApiClient : MonoBehaviour
{
    public static PlayerApiClient Instance;

    private string baseURL = "http://129.227.153.67:3491";
    private string appKey = "31089997";
    private int timeoutSeconds = 15;

    private const string PlayerIdKey = "PlayerApiClient.PlayerId";
    private const string ApiPrefix = "/api/v1";

    public string PlayerId => PlayerPrefs.GetString(PlayerIdKey, string.Empty);
    public bool IsRegistered { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
        {
            Register(null, null);
        });
    }
    /// <summary>
    /// 注册或更新当前设备对应的玩家信息。
    /// </summary>
    /// <param name="onSuccess">请求成功回调，返回玩家注册信息。</param>
    /// <param name="onFailure">请求失败回调，返回错误信息。</param>
    public void Register(Action<PlayerRegisterData> onSuccess, Action<string> onFailure = null)
    {
        string deviceId = DeviceManager.ClientUUID;
        string gaid = DeviceManager.GetDeviceAdvertisingId();
        if (string.IsNullOrEmpty(gaid))
        {
            gaid = deviceId;
        }

        RegisterRequest request = new RegisterRequest
        {
            appKey = appKey,
            gaid = gaid,
            deviceId = deviceId,
            deviceModel = DeviceManager.GetDeviceModel(),
            deviceBrand = DeviceManager.GetBrand(),
            osType = DeviceManager.GetOSType(),
            osVersion = DeviceManager.GetOSVersion(),
            appVersion = DeviceManager.GetAppVersion(),
            countryCode = GetCountryCode()
        };

        Action<PlayerRegisterData> registerSuccess = response =>
        {
            SavePlayerId(response.playerId);
            IsRegistered = true;
            onSuccess?.Invoke(response);
        };
        StartCoroutine(Post<RegisterRequest, PlayerRegisterData>("/player/register", request, registerSuccess, onFailure));
    }

    private string GetCountryCode()
    {
        try
        {
            return RegionInfo.CurrentRegion.TwoLetterISORegionName;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <param name="onSuccess">请求成功回调，返回玩家信息。</param>
    /// <param name="onFailure">请求失败回调，返回错误信息。</param>
    public void GetPlayerInfo(Action<PlayerInfoData> onSuccess, Action<string> onFailure = null)
    {
        StartCoroutine(Post<PlayerInfoRequest, PlayerInfoData>("/player/info", new PlayerInfoRequest
        {
            appKey = appKey,
            playerId = PlayerId
        }, onSuccess, onFailure));
    }

    /// <summary>
    /// 提交WDL申请，申请金额由服务端按当前可用余额计算。
    /// </summary>
    /// <param name="name">收款人姓名。</param>
    /// <param name="phone">收款人手机号。</param>
    /// <param name="email">收款人邮箱。</param>
    /// <param name="bankCode">支付方式，只支持 PayPal 或 VENMO。</param>
    /// <param name="accountNo">收款账号。</param>
    /// <param name="onSuccess">请求成功回调，返回WDL订单信息。</param>
    /// <param name="onFailure">请求失败回调，返回错误信息。</param>
    public void ApplyWDL(string name, string phone, string email, string bankCode, string accountNo,
        Action<WDLApplyData> onSuccess, Action<int> onBusinessFailure, Action<string> onFailure = null)
    {
        StartCoroutine(ApplyWDLRequest(new WDLApplyRequest
        {
            appKey = appKey,
            playerId = PlayerId,
            name = name,
            phone = phone,
            email = email,
            bankCode = bankCode,
            accountNo = accountNo
        }, onSuccess, onBusinessFailure, onFailure));
    }

    private IEnumerator ApplyWDLRequest(WDLApplyRequest request, Action<WDLApplyData> onSuccess,
        Action<int> onBusinessFailure, Action<string> onFailure)
    {
        string endpoint = "/" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("d2l0aGRyYXdhbA==")) + "/apply";
        string encryptedJson = EncryptDecodeUtils.CommunicationEncrypt(JsonConvert.SerializeObject(request));
        using (UnityWebRequest webRequest = new UnityWebRequest(baseURL.TrimEnd('/') + ApiPrefix + endpoint, UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(encryptedJson));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.timeout = timeoutSeconds;
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                onFailure?.Invoke(webRequest.error);
                yield break;
            }

            try
            {
                string responseJson = EncryptDecodeUtils.CommunicationDecrypt(webRequest.downloadHandler.text);
                ApiResponse<WDLApplyData> response = JsonConvert.DeserializeObject<ApiResponse<WDLApplyData>>(responseJson);
                if (response.code == 200)
                {
                    Debug.Log($"PlayerApiClient request success: {endpoint}\n{responseJson}");
                    onSuccess?.Invoke(response.data);
                }
                else
                {
                    Debug.Log($"PlayerApiClient request fail: {endpoint}\n{responseJson}");
                    onBusinessFailure?.Invoke(response.code);
                }
            }
            catch (Exception exception)
            {
                onFailure?.Invoke(exception.Message);
            }
        }
    }

    /// <summary>
    /// 分页获取当前玩家的WDL记录。
    /// </summary>
    /// <param name="pageNum">页码，从 1 开始。</param>
    /// <param name="pageSize">每页记录数，最大 50。</param>
    /// <param name="onSuccess">请求成功回调，返回WDL记录列表。</param>
    /// <param name="onFailure">请求失败回调，返回错误信息。</param>
    public void GetWDLList(int pageNum, int pageSize, Action<WDLListData> onSuccess,
        Action<string> onFailure = null)
    {
        StartCoroutine(Post<WDLListRequest, WDLListData>("/" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("d2l0aGRyYXdhbA==")) + "/list", new WDLListRequest
        {
            appKey = appKey,
            playerId = PlayerId,
            pageNum = pageNum,
            pageSize = pageSize
        }, onSuccess, onFailure));
    }

    /// <summary>
    /// 序列化、加密并发送接口请求，然后解密和解析服务端响应。
    /// </summary>
    /// <param name="endpoint">接口相对路径。</param>
    /// <param name="request">请求参数对象。</param>
    /// <param name="onSuccess">业务成功回调。</param>
    /// <param name="onFailure">网络或业务失败回调。</param>
    private IEnumerator Post<TRequest, TData>(string endpoint, TRequest request, Action<TData> onSuccess,
        Action<string> onFailure)
    {
        string plainJson = JsonConvert.SerializeObject(request);
        string encryptedJson = EncryptDecodeUtils.CommunicationEncrypt(plainJson);
        using (UnityWebRequest webRequest = new UnityWebRequest(baseURL.TrimEnd('/') + ApiPrefix + endpoint, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] body = System.Text.Encoding.UTF8.GetBytes(encryptedJson);
            webRequest.uploadHandler = new UploadHandlerRaw(body);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.timeout = timeoutSeconds;
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                string error = $"PlayerApiClient request failed: {endpoint}, error: {webRequest.error}, responseCode: {webRequest.responseCode}";
                Debug.LogError(error);
                onFailure?.Invoke(error);
                yield break;
            }

            string responseJson;
            try
            {
                responseJson = EncryptDecodeUtils.CommunicationDecrypt(webRequest.downloadHandler.text);
                ApiResponse<TData> response = JsonConvert.DeserializeObject<ApiResponse<TData>>(responseJson);
                if (response.code != 200)
                {
                    string error = $"PlayerApiClient business failed: {endpoint}, code: {response.code}, msg: {response.msg}";
                    Debug.Log(error);
                    onFailure?.Invoke(error);
                    yield break;
                }

                Debug.Log($"PlayerApiClient request success: {endpoint}\n{responseJson}");
                onSuccess?.Invoke(response.data);
            }
            catch (Exception exception)
            {
                string error = $"PlayerApiClient response parse failed: {endpoint}, error: {exception.Message}";
                Debug.LogError(error);
                onFailure?.Invoke(error);
            }
        }
    }

    private void SavePlayerId(string playerId)
    {
        PlayerPrefs.SetString(PlayerIdKey, playerId);
        PlayerPrefs.Save();
    }

    [Serializable]
    private class RegisterRequest
    {
        public string appKey;
        public string gaid;
        public string deviceId;
        public string deviceModel;
        public string deviceBrand;
        public string osType;
        public string osVersion;
        public string appVersion;
        public string countryCode;
    }

    [Serializable]
    private class PlayerInfoRequest
    {
        public string appKey;
        public string playerId;
    }

    [Serializable]
    private class WDLApplyRequest
    {
        public string appKey;
        public string playerId;
        public string name;
        public string phone;
        public string email;
        public string bankCode;
        public string accountNo;
    }

    [Serializable]
    private class WDLListRequest
    {
        public string appKey;
        public string playerId;
        public int pageNum;
        public int pageSize;
    }

    [Serializable]
    private class ApiResponse<T>
    {
        public int code;
        public string msg;
        public T data;
    }

    [Serializable]
    public class PlayerRegisterData
    {
        public string playerId;
        public string nickname;
        public string appKey;
        public decimal availableBalance;
        public decimal frozenBalance;
        public bool isNew;
        public long createdAt;
    }

    [Serializable]
    public class PlayerInfoData
    {
        public string playerId;
        public string appKey;
        public decimal availableBalance;
        public decimal frozenBalance;
    }

    [Serializable]
    public class WDLApplyData
    {
        public string orderNo;
        public decimal applyAmount;
        public string status;
        public long createdAt;
    }

    [Serializable]
    public class WDLListData
    {
        public long total;
        public WDLRecord[] list;
    }

    [Serializable]
    public class WDLRecord
    {
        public string orderNo;
        public decimal applyAmount;
        public string status;
        public string failReason;
        public long createdAt;
        public long? finishedAt;
    }
}
