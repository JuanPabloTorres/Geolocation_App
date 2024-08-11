using ToolsLibrary.Extensions;

namespace ToolsLibrary.Tools
{
    public class NavigationTool
    {
        public static async Task NavigateAsync(string pageName, Dictionary<string, object> queryParameters = null)
        {
            try
            {
                var queryString = string.Empty;

                if (!queryParameters.IsObjectNull() && queryParameters.Count > 0)
                {
                    queryString = string.Join("&", queryParameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value.ToString())}"));

                    queryString = "?" + queryString;
                }

                await Shell.Current.GoToAsync($"{pageName}{queryString}");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);

            }


        }
    }
}
