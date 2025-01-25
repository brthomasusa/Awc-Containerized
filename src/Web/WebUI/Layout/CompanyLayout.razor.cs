using System.Reflection;

namespace WebUI.Layout
{
    public partial class CompanyLayout
    {
        string currentVersion = string.Empty;

        protected override void OnInitialized()
        {

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            currentVersion = $"AdventureWorksCycles v{currentAssembly.GetName().Version}";

            base.OnInitialized();
        }
    }
}