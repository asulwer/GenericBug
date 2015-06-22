namespace GenericBug.Core.Util.Storage
{
    public enum StoredItemType
    {
        Exception,
        Report,
        MiniDump
    }

    // This class must remain internal otherwise it should not use constant strings
    public static class StoredItemFile
    {
        public const string Exception = "Exception.xml";
        public const string Report = "Report.xml";
        public const string MiniDump = "MiniDump.mdmp";
    }
}
