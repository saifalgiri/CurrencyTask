using System.Waf.Applications;

namespace WPFClient.Applications.Views
{
    internal interface IShellView : IView
    {
        void Show();

        void Close();
    }
}
