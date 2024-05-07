namespace ToolsLibrary.Tools
{
    public static class ConstantsTools
    {
        public const int BUFFER_SIZE = 1000000;

        public const int PageSize = 5;

        public const long SegmentSize = 1048576; // Tamaño de segmento en bytes (1 MB)

        public const int TIMEOUT = 10;

        public const int MaxFileSize = 100 * 1024 * 1024; // 500 MB in bytes

        public const int MaxRequestBodySize = 100 * 1024 * 1024; // 500 MB in bytes

        public const string FILENAME = "mediacontent.png";
    }
}