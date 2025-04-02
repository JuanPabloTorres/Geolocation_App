//namespace GeolocationAds.CustomCotrols
//{
//    public class SemiCircle : IDrawable
//    {
//        public void Draw(ICanvas canvas, RectF dirtyRect)
//        {
//            canvas.SaveState();

// //canvas.FillColor = Colors.Black;

// canvas.FillColor = (Color)Application.Current.Resources["BrightOrangeRed"];

// // Create a path to draw the shape with a semi-circular top var path = new PathF();
// path.MoveTo(0, 0); // Start at the top left corner

// // Create a line to the bottom left corner maintaining the same X but 80% of the height
// path.LineTo(0, dirtyRect.Height * 0.8f);

// // Create a quadratic bezier curve for a smooth semi-circular shape // The control point is at
// the middle top of the rectangle and the end point at bottom right at 80% height
// path.QuadTo(dirtyRect.Width / 2, dirtyRect.Height * 1.2f, dirtyRect.Width, dirtyRect.Height * 0.8f);

// // Draw a line to the top right corner path.LineTo(dirtyRect.Width, 0);

// // Close the path to connect back to the start point path.Close();

// canvas.FillPath(path); // Fill the path with the selected color

//            canvas.RestoreState();
//        }
//    }
//}

namespace GeolocationAds.CustomCotrols
{
    public class SemiCircle : IDrawable
    {
        public float ArcHeightRatio { get; set; } = 0.8f; // Qué tan bajo baja el arco (0.0 - 1.0)
        public Color FillColor { get; set; } = (Color)Application.Current.Resources["appPrimaryOpt2ColorPrimaryDark"];

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // Dibujar fondo transparente si se desea canvas.FillColor =
            // (Color)Application.Current.Resources["WhiteSmoke"]; canvas.FillRectangle(dirtyRect);

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