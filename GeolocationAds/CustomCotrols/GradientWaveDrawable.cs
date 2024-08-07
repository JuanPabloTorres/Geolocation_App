namespace GeolocationAds.CustomCotrols
{
    public class GradientWaveDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            var gradient = new LinearGradientPaint
            {
                StartColor = Colors.Blue,
                EndColor = Colors.Orange,
                StartPoint = new PointF(0, 0.5f),
                EndPoint = new PointF(1, 0.5f)
            };

            // Define a path for the inverted wave
            var path = new PathF();
            float heightFactor = dirtyRect.Height;

            // Start from the bottom left
            path.MoveTo(0, heightFactor);

            // Create a quadratic curve that dips in the middle and rises at the ends
            path.QuadTo(dirtyRect.Width * 0.25f, dirtyRect.Height * 0.9f, dirtyRect.Width * 0.5f, dirtyRect.Height * 0.7f);
            path.QuadTo(dirtyRect.Width * 0.75f, dirtyRect.Height * 0.5f, dirtyRect.Width, dirtyRect.Height * 0.7f);

            // Close the path to the bottom right
            path.LineTo(dirtyRect.Width, 0);
            path.LineTo(0, 0);
            path.Close();

            canvas.SetFillPaint(gradient, new RectF(0, 0, dirtyRect.Width, dirtyRect.Height));
            canvas.FillPath(path);

            canvas.RestoreState();
        }
    }
}
