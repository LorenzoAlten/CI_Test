using mSwAgilogDll;
using Caliburn.Micro;
using EntranceMgr.ViewModels;
using mSwDllWPFUtils;
using mSwDllWPFUtils.Caliburn;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EntranceMgr
{
    class AppBootstrapper : AppBootstrapperBase
    {
        public AppBootstrapper() : base() { }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] 
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(UdcUdc)),
                Assembly.GetAssembly(typeof(Global))
            };
        }

        protected override void Configure()
        {
            Manager.Instance.Standalone = true;

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
            settings.MinHeight = 768;
            settings.MinWidth = 1024;
            settings.Icon = Global.Instance.GetImageSourceWithTheme(GetImage("entrance.png"));
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.WindowState = WindowState.Maximized;
            settings.SizeToContent = SizeToContent.Manual;
            DisplayRootViewForAsync<AppViewModel>(settings);
        }
    }
}
