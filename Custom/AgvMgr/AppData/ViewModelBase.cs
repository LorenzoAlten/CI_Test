using Caliburn.Micro;
using mSwDllEntities;
using mSwDllUtils;
using mSwDllWPFUtils;
using AgvMgr.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Threading;

namespace AgvMgr.ViewModels
{
    class ViewModelBase : Conductor<Screen>
    {
        #region Members

        protected readonly IWindowManager _windowManager;
        protected readonly IEventAggregator _eventAggregator;

        protected bool _IsLoading = false;
        protected string _SnackBarMessage;

        protected SqlConnection _conn;
        protected bool _waitExecution;
        protected DateTime _lastExec;
        protected int _execTimeout;
        protected bool _firstTimeDone;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);

                _eventAggregator.PublishOnUIThreadAsync(!value);
            }
        }

        public string SnackBarMessage
        {
            get { return _SnackBarMessage; }
            set
            {
                // Forzo cambio di valore
                _SnackBarMessage = string.Empty;
                NotifyOfPropertyChange(() => SnackBarMessage);

                _SnackBarMessage = value;
                NotifyOfPropertyChange(() => SnackBarMessage);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ViewModelBase(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region ViewModel Override

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);

            return base.OnInitializeAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (_conn != null && _conn.State != ConnectionState.Closed)
                _conn.Close();

            _eventAggregator.Unsubscribe(this);

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion

        #region Public methods

        #endregion

        #region Private methods

        #endregion

        #region Global Events

        #endregion

        #region Events

        #endregion
    }
}
