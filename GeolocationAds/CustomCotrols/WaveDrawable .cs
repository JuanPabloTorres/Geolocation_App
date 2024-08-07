namespace GeolocationAds.CustomCotrols
{
    public class WaveDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // Define the color of the wave
            canvas.FillColor = (Color)Application.Current.Resources["White"];

            // Create a path for the wave
            var path = new PathF();

            // Move to the starting point at the bottom left corner
            path.MoveTo(0, dirtyRect.Height * 0.7f);

            // Create a quadratic curve that dips in the middle and rises at the ends
            path.QuadTo(dirtyRect.Width * 0.25f, dirtyRect.Height * 0.9f, dirtyRect.Width * 0.5f, dirtyRect.Height * 0.7f);
            path.QuadTo(dirtyRect.Width * 0.75f, dirtyRect.Height * 0.5f, dirtyRect.Width, dirtyRect.Height * 0.7f);

            // Draw a line to the top right corner
            path.LineTo(dirtyRect.Width, 0);

            // Draw a line to the top left corner
            path.LineTo(0, 0);

            // Close the path back to the starting point
            path.Close();

            // Fill the path with the color
            canvas.FillPath(path);

            canvas.RestoreState();
        }
    }
}
