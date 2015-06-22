using System;

namespace GenericBug.Core.Util.Storage
{
    /// <summary>
    /// This structure holds the information about storage path. It does emit its internal data either as string or
    /// <see cref="Enums.StoragePath"/> type according to usage so it must be handled with care.
    /// </summary>
    // ToDo: [TypeConverter(typeof(StoragePath))]
    public struct StoragePath
    {
        private static Enums.StoragePath storagePathEnum;
        private static string storagePathString;

        public StoragePath(Enums.StoragePath storagePath)
        {
            storagePathEnum = storagePath;
        }

        public StoragePath(string storagePath)
        {
            storagePathString = storagePath;
            storagePathEnum = Enums.StoragePath.Custom;
        }

        public static implicit operator StoragePath(Enums.StoragePath path)
        {
            return new StoragePath(path);
        }

        public static implicit operator StoragePath(string path)
        {
            foreach (Enums.StoragePath value in Enum.GetValues(typeof(Enums.StoragePath)))
            {
                if (value.ToString() == path)
                {
                    return new StoragePath(value);
                }
            }

            return new StoragePath(path);
        }

        public static implicit operator Enums.StoragePath(StoragePath path)
        {
            return storagePathEnum;
        }

        public static implicit operator string(StoragePath path)
        {
            return storagePathString;
        }
    }
}
