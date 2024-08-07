using Android.App;
using Android.Content.PM;
using Android.Net.Http;
using Android.Webkit;

namespace GeolocationAds;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);

    //    // Configurar el WebView
    //    var webView = new Android.Webkit.WebView(this);

    //    webView.Settings.JavaScriptEnabled = true;

    //    webView.Settings.DomStorageEnabled = true;

    //    webView.Settings.AllowFileAccess = true;

    //    webView.Settings.SetAppCacheEnabled(true);

    //    webView.Settings.LoadWithOverviewMode = true;

    //    webView.Settings.UseWideViewPort = true;

    //    // Configurar el WebViewClient personalizado para manejar SSL
    //    webView.SetWebViewClient(new SslWebViewClient());

    //    //// Establecer la URL a cargar
    //    //webView.LoadUrl("https://www.instagram.com");

    //    // Establecer el WebView como el contenido de la actividad
    //    SetContentView(webView);
    //}
}

public class SslWebViewClient : WebViewClient
{
    public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
    {
        // Ignorar errores SSL para fines de prueba (no recomendado para producción)
        handler.Proceed();
    }

    public override void OnPageFinished(Android.Webkit.WebView view, string url)
    {
        base.OnPageFinished(view, url);
        // Lógica adicional cuando la página ha terminado de cargarse
    }

    public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
    {
        base.OnReceivedError(view, request, error);
        // Manejar errores de carga de página
    }
}

//public class CertInfo
//{
//    public CertInfo(SslCertificate certificate)
//    {
//        // Extract and store relevant certificate information
//    }
//}
