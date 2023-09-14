using Caliburn.Micro;
using HelixToolkit.Wpf;
using mSwDllGraphics;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AgvMgr.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AgvLayoutView : UserControl
    {
        //#region Members

        //private SortedList _Materials = new SortedList();
        //private List<ModelVisual3D> _Visual3DModels = new List<ModelVisual3D>();
        //private SortedList<string, Graphic3DObject> _3DObjects = new SortedList<string, Graphic3DObject>();
        //private Graphic3DObject _Side;

        //#endregion
        public AgvLayoutView()
        {
            //    string multimedia;


            //    multimedia = System.IO.Path.Combine(
            //        AppConfig.XmlRelativePath,
            //        AppConfig.GetXmlKeyValue("Graphics", "MultimediaPath"));
            //    Graphic3DTools.Init(multimedia);
            //    Graphic2DTools.Init(multimedia);
            InitializeComponent();

            //    InitializeEnvironment();

            //    DrawObject(EObjType.Agv, new Point3D(100, 100, 80), 0);

        }



        //#region Initialization

        //private void ReadConfig()
        //{
        //    _3DObjects = new SortedList<string, Graphic3DObject>();

        //    foreach (string key in Graphic3DTools.ListaOggetti.Keys)
        //    {
        //        _3DObjects.Add(key, Graphic3DTools.ListaOggetti[key].Clone());
        //    }
        //}

        //private void InitializeEnvironment()
        //{
        //    ReadConfig();
        //    SetEnvironment();
        //    //InitEvents();
        //    InitGestures();
        //    SetLights();
        //}

        //private void SetEnvironment()
        //{
        //    LinearGradientBrush bg = new LinearGradientBrush(
        //        System.Windows.Media.Color.FromRgb(0, 130, 180),
        //        Colors.White, new Point(0, 0),
        //        new Point(1, 1));
        //    BoxGrid.Background = bg;
        //}

        //private void SetLights()
        //{
        //    var lights = new DefaultLights();
        //    View3D.Children.Add(lights);
        //}
        //private void InitGestures()
        //{
        //    View3D.IsMoveEnabled = false;

        //    View3D.ShowCoordinateSystem = true;
        //    View3D.CoordinateSystemHeight += 30;
        //    View3D.CoordinateSystemWidth += 30;

        //    View3D.TopViewGesture = new KeyGesture(Key.Z, ModifierKeys.Control);
        //    View3D.LeftViewGesture = new KeyGesture(Key.X, ModifierKeys.Control);
        //    View3D.BackViewGesture = new KeyGesture(Key.Y, ModifierKeys.Control);

        //    View3D.BottomViewGesture = null;
        //    View3D.RightViewGesture = null;
        //    View3D.FrontViewGesture = null;

        //    View3D.Cursor = Graphic2DTools.Cursors["Grab"];
        //    View3D.RotateCursor = Graphic2DTools.Cursors["Grabbing"];
        //}
        //#endregion

        //#region Draw
        //public void DrawObject(EObjType Object, Point3D Scale, int LabelSide = 0)
        //{
        //    Graphic3DObject gObj;
        //    List<ERectangleSides> labels = Graphic2DTools.IntToLabels(LabelSide);
        //    _Visual3DModels.Clear();

        //    switch (Object)
        //    {
        //        case EObjType.Box:
        //            gObj = _3DObjects[E3DObjList.BoxSideB.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            gObj = _3DObjects[E3DObjList.BoxSideL.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            gObj = _3DObjects[E3DObjList.BoxSideF.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            gObj = _3DObjects[E3DObjList.BoxSideR.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            gObj = _3DObjects[E3DObjList.BoxSideU.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            gObj = _3DObjects[E3DObjList.BoxSideD.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            SetLabels(labels);
        //            break;

        //        case EObjType.Slipsheet:
        //            gObj = _3DObjects[E3DObjList.Slipsheet.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());
        //            break;

        //        case EObjType.Pallet:
        //            gObj = _3DObjects[E3DObjList.Pallet.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());
        //            break;

        //        case EObjType.Roll:
        //            gObj = _3DObjects[E3DObjList.Roll.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());

        //            SetRollLabel(labels.Count > 0 ? (ERectangleSides?)labels.First() : null);
        //            break;
        //        case EObjType.Agv:
        //            gObj = _3DObjects[E3DObjList.Agv.ToString()];
        //            gObj.Scale(Scale);
        //            _Visual3DModels.Add(new ModelVisual3D());
        //            _Visual3DModels.Last().Content = gObj.GraphicRepresentation;
        //            View3D.Children.Add(_Visual3DModels.Last());
        //            break;
        //    }
        //}

        //public void SetLabels(List<ERectangleSides> Sides)
        //{
        //    RemoveLabels();

        //    for (int i = 0; i < Sides.Count; i++)
        //    {
        //        if (Sides[i] == ERectangleSides.Left)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideL.ToString()];
        //            _Side.SetMaterial(1);
        //        }

        //        if (Sides[i] == ERectangleSides.Right)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideR.ToString()];
        //            _Side.SetMaterial(1);
        //        }

        //        if (Sides[i] == ERectangleSides.Up)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideU.ToString()];
        //            _Side.SetMaterial(1);
        //        }

        //        if (Sides[i] == ERectangleSides.Down)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideD.ToString()];
        //            _Side.SetMaterial(1);
        //        }

        //        if (Sides[i] == ERectangleSides.Front)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideF.ToString()];
        //            _Side.SetMaterial(1);
        //        }

        //        if (Sides[i] == ERectangleSides.Back)
        //        {
        //            _Side = _3DObjects[E3DObjList.BoxSideB.ToString()];
        //            _Side.SetMaterial(1);
        //        }
        //    }
        //}

        //private void RemoveLabels()
        //{
        //    Graphic3DObject gObj;
        //    gObj = _3DObjects[E3DObjList.BoxSideB.ToString()];
        //    gObj.SetMaterial(0);
        //    gObj = _3DObjects[E3DObjList.BoxSideL.ToString()];
        //    gObj.SetMaterial(0);
        //    gObj = _3DObjects[E3DObjList.BoxSideF.ToString()];
        //    gObj.SetMaterial(0);
        //    gObj = _3DObjects[E3DObjList.BoxSideR.ToString()];
        //    gObj.SetMaterial(0);
        //    gObj = _3DObjects[E3DObjList.BoxSideU.ToString()];
        //    gObj.SetMaterial(0);
        //    gObj = _3DObjects[E3DObjList.BoxSideD.ToString()];
        //    gObj.SetMaterial(0);
        //}

        //public void SetRollLabel(ERectangleSides? side)
        //{
        //    RemoveRollLabels();

        //    if (!side.HasValue) return;

        //    if (side == ERectangleSides.Left)
        //    {
        //        _Side = _3DObjects[E3DObjList.Roll.ToString()];
        //        _Side.SetMaterial(1);
        //    }
        //    else if (side == ERectangleSides.Right)
        //    {
        //        _Side = _3DObjects[E3DObjList.Roll.ToString()];
        //        _Side.SetMaterial(2);
        //    }
        //    else if (side == ERectangleSides.Front)
        //    {
        //        _Side = _3DObjects[E3DObjList.Roll.ToString()];
        //        _Side.SetMaterial(3);
        //    }
        //    else if (side == ERectangleSides.Back)
        //    {
        //        _Side = _3DObjects[E3DObjList.Roll.ToString()];
        //        _Side.SetMaterial(4);
        //    }
        //}

        //public void RemoveRollLabels()
        //{
        //    _Side = _3DObjects[E3DObjList.Roll.ToString()];
        //    _Side.SetMaterial(0);
        //}

        //public void Scale(EObjType Object, Point3D Scala)
        //{
        //    Graphic3DObject gObj;
        //    Point3D pScale = new Point3D();
        //    // Eseguo lo Scale dell'oggetto indicato
        //    switch (Object)
        //    {
        //        case EObjType.Box:
        //            gObj = _3DObjects[E3DObjList.BoxSideB.ToString()];
        //            gObj.Scale(Scala);

        //            gObj = _3DObjects[E3DObjList.BoxSideL.ToString()];
        //            gObj.Scale(Scala);

        //            gObj = _3DObjects[E3DObjList.BoxSideF.ToString()];
        //            gObj.Scale(Scala);

        //            gObj = _3DObjects[E3DObjList.BoxSideR.ToString()];
        //            gObj.Scale(Scala);

        //            gObj = _3DObjects[E3DObjList.BoxSideU.ToString()];
        //            gObj.Scale(Scala);

        //            gObj = _3DObjects[E3DObjList.BoxSideD.ToString()];
        //            gObj.Scale(Scala);

        //            pScale = gObj.CurrentScale;
        //            break;

        //        case EObjType.Pallet:
        //            gObj = _3DObjects[E3DObjList.Pallet.ToString()];
        //            gObj.Scale(Scala);
        //            pScale = gObj.CurrentScale;
        //            break;

        //        case EObjType.Slipsheet:
        //            gObj = _3DObjects[E3DObjList.Slipsheet.ToString()];
        //            gObj.Scale(Scala);
        //            pScale = gObj.CurrentScale;
        //            break;

        //        case EObjType.Roll:
        //            gObj = _3DObjects[E3DObjList.Roll.ToString()];
        //            gObj.Scale(Scala);
        //            pScale = gObj.CurrentScale;
        //            break;
        //    }

        //    View3D.ResetCamera();
        //}

        //public void LookAt(double X, double Y, double Z)
        //{
        //    View3D.Camera.LookDirection = new Vector3D(X, Y, Z);
        //}

        //#endregion

        //#region Enum

        //public enum EObjType
        //{
        //    Box,
        //    Pallet,
        //    Roll,
        //    Slipsheet,
        //    Agv,
        //}

        //public enum E3DObjList
        //{
        //    BoxSideB = 0,
        //    BoxSideL = 1,
        //    BoxSideF = 2,
        //    BoxSideR = 3,
        //    BoxSideU = 4,
        //    BoxSideD = 5,
        //    Slipsheet,
        //    Pallet,
        //    Roll,
        //    Agv,
        //}

        //#endregion
    }
}
