namespace ToolsLibrary.Extensions
{
    public static class ObjectExtension
    {
        public static bool IsObjectNull(this object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}