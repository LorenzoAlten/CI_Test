using Caliburn.Micro;
using AgvMgr.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;
using mSwDllWPFUtils;
using mSwAgilogDll;
using AgilogDll;

namespace AgvMgr
{
    public class AppBootstrapper : AppBootstrapperBase
    {
        public AppBootstrapper() : base() { }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetExecutingAssembly(),
                           Assembly.GetAssembly(typeof(Global)),
                           Assembly.GetAssembly(typeof(AgvMovitrans)) };
        }

        protected override void Configure()
        {
            container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            CompositionBatch batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.Height = 800;
            settings.MinHeight = 800;
            settings.Width = 1280;
            settings.MinWidth = 1280;
            settings.Icon = Global.Instance.GetImageSourceWithTheme(GetImage("agv.png"));
            settings.Title = "";
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Maximized;
            settings.SizeToContent = SizeToContent.Manual;
            DisplayRootViewForAsync<AppViewModel>(settings);
        }
    }
}
