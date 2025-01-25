using System.Reflection;

namespace WebUI.Layout
{
    public partial class MainLayout
    {
        string currentVersion = string.Empty;
        private bool sidebarExpanded = true;

        protected override void OnInitialized()
        {

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            currentVersion = $"AdventureWorksCycles v{currentAssembly.GetName().Version}";

            base.OnInitialized();
        }
    }
}