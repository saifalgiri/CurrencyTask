using System.ServiceModel;
using System.ServiceModel.Web;

namespace WCFService
{
    [ServiceContract]
    public interface IConverter
    {

        [OperationContract]
        [WebInvoke(Method = "GET",
          UriTemplate = "/GetData/{amount}", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string ConverstionRequest(string amount);

        [OperationContract]
        string NumberTophrase(double value);

    }
}
