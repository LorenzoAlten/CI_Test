using HelixToolkit.Wpf;
using mSwAgilogDll;
using mSwAgilogDll.SEW;
using mSwDllGraphics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AgvMgr.AppData
{
    public class AgvModel3D
    {
        AgvPath _agvPath;
        public CustomizedModelVisual3D AgvVisualModel { get; set; }
        public ModelVisual3D Pallet { get; set; } = new ModelVisual3D();

        public ModelVisual3D Box { get; set; } = new ModelVisual3D();

        public Graphic3DObject AgvGraphicObject { get; set; }
        public Graphic3DObject PalletGraphicObject { get; set; }
        public Graphic3DObject BoxGraphicObject { get; set; }
        public SolidColorBrush Colore = System.Windows.Media.Brushes.Aqua;

        public SEW_AGV Agv { get; set; }

        public AgvModel3D(SEW_AGV agv)
        {
            _agvPath = AgvPath.GetInstance();
            Agv = agv;
            AgvGraphicObject = Graphic3DTools.ListaOggetti["Agv"].Clone();

            PalletGraphicObject = Graphic3DTools.ListaOggetti["Pallet"].Clone();
            BoxGraphicObject = Graphic3DTools.ListaOggetti["Cubo"].Clone();

            AgvVisualModel = new CustomizedModelVisual3D();
            AgvVisualModel.Content = AgvGraphicObject.GraphicRepresentation;
            AgvVisualModel.IsAgv = true;
            AgvVisualModel.AgvCode = agv.AGV_Code;

            switch (Agv.AGV_CTR_Id)
            {
                case 101:
                    Colore = Brushes.Red;
                    break;
                case 102:
                    Colore = Brushes.Blue;
                    break;
                case 103:
                    Colore = Brushes.Green;
                    break;
                case 104:
                    Colore = Brushes.Orange;
                    break;
                case 105:
                    Colore = Brushes.Silver;
                    break;
                case 106:
                    Colore = Brushes.Maroon;
                    break;
                case 107:
                    Colore = Brushes.HotPink;
                    break;
            }
            //-----------------------------------------------------------------------
            // Compongo il visual 3d del testo dell'Agv
            //-----------------------------------------------------------------------

            BillboardTextVisual3D testo = new BillboardTextVisual3D();
            testo.Foreground = Colore;
            testo.Background = Brushes.Transparent;
            testo.Material = new DiffuseMaterial(Brushes.Transparent);
            if (Agv.AgvRequest.AGV_Mission.HasValue && Agv.AgvRequest.AGV_Mission.Value > 0)
            {
                //testo.Height = 1000;
                //testo.HeightFactor = 2;
                testo.Text = Agv.AGV_Code + "\n" + Agv.AgvRequest.AGV_Mission;
                testo.Position = new Point3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ + 1300);
            }
            else
            {
                //testo.Height = 500;
                //testo.HeightFactor = 2;
                testo.Position = new Point3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + 300);
                testo.Text = Agv.AGV_Code;
            }

            AgvVisualModel.Children.Add(testo);
            //-----------------------------------------------------------------------
            // Compongo il visual 3d del carico dell'Agv.
            // Composto da:
            //      - Pallet
            //      - Box sopra il pallet
            //-----------------------------------------------------------------------
            Pallet.Content = PalletGraphicObject.GraphicRepresentation;
            var matrix = Pallet.Transform.Value;
            matrix.Translate(new Vector3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + 2));
            Pallet.Transform = new MatrixTransform3D(matrix);
            Box.Content = BoxGraphicObject.GraphicRepresentation;
            var matrix2 = Box.Transform.Value;
            matrix2.Translate(new Vector3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ + 4));
            Box.Transform = new MatrixTransform3D(matrix2);
        }
        public void RefreshVisual(bool flip_Flop)
        {
            //------------------------------------------------------------------------------------------------------------------
            // Se non sono cambiati dati "Visuali" non sto ad aggiornare il ModelVisual3D per risparmiare fatica al processore 
            //------------------------------------------------------------------------------------------------------------------
            if (AgvVisualModel.X == Agv.Absolute_Y &&
               AgvVisualModel.Y == -Agv.Absolute_X &&
               AgvVisualModel.Load == Agv.AgvRequest.AGV_Loaded &&
               AgvVisualModel.Angle == (Agv.Angle / 100) &&
               AgvVisualModel.AgvState == Agv.AgvRequest.State &&
               AgvVisualModel.Mission == Agv.AgvRequest.AGV_Mission &&
               AgvVisualModel.EnablingMode == Agv.AgvRequest.Enabling_Mode &&
               !Agv.AgvRequest.State.HasFlag(State_Flags.Alarm)) return;

            //------------------------------------------------------------------------
            // Gestisco il colore dell'agv in base allo stato
            //------------------------------------------------------------------------
            if (Agv.AgvRequest.State.HasFlag(State_Flags.Alarm) && flip_Flop)
            {
                AgvGraphicObject.SetMaterial(0);
            }
            else
            {
                if (Agv.AgvRequest.Enabling_Mode == AGV_Enabling_Mode.Automatic)
                    AgvGraphicObject.SetMaterial(3);
                if (Agv.AgvRequest.Enabling_Mode == AGV_Enabling_Mode.MissionExclusion)
                    AgvGraphicObject.SetMaterial(3);
                if (Agv.AgvRequest.Enabling_Mode == AGV_Enabling_Mode.None)
                    AgvGraphicObject.SetMaterial(1);
                if (Agv.AgvRequest.Enabling_Mode == AGV_Enabling_Mode.Semiautomatic)
                    AgvGraphicObject.SetMaterial(2);
            }
            //------------------------------------------------------------------------
            // Visualizzo o meno il carico a bordo agv
            //------------------------------------------------------------------------
            if (Agv.AgvRequest.AGV_Loaded)
            {
                if (AgvVisualModel.Load == false)
                {
                    AgvVisualModel.Children.Insert(0, Pallet);
                    AgvVisualModel.Children.Insert(0, Box);

                    AgvVisualModel.Load = true;
                }
            }
            else
            {
                if (AgvVisualModel.Load == true)
                {

                    for (int i = 0; i < AgvVisualModel.Children.Count; i++)
                    {
                        if (!(AgvVisualModel.Children[i] is BillboardTextVisual3D))
                        {

                            AgvVisualModel.Children.Remove(AgvVisualModel.Children[i]);
                            i--;
                        }
                    }
                }

                AgvVisualModel.Load = false;
            }
            //------------------------------------------------------------------------
            // Aggiorno il testo sopra l'agv
            //------------------------------------------------------------------------
            BillboardTextVisual3D visual3DTesto = AgvVisualModel.Children.Where(c => c is BillboardTextVisual3D).Cast<BillboardTextVisual3D>().FirstOrDefault();
            if (visual3DTesto != null)
            {

                if (Agv.AgvRequest.AGV_Mission.HasValue && Agv.AgvRequest.AGV_Mission.Value > 0)
                {
                    //visual3DTesto.Height = 1000;
                    //visual3DTesto.HeightFactor = 2;
                    visual3DTesto.Text = Agv.AGV_Code + "\n" + Agv.AgvRequest.AGV_Mission;
                    visual3DTesto.Position = new Point3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + PalletGraphicObject.GraphicRepresentation.Bounds.SizeZ + 1400);
                }
                else
                {
                    //visual3DTesto.Height = 500;
                    //visual3DTesto.HeightFactor = 2;
                    visual3DTesto.Text = Agv.AGV_Code;
                    visual3DTesto.Position = new Point3D(0, 0, AgvGraphicObject.GraphicRepresentation.Bounds.SizeZ + 900);
                }
            }

            for (int i = 0; i < AgvVisualModel.Children.Count; i++)
            {
                if (AgvVisualModel.Children[i] is CustomizedModelVisual3D)
                {
                    if (((CustomizedModelVisual3D)AgvVisualModel.Children[i]).IsPath)
                    {
                        AgvVisualModel.Children.Remove(AgvVisualModel.Children[i]);
                        i--;
                    }
                }
            }
            //-----------------------------------------------------------------------------
            // Aggiorno le trasformazioni dell'agv per aggiustare spostamento e rotazione 
            //-----------------------------------------------------------------------------
            var axis = new Vector3D(0, 0, 1);
            var matrix = new Matrix3D(); //agv.AgvVisualModel.Transform.Value;

            matrix.Rotate(new Quaternion(axis, Agv.Angle / 100));
            matrix.Translate(new Vector3D(Agv.Absolute_Y, -Agv.Absolute_X, 0));

            AgvVisualModel.Transform = new MatrixTransform3D(matrix);

            AgvVisualModel.X = Agv.Absolute_Y;
            AgvVisualModel.Y = -Agv.Absolute_X;
            AgvVisualModel.Angle = Agv.Angle / 100;
            AgvVisualModel.AgvState = Agv.AgvRequest.State;
            AgvVisualModel.Mission = Agv.AgvRequest.AGV_Mission;
            AgvVisualModel.EnablingMode = Agv.AgvRequest.Enabling_Mode;
        }
    }
}
