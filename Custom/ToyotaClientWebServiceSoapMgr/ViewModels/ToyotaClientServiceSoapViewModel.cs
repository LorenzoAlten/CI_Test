using Caliburn.Micro;
using mSwAgilogDll;
using mSwDllMFC;
using mSwDllUtils;
using mSwDllWPFUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ToyotaClientWebServiceSoapMgr.Models;
using WebServiceSoapToyota;

namespace ToyotaClientWebServiceSoapMgr.ViewModels
{


    [Export(typeof(ToyotaClientServiceSoapViewModel))]
    public class ToyotaClientServiceSoapViewModel : Conductor<Screen>
    {
        #region Enum
        public enum EOperationType
        {
            ToWarehouse,
            OnlyWrap,
        }
        #endregion

        #region Members

        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly SqlConnection _conn;
        private RunUtils _utils;
        private object _lockObj = new object();
        private bool _IsLoading = false;

        #endregion Members
        private ToyotaOrder _toyotaorder;
        #region Properties
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public ToyotaOrder Toyotaorder
        {
            get { return _toyotaorder; }
            set { _toyotaorder = value; }
        }


        #endregion Properties

        #region Constructor

        [ImportingConstructor]
        public ToyotaClientServiceSoapViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, bool reEntry = false)
        {
            DisplayName = Global.Instance.LangTl("Toyota Service Soap");

            _windowManager = windowManager;

            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            _conn = (SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal);
            _utils = new RunUtils((SqlConnection)DbUtils.CloneConnection(Global.Instance.ConnGlobal));

            Global.Instance.OnEvery1Sec += Global_OnEvery1Min;
        }

        #endregion Constructor

        #region ViewModel Override

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                Global.Instance.OnEvery1Minute -= Global_OnEvery1Min;
            }

            await base.OnDeactivateAsync(close, cancellationToken);
        }


        Views.ToyotaClientServiceSoapView View;

        protected override async void OnViewLoaded(object view)
        {
            View = (Views.ToyotaClientServiceSoapView)view;
            await LoadData();
            base.OnViewLoaded(view);
        }

        #endregion ViewModel Override

        #region Global Events

        private async void Global_OnEvery1Min(object sender, GenericEventArgs e)
        {
            try
            {

                var Toyotaorder = await ImpToyotaOrder();
                //if (Toyotaorder != null)
                //{


                //    WebServiceOrder webServiceHeaderOrder = new WebServiceOrder();
                //    webServiceHeaderOrder.OrderRows = new WebServiceOrderRow[2];
                //    webServiceHeaderOrder.OrderId = Convert.ToString(Toyotaorder.OrderId);
                //    webServiceHeaderOrder.GoodsCarrier = Toyotaorder.GoodsCarrier;
                //    webServiceHeaderOrder.Prio = Toyotaorder.Priority;
                //    webServiceHeaderOrder.GoodsId = Toyotaorder.GoodsId;

                //    for (int i = 0; i < Toyotaorder.LineMission.Count; i++)
                //    {
                //        WebServiceOrderRow webServiceOrderRow = new WebServiceOrderRow();
                //        webServiceOrderRow.TaskId = Toyotaorder.LineMission[i].TaskId;
                //        webServiceOrderRow.OperationType = Toyotaorder.LineMission[i].OperationType;
                //        webServiceOrderRow.Location = Toyotaorder.LineMission[i].Location;
                //        webServiceOrderRow.Quantity = Toyotaorder.LineMission[i].Quantity;
                //        webServiceHeaderOrder.OrderRows[i] = webServiceOrderRow;                        
                //    }

                //    //string retval = SOAPManual();

                //    var binding = new BasicHttpBinding()
                //    {
                //        Name = "BasicHttpBinding_IFakeService",
                //        MaxBufferSize = 2147483647,
                //        MaxReceivedMessageSize = 2147483647
                //    };                                   

                //    var endpoint = new EndpointAddress("http://127.0.0.1:8080/OrderManagerHost");
                //    ServiceToyotaExport.OrderManagerAsHostClient client = new ServiceToyotaExport.OrderManagerAsHostClient(binding, endpoint);

                //    int serviceResponse = client.CreateOrder(webServiceHeaderOrder);
                //    //UpdateOrder(webServiceHeaderOrder.OrderId, serviceResponse);                    
                //}
                if (Toyotaorder != null)
                {


                    WebServiceOrder webServiceHeaderOrder = new WebServiceOrder();
                    webServiceHeaderOrder.OrderRows = new WebServiceOrderRow[2];
                    webServiceHeaderOrder.OrderId = Convert.ToString(Toyotaorder.OrderId);
                    webServiceHeaderOrder.GoodsCarrier = Toyotaorder.GoodsCarrier;
                    webServiceHeaderOrder.Prio = Toyotaorder.Priority;
                    webServiceHeaderOrder.GoodsId = Toyotaorder.GoodsId;

                    for (int i = 0; i < Toyotaorder.LineMission.Count; i++)
                    {
                        WebServiceOrderRow webServiceOrderRow = new WebServiceOrderRow();
                        webServiceOrderRow.TaskId = Toyotaorder.LineMission[i].TaskId;
                        webServiceOrderRow.OperationType = Toyotaorder.LineMission[i].OperationType;
                        webServiceOrderRow.Location = Toyotaorder.LineMission[i].Location;
                        webServiceOrderRow.Quantity = Toyotaorder.LineMission[i].Quantity;
                        webServiceHeaderOrder.OrderRows[i] = webServiceOrderRow;
                    }

                    //string retval = SOAPManual();

                    var binding = new BasicHttpBinding()
                    {
                        Name = "BasicHttpBinding_IFakeService",
                        MaxBufferSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647
                    };

                    var endpoint = new EndpointAddress("http://127.0.0.1:8080/OrderManagerHost");
                    OrderManagerAsHostClient client = new OrderManagerAsHostClient(binding, endpoint);

                    int serviceResponse = client.CreateOrder(webServiceHeaderOrder);





                    //UpdateOrder(webServiceHeaderOrder.OrderId, serviceResponse);                    
                }
                executeAgilogWebServiceSoap();
                //execute();
                //executeTestSoapDemo();
                //executeLoginSoapDemo();
                //var binding = new BasicHttpBinding()
                //{
                //    Name = "BasicHttpBinding_IFakeService",
                //    MaxBufferSize = 2147483647,
                //    MaxReceivedMessageSize = 2147483647
                //};

                //var endpoint = new EndpointAddress("http://www.dneonline.com/calculator.asmx");


                //ServiceCalculator.CalculatorSoapClient client = new ServiceCalculator.CalculatorSoapClient(binding, endpoint);
                //int a, b, c;
                //a = 5;
                //b = 6;
                //c = client.Multiply(a, b);

                //using (StreamWriter writer = new StreamWriter(@"C:\Files\result.txt"))
                //{
                //    writer.WriteLine(c);
                //}

                //var binding = new BasicHttpBinding()
                //{
                //    Name = "BasicHttpBinding_IFakeService",
                //    MaxBufferSize = 2147483647,
                //    MaxReceivedMessageSize = 2147483647
                //};

                //var endpoint = new EndpointAddress("http://localhost:44314/SoapDemo.asmx");


                //ServiceSoapDemo.SoapDemoSoapClient client = new ServiceSoapDemo.SoapDemoSoapClient(binding, endpoint);
                //client.login("antonio.filazzola@erreviautomation.com", "FLZnns");


                //ServiceHost host = new ServiceHost(typeof(Service), new Uri("http://localhost:8000"));
                //host.AddServiceEndpoint(typeof(IService), new BasicHttpBinding(), "Soap");
                //ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "Web");
                //endpoint.Behaviors.Add(new WebHttpBehavior());
                //host.Open();


            }


            catch (Exception ex)
            {
                Global.Instance.Log(ex.Message, LogLevels.Fatal);
            }
        }

        //private void executeTestSoapDemo()
        //{
        //    var binding = new BasicHttpBinding()
        //    {
        //        Name = "BasicHttpBinding_IFakeService",
        //        MaxBufferSize = 2147483647,
        //        MaxReceivedMessageSize = 2147483647
        //    };
        //    var endpoint = new EndpointAddress("http://localhost:44314/SoapDemo.asmx");
        //    ServiceSoapDemo.SoapDemoSoapClient client = new ServiceSoapDemo.SoapDemoSoapClient(binding, endpoint);

        //    ResponseModelOfString pippo = client.login("antonio.filazzola@gmail.com", "FLZnns");
        //}

        public static HttpWebRequest CreateWebRequest()
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://localhost:44399/SoapDemo.asmx");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            return webRequest;
        }
        //public void execute()
        //{
        //    try
        //    {
        //        //HttpWebRequest request = CreateWebRequest();

        //        //XmlDocument soapEnvelopeXml = new XmlDocument();
        //        //soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
        //        ////    <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://agv/WebService.wsdl"" xmlns:q2=""http://schemas.xmlsoap.org/soap/encoding/"">
        //        ////       <soapenv:Header/>
        //        ////       <soapenv:Body>
        //        ////          <web:CreateOrder soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
        //        ////             <orderToCreate xsi:type=""enc:WebServiceOrder"" xmlns:enc=""http://agv/WebService.wsdl/encoded"">
        //        ////                <!--Optional:-->
        //        ////                <OrderRows xsi:type=""enc:ArrayOfWebServiceOrderRow"" q2:arrayType=""enc:WebServiceOrderRow[]""/>
        //        ////                <!--Optional:-->
        //        ////                <OrderId xsi:type=""xsd:string"">?</OrderId>
        //        ////                <GoodsCarrier xsi:type=""xsd:int"">?</GoodsCarrier>
        //        ////                <!--Optional:-->
        //        ////                <GoodsId xsi:type=""xsd:string"">?</GoodsId>
        //        ////                <GoodsContainerId xsi:type=""xsd:int"">?</GoodsContainerId>
        //        ////                <GoodsWidth xsi:type=""xsd:int"">?</GoodsWidth>
        //        ////                <GoodsHeight xsi:type=""xsd:int"">?</GoodsHeight>
        //        ////                <GoodsLength xsi:type=""xsd:int"">?</GoodsLength>
        //        ////                <GoodsWeight xsi:type=""xsd:int"">?</GoodsWeight>
        //        ////                <!--Optional:-->
        //        ////                <ArticleID xsi:type=""xsd:string"">?</ArticleID>
        //        ////                <!--Optional:-->
        //        ////                <BatchID xsi:type=""xsd:string"">?</BatchID>
        //        ////                <BatchSize xsi:type=""xsd:int"">?</BatchSize>
        //        ////                <!--Optional:-->
        //        ////                <GoodsAmount xsi:type=""xsd:string"">?</GoodsAmount>
        //        ////                <Prio xsi:type=""xsd:int"">?</Prio>
        //        ////                <VehicleId xsi:type=""xsd:int"">?</VehicleId>
        //        ////                <!--Optional:-->
        //        ////                <GoodsCategory xsi:type=""xsd:string"">?</GoodsCategory>
        //        ////                <DueTime xsi:type=""xsd:dateTime"">?</DueTime>
        //        ////             </orderToCreate>
        //        ////          </web:CreateOrder>
        //        ////       </soapenv:Body>
        //        ////</soapenv:Envelope>"

        //        //);



        //        HttpWebRequest request = CreateWebRequest();

        //        XmlDocument soapEnvelopeXml = new XmlDocument();
        //        soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
        //        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:item=""http://tempuri.org/"">
        //            <soapenv:Header/>
        //            <soapenv:Body>
        //                <item:login>
        //                    <!--Optional:-->
        //                    <item:email>antonio.filazzola@gmail.com</item:email>
        //                    <!--Optional:-->
        //                    <item:password>FLZnns</item:password>
        //                </item:login>
        //            </soapenv:Body>
        //        </soapenv:Envelope>"
        //        );

        //        using (Stream stream = request.GetRequestStream())
        //        {
        //            soapEnvelopeXml.Save(stream);
        //        }


        //        //Get the Response    
        //        HttpWebResponse wr = (HttpWebResponse)request.GetResponse();
        //        StreamReader rd = new StreamReader(wr.GetResponseStream());

        //        //The response to parse
        //        string xmlresponse = rd.ReadToEnd();


        //        StringReader sr = new StringReader(xmlresponse);
        //        XDocument doc = XDocument.Load(sr);



        //        //XDocument soapEnvelopeReturn = new XDocument();
        //        //XmlDocument soapEnvelopeReturn = new XmlDocument();
        //        //string strResult = @" 
        //        //    <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
        //        //    <soap:Body>
        //        //    <loginResponse xmlns=""http://tempuri.org/"">
        //        //    <loginResult>
        //        //    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
        //        //    <resultCode>200</resultCode>
        //        //    </loginResult>
        //        //    </loginResponse>
        //        //    </soap:Body>
        //        //    </soap:Envelope>
        //        //";

        //        string strResult = @" 
        //            <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
        //            <soap:Body>
        //            <loginResponse xmlns=""http://tempuri.org/"">   
        //             <loginResult>
        //            <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
        //            <resultCode>200</resultCode> 
        //             </loginResult>
        //            </loginResponse>
        //            </soap:Body>
        //            </soap:Envelope>
        //        ";
        //        string strResult1 = @"                    
        //            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
        //             <soap:Body>
        //              <LoginResponse xmlns=""http://example.com/SystemIntegration"">
        //                 <FirstName>@FirstName</FirstName>
        //               <LastName>@LastName</LastName>
        //              </LoginResponse>
        //                </soap:Body>
        //             </soap:Envelope>
        //        ";

        //        //soapEnvelopeReturn.LoadXml(@"
        //        //<soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
        //        //    <soap:Body>
        //        //    <loginResponse xmlns=""http://tempuri.org/"">
        //        //    <loginResult>
        //        //    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
        //        //    <resultCode>200</resultCode>
        //        //    </loginResult>
        //        //    </loginResponse>
        //        //    </soap:Body>
        //        //    </soap:Envelope>
        //        //"
        //        //);
        //        //soapEnvelopeReturn.LoadXml(strResult
        //        //);

        //        XDocument docx = XDocument.Parse(strResult);
        //        XNamespace ns = "http://tempuri.org/";

        //        //IEnumerable<XElement> responses = docx.Descendants(ns + "loginResponse");
        //        //foreach (XElement response in responses)
        //        //{                   
        //        //    string strResponseCode = (string)response.Element(ns + "Data");
        //        //    string strResponseText = (string)response.Element(ns + "resultCode");
        //        //}


        //        IEnumerable<XElement> responses = docx.Descendants(ns +"loginResult");
        //        foreach (XElement response in responses)
        //        {
        //            string strResponseCode = (string)response.Element( ns + "Data");
        //            string strResponseText = (string)response.Element( ns + "resultCode");
        //        }


        //        XDocument doc1 = XDocument.Parse(strResult1);

        //        XNamespace ns1 = "http://example.com/SystemIntegration";

        //        IEnumerable<XElement> responses1 = doc1.Descendants(ns1 + "LoginResponse");
        //        foreach (XElement response1 in responses1)
        //        {
        //            string strResponseCode = (string)response1.Element(ns1 + "FirstName");
        //            string strResponseText = (string)response1.Element(ns1 + "LastName");
        //        }




        //        //XNamespace ns1 = "http://example.com/SystemIntegration";

        //        //IEnumerable<XElement> responses = doc1.Descendants(ns1 + "LoginResponse");
        //        //foreach (XElement response in responses)
        //        //{
        //        //    string strResponseCode = (string)response.Element(ns1 + "FirstName");
        //        //    string strResponseText = (string)response.Element(ns1 + "LastName");
        //        //}








        //        //foreach (XElement response in responses)
        //        //{
        //        //    string strResponseCode = (string)response.Element(ns + "Data");
        //        //    string strResponseText = (string)response.Element(ns + "resultCode");
        //        //}



        //        //XNamespace ns = "https://localhost:44314/SoapDemo.asmx";
        //        //XElement el = doc.Elements().DescendantsAndSelf().FirstOrDefault(e => e.Name == ns + "resultCode");





        //        //string myNamespace = "https://localhost:44314/SoapDemo.asmx";

        //        //var results = from result in doc.Descendants(XName.Get("loginResult", myNamespace))
        //        //              select result.Element("resultCode").Value;


        //        //var results = doc.Descendants("loginResult").Select(x => new {
        //        //    name = (string)x.Element("Data"),
        //        //    count = (int)x.Element("resultCode")
        //        //}).ToList();





        //        //XNode node = doc.DescendantNodes()


        //        //foreach (XElement element in doc.De)

        //        //{
        //        //    Console.WriteLine("Name: {0}; Value: {1}",
        //        //                      (string)element.Attribute("name"),
        //        //                      (string)element.Element(ns + "value"));
        //        //}


        //        //int count = doc.DescendantNodes().Count();




        //        //XElement elem = doc.Root.Descendants().FirstOrDefault();

        //        //if(elem != null)
        //        //{
        //        //    elem.Element("resultCode").Value = "w";
        //        //}


        //        ////Response.Write(doc);
        //        ///

        //        //var items = from p in doc.Descendants("Data")
        //        //            select new
        //        //            {
        //        //                Phone = p.Element("email").Value,
        //        //                Email = p.Element("password").Value
        //        //            };
        //        //foreach (var re in items)
        //        //{
        //        //    Console.Write(re.Phone);

        //        //}


        //        //


        //        //foreach (XElement element in doc.Descendants(ns + "loginResult"))
        //        //{
        //        //    MessageBox.Show(element.ToString());
        //        //}




        //        //XNamespace ns = "http://ns.adobe.com/xfdf/";
        //        //var field = doc.Descendants("field")
        //        //               .FirstOrDefault();

        //        //if (field != null)
        //        //{
        //        //    string value = (string)field.Element("value");
        //        //    // Use value here
        //        //}
        //    }

        //    catch (Exception ex)
        //    {
        //        Global.ErrorAsync(_windowManager, ex.Message);

        //    }


        //}
        private void executeAgilogWebServiceSoap()
        {
            
            if(!IsLoading)
            {
                IsLoading = true;
                try
                {
                    var binding = new BasicHttpBinding() { MaxReceivedMessageSize = 1000000 };
                    var endpoint = new EndpointAddress(ConfigurationManager.AppSettings["ServerEndPoint"]);

                    WebServiceSoapAgilog.OperationsClient client = new WebServiceSoapAgilog.OperationsClient(binding, endpoint);

                    int ArrayLenght = 2;
                    WebServiceSoapAgilog.WebServiceOrderRow[] TestRows = new WebServiceSoapAgilog.WebServiceOrderRow[ArrayLenght];

                    for (int i = 0; i < ArrayLenght; i++)
                    {
                        WebServiceSoapAgilog.WebServiceOrderRow TmpRow = new WebServiceSoapAgilog.WebServiceOrderRow();
                        switch (i)
                        {
                            case 0:
                                TmpRow.TaskId = 1;
                                TmpRow.OperationType = 1;
                                TmpRow.Location = "A";
                                TmpRow.Quantity = 1;

                                break;
                            case 1:
                                TmpRow.TaskId = 1;
                                TmpRow.OperationType = 2;
                                TmpRow.Location = "B";
                                TmpRow.Quantity = 1;
                                break;

                            default:
                                break;
                        }
                        TestRows[i] = TmpRow;
                    }
                    client.TransportOrderStatusUpdate("XXX", 2, TestRows, 1, "ABC", 1, 2, 3, 4, 5, 12, 1);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }



            Console.Read();
        }


        public void execute()
        {
            try
            {
                //HttpWebRequest request = CreateWebRequest();

                //XmlDocument soapEnvelopeXml = new XmlDocument();
                //soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                ////    <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://agv/WebService.wsdl"" xmlns:q2=""http://schemas.xmlsoap.org/soap/encoding/"">
                ////       <soapenv:Header/>
                ////       <soapenv:Body>
                ////          <web:CreateOrder soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                ////             <orderToCreate xsi:type=""enc:WebServiceOrder"" xmlns:enc=""http://agv/WebService.wsdl/encoded"">
                ////                <!--Optional:-->
                ////                <OrderRows xsi:type=""enc:ArrayOfWebServiceOrderRow"" q2:arrayType=""enc:WebServiceOrderRow[]""/>
                ////                <!--Optional:-->
                ////                <OrderId xsi:type=""xsd:string"">?</OrderId>
                ////                <GoodsCarrier xsi:type=""xsd:int"">?</GoodsCarrier>
                ////                <!--Optional:-->
                ////                <GoodsId xsi:type=""xsd:string"">?</GoodsId>
                ////                <GoodsContainerId xsi:type=""xsd:int"">?</GoodsContainerId>
                ////                <GoodsWidth xsi:type=""xsd:int"">?</GoodsWidth>
                ////                <GoodsHeight xsi:type=""xsd:int"">?</GoodsHeight>
                ////                <GoodsLength xsi:type=""xsd:int"">?</GoodsLength>
                ////                <GoodsWeight xsi:type=""xsd:int"">?</GoodsWeight>
                ////                <!--Optional:-->
                ////                <ArticleID xsi:type=""xsd:string"">?</ArticleID>
                ////                <!--Optional:-->
                ////                <BatchID xsi:type=""xsd:string"">?</BatchID>
                ////                <BatchSize xsi:type=""xsd:int"">?</BatchSize>
                ////                <!--Optional:-->
                ////                <GoodsAmount xsi:type=""xsd:string"">?</GoodsAmount>
                ////                <Prio xsi:type=""xsd:int"">?</Prio>
                ////                <VehicleId xsi:type=""xsd:int"">?</VehicleId>
                ////                <!--Optional:-->
                ////                <GoodsCategory xsi:type=""xsd:string"">?</GoodsCategory>
                ////                <DueTime xsi:type=""xsd:dateTime"">?</DueTime>
                ////             </orderToCreate>
                ////          </web:CreateOrder>
                ////       </soapenv:Body>
                ////</soapenv:Envelope>"

                //);



                HttpWebRequest request = CreateWebRequest();

                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <tem:login>
                            <!--Optional:-->
                            <tem:email>antonio.filazzola@gmail.com</tem:email>
                            <!--Optional:-->
                            <tem:password>FLZnns</tem:password>
                        </tem:login>
                    </soapenv:Body>
                </soapenv:Envelope>"
                );

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }


                //Get the Response    
                HttpWebResponse wr = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(wr.GetResponseStream());

                //The response to parse
                string xmlresponse = rd.ReadToEnd();


                StringReader sr = new StringReader(xmlresponse);
                XDocument doc = XDocument.Load(sr);



                //XDocument soapEnvelopeReturn = new XDocument();
                //XmlDocument soapEnvelopeReturn = new XmlDocument();
                //string strResult = @" 
                //    <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                //    <soap:Body>
                //    <loginResponse xmlns=""http://tempuri.org/"">
                //    <loginResult>
                //    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                //    <resultCode>200</resultCode>
                //    </loginResult>
                //    </loginResponse>
                //    </soap:Body>
                //    </soap:Envelope>
                //";

                string strResult = @" 
                    <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <loginResponse xmlns=""http://tempuri.org/"">   
                     <loginResult>
                    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                    <resultCode>200</resultCode> 
                     </loginResult>
                    </loginResponse>
                    </soap:Body>
                    </soap:Envelope>
                ";
                string strResult1 = @"                    
                    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
	                    <soap:Body>
		                    <LoginResponse xmlns=""http://example.com/SystemIntegration"">
  			                    <FirstName>@FirstName</FirstName>
			                    <LastName>@LastName</LastName>
		                    </LoginResponse>
                        </soap:Body>
                     </soap:Envelope>
                ";

                //soapEnvelopeReturn.LoadXml(@"
                //<soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                //    <soap:Body>
                //    <loginResponse xmlns=""http://tempuri.org/"">
                //    <loginResult>
                //    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                //    <resultCode>200</resultCode>
                //    </loginResult>
                //    </loginResponse>
                //    </soap:Body>
                //    </soap:Envelope>
                //"
                //);
                //soapEnvelopeReturn.LoadXml(strResult
                //);

                XDocument docx = XDocument.Parse(strResult);
                XNamespace ns = "http://tempuri.org/";

                //IEnumerable<XElement> responses = docx.Descendants(ns + "loginResponse");
                //foreach (XElement response in responses)
                //{                   
                //    string strResponseCode = (string)response.Element(ns + "Data");
                //    string strResponseText = (string)response.Element(ns + "resultCode");
                //}


                IEnumerable<XElement> responses = docx.Descendants(ns + "loginResult");
                foreach (XElement response in responses)
                {
                    string strResponseCode = (string)response.Element(ns + "Data");
                    string strResponseText = (string)response.Element(ns + "resultCode");
                }


                XDocument doc1 = XDocument.Parse(strResult1);

                XNamespace ns1 = "http://example.com/SystemIntegration";

                IEnumerable<XElement> responses1 = doc1.Descendants(ns1 + "LoginResponse");
                foreach (XElement response1 in responses1)
                {
                    string strResponseCode = (string)response1.Element(ns1 + "FirstName");
                    string strResponseText = (string)response1.Element(ns1 + "LastName");
                }




                //XNamespace ns1 = "http://example.com/SystemIntegration";

                //IEnumerable<XElement> responses = doc1.Descendants(ns1 + "LoginResponse");
                //foreach (XElement response in responses)
                //{
                //    string strResponseCode = (string)response.Element(ns1 + "FirstName");
                //    string strResponseText = (string)response.Element(ns1 + "LastName");
                //}








                //foreach (XElement response in responses)
                //{
                //    string strResponseCode = (string)response.Element(ns + "Data");
                //    string strResponseText = (string)response.Element(ns + "resultCode");
                //}



                //XNamespace ns = "https://localhost:44314/SoapDemo.asmx";
                //XElement el = doc.Elements().DescendantsAndSelf().FirstOrDefault(e => e.Name == ns + "resultCode");





                //string myNamespace = "https://localhost:44314/SoapDemo.asmx";

                //var results = from result in doc.Descendants(XName.Get("loginResult", myNamespace))
                //              select result.Element("resultCode").Value;


                //var results = doc.Descendants("loginResult").Select(x => new {
                //    name = (string)x.Element("Data"),
                //    count = (int)x.Element("resultCode")
                //}).ToList();





                //XNode node = doc.DescendantNodes()


                //foreach (XElement element in doc.De)

                //{
                //    Console.WriteLine("Name: {0}; Value: {1}",
                //                      (string)element.Attribute("name"),
                //                      (string)element.Element(ns + "value"));
                //}


                //int count = doc.DescendantNodes().Count();




                //XElement elem = doc.Root.Descendants().FirstOrDefault();

                //if(elem != null)
                //{
                //    elem.Element("resultCode").Value = "w";
                //}


                ////Response.Write(doc);
                ///

                //var items = from p in doc.Descendants("Data")
                //            select new
                //            {
                //                Phone = p.Element("email").Value,
                //                Email = p.Element("password").Value
                //            };
                //foreach (var re in items)
                //{
                //    Console.Write(re.Phone);

                //}


                //


                //foreach (XElement element in doc.Descendants(ns + "loginResult"))
                //{
                //    MessageBox.Show(element.ToString());
                //}




                //XNamespace ns = "http://ns.adobe.com/xfdf/";
                //var field = doc.Descendants("field")
                //               .FirstOrDefault();

                //if (field != null)
                //{
                //    string value = (string)field.Element("value");
                //    // Use value here
                //}
            }

            catch (Exception ex)
            {
                Global.ErrorAsync(_windowManager, ex.Message);

            }


        }

        public void executeLoginSoapDemo()
        {
            try
            {
                //HttpWebRequest request = CreateWebRequest();

                //XmlDocument soapEnvelopeXml = new XmlDocument();
                //soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                ////    <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://agv/WebService.wsdl"" xmlns:q2=""http://schemas.xmlsoap.org/soap/encoding/"">
                ////       <soapenv:Header/>
                ////       <soapenv:Body>
                ////          <web:CreateOrder soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                ////             <orderToCreate xsi:type=""enc:WebServiceOrder"" xmlns:enc=""http://agv/WebService.wsdl/encoded"">
                ////                <!--Optional:-->
                ////                <OrderRows xsi:type=""enc:ArrayOfWebServiceOrderRow"" q2:arrayType=""enc:WebServiceOrderRow[]""/>
                ////                <!--Optional:-->
                ////                <OrderId xsi:type=""xsd:string"">?</OrderId>
                ////                <GoodsCarrier xsi:type=""xsd:int"">?</GoodsCarrier>
                ////                <!--Optional:-->
                ////                <GoodsId xsi:type=""xsd:string"">?</GoodsId>
                ////                <GoodsContainerId xsi:type=""xsd:int"">?</GoodsContainerId>
                ////                <GoodsWidth xsi:type=""xsd:int"">?</GoodsWidth>
                ////                <GoodsHeight xsi:type=""xsd:int"">?</GoodsHeight>
                ////                <GoodsLength xsi:type=""xsd:int"">?</GoodsLength>
                ////                <GoodsWeight xsi:type=""xsd:int"">?</GoodsWeight>
                ////                <!--Optional:-->
                ////                <ArticleID xsi:type=""xsd:string"">?</ArticleID>
                ////                <!--Optional:-->
                ////                <BatchID xsi:type=""xsd:string"">?</BatchID>
                ////                <BatchSize xsi:type=""xsd:int"">?</BatchSize>
                ////                <!--Optional:-->
                ////                <GoodsAmount xsi:type=""xsd:string"">?</GoodsAmount>
                ////                <Prio xsi:type=""xsd:int"">?</Prio>
                ////                <VehicleId xsi:type=""xsd:int"">?</VehicleId>
                ////                <!--Optional:-->
                ////                <GoodsCategory xsi:type=""xsd:string"">?</GoodsCategory>
                ////                <DueTime xsi:type=""xsd:dateTime"">?</DueTime>
                ////             </orderToCreate>
                ////          </web:CreateOrder>
                ////       </soapenv:Body>
                ////</soapenv:Envelope>"

                //);



                HttpWebRequest request = CreateWebRequest();

                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(@"<?xml version='1.0' encoding='utf-8'?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <tem:login>
                            <!--Optional:-->
                            <tem:email>antonio.filazzola@gmail.com</tem:email>
                            <!--Optional:-->
                            <tem:password>FLZnns</tem:password>
                        </tem:login>
                    </soapenv:Body>
                </soapenv:Envelope>"
                );




                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }


                //Get the Response    
                HttpWebResponse wr = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(wr.GetResponseStream());

                //The response to parse
                string xmlresponse = rd.ReadToEnd();


                StringReader sr = new StringReader(xmlresponse);
                XDocument doc = XDocument.Load(sr);

                XNamespace ns = "http://tempuri.org/";


                IEnumerable<XElement> responses = doc.Descendants(ns + "loginResult");
                foreach (XElement response in responses)
                {
                    string strResponseCode = (string)response.Element(ns + "Data");
                    string strResponseText = (string)response.Element(ns + "resultCode");
                }


                //IEnumerable<XElement> responses1 = doc.Descendants(ns + "LoginResponse");
                //foreach (XElement response1 in responses1)
                //{
                //    string strResponseCode = (string)response1.Element(ns + "FirstName");
                //    string strResponseText = (string)response1.Element(ns+ "LastName");
                //}



                ////XDocument soapEnvelopeReturn = new XDocument();
                ////XmlDocument soapEnvelopeReturn = new XmlDocument();
                ////string strResult = @" 
                ////    <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                ////    <soap:Body>
                ////    <loginResponse xmlns=""http://tempuri.org/"">
                ////    <loginResult>
                ////    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                ////    <resultCode>200</resultCode>
                ////    </loginResult>
                ////    </loginResponse>
                ////    </soap:Body>
                ////    </soap:Envelope>
                ////";

                //string strResult = @" 
                //    <soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                //    <soap:Body>
                //    <loginResponse xmlns=""http://tempuri.org/"">   
                //     <loginResult>
                //    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                //    <resultCode>200</resultCode> 
                //     </loginResult>
                //    </loginResponse>
                //    </soap:Body>
                //    </soap:Envelope>
                //";
                //string strResult1 = @"                    
                //    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                //     <soap:Body>
                //      <LoginResponse xmlns=""http://example.com/SystemIntegration"">
                //         <FirstName>@FirstName</FirstName>
                //       <LastName>@LastName</LastName>
                //      </LoginResponse>
                //        </soap:Body>
                //     </soap:Envelope>
                //";

                ////soapEnvelopeReturn.LoadXml(@"
                ////<soap:Envelope xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                ////    <soap:Body>
                ////    <loginResponse xmlns=""http://tempuri.org/"">
                ////    <loginResult>
                ////    <Data>[{""ID"":5,""Email"":""antonio.filazzola@gmail.com"",""Password"":""94721ACB59776A9225F2EDD9A8381C25"",""Role"":""Admin"",""Reg_Date"":""2023-03-27T20:54:28.96""}]</Data>
                ////    <resultCode>200</resultCode>
                ////    </loginResult>
                ////    </loginResponse>
                ////    </soap:Body>
                ////    </soap:Envelope>
                ////"
                ////);
                ////soapEnvelopeReturn.LoadXml(strResult
                ////);

                //XDocument docx = XDocument.Parse(strResult);
                //XNamespace ns = "http://tempuri.org/";

                ////IEnumerable<XElement> responses = docx.Descendants(ns + "loginResponse");
                ////foreach (XElement response in responses)
                ////{                   
                ////    string strResponseCode = (string)response.Element(ns + "Data");
                ////    string strResponseText = (string)response.Element(ns + "resultCode");
                ////}


                //IEnumerable<XElement> responses = docx.Descendants(ns + "loginResult");
                //foreach (XElement response in responses)
                //{
                //    string strResponseCode = (string)response.Element(ns + "Data");
                //    string strResponseText = (string)response.Element(ns + "resultCode");
                //}


                //XDocument doc1 = XDocument.Parse(strResult1);

                //XNamespace ns1 = "http://example.com/SystemIntegration";

                //IEnumerable<XElement> responses1 = doc1.Descendants(ns1 + "LoginResponse");
                //foreach (XElement response1 in responses1)
                //{
                //    string strResponseCode = (string)response1.Element(ns1 + "FirstName");
                //    string strResponseText = (string)response1.Element(ns1 + "LastName");
                //}




                ////XNamespace ns1 = "http://example.com/SystemIntegration";

                ////IEnumerable<XElement> responses = doc1.Descendants(ns1 + "LoginResponse");
                ////foreach (XElement response in responses)
                ////{
                ////    string strResponseCode = (string)response.Element(ns1 + "FirstName");
                ////    string strResponseText = (string)response.Element(ns1 + "LastName");
                ////}








                ////foreach (XElement response in responses)
                ////{
                ////    string strResponseCode = (string)response.Element(ns + "Data");
                ////    string strResponseText = (string)response.Element(ns + "resultCode");
                ////}



                ////XNamespace ns = "https://localhost:44314/SoapDemo.asmx";
                ////XElement el = doc.Elements().DescendantsAndSelf().FirstOrDefault(e => e.Name == ns + "resultCode");





                ////string myNamespace = "https://localhost:44314/SoapDemo.asmx";

                ////var results = from result in doc.Descendants(XName.Get("loginResult", myNamespace))
                ////              select result.Element("resultCode").Value;


                ////var results = doc.Descendants("loginResult").Select(x => new {
                ////    name = (string)x.Element("Data"),
                ////    count = (int)x.Element("resultCode")
                ////}).ToList();





                ////XNode node = doc.DescendantNodes()


                ////foreach (XElement element in doc.De)

                ////{
                ////    Console.WriteLine("Name: {0}; Value: {1}",
                ////                      (string)element.Attribute("name"),
                ////                      (string)element.Element(ns + "value"));
                ////}


                ////int count = doc.DescendantNodes().Count();




                ////XElement elem = doc.Root.Descendants().FirstOrDefault();

                ////if(elem != null)
                ////{
                ////    elem.Element("resultCode").Value = "w";
                ////}


                //////Response.Write(doc);
                /////

                ////var items = from p in doc.Descendants("Data")
                ////            select new
                ////            {
                ////                Phone = p.Element("email").Value,
                ////                Email = p.Element("password").Value
                ////            };
                ////foreach (var re in items)
                ////{
                ////    Console.Write(re.Phone);

                ////}


                ////


                ////foreach (XElement element in doc.Descendants(ns + "loginResult"))
                ////{
                ////    MessageBox.Show(element.ToString());
                ////}




                ////XNamespace ns = "http://ns.adobe.com/xfdf/";
                ////var field = doc.Descendants("field")
                ////               .FirstOrDefault();

                ////if (field != null)
                ////{
                ////    string value = (string)field.Element("value");
                ////    // Use value here
                ////}
            }

            catch (Exception ex)
            {
                Global.ErrorAsync(_windowManager, ex.Message);

            }


        }


        private void UpdateOrder(string orderId, int createOrderResult)
        {
            var Exc = "";
            var pOrderId = new SqlParameter("@OrderId", (object)orderId ?? DBNull.Value);
            var pResultError = new SqlParameter("@ResultError", (object)createOrderResult ?? DBNull.Value);
            //var pOrderId = new SqlParameter("@OrderId", (object)1 ?? DBNull.Value);
            //var pResultError = new SqlParameter("@ResultError", (object)1 ?? DBNull.Value);
            var pError = new SqlParameter("@Error", SqlDbType.NVarChar, 255);
            var retValue = DbUtils.ExecuteStoredProcedure(
            "msp_ToyotaResultMissions",
            (SqlConnection)Global.Instance.ConnGlobal, ref Exc, ref pError, pOrderId, pResultError);
            if (retValue == null || (int.Parse(retValue.ToString()) != 0) || (pError.Value != null && !string.IsNullOrWhiteSpace(pError.Value.ToString())))
            {
                string errorDesc = (pError.Value != null && !string.IsNullOrWhiteSpace(pError.Value.ToString())) ? pError.Value.ToString() : "unkown error";
                throw new Exception($"Error ");
            }
            else
            {
                retValue = ERetVal.OK;
            }
        }
        public string SOAPManual()
        {
            //const string url = "http://127.0.0.1:8080/OrderManagerHost/";
            //const string action = "CreateOrder";

            const string url = "http://www.dneonline.com/Calculator.asmx";
            const string action = "Add";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(url, action);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }
            return result;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <soap:Body>
                    <Method xmlns=""http://www.sample.com/path/"">
                    <Parameter1>param1</Parameter1>
                    <Parameter2>param2</Parameter2>
                    </Method>
                    </soap:Body>
                    </soap:Envelope>");
            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        private async Task<ToyotaOrder> ImpToyotaOrder()
        {

            ToyotaOrder retval = null;
            string error = null;
            //await Task.Factory.StartNew(() =>
            //{
            try
            {

                DataTable dt = DbUtils.ExecuteDataTable("msp_Toyota_CreateMissions", DbUtils.CloneConnection(Global.Instance.ConnGlobal), false);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ToyotaOrder toyotaOrder = new ToyotaOrder();
                    toyotaOrder.LineMission = new List<ToyotaDetailsOrder>();
                    List<ToyotaDetailsOrder> toyotaDetailsOrders = new List<ToyotaDetailsOrder>();
                    foreach (DataRow row in dt.Rows)
                    {
                        ToyotaDetailsOrder toyotaDetailsOrder = new ToyotaDetailsOrder();
                        toyotaOrder.OrderId = Convert.ToInt64(row.GetValue("TOYH_OrderId"));
                        toyotaOrder.GoodsId = Convert.ToString(row.GetValue("TOYH_UdcCode"));
                        toyotaOrder.GoodsCarrier = Convert.ToInt32(row.GetValue("TOYH_PalletType"));
                        toyotaDetailsOrder.TaskId = Convert.ToInt32(row.GetValue("TOYD_TaskId"));
                        toyotaDetailsOrder.OperationType = Convert.ToInt32(row.GetValue("TOYD_OperationType"));
                        toyotaDetailsOrder.Location = Convert.ToString(row.GetValue("TOYD_Location"));
                        toyotaOrder.LineMission.Add(toyotaDetailsOrder);
                    }
                    retval = toyotaOrder;
                }
            }
            catch (Exception ex)
            {
                Global.ErrorAsync(_windowManager, ex.Message);

            }

            //});
            return retval;

        }

        #endregion Global Events

        #region Private methods

        private async Task LoadData()
        {
            IsLoading = true;

            await Task.Factory.StartNew(() =>
             {
                 try
                 {
                     ParamManager.Init(Global.Instance.ConnGlobal);

                     //LoadBays();


                     //if (Controller > 0)
                     //    Global.Instance.AddCallBackClientMfcAsync(Controller);
                 }
                 catch (Exception ex)
                 {
                     Global.ErrorAsync(_windowManager, ex.Message);
                 }
             });

            IsLoading = true;
        }

        /// <summary>
        /// Carico la lista delle baie gestite
        /// </summary>





        #endregion Private methods

        #region Public Methods






        #endregion Public Methods
    }
}