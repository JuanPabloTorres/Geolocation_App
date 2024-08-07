

//namespace GeolocationAds.CustomCotrols
//{
//    public class SemiCircle : IDrawable
//    {
//        public void Draw(ICanvas canvas, RectF dirtyRect)
//        {
//            canvas.SaveState();

//            //canvas.FillColor = Colors.Black;

//            canvas.FillColor = (Color)Application.Current.Resources["BrightOrangeRed"];



//            // Create a path to draw the shape with a semi-circular top
//            var path = new PathF();
//            path.MoveTo(0, 0); // Start at the top left corner

//            // Create a line to the bottom left corner maintaining the same X but 80% of the height
//            path.LineTo(0, dirtyRect.Height * 0.8f);

//            // Create a quadratic bezier curve for a smooth semi-circular shape
//            // The control point is at the middle top of the rectangle and the end point at bottom right at 80% height
//            path.QuadTo(dirtyRect.Width / 2, dirtyRect.Height * 1.2f, dirtyRect.Width, dirtyRect.Height * 0.8f);

//            // Draw a line to the top right corner
//            path.LineTo(dirtyRect.Width, 0);

//            // Close the path to connect back to the start point
//            path.Close();

//            canvas.FillPath(path); // Fill the path with the selected color

//            canvas.RestoreState();
//        }
//    }
//}

namespace GeolocationAds.CustomCotrols
{
    public class SemiCircle : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // Primero, llena todo el canvas con WhiteSmoke
            canvas.FillColor = (Color)Application.Current.Resources["WhiteSmoke"];

            canvas.FillRectangle(dirtyRect);

            // Luego, dibuja el semi-círculo anaranjado encima
            canvas.FillColor = (Color)Application.Current.Resources["BrightOrangeRed"];

            // Crear una ruta para dibujar la forma con una parte superior semicircular
            var path = new PathF();
            path.MoveTo(0, 0); // Empieza en la esquina superior izquierda

            // Crea una línea hasta la esquina inferior izquierda manteniendo la misma X pero al 80% de la altura
            path.LineTo(0, dirtyRect.Height * 0.8f);

            // Crea una curva cuadrática bezier para una forma semicircular suave
            // El punto de control está en el medio superior del rectángulo y el punto final en la parte inferior derecha al 80% de la altura
            path.QuadTo(dirtyRect.Width / 2, dirtyRect.Height * 1.2f, dirtyRect.Width, dirtyRect.Height * 0.8f);

            // Dibuja una línea hasta la esquina superior derecha
            path.LineTo(dirtyRect.Width, 0);

            // Cierra el camino para conectarse de vuelta al punto de inicio
            path.Close();

            canvas.FillPath(path); // Llena el camino con el color seleccionado

            canvas.RestoreState();
        }
    }
}
