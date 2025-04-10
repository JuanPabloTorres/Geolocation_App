namespace GeolocationAds.ViewTemplates;

public partial class RadarSearchView : ContentView
{
    public RadarSearchView()
    {
        InitializeComponent();


        // ?? Esta línea es clave para que la animación se dispare correctamente
        //this.Loaded += RadarSearchView_Loaded;
    }


    //private async void RadarSearchView_Loaded(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Forzamos un pequeño delay para garantizar que el render esté listo
    //        await Task.Delay(100);

    //        // Reiniciamos la animación por si estaba en mal estado
    //        captureAnimation.IsAnimationEnabled = false;

    //        captureAnimation.RepeatCount = -1; // Infinito
    //        captureAnimation.IsAnimationEnabled = true;
            
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Diagnostics.Debug.WriteLine($"[RadarSearchView] Error: {ex.Message}");
    //    }
    //}
}