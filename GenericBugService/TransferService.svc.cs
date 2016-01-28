using System.IO;

namespace GenericBugService
{
    public class TransferService : ITransferService
    {
        public void UploadFile(RemoteFileInfo rfi)
        {
            FileStream targetStream = null;

            using (Stream sourceStream = rfi.FileByteStream)
            {
                using (targetStream = new FileStream(rfi.ServerLocation, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    const int bufferLen = 65000;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;

                    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        targetStream.Write(buffer, 0, count);
                    }
                }
            }
        }
    }
}
