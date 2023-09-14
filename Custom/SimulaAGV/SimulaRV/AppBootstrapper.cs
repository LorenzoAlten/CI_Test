using Caliburn.Micro;
using mSwDllWPFUtils;
using mSwDllWPFUtils.Caliburn;
using SimulaRV.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace SimulaRV
{
    class AppBootstrapper : AppBootstrapperBase
    {
        public AppBootstrapper() : base() { }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly(),
                           Assembly.GetAssembly(typeof(Global)) };
        }

        protected override void Configure()
        {
            container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            CompositionBatch batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new AppWindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.Height = 750;
            settings.MinHeight = 600;
            settings.Width = 800;
            settings.MinWidth = 800;
            settings.Icon = Global.Instance.GetImageSourceWithTheme(GetImage("Auto.png"));
            settings.Title = "SimulaRV";
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Normal;
            settings.SizeToContent = SizeToContent.Manual;
            DisplayRootViewFor<AppViewModel>(settings);
        }
    }
}
