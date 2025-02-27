using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using System.Web;


namespace GeolocationAds.Platforms.Android
{

    //[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    //[IntentFilter(new[] { Intent.ActionView },
    // Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    // DataScheme = "https",
    // DataHost = "com.mycompany.myapp"
    //)]
    //public class FacebookAuthActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    //{
    //    protected override void OnCreate(Bundle savedInstanceState)
    //    {
    //        base.OnCreate(savedInstanceState);

    //        var uri = Intent?.Data?.ToString();
    //        Log.Debug("FacebookAuthActivity", $"🔹 FacebookAuthActivity recibió la URL: {uri}");

    //        if (!string.IsNullOrEmpty(uri) && uri.Contains("access_token"))
    //        {
    //            // 🔹 Extraer el token de la URL manualmente
    //            string accessToken = ExtractAccessToken(uri);
    //            Log.Debug("FacebookAuthActivity", $"🔹 Token de acceso extraído: {accessToken}");

    //            // 🔹 Pasar el token a MainActivity
    //            Intent newIntent = new Intent(this, typeof(MainActivity));
    //            newIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
    //            newIntent.PutExtra("auth_callback_token", accessToken);
    //            StartActivity(newIntent);
    //        }

    //        Finish();
    //    }

    //    private string ExtractAccessToken(string url)
    //    {
    //        var uri = new Uri(url);
    //        var fragment = uri.Fragment.TrimStart('#');  // Elimina el '#' inicial
    //        var queryParams = HttpUtility.ParseQueryString(fragment);
    //        return queryParams["access_token"];
    //    }
    //    //protected override void OnCreate(Bundle savedInstanceState)
    //    //{
    //    //    base.OnCreate(savedInstanceState);

    //    //    // 🔹 Capturar la URL de redirección de Facebook
    //    //    var uri = Intent?.Data?.ToString();

    //    //    if (!string.IsNullOrEmpty(uri))
    //    //    {
    //    //        // 🔹 Convertir la URL en un Intent para abrir la app
    //    //        var intent = new Intent(Intent.ActionView);
    //    //        intent.SetData(global::Android.Net.Uri.Parse("intent://com.mycompany.myapp/auth/callback#Intent;scheme=https;package=com.mycompany.myapp;end;"));
    //    //        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
    //    //        StartActivity(intent);
    //    //    }

    //    //    Finish(); // 🔹 Cierra esta actividad para no dejarla en la pila de navegación
    //    //}

    //    //    protected override void OnCreate(Bundle savedInstanceState)
    //    //    {
    //    //        base.OnCreate(savedInstanceState);

    //    //        var uri = Intent?.Data?.ToString();
    //    //        Log.Debug("FacebookAuthActivity", $"🔹 FacebookAuthActivity recibió la URL: {uri}");

    //    //        if (!string.IsNullOrEmpty(uri))
    //    //        {
    //    //            var newIntent = new Intent(this, typeof(MainActivity));
    //    //            newIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
    //    //            newIntent.PutExtra("auth_callback_url", uri);

    //    //            StartActivity(newIntent);
    //    //        }

    //    //        Finish();
    //    //    }
    //    //}
    //}
}