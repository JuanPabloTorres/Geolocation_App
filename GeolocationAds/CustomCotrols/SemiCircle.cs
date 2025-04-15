namespace GeolocationAds.CustomCotrols
{
    public class SemiCircle : IDrawable
    {
        public float ArcHeightRatio { get; set; } = 0.8f; // Qué tan bajo baja el arco (0.0 - 1.0)
        public Color FillColor { get; set; } = (Color)Application.Current.Resources["appPrimaryOpt02ColorPrimary"];

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.FillColor = FillColor;

            float arcY = dirtyRect.Height * ArcHeightRatio;

            var path = new PathF();

            path.MoveTo(0, 0); // Parte superior izquierda

            path.LineTo(0, arcY); // Borde inferior izquierdo (curva empieza aquí)

            path.QuadTo(dirtyRect.Width / 2, dirtyRect.Height * 1.2f, dirtyRect.Width, arcY); // Curva

            path.LineTo(dirtyRect.Width, 0); // Parte superior derecha

            path.Close(); // Cierra la figura

            canvas.FillPath(path);

            canvas.RestoreState();
        }
    }
}