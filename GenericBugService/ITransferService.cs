using System;
using System.IO;
using System.ServiceModel;

namespace GenericBugService
{
    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string ServerLocation;

        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    [ServiceContract]
    public interface ITransferService
    {
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [FaultContract(typeof(Exception))]
        void UploadFile(RemoteFileInfo rfi);
    }    
}
