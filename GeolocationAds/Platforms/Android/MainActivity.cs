using Android.App;
using Android.Content.PM;
using Android.Net.Http;
using Android.OS;
using Android.Util;
using Android.Webkit;


namespace GeolocationAds;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
   

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Capturar el Token recibido desde FacebookAuthActivity
        string accessToken = Intent?.GetStringExtra("auth_callback_token");

        if (!string.IsNullOrEmpty(accessToken))
        {
            System.Diagnostics.Debug.WriteLine($"🔹 MainActivity recibió el Token de Facebook: {accessToken}");

            // 🔹 Aquí puedes continuar con el login en tu app o guardar el token
        }

        //callbackManager = CallbackManagerFactory.Create();
    }

    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);

    //    // Capturar la URL recibida
    //    string callbackUri = Intent?.GetStringExtra("auth_callback_url");

    //    if (!string.IsNullOrEmpty(callbackUri))
    //    {
    //        System.Diagnostics.Debug.WriteLine($"🔹 MainActivity recibió la URL de Facebook: {callbackUri}");

    //        // Convertir la URL a un Uri válido
    //        var authUri = new Uri(callbackUri);

    //        // Completar la autenticación manualmente
    //        Microsoft.Maui.Authentication.WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
    //        {
    //            Url = authUri,
    //            CallbackUrl = authUri
    //        });
    //    }
    //}

    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);

    //    // Capturar la URL recibida
    //    string callbackUri = Intent?.GetStringExtra("auth_callback_url");

    //    if (!string.IsNullOrEmpty(callbackUri))
    //    {
    //        System.Diagnostics.Debug.WriteLine($"🔹 MainActivity recibió la URL de Facebook: {callbackUri}");

    //        try
    //        {
    //            var authUri = new Uri(callbackUri);

    //            // Procesar la autenticación manualmente
    //            MainThread.BeginInvokeOnMainThread(async () =>
    //            {
    //                await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
    //                {
    //                    Url = authUri,
    //                    CallbackUrl = authUri
    //                });
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Diagnostics.Debug.WriteLine($"❌ Error al procesar la autenticación: {ex.Message}");
    //        }
    //    }



        //protected override void OnActivityResult(int requestCode, int resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    FacebookSdk.SdkInitialize(this.ApplicationContext);

        //    CallbackManager callbackManager = CallbackManager.Factory.Create();

        //    callbackManager.OnActivityResult(requestCode, resultCode, data);
        //}



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
