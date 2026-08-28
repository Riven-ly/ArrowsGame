using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 设备管理器 - 提供设备信息、系统信息、广告标识符等获取功能
/// </summary>
public static class DeviceManager 
{
    /// <summary>
    /// 客户端唯一标识符（UUID）
    /// </summary>
    public static string ClientUUID
    {
        //get
        //{
        //    string _UUID = PlayerPrefs.GetString("ClientUUID", "");
        //    if (string.IsNullOrEmpty(_UUID))
        //    {
        //        _UUID = Guid.NewGuid().ToString();
        //        PlayerPrefs.SetString("ClientUUID", _UUID);
        //        PlayerPrefs.Save();
        //    }
        //    return _UUID;
        //}

        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    /// <summary>
    /// 获取操作系统类型
    /// </summary>
    /// <returns>返回 "iOS" 或 "Android"</returns>
    public static string GetOSType()
    {
#if UNITY_IOS
         return "iOS";
#else
        return "Android";
#endif
    }

    /// <summary>
    /// 获取操作系统版本号（从系统信息字符串中提取纯数字版本号）
    /// </summary>
    /// <returns>版本号字符串，匹配失败返回 "Unknown"</returns>
    public static string GetOSVersion()
    {
        string osStr = SystemInfo.operatingSystem;
        // 正则匹配 纯版本号格式 (数字+小数点)
        Match match = Regex.Match(osStr, @"(\d+(\.\d+)+|\d+)");
        return match.Success ? match.Value : "Unknown";
    }

    /// <summary>
    /// 获取设备品牌
    /// </summary>
    /// <returns>iOS返回"Apple"，Android返回型号的第一个单词（通常为品牌名），编辑器返回"Simulator"</returns>
    public static string GetBrand()
    {
#if UNITY_IOS
    return "Apple";//iOS品牌固定为Apple
#elif UNITY_ANDROID
        string model = SystemInfo.deviceModel;
        if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(model.Trim()))
        {
            return "Unknown ANDROID";
        }
        return model.Trim().Split(' ')[0];
#else
    return "Simulator";
#endif
    }

    /// <summary>
    /// 获取设备型号
    /// </summary>
    /// <returns>iOS返回具体机型标识（如iPhone15,2），Android返回品牌后的型号部分，编辑器返回"Simulator"</returns>
    public static string GetDeviceModel()
    {
#if UNITY_IOS
    return UnityEngine.iOS.Device.generation.ToString(); // iOS机型（如iPhone15,2）
#elif UNITY_ANDROID
        string model = SystemInfo.deviceModel;
        if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(model.Trim()))
        {
            return "Unknown ANDROID";
        }
        model = model.Trim();//去除首尾空格，避免空格干扰IndexOf
        int index = model.IndexOf(" ");
        return index > 0 ? model.Substring(index + 1) : model;
#else
    return "Simulator";
#endif
    }

    /// <summary>
    /// 获取应用程序版本号（字符串形式）
    /// </summary>
    /// <returns>Application.version 返回的版本字符串</returns>
    public static string GetAppVersion()
    {
        return Application.version;
    }

    /// <summary>
    /// 获取应用程序的数字版本号（VersionCode）
    /// </summary>
    /// <remarks>
    /// 仅Android平台有效，编辑器和iOS返回1。
    /// Android 9+（API 28+）使用 getLongVersionCode()，否则使用 versionCode。
    /// </remarks>
    /// <returns>数字版本号，获取失败返回1</returns>
    public static int GetAppNumberVersion()
    {
#if UNITY_EDITOR
        return 1;
#endif
        try
        {

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            string packageName = currentActivity.Call<string>("getPackageName");

            // 获取包信息
            AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);

            // 根据 Android 版本使用不同的方法
            int versionCode = 0;

            // Android API 28+ (Android 9)
            if (GetAndroidAPILevel() >= 28)
            {
                Debug.Log("GetAndroidAPILevel " );
                // 使用 getLongVersionCode()
                versionCode = (int)packageInfo.Call<long>("getLongVersionCode");
            }
            else
            {
                // 使用 getVersionCode()
                versionCode = packageInfo.Get<int>("versionCode");
            }

            return versionCode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("获取 Android VersionCode 失败: " + e.Message);
            return 1;
        }
    }

    /// <summary>
    /// 获取Android系统的API级别
    /// </summary>
    /// <returns>API级别整数（如Android 13对应33），获取失败返回0</returns>
    public static int GetAndroidAPILevel()
    {
        try
        {
            using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {

                // SDK_INT 对应当前Android系统的API等级（如Android 13对应33）
                return versionClass.GetStatic<int>("SDK_INT");
            }
        }
        catch (System.Exception)
        {
            return 0;
        }
      
    }

    /// <summary>
    /// 获取简化的网络类型
    /// </summary>
    /// <returns>"WiFi" - 无线局域网, "4G" - 蜂窝网络, "no network" - 无网络</returns>
    public static string GetSimpleNetworkType()
    {
        string net = Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ? "WiFi" : (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ? "4G" : "no network");
        return net;
    }

    /// <summary>
    /// 获取当前设备所在时区（相对于UTC的小时偏移）
    /// </summary>
    /// <returns>UTC偏移小时数（如东八区返回8）</returns>
    public static int GetTimeZoneNumber()
    {
        int timeZoneNum = System.TimeZoneInfo.Local.BaseUtcOffset.Hours;
        return timeZoneNum;
    }

    /// <summary>
    /// 获取设备广告标识符（自动适配平台）
    /// </summary>
    /// <returns>广告标识符，如果用户限制跟踪则返回空字符串</returns>
    public static string GetDeviceAdvertisingId()
    {
#if UNITY_IOS && !UNITY_EDITOR
    return GetDeviceAdvertisingIdIOS();
#elif UNITY_ANDROID && !UNITY_EDITOR
    return GetDeviceAdvertisingIdAndroid();
#else
        return "";
#endif
    }

    /// <summary>
    /// iOS平台获取广告标识符
    /// </summary>
    private static string GetDeviceAdvertisingIdIOS()
    {
        //try
        //{
        //    if (UnityEngine.iOS.Device.advertisingTrackingEnabled)
        //    {
        //        return UnityEngine.iOS.Device.advertisingIdentifier;
        //    }
        //    return ""; // 用户限制了广告跟踪
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.Log($"Failed to get iOS advertising ID: {ex.Message}");
        //    return "";
        //}
        return "";
    }

    /// <summary>
    /// Android平台获取广告标识符
    /// </summary>
    private static string GetDeviceAdvertisingIdAndroid()
    {
        try
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject act = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
            AndroidJavaObject info = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", act);

            // 检查是否启用了限制广告跟踪
            bool isLimitAdTrackingEnabled = info.Call<bool>("isLimitAdTrackingEnabled");
            if (isLimitAdTrackingEnabled)
            {
                return ""; // 用户限制了广告跟踪
            }

            return info.Call<string>("getId");
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Failed to get Android advertising ID: {ex.Message}");
            return "";
        }
    }

}
