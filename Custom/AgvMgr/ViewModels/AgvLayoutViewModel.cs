using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using mSwAgilogDll;
using HelixToolkit.Wpf;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using mSwDllGraphics;
using mSwDllUtils;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using mSwDllWPFUtils;
using System;
using System.Windows.Media.Imaging;
using mSwAgilogDll.SEW;
using static mSwAgilogDll.SEW.AgvPath;
using static mSwAgilogDll.SEW.AgvPath.VTrack;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using AgvMgr.Entites;
using System.Threading;
using AgvMgr.AppData;
using System.ComponentModel;
using System.Diagnostics;

namespace AgvMgr.ViewModels
{
    [Export(typeof(AgvLayoutViewModel))]
    public class AgvLayoutViewModel : Screen
    {
        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        private bool _flip_flop = false;

        private Visibility _commandAgvVisibility = Visibility.Collapsed;
        private bool _expandDetails = false;
        public SEWAgvViewModel _selectedAgvModel;

        protected List<AgvStation> _agvStations;  // Stazioni di Prelievo/deposito
        protected AgvStation _selectedStation;
        protected AgvStation _bufferedStation;

        private bool _stationInsert = false;
        private bool _stationEdit = false;

        private bool _showTransponders = false;
        private bool _showBackground = true;
        private bool _showConveyor = true;
        public HelixViewport3D View3D { get; } = new HelixViewport3D();
        private SortedList _Materials = new SortedList();
        private static List<AgvModel3D> _Agv3DModels;

        private static Graphic3DObject Conveyor = Graphic3DTools.ListaOggetti["Conveyor"].Clone();
        private static Graphic3DObject ConveyorAlarm = Graphic3DTools.ListaOggetti["ConveyorAlarm"].Clone();
        private static Graphic3DObject ConveyorNoAuto = Graphic3DTools.ListaOggetti["ConveyorNoAuto"].Clone();
        private static Graphic3DObject PalletGraphicObject = Graphic3DTools.ListaOggetti["Pallet"].Clone();
        private static Graphic3DObject PalletDeselectedGraphicObject = Graphic3DTools.ListaOggetti["PalletDeselected"].Clone();
        private static Graphic3DObject BoxGraphicObject = Graphic3DTools.ListaOggetti["Cubo"].Clone();
        private static Graphic3DObject BoxDeselectedGraphicObject = Graphic3DTools.ListaOggetti["CuboDeselected"].Clone();

        private bool _simulationOn = true;
        private bool _simulationEnabled = false;

        private string _cameraCordinate;

        private BackgroundWorker _AgvModel3DWorker;

        private double _AgvCameraCoordinateX;
        private double _AgvCameraCoordinateY;
        private double _AgvCameraCoordinateZ;
        private double _AgvCameraDirectionX;
        private double _AgvCameraDirectionY;
        private double _AgvCameraDirectionZ;
        private double _AgvCameraUpDirectionX;
        private double _AgvCameraUpDirectionY;
        private double _AgvCameraUpDirectionZ;

        #endregion

        #region Properties

        public List<SEWAgvViewModel> AgvViewModels { get; private set; } = new List<SEWAgvViewModel>();

        public bool StationInsert
        {
            get { return _stationInsert; }
            set
            {
                _stationInsert = value;
                NotifyOfPropertyChange(() => StationInsert);
            }
        }
        public bool StationEdit
        {
            get { return _stationEdit; }
            set
            {
                _stationEdit = value;
                NotifyOfPropertyChange(() => StationEdit);
            }
        }
        public string CameraCordinate
        {
            get { return _cameraCordinate; }
            set
            {
                _cameraCordinate = value;
                NotifyOfPropertyChange(() => CameraCordinate);
            }
        }
        public bool SimulationEnabled
        {
            get { return _simulationEnabled; }
            set
            {
                _simulationEnabled = value;
                NotifyOfPropertyChange(() => SimulationEnabled);
            }
        }
        public bool SimulationOn
        {
            get { return _simulationOn; }
            set
            {
                _simulationOn = value;
                NotifyOfPropertyChange(() => SimulationOn);
                NotifyOfPropertyChange(() => SimulationOff);
            }
        }
        public bool SimulationOff
        {
            get { return !_simulationOn; }
        }
        public List<AgvStation> AgvStations
        {
            get { return _agvStations; }
            set
            {
                _agvStations = value;
                NotifyOfPropertyChange(() => AgvStations);
            }
        }
        public AgvStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                NotifyOfPropertyChange(() => SelectedStation);
            }
        }
        public bool ExpandDetails
        {
            get { return _expandDetails; }
            set
            {
                _expandDetails = value;
                NotifyOfPropertyChange(() => ExpandDetails);
            }
        }
        public Visibility CommandAgvVisibility
        {
            get { return _commandAgvVisibility; }
            set
            {
                _commandAgvVisibility = value;
                NotifyOfPropertyChange(() => CommandAgvVisibility);
            }
        }
        public bool ShowTransponders
        {
            get { return _showTransponders; }
            set
            {
                _showTransponders = value;
                NotifyOfPropertyChange(() => ShowTransponders);

                if (_showTransponders == false)
                {
                    for (int i = 0; i < View3D.Children.Count; i++)
                    {
                        if (View3D.Children[i] is CustomizedModelVisual3D)
                        {
                            if (((CustomizedModelVisual3D)View3D.Children[i]).IsTransponder == true)
                            {
                                View3D.Children.Remove(View3D.Children[i]);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    DrawTransponder();
                }
            }
        }
        public bool ShowBackground
        {
            get { return _showBackground; }
            set
            {
                _showBackground = value;
                if (_showBackground == false)
                {
                    for (int i = 0; i < View3D.Children.Count; i++)
                    {
                        if (View3D.Children[i] is CustomizedModelVisual3D)
                        {
                            if (((CustomizedModelVisual3D)View3D.Children[i]).IsSfondo == true)
                            {
                                View3D.Children.Remove(View3D.Children[i]);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    DrawBackground();
                }
                NotifyOfPropertyChange(() => ShowTransponders);
            }
        }
        public bool ShowConveyor
        {
            get { return _showConveyor; }
            set
            {
                _showConveyor = value;
                if (_showConveyor == false)
                {
                    for (int i = 0; i < View3D.Children.Count; i++)
                    {
                        if (View3D.Children[i] is CustomizedModelVisual3D)
                        {
                            if (((CustomizedModelVisual3D)View3D.Children[i]).IsConveyor == true)
                            {
                                View3D.Children.Remove(View3D.Children[i]);
                                i--;
                            }
                        }
                    }
                }
                else
                {
                    DrawConveyors();
                }
                NotifyOfPropertyChange(() => ShowTransponders);
            }
        }

        public bool ShowAgvsPath { get; set; } = true;

        public AgvPath AgvPath { get; set; }

        public SEWAgvViewModel SelectedAgvModel
        {
            get { return _selectedAgvModel; }
            set
            {
                _selectedAgvModel = value;
                NotifyOfPropertyChange(() => SelectedAgvModel);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AgvLayoutViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, List<AgvStation> agvStations)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _AgvModel3DWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            _AgvModel3DWorker.DoWork += AgvModel3DWorker_DoWork;

            Common.Instance.Agvs?.ForEach(x => AgvViewModels.Add(new SEWAgvViewModel(windowManager, eventAggregator, x)));
            SelectedAgvModel = AgvViewModels.FirstOrDefault();

            AgvPath = GetInstance();
            AgvPath.LoadTransponderDataFromXml();
            AgvPath.LoadGraphicDataFromXml();
            AgvStations = agvStations;
            SelectedStation = AgvStations.FirstOrDefault();

            InitializeEnvironment();

            if (ShowTransponders)
                DrawTransponder();

            if (ShowConveyor)
                DrawConveyors();

            if (ShowBackground)
                DrawBackground();

            Global.Instance.OnEvery100mSec += Global_OnEvery100mSec;

            InitAgvGraphics();
        }

        #endregion

        #region Events

        private void Global_OnEvery100mSec(object sender, GenericEventArgs e)
        {
            if (Global.Instance.Repetitions100mSec % 10 == 0)
            {
                if (ShowConveyor)
                    EditConveyors();
            }

            //if (Global.Instance.Repetitions100mSec % 6 == 0)
            //{
            _flip_flop = !_flip_flop;
            //}
        }

        #endregion

        #region Initialization

        private void InitializeEnvironment()
        {
            _AgvCameraCoordinateX = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraCoordinate").Split(',')[0].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraCoordinateY = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraCoordinate").Split(',')[1].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraCoordinateZ = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraCoordinate").Split(',')[2].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraDirectionX = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraDirection").Split(',')[0].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraDirectionY = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraDirection").Split(',')[1].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraDirectionZ = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraDirection").Split(',')[2].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraUpDirectionX = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraUpDirection").Split(',')[0].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraUpDirectionY = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraUpDirection").Split(',')[1].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            _AgvCameraUpDirectionZ = double.Parse(AppConfig.GetXmlKeyValue("Settings", "AgvCameraUpDirection").Split(',')[2].Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

            InitGestures();

            if (_AgvCameraCoordinateX == 0 &&
                _AgvCameraCoordinateY == 0 &&
                _AgvCameraCoordinateZ == 0)
            {
                View3D.ZoomExtentsWhenLoaded = true;
            }

            View3D.VerticalAlignment = VerticalAlignment.Stretch;
            View3D.VerticalContentAlignment = VerticalAlignment.Stretch;

            SetLights();

            View3D.InputBindings.Add(new MouseBinding(new PointSelectionCommand(View3D.Viewport, VisualSelectionEvent), new MouseGesture(MouseAction.LeftClick)));
            View3D.Camera.Changed += Camera_Changed;
        }

        private void SetLights()
        {
            var lights = new DefaultLights();
            View3D.Children.Add(lights);
        }

        private void InitGestures()
        {
            View3D.IsMoveEnabled = true;

            View3D.ShowCoordinateSystem = true;
            View3D.CoordinateSystemHeight += 30;
            View3D.CoordinateSystemWidth += 30;

            View3D.TopViewGesture = new KeyGesture(Key.Z, ModifierKeys.Control);
            View3D.LeftViewGesture = new KeyGesture(Key.X, ModifierKeys.Control);
            View3D.BackViewGesture = new KeyGesture(Key.Y, ModifierKeys.Control);

            View3D.BottomViewGesture = null;
            View3D.RightViewGesture = null;
            View3D.FrontViewGesture = null;

            View3D.CalculateCursorPosition = true;
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (!_AgvModel3DWorker.IsBusy)
            {
                _AgvModel3DWorker.RunWorkerAsync();
            }

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _AgvModel3DWorker.CancelAsync();

            return base.OnDeactivateAsync(close, cancellationToken);

        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (_AgvCameraCoordinateX != 0 || _AgvCameraCoordinateY != 0 || _AgvCameraCoordinateZ != 0)
            {
                ZoomReset();
            }

            View3D.CameraController.ZoomSensitivity = 0.5;

            if (!Global.Instance.SendCommandTrafficManager("PLC", "SIMULATION_STATUS", new Dictionary<string, object>(), out string error))
            {
                switch (error)
                {
                    case "DISABLED":
                        SimulationOn = false;
                        SimulationEnabled = false;
                        break;
                    case "OFF":
                        SimulationOn = false;
                        SimulationEnabled = true;
                        break;
                    case "ON":
                        SimulationOn = true;
                        SimulationEnabled = true;
                        break;
                    default:
                        SimulationOn = false;
                        SimulationEnabled = false;
                        break;
                }
            }
        }

        private void Camera_Changed(object sender, EventArgs e)
        {
            CameraCordinate = "X: " + View3D.Camera.Position.X.ToString("0.0") + " Y: " + View3D.Camera.Position.Y.ToString("0.0") + " Z: " + View3D.Camera.Position.Z.ToString("0.0") +
                              "\nX: " + View3D.Camera.LookDirection.X.ToString("0.0") + " Y: " + View3D.Camera.LookDirection.Y.ToString("0.0") + " Z: " + View3D.Camera.LookDirection.Z.ToString("0.0") +
                              "\nX: " + View3D.Camera.UpDirection.X.ToString("0.0") + " Y: " + View3D.Camera.UpDirection.Y.ToString("0.0") + " Z: " + View3D.Camera.UpDirection.Z.ToString("0.0");
        }

        //------------------------------------------------------------------------------------------------------------------
        // Evento che mi gestisce un clic del Mouse passandomi anche eventualmente l'oggetto visuale che è stato cliccato
        //------------------------------------------------------------------------------------------------------------------
        private void VisualSelectionEvent(object sender, VisualsSelectedEventArgs args)
        {
            //--------------------------------------------------------------------------------------
            // Setto l'agv o la rulliera selezionata
            //--------------------------------------------------------------------------------------
            VisualsSelectedByPointEventArgs vp = args as VisualsSelectedByPointEventArgs;
            if (vp.SelectedVisuals != null)
            {
                //----------------------------------------------------------------
                // Selezione di un Agv
                //----------------------------------------------------------------
                CustomizedModelVisual3D selectedVisual = (CustomizedModelVisual3D)vp.SelectedVisuals.Where(vis => vis is CustomizedModelVisual3D && ((CustomizedModelVisual3D)vis).IsAgv == true).FirstOrDefault();
                if (selectedVisual != null)
                {
                    SelectedAgvModel = AgvViewModels.FirstOrDefault(x => x.Agv.AGV_Code == selectedVisual.AgvCode);

                    // Se clicco su un agv espando la view dell'agv
                    ExpandDetails = true;
                }
                else
                {
                    //----------------------------------------------------------------
                    // Selezione di una Rulliera
                    //----------------------------------------------------------------
                    selectedVisual = (CustomizedModelVisual3D)vp.SelectedVisuals.FirstOrDefault(vis => vis is CustomizedModelVisual3D && ((CustomizedModelVisual3D)vis).IsConveyor == true);
                    if (selectedVisual != null)
                    {
                        SelectedStation = _agvStations.FirstOrDefault(s => s.MOD_Code == selectedVisual.ConveyorCode);
                    }
                    // Se clicco fuori da un agv comprimo la view dell'agv
                    ExpandDetails = false;
                }
            }
            else
            {
                // Se clicco fuori da un agv comprimo la view dell'agv
                ExpandDetails = false;
            }
            //--------------------------------------------------------------------------------------------------------------------------------
            // Se sono in stato di inserimento o modifica di una rulliera al click aggiorno le coordinate della rulliera col punto cliccato
            //--------------------------------------------------------------------------------------------------------------------------------
            if (StationInsert || StationEdit)
            {
                SelectedStation.MOD_QuotaX = (int)View3D.CursorOnConstructionPlanePosition.Value.X;
                SelectedStation.MOD_QuotaY = (int)View3D.CursorOnConstructionPlanePosition.Value.Y;
                if (StationEdit)
                {
                    View3D.Children.Remove(View3D.Children.Where(c => c is CustomizedModelVisual3D && ((CustomizedModelVisual3D)c).IsConveyor && ((CustomizedModelVisual3D)c).ConveyorCode == SelectedStation.MOD_Code).FirstOrDefault());
                    DrawConveyors(SelectedStation.MOD_Code);
                }
            }
        }

        #endregion

        #region Draw

        private void AgvModel3DWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (_AgvModel3DWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                foreach (AgvModel3D agv in _Agv3DModels)
                {
                    try
                    {
                        OnUIThread(() => DrawAgvs(agv));
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void DrawAgvs(AgvModel3D agv)
        {
            agv.RefreshVisual(_flip_flop);

            for (int i = 0; i < View3D.Children.Count; i++)
            {
                if (View3D.Children[i] is CustomizedModelVisual3D)
                {
                    CustomizedModelVisual3D pathModel = ((CustomizedModelVisual3D)View3D.Children[i]);
                    if (pathModel.IsPath == true &&
                        pathModel.AgvCode == agv.Agv.AGV_Code)

                    {
                        View3D.Children.Remove(View3D.Children[i]);
                        break;
                    }
                }
            }

            if (!(SelectedAgvModel.Agv == agv.Agv || ShowAgvsPath)) return;

            //--------------------------------------------------------------------------
            // Mi creo il Visual Model che conterrà il disegno del percorso
            //--------------------------------------------------------------------------
            CustomizedModelVisual3D PathVisualModel = new CustomizedModelVisual3D();
            PathVisualModel.IsPath = true;
            PathVisualModel.AgvCode = agv.Agv.AGV_Code;
            // Inizializzo l'oggetto che mi costruisce il disegno del percorso 
            // e l'insieme dei puni che lo rappresentano
            LineGeometryBuilder pathConstructor = new LineGeometryBuilder(PathVisualModel);
            List<Point3D> puntiPath = new List<Point3D>();
            puntiPath.Clear();
            //---------------------------------------------------------------------------------------
            // Scorro tutti i VTrack che rappresentano il percorso che sono nel mio oggetto AgvPath 
            // per ricavare una sequenza di punti che rappresenta il percorso
            //---------------------------------------------------------------------------------------
            List<int> transponderList = new List<int>();
            if (agv.Agv.Part_data > 0)
            {
                int dist = AgvPath.DistanceFromVTrackToStation(agv.Agv.Part_data, agv.Agv.Virtual_track_ID, agv.Agv.Transponder, agv.Agv.Part_data, agv.Agv.DistanceToTransponder, out transponderList);
                if (dist < 0)
                    dist = AgvPath.DistanceFromTransponderToStation(agv.Agv.Part_data, agv.Agv.Transponder, agv.Agv.Part_data, agv.Agv.DistanceToTransponder, out transponderList);
                if (transponderList.Count > 1)
                {
                    foreach (int transponderInt in transponderList)
                    {
                        if (transponderList.Last() == transponderInt) break;
                        Node nodo = AgvPath.Transponders[transponderInt];
                        Routing routing = nodo.PartDataRouting.Where(r => (r.PartData_start <= agv.Agv.Part_data && r.PartData_end >= agv.Agv.Part_data) || (r.PartData_start == 0 && r.PartData_end == 0)).FirstOrDefault();

                        List<Point3D> punti = new List<Point3D>();
                        punti.Clear();
                        bool spline = false;
                        punti.Add(new Point3D(nodo.Y, -nodo.X, 100));
                        foreach (InterpolationPoint point in routing.Vtrack.Points)
                        {
                            spline = true;  // se ci sono dei punti di interpolazione per andare al transponder successivo suppongo che sia una spline
                            punti.Add(new Point3D(point.Y, -point.X, 100));
                        }
                        punti.Add(new Point3D(AgvPath.Transponders[routing.Next_transponder].Y, -AgvPath.Transponders[routing.Next_transponder].X, 100));
                        //----------------------------------------------------------------------------
                        // Le Spline sono rappresentate da un limitato numeor di punti di controllo
                        // per evitare che vanga visualizzata spezzettata mi ricavo un  
                        // numero (molto più alto) di punti intermedi
                        //----------------------------------------------------------------------------
                        if (spline)
                        {
                            List<Point3D> puntiSpline = CanonicalSplineHelper.CreateSpline(punti, 0.5, null, false, 300);
                            punti.Clear();
                            punti.Add(puntiSpline[0]);
                            foreach (Point3D punto in puntiSpline.GetRange(1, puntiSpline.Count - 2))
                            {
                                punti.Add(punto);
                                punti.Add(punto);
                            }
                            punti.Add(puntiSpline.Last());
                        }
                        puntiPath.AddRange(punti);

                    }
                    int prova = puntiPath.FindIndex(p => (p.X >= agv.Agv.Absolute_Y - 500 && p.X <= agv.Agv.Absolute_Y + 500) && (p.Y >= -agv.Agv.Absolute_X - 500 && p.Y <= -agv.Agv.Absolute_X + 500));
                    if (prova > 0)
                        puntiPath.RemoveRange(0, prova + 1);
                    else
                    {
                        puntiPath.Add(new Point3D(agv.Agv.Absolute_Y, -agv.Agv.Absolute_X, 100));
                    }
                    // Tramite l'oggetto dell'Helixtoolkit creo la Geometria del percorso che è una linea spezzata che congiunge tutti i punti
                    MeshGeometry3D pathGeometry = new MeshGeometry3D
                    {
                        Positions = pathConstructor.CreatePositions(puntiPath, 100, 0),
                        TriangleIndices = pathConstructor.CreateIndices(puntiPath.Count)
                    };
                    pathGeometry.CalculateNormals();
                    // Creo il Modello 3D
                    GeometryModel3D pathModel = new GeometryModel3D
                    {
                        Material = new DiffuseMaterial(agv.Colore),
                        BackMaterial = new DiffuseMaterial(agv.Colore),
                        Geometry = pathGeometry
                    };
                    PathVisualModel.Content = pathModel;
                    //--------------------------------------------------------------------------
                    // Aggiungo il modello 3D che rappresenta il percorso
                    //--------------------------------------------------------------------------
                    View3D.Children.Insert(0, PathVisualModel);
                }
            }
        }

        public void DrawTransponder()
        {
            //----------------------------------------------------------------------
            // Carico la generica Rulliera
            //----------------------------------------------------------------------
            Graphic3DObject Transponder = Graphic3DTools.ListaOggetti["Transponder"].Clone();
            Graphic3DObject Freccia = Graphic3DTools.ListaOggetti["Freccia"].Clone();

            Model3DGroup transponderModels = new Model3DGroup();
            Model3DGroup frecciaModels = new Model3DGroup();
            Model3DGroup testoModels = new Model3DGroup();

            //----------------------------------------------------------------------
            // Preparo un modello 3D per ogni Transponder
            //----------------------------------------------------------------------
            foreach (var item in AgvPath.Transponders)
            {
                Node transponder = item.Value;

                transponderModels.Children.Add(Transponder.GraphicRepresentation.Clone());
                var matrix = transponderModels.Children.Last().Transform.Value;
                matrix.Translate(new Vector3D(transponder.Y, -transponder.X, 1));
                transponderModels.Children.Last().Transform = new MatrixTransform3D(matrix);

                frecciaModels.Children.Add(Freccia.GraphicRepresentation.Clone());
                matrix = frecciaModels.Children.Last().Transform.Value;
                var axis = new Vector3D(0, 0, 1);
                matrix.Rotate(new Quaternion(axis, transponder.Angle));
                matrix.Translate(new Vector3D(transponder.Y, -transponder.X, 1));
                frecciaModels.Children.Last().Transform = new MatrixTransform3D(matrix);

                TextVisual3D testo = new TextVisual3D
                {
                    Height = 500,
                    Text = "" + transponder.Transponder,
                    Position = new Point3D(transponder.Y, -transponder.X, 1200)
                };
                testoModels.Children.Add(testo.Content);
            }
            //--------------------------------------------------------------------------
            // Aggiungo i modelli 3D che rappresentano il pallino per ogni Transponder
            //--------------------------------------------------------------------------
            CustomizedModelVisual3D TransponderVisualModel = new CustomizedModelVisual3D
            {
                IsTransponder = true,
                Content = transponderModels
            };
            View3D.Children.Insert(0, TransponderVisualModel);
            //--------------------------------------------------------------------------
            // Aggiungo i modelli 3D che rappresentano la freccia per ogni Transponder
            //--------------------------------------------------------------------------
            CustomizedModelVisual3D FrecciaVisualModel = new CustomizedModelVisual3D
            {
                IsTransponder = true,
                Content = frecciaModels
            };
            View3D.Children.Insert(0, FrecciaVisualModel);
            //--------------------------------------------------------------------------
            // Aggiungo i modelli 3D che rappresentano il testo per ogni Transponder
            //--------------------------------------------------------------------------
            CustomizedModelVisual3D TestoVisualModel = new CustomizedModelVisual3D
            {
                IsTransponder = true,
                Content = testoModels
            };
            View3D.Children.Insert(0, TestoVisualModel);

            //--------------------------------------------------------------------------
            // Mi creo il Visual Model che conterrà il disegno del percorso
            //--------------------------------------------------------------------------
            CustomizedModelVisual3D PathVisualModel = new CustomizedModelVisual3D();
            PathVisualModel.IsTransponder = true;

            // Inizializzo l'oggetto che mi costruisce il disegno del percorso 
            // e l'insieme dei puni che lo rappresentano
            LineGeometryBuilder pathConstructor = new LineGeometryBuilder(PathVisualModel);
            List<Point3D> puntiPath = new List<Point3D>();
            puntiPath.Clear();
            //---------------------------------------------------------------------------------------
            // Scorro tutti i VTrack che rappresentano il percorso che sono nel mio oggetto AgvPath 
            // per ricavare una sequenza di punti che rappresenta il percorso
            //---------------------------------------------------------------------------------------
            foreach (Node nodo in AgvPath.Transponders.Values.ToList())
            {
                foreach (Routing routing in nodo.PartDataRouting)
                {
                    List<Point3D> punti = new List<Point3D>();
                    punti.Clear();
                    bool spline = false;
                    punti.Add(new Point3D(nodo.Y, -nodo.X, 0));
                    foreach (InterpolationPoint point in routing.Vtrack.Points)
                    {
                        spline = true;  // se ci sono dei punti di interpolazione per andare al transponder successivo suppongo che sia una spline
                        punti.Add(new Point3D(point.Y, -point.X, 0));
                    }
                    punti.Add(new Point3D(AgvPath.Transponders[routing.Next_transponder].Y, -AgvPath.Transponders[routing.Next_transponder].X, 0));
                    //----------------------------------------------------------------------------
                    // Le Spline sono rappresentate da un limitato numeor di punti di controllo
                    // per evitare che vanga visualizzata spezzettata mi ricavo un  
                    // numero (molto più alto) di punti intermedi
                    //----------------------------------------------------------------------------
                    if (spline)
                    {

                        List<Point3D> puntiSpline = CanonicalSplineHelper.CreateSpline(punti, 0.5, null, false, 250);
                        punti.Clear();
                        punti.Add(puntiSpline[0]);
                        foreach (Point3D punto in puntiSpline.GetRange(1, puntiSpline.Count - 2))
                        {
                            punti.Add(punto);
                            punti.Add(punto);
                        }
                        punti.Add(puntiSpline.Last());
                    }
                    puntiPath.AddRange(punti);
                }
            }
            // Tramite l'oggetto dell'Helixtoolkit creo la Geometria del percorso che è una linea spezzata che congiunge tutti i punti
            MeshGeometry3D pathGeometry = new MeshGeometry3D
            {
                Positions = pathConstructor.CreatePositions(puntiPath, 50, -1),
                TriangleIndices = pathConstructor.CreateIndices(puntiPath.Count)
            };
            pathGeometry.CalculateNormals();
            // Creo il Modello 3D
            GeometryModel3D pathModel = new GeometryModel3D
            {
                Material = new DiffuseMaterial(Brushes.Aqua),
                BackMaterial = new DiffuseMaterial(Brushes.Aqua),
                Geometry = pathGeometry
            };
            PathVisualModel.Content = pathModel;
            //--------------------------------------------------------------------------
            // Aggiungo il modello 3D che rappresenta il percorso
            //--------------------------------------------------------------------------
            View3D.Children.Insert(0, PathVisualModel);
        }

        //public void EditConveyors()
        //{
        //    List<MisMissionAgv> listaMissioni = Common.Instance.MissionsList;

        //    foreach (CustomizedModelVisual3D conveyorVisualModel in View3D.Children.Where(c => c is CustomizedModelVisual3D))
        //    {
        //        AgvStation station = AgvStations.FirstOrDefault(s => s.MOD_Code == conveyorVisualModel.ConveyorCode);
        //        if (station != null)
        //        {
        //            long? mission = listaMissioni.FirstOrDefault(m => m.MIS_Dest_MOD_Code == station.MOD_Code || m.MIS_Source_MOD_Code == station.MOD_Code)?.MIS_Id;
        //            //--------------------------------------------------------------------
        //            // Controllo se è cambiato uno stato "visuale" della stazione
        //            //--------------------------------------------------------------------
        //            if (conveyorVisualModel.Udc != station.Udc ||
        //                conveyorVisualModel.ConveyorStatus != station.Status ||
        //                conveyorVisualModel.Destination != station.Destination ||
        //                conveyorVisualModel.Mission != mission)
        //            {
        //                //----------------------------------------------------------------------
        //                // Presenza mancante sopra la rulliera
        //                //----------------------------------------------------------------------
        //                if (station.Udc == null || station.Udc.Count() <= 0)
        //                {
        //                    //-------------------------------------------------------------
        //                    // Se il 3D ha un pallet disegnato sopra la rulliera lo tolgo
        //                    //-------------------------------------------------------------
        //                    if (conveyorVisualModel.Children.Count > 1)
        //                    {
        //                        conveyorVisualModel.Children.RemoveAt(0);
        //                        conveyorVisualModel.Children.RemoveAt(0);
        //                    }
        //                }
        //                //----------------------------------------------------------------------
        //                // Presenza sopra la rulliera
        //                //----------------------------------------------------------------------
        //                else
        //                {
        //                    //-------------------------------------------------------------
        //                    // Se il 3D non ha un pallet disegnato sopra ce lo metto
        //                    //-------------------------------------------------------------
        //                    if (conveyorVisualModel.Children.Count < 2)
        //                    {
        //                        ModelVisual3D Pallet = new ModelVisual3D();
        //                        ModelVisual3D Box = new ModelVisual3D();
        //                        //------------------------------------------------------------------
        //                        // In espulsione visualizzo il pallet "acceso"
        //                        //------------------------------------------------------------------
        //                        if (station.Status.HasFlag(Agv_Station_Status.Expulsion))
        //                        {
        //                            Pallet.Content = PalletGraphicObject.GraphicRepresentation;
        //                            Box.Content = BoxGraphicObject.GraphicRepresentation;
        //                        }
        //                        //------------------------------------------------------------------
        //                        // Senza espulsione visualizzo il pallet "spento"
        //                        //------------------------------------------------------------------
        //                        else
        //                        {
        //                            Pallet.Content = PalletDeselectedGraphicObject.GraphicRepresentation;
        //                            Box.Content = BoxDeselectedGraphicObject.GraphicRepresentation;
        //                        }
        //                        var matrixPallet = Pallet.Transform.Value;
        //                        matrixPallet.Translate(new Vector3D(0, 0, conveyorVisualModel.Content.Bounds.SizeZ + 5));
        //                        Pallet.Transform = new MatrixTransform3D(matrixPallet);
        //                        var matrixBox = Box.Transform.Value;
        //                        matrixBox.Translate(new Vector3D(0, 0, conveyorVisualModel.Content.Bounds.SizeZ + PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ + 10));
        //                        Box.Transform = new MatrixTransform3D(matrixBox);

        //                        conveyorVisualModel.Children.Insert(0, Pallet);
        //                        conveyorVisualModel.Children.Insert(1, Box);
        //                    }
        //                    //--------------------------------------------------------------------------------------------
        //                    // Se il 3D ha già un pallet disegnato controllo se si è attivata\disattivata l'espulsione
        //                    //--------------------------------------------------------------------------------------------
        //                    else
        //                    {
        //                        if (station.Status.HasFlag(Agv_Station_Status.Expulsion) != conveyorVisualModel.ConveyorStatus.HasFlag(Agv_Station_Status.Expulsion))
        //                        {
        //                            ModelVisual3D pallet = conveyorVisualModel.Children[0] as ModelVisual3D;
        //                            ModelVisual3D box = conveyorVisualModel.Children[1] as ModelVisual3D;
        //                            if (station.Status.HasFlag(Agv_Station_Status.Expulsion))
        //                            {
        //                                pallet.Content = PalletGraphicObject.GraphicRepresentation;
        //                                box.Content = BoxGraphicObject.GraphicRepresentation;
        //                            }
        //                            else
        //                            {
        //                                pallet.Content = PalletDeselectedGraphicObject.GraphicRepresentation;
        //                                box.Content = BoxDeselectedGraphicObject.GraphicRepresentation;
        //                            }
        //                        }
        //                    }
        //                }
        //                if (station.Status.HasFlag(Agv_Station_Status.Alarm))
        //                {
        //                    conveyorVisualModel.Content = ConveyorAlarm.GraphicRepresentation;
        //                }
        //                else if (!station.Status.HasFlag(Agv_Station_Status.Auto))
        //                {
        //                    conveyorVisualModel.Content = ConveyorNoAuto.GraphicRepresentation;
        //                }
        //                else
        //                {
        //                    conveyorVisualModel.Content = Conveyor.GraphicRepresentation;
        //                }
        //                if (mission != conveyorVisualModel.Mission)
        //                {
        //                    TextVisual3D visualTesto = conveyorVisualModel.Children.Last() as TextVisual3D;
        //                    string testo = station.MOD_Code;
        //                    if (mission != null) testo = testo + " Mis: " + mission;
        //                    visualTesto.Text = testo;
        //                    Thickness tick = visualTesto.BorderThickness;
        //                    tick.Left = 50;
        //                    tick.Right = 50;
        //                    tick.Top = 50;
        //                    tick.Bottom = 50;
        //                    visualTesto.BorderThickness = tick;
        //                }
        //                conveyorVisualModel.Udc = station.Udc;
        //                conveyorVisualModel.ConveyorStatus = station.Status;
        //                conveyorVisualModel.Destination = station.Destination;
        //                conveyorVisualModel.Mission = mission;
        //            }
        //        }
        //    }
        //}

        public void EditConveyors()
        {
            List<MisMissionAgv> listaMissioni = Common.Instance.MissionsList;

            foreach (CustomizedModelVisual3D conveyorVisualModel in View3D.Children.Where(c => c is CustomizedModelVisual3D && !string.IsNullOrEmpty(((CustomizedModelVisual3D)c).ConveyorCode)))
            {
                AgvStation station = AgvStations.FirstOrDefault(s => s.MOD_Code == conveyorVisualModel.ConveyorCode);
                if (station == null) continue;

                MisMissionAgv mission = listaMissioni.FirstOrDefault(m => m.MIS_Dest_MOD_Code == station.MOD_Code || m.MIS_Source_MOD_Code == station.MOD_Code);
                //if (mission == null) continue;

                //// Controllo se è cambiato uno stato "visuale" della stazione
                //if (mission.MIS_Current_MOD_Code == station.MOD_Code)
                //{
                //    ModelVisual3D Pallet = new ModelVisual3D();
                //    ModelVisual3D Box = new ModelVisual3D();
                //    Pallet.Content = PalletGraphicObject.GraphicRepresentation;
                //    Box.Content = BoxGraphicObject.GraphicRepresentation;

                //    var matrixPallet = Pallet.Transform.Value;
                //    matrixPallet.Translate(new Vector3D(0, 0, conveyorVisualModel.Content.Bounds.SizeZ + 5));
                //    Pallet.Transform = new MatrixTransform3D(matrixPallet);
                //    var matrixBox = Box.Transform.Value;
                //    matrixBox.Translate(new Vector3D(0, 0, conveyorVisualModel.Content.Bounds.SizeZ + PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ + 10));
                //    Box.Transform = new MatrixTransform3D(matrixBox);

                //    conveyorVisualModel.Children.Insert(0, Pallet);
                //    conveyorVisualModel.Children.Insert(1, Box);
                //}
                //else
                //{
                //    if (conveyorVisualModel.Children.Count > 1)
                //    {
                //        conveyorVisualModel.Children.RemoveAt(0);
                //        conveyorVisualModel.Children.RemoveAt(0);
                //    }
                //}

                //if (station.Status.HasFlag(Agv_Station_Status.Alarm))
                //{
                //    conveyorVisualModel.Content = ConveyorAlarm.GraphicRepresentation;
                //}
                //else if (!station.Status.HasFlag(Agv_Station_Status.Auto))
                //{
                //    conveyorVisualModel.Content = ConveyorNoAuto.GraphicRepresentation;
                //}
                //else
                //{
                //    conveyorVisualModel.Content = Conveyor.GraphicRepresentation;
                //}

                if (mission?.MIS_Id != conveyorVisualModel.Mission)
                {
                    TextVisual3D visualTesto = conveyorVisualModel.Children.Last() as TextVisual3D;
                    string testo = station.MOD_Code;
                    if (mission != null) testo = testo + " Mis: " + mission;
                    visualTesto.Text = testo;
                    Thickness tick = visualTesto.BorderThickness;
                    tick.Left = 50;
                    tick.Right = 50;
                    tick.Top = 50;
                    tick.Bottom = 50;
                    visualTesto.BorderThickness = tick;
                }
                //conveyorVisualModel.Udc = station.Udc;
                //conveyorVisualModel.ConveyorStatus = station.Status;
                conveyorVisualModel.Destination = station.Destination;
                conveyorVisualModel.Mission = mission?.MIS_Id;
            }
        }

        public void DrawConveyors()
        {
            foreach (HndModule conveyor in AgvStations)
            {
                if (conveyor.MOD_QuotaX != null && conveyor.MOD_QuotaY != null)
                {
                    CustomizedModelVisual3D ConveyorVisualModel = new CustomizedModelVisual3D
                    {
                        IsConveyor = true,
                        ConveyorCode = conveyor.MOD_Code,
                        Content = Conveyor.GraphicRepresentation
                    };

                    TextVisual3D testo = new TextVisual3D
                    {
                        Height = 500,
                        Text = conveyor.MOD_Code,
                        Position = new Point3D(0, 0, PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ +
                                                       BoxGraphicObject.GraphicRepresentation.Bounds.SizeZ +
                                                       Conveyor.GraphicRepresentation.Bounds.SizeZ + 500)
                    };
                    ConveyorVisualModel.Children.Add(testo);

                    View3D.Children.Add(ConveyorVisualModel);
                    var matrix = ConveyorVisualModel.Transform.Value;
                    if (conveyor.MOD_Angle != null)
                    {
                        var axis = new Vector3D(0, 0, 1);
                        matrix.Rotate(new Quaternion(axis, (double)conveyor.MOD_Angle));
                    }
                    matrix.Translate(new Vector3D((double)conveyor.MOD_QuotaX, (double)conveyor.MOD_QuotaY, 0));
                    ConveyorVisualModel.Transform = new MatrixTransform3D(matrix);
                }
            }
        }

        public void DrawConveyors(string code)
        {
            HndModule conveyor = AgvStations.FirstOrDefault(s => s.MOD_Code == code);
            if (conveyor.MOD_QuotaX != null && conveyor.MOD_QuotaY != null)
            {
                CustomizedModelVisual3D ConveyorVisualModel = new CustomizedModelVisual3D
                {
                    IsConveyor = true,
                    ConveyorCode = conveyor.MOD_Code,
                    //Content = Conveyor.GraphicRepresentation
                };

                TextVisual3D testo = new TextVisual3D
                {
                    Height = 500,
                    Text = conveyor.MOD_Code,
                    Position = new Point3D(0, 0, PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ +
                                                   BoxGraphicObject.GraphicRepresentation.Bounds.SizeZ +
                                                   Conveyor.GraphicRepresentation.Bounds.SizeZ + 500)
                };
                ConveyorVisualModel.Children.Add(testo);

                View3D.Children.Add(ConveyorVisualModel);
                var matrix = ConveyorVisualModel.Transform.Value;
                if (conveyor.MOD_Angle != null)
                {
                    var axis = new Vector3D(0, 0, 1);
                    matrix.Rotate(new Quaternion(axis, (double)conveyor.MOD_Angle));
                }
                matrix.Translate(new Vector3D((double)conveyor.MOD_QuotaX, (double)conveyor.MOD_QuotaY, 0));
                ConveyorVisualModel.Transform = new MatrixTransform3D(matrix);
            }

        }

        public void DrawBackground()
        {
            //-----------------------------------------------------------------------------------
            // Creo la Geometria Mesh che conterra l'immagine del Layout
            // Non sarà altro che 2 triangoli (per creare un piano rettangolare senza spessore 
            // che conterranno l'immagine.
            //-----------------------------------------------------------------------------------
            MeshGeometry3D SfondoGeometry = new MeshGeometry3D();
            //-------------------------------------------------------------------
            // Carico l'immagine di sfondo
            //-------------------------------------------------------------------
            var bmp = new BitmapImage(new Uri(Path.Combine(Global.Instance.XmlPath,
                                                           AppConfig.GetXmlKeyValue(Global.Instance.XmlPath, "Settings", "AgvLayoutPath"),
                                                           "Tracklayout.png"), UriKind.Relative));
            // il cad SEW da cui prendo alcuni parametri per calcolarmi le dimensioni e proporzioni dell'immagine del layout in modo che coincida
            // col percorso degli agv lavora con un immagine con rapporto 1080/700 quindi alcuni calcoli faranno riferimento a quel valore
            double largh_img_needed = ((double)700 / (double)1080) * bmp.PixelHeight;
            double largh_img_to_need = largh_img_needed - bmp.PixelWidth;

            SfondoGeometry.Positions.Add(new Point3D((700 * AgvPath.Resolution_Y) - (AgvPath.Offset_Y * AgvPath.Resolution_Y), -1080 * AgvPath.Resolution_X, 0));
            SfondoGeometry.Positions.Add(new Point3D((700 * AgvPath.Resolution_Y) - (AgvPath.Offset_Y * AgvPath.Resolution_Y), 0, 0));
            SfondoGeometry.Positions.Add(new Point3D(-AgvPath.Offset_Y * AgvPath.Resolution_Y, 0, 0));
            SfondoGeometry.Positions.Add(new Point3D(-AgvPath.Offset_Y * AgvPath.Resolution_Y, -(1080 * AgvPath.Resolution_X), 0));

            SfondoGeometry.TriangleIndices.Add(0);
            SfondoGeometry.TriangleIndices.Add(1);
            SfondoGeometry.TriangleIndices.Add(2);

            SfondoGeometry.TriangleIndices.Add(0);
            SfondoGeometry.TriangleIndices.Add(2);
            SfondoGeometry.TriangleIndices.Add(3);

            SfondoGeometry.CalculateNormals();

            SfondoGeometry.TextureCoordinates.Add(new Point(0, 0));
            SfondoGeometry.TextureCoordinates.Add(new Point(0, 1));
            SfondoGeometry.TextureCoordinates.Add(new Point(1, 1));
            SfondoGeometry.TextureCoordinates.Add(new Point(1, 0));

            //----------------------------------------------------------------------------------------------------
            // Creo un Brush che avrà la mia immagine di Layout da mettere come Texture alla Geometria di Sfondo
            //----------------------------------------------------------------------------------------------------
            DrawingBrush db = new DrawingBrush
            {
                Stretch = Stretch.Fill, // La Texture andrà ad adattarsi alle dimensioni della mia Geometria Mesh dato che ho già calcolato le giuste dimensioni 
                                        // perchè tutti corrisponda nel creare la Geometria
                                        //--------------------------------------------------------------------------------------------------------------------------------------------
                                        // Il Brush non conterrà solo l'immagine del layout in realtà, ma anche qualche altra Geometria che serve ad aggiungere degli spazi intorno 
                                        // all'immagine del layout per portarla alle proporzioni giuste 700/1080 senza deformarla
                                        //--------------------------------------------------------------------------------------------------------------------------------------------
                Drawing = new DrawingGroup()
            };

            // Aggiungo un primo blocco per aggiustare le proporzioni da un lato
            RectangleGeometry bloccoSpaziatore1 = new RectangleGeometry
            {
                Rect = new Rect(0, 0, largh_img_to_need / 2, bmp.PixelHeight)
            };
            ((DrawingGroup)db.Drawing).Children.Add(new GeometryDrawing(Brushes.Transparent, null, bloccoSpaziatore1));

            // Aggiungo l'immagine
            GeometryDrawing layoutPng = new GeometryDrawing();
            RectangleGeometry rectangle = new RectangleGeometry
            {
                Rect = new Rect(largh_img_to_need / 2, 0, bmp.PixelWidth, bmp.PixelHeight)
            };
            layoutPng.Geometry = rectangle;

            //---------------------------------------------------------------------------------------------------------------------------------------------
            // Carico l'immagine da mettere di sfondo.
            // Ottengo il colore del primo pixel, lo considero come colore dello sfondo dell'immagine e lo converto a trasparente. SAMUELE 12/07/2021
            //---------------------------------------------------------------------------------------------------------------------------------------------
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(Path.Combine(Global.Instance.XmlPath,
                                                                   AppConfig.GetXmlKeyValue(Global.Instance.XmlPath, "Settings", "AgvLayoutPath"),
                                                                   "Tracklayout_redo.png"));
            //System.Drawing.Color color = img.GetPixel(0, 0);
            //img.MakeTransparent(color);

            //----------------------------------------------------------------------------------------------------
            // Converto il bitmap in ImageBrush da usare come sfondo della Geometria 3D. SAMUELE 12/07/2021
            //----------------------------------------------------------------------------------------------------
            var handle = img.GetHbitmap();
            ImageBrush br = null;
            try
            {
                ImageSource imgSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                br = new ImageBrush(imgSrc);
            }
            catch
            {
                Global.AlertAsync(_windowManager, "Error to load background image");
            }

            layoutPng.Brush = br;
            br.Stretch = Stretch.Fill;
            br.Opacity = 1;

            ((DrawingGroup)db.Drawing).Children.Add(layoutPng);

            // Aggiungo un secondo blocco per aggiustare le proporzioni dall'altro lato
            RectangleGeometry bloccoSpaziatore2 = new RectangleGeometry
            {
                Rect = new Rect(bmp.PixelWidth + largh_img_to_need / 2, 0, largh_img_to_need / 2, bmp.PixelHeight)
            };
            ((DrawingGroup)db.Drawing).Children.Add(new GeometryDrawing(Brushes.Transparent, null, bloccoSpaziatore2));

            //-----------------------------------------------------------------------------------------------------------
            // Il Brush con l'immagine aggiustata lo assegno ad un materiale che abbinerò alla Geometria Mesh di sfondo
            //-----------------------------------------------------------------------------------------------------------
            DiffuseMaterial SfondoMaterial = new DiffuseMaterial
            {
                Brush = db
            };

            //-----------------------------------------------------------------------------------------------------------
            // Creo il modello 3D e gli assegno la mia Geometria Mesh e il mio materile appena creati
            //-----------------------------------------------------------------------------------------------------------
            GeometryModel3D SfondoModel = new GeometryModel3D
            {
                Geometry = SfondoGeometry,
                Material = SfondoMaterial
            };

            //-------------------------------------------------------------------
            // Assegno il modello 3D ad un VisualModel3d
            //-------------------------------------------------------------------
            CustomizedModelVisual3D VisualModelSfondo = new CustomizedModelVisual3D
            {
                IsSfondo = true,
                Content = SfondoModel
            };

            //-------------------------------------------------------------------
            // Aggiungo il VisualModel3d alla view3d generale
            //-------------------------------------------------------------------
            View3D.Children.Add(VisualModelSfondo);
        }

        public void InitAgvGraphics()
        {
            if (_Agv3DModels == null)
            {
                _Agv3DModels = new List<AgvModel3D>();
                foreach (SEW_AGV agv in Common.Instance.Agvs)
                {
                    _Agv3DModels.Add(new AgvModel3D(agv));
                }
            }
            _Agv3DModels.ForEach(agv => View3D.Children.Add(agv.AgvVisualModel));
        }

        #endregion

        #region Pulsanti

        public void NextAgv()
        {
            if (Common.Instance.Agvs?.Count == 0) return;

            string currentAgvCode = SelectedAgvModel.Agv.AGV_Code;
            bool currentAgvMoreDataExpanded = SelectedAgvModel.MoreDataExpanded;
            string nextAgvCode = SelectedAgvModel.Agv.AGV_Code;

            int agvIndex = Common.Instance.Agvs.FindIndex(a => a.AGV_Code == currentAgvCode);
            if (agvIndex >= Common.Instance.Agvs.Count - 1)
                return;

            nextAgvCode = Common.Instance.Agvs.ElementAt(agvIndex + 1).AGV_Code;

            SelectedAgvModel = AgvViewModels.FirstOrDefault(x => x.Agv.AGV_Code == nextAgvCode);
            SelectedAgvModel.MoreDataExpanded = currentAgvMoreDataExpanded;
        }

        public void PrevAgv()
        {
            if (Common.Instance.Agvs?.Count == 0) return;

            string currentAgvCode = SelectedAgvModel.Agv.AGV_Code;
            bool currentAgvMoreDataExpanded = SelectedAgvModel.MoreDataExpanded;
            string prevAgvCode = SelectedAgvModel.Agv.AGV_Code;

            int agvIndex = Common.Instance.Agvs.FindIndex(a => a.AGV_Code == currentAgvCode);
            if (agvIndex <= 0)
                return;

            prevAgvCode = Common.Instance.Agvs.ElementAt(agvIndex - 1).AGV_Code;

            SelectedAgvModel = AgvViewModels.FirstOrDefault(x => x.Agv.AGV_Code == prevAgvCode);
            SelectedAgvModel.MoreDataExpanded = currentAgvMoreDataExpanded;
        }

        public void AddStation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            _bufferedStation = SelectedStation;
            SelectedStation = new AgvStation((SqlConnection)Global.Instance.ConnGlobal);
            StationInsert = true;
        }

        public void EditStation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            _bufferedStation = (AgvStation)SelectedStation.Clone();
            StationEdit = true;
        }

        public void CancelChange()
        {
            StationInsert = false;
            StationEdit = false;

            SelectedStation = _bufferedStation;
        }

        public void SaveStation()
        {
            if (StationEdit)
            {
                if (!SelectedStation.Update())
                {
                    Global.ErrorAsync(_windowManager, "Error to update station. Check Logs!");
                }
                else
                {
                    View3D.Children.Remove(View3D.Children.Where(c => c is CustomizedModelVisual3D && ((CustomizedModelVisual3D)c).IsConveyor && ((CustomizedModelVisual3D)c).ConveyorCode == SelectedStation.MOD_Code).FirstOrDefault());
                    DrawConveyors(SelectedStation.MOD_Code);
                }
            }
            if (StationInsert)
            {
                SelectedStation.MOD_HMT_Code = "AGVSTATION";
                if (SelectedStation.Add() == null)
                {
                    Global.ErrorAsync(_windowManager, "Error to insert station. Check Logs!");
                }
                else
                {
                    _agvStations.Add(SelectedStation);
                    DrawConveyors(SelectedStation.MOD_Code);
                }
            }

            StationInsert = false;
            StationEdit = false;
        }

        public async void DeleteStation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                await Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            if (await Global.ConfirmAsync(_windowManager, "Are You Sure to delete station " + SelectedStation.MOD_Code + "?"))
            {
                if (!SelectedStation.Delete())
                {
                    await Global.ErrorAsync(_windowManager, "Error to delete station. Check Logs!");
                }
                else
                {
                    _agvStations.Remove(SelectedStation);
                    View3D.Children.Remove(View3D.Children.Where(c => c is CustomizedModelVisual3D && ((CustomizedModelVisual3D)c).IsConveyor &&
                                                                        ((CustomizedModelVisual3D)c).ConveyorCode == SelectedStation.MOD_Code).FirstOrDefault());
                    SelectedStation = AgvStations.First();
                }
            }
        }

        public void SetTrack()
        {
            string path = Path.GetFullPath(@"PackDataViewer.exe");
            Process process;

            if (!Utils.IsProcessOpen(path, out process))
            {
                process = new Process();
                process.StartInfo.FileName = path;

                process.StartInfo.Arguments = $"1 {SelectedStation.MOD_CTR_Id} {SelectedStation.MOD_Code}";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
            }
            else
            {
                // Se il log allarmi è già aperto in riferimento ad una altra navetta lo chiudo e riapro
                if (!process.CloseMainWindow())
                {
                    process.Kill();
                }
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = $"1 {SelectedStation.MOD_CTR_Id} {SelectedStation.MOD_Code}";
                process.Start();
            }

        }

        public void ZoomReset()
        {
            View3D.Camera.Position = new Point3D(_AgvCameraCoordinateX, _AgvCameraCoordinateY, _AgvCameraCoordinateZ);
            View3D.Camera.LookDirection = new Vector3D(_AgvCameraDirectionX, _AgvCameraDirectionY, _AgvCameraDirectionZ);
            View3D.Camera.UpDirection = new Vector3D(_AgvCameraUpDirectionX, _AgvCameraUpDirectionY, _AgvCameraUpDirectionZ);
        }

        public void StartSimulation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            if (!Global.Instance.SendCommandTrafficManager("PLC",
                                             "START_SIMULATION",
                                             new Dictionary<string, object>(),
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
            else
                SimulationOn = true;
        }

        public void StopSimulation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            if (!Global.Instance.SendCommandTrafficManager("PLC",
                                             "STOP_SIMULATION",
                                             new Dictionary<string, object>(),
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
            else
            {
                SimulationOn = false;
            }
        }

        public void ResetSimulation()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }
            if (!Global.Instance.SendCommandTrafficManager("PLC",
                                             "RESET_SIMULATION",
                                             new Dictionary<string, object>(),
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }

        public void ReloadTrackDataAllAgv()
        {
            if (!Global.Instance.CheckUserPerm("XXWW"))
            {
                Global.ErrorAsync(_windowManager, "Operation not allowed. Check user permission!");
                return;
            }

            foreach (var agv in Common.Instance.Agvs.Where(a => a.CTR_Enabled))
            {
                SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
                {
                    Service = SEW_Agv_Service.NO_SERVICE,
                    Part_data = 0,
                    UDP_receive_port = 0,
                    Index = 0,
                    Program_number = 0,
                    Commands = SEW_Commands.Reload_track_data
                };

                Global.Instance.SendTelegram((int)agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
            }
        }

        public void StopAllAgv()
        {
            if (!Global.Instance.SendCommandTrafficManager(Common.Instance.Agvs.FirstOrDefault().MMG_Code,
                                             "STOPALL",
                                            new Dictionary<string, object>(),
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }

        public void StartAllAgv()
        {
            if (!Global.Instance.SendCommandTrafficManager(Common.Instance.Agvs.FirstOrDefault().MMG_Code,
                                             "STARTALL",
                                            new Dictionary<string, object>(),
                                             out string error))
                Global.ErrorAsync(_windowManager, error);
        }

        public void ResetAllAgv()
        {
            foreach (var agv in Common.Instance.Agvs.Where(a => a.CTR_Enabled))
            {
                SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
                {
                    Service = SEW_Agv_Service.NO_SERVICE,
                    Part_data = 0,
                    UDP_receive_port = 0,
                    Index = 0,
                    Program_number = 0,
                    Commands = SEW_Commands.Error_reset
                };

                Global.Instance.SendTelegram((int)agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
            }
        }

        public void SimulaAll()
        {
            foreach (var agv in Common.Instance.Agvs)
            {
                if (agv.Agv_State.HasFlag(SEW_State_Flags.Simulation))
                {
                    var b = "";
                    ushort transponder = 0;

                    for (int i = 0; i < agv.AGV_Code.Length; i++)
                    {
                        if (char.IsDigit(agv.AGV_Code[i]))
                            b += agv.AGV_Code[i];
                    }

                    if (b.Length > 0)
                        transponder = ushort.Parse(b);

                    SEW_Agv_IN_Tel cmd_telegram = new SEW_Agv_IN_Tel
                    {
                        Part_data = 0,
                        UDP_receive_port = 0,
                        Index = 0,
                        Program_number = 0,
                        Set_transponder = (ushort)(transponder * 2),
                        Commands = SEW_Commands.Continue
                    };

                    Global.Instance.SendTelegram((int)agv.AGV_CTR_Id, cmd_telegram, false, 0, out string error);
                }
            }

            foreach (var agv in Common.Instance.Agvs)
            {
                if (agv.Agv_State.HasFlag(SEW_State_Flags.Simulation))
                {
                    SEW_Agv_IN_Tel cmd_telegram_Continue = new SEW_Agv_IN_Tel
                    {
                        Service = SEW_Agv_Service.NO_SERVICE,
                        Part_data = 0,
                        UDP_receive_port = 0,
                        Index = 0,
                        Program_number = 0,
                        Commands = SEW_Commands.Continue
                    };

                    Global.Instance.SendTelegram((int)agv.AGV_CTR_Id, cmd_telegram_Continue, false, 0, out string test);

                    Thread.Sleep(300);
                }
            }
        }

        #endregion
    }

    #region Enum

    public enum EObjType
    {
        Box,
        Pallet,
        Roll,
        Slipsheet,
        Agv,
    }
    #endregion
}
