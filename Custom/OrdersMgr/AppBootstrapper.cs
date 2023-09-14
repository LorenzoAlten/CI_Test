using Caliburn.Micro;
using mSwDllWPFUtils;
using mSwDllWPFUtils.Caliburn;
using OrdersMgr.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OrdersMgr
{
    class AppBootstrapper : AppBootstrapperBase
    {
        public AppBootstrapper() : base() { }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] 
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(mSwAgilogDll.MisOrder)),
                //Assembly.GetAssembly(typeof(ExitListViewModel)),
                Assembly.GetAssembly(typeof(Global))
            };
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
            DisplayRootViewForAsync<AppViewModel>();
        }
    }
}
