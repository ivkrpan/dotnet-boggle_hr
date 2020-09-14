using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BoggleService
{

    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IBoggleService
    {
        [OperationContract]
        bool napraviIgru();

        [OperationContract]
        IEnumerable<String> dohvatiSlova();

        [OperationContract]
        string provjeriRijec(string rijec);

        [OperationContract]
        int preostaloVremena();

        [OperationContract]
        List<KeyValuePair<string, string>> popisRezultata();

    }

}
