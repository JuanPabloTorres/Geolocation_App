namespace ToolsLibrary.Tools
{
    public static class CommonsTool
    {
        public static async Task<byte[]> GetFileBytesAsync(FileResult fileResult)
        {
            if (fileResult != null)
            {
                using (var stream = await fileResult.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        return ms.ToArray();
                    }
                }
            }
            return null;
        }
    }
}
