//#if ANDROID
//using Android.Webkit;
//using Microsoft.Maui.Controls;

//public partial class PlatformSpecificDataViewModel
//{
//    partial void ClearPlatformSpecificData(WebView webView)
//    {
//        if (webView == null) return;

//        WebStorage.Instance.DeleteAllData();
//        CookieManager.Instance.RemoveAllCookies(null);
//        CookieManager.Instance.Flush();

//        if (webView.Handler?.PlatformView is Android.Webkit.WebView nativeWebView)
//        {
//            nativeWebView.ClearCache(true);
//            nativeWebView.ClearHistory();
//        }
//    }
//}
//#endif
