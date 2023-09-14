using System.Collections.Generic;

namespace AstarMgr.ViewModels
{
    class ExotecMethod
    {
        public string Json { get; set; }

        public string Url { get; set; }

        public List<ExotecMethod> GetExotecMethods()
        {
            List<ExotecMethod> methodsList = new List<ExotecMethod>();

            methodsList.Add(new ExotecMethod { Json = "Health", Url = "health/"});
            methodsList.Add(new ExotecMethod { Json = "ArticleDescription", Url = "article/{id}" });
            methodsList.Add(new ExotecMethod { Json = "StockDescription", Url = "stock" });
            //methodsList.Add(new ExotecMethod { Json = "Author", Url = "authors" });
            //methodsList.Add(new ExotecMethod { Json = "AuthorCollection", Url = "authorcollections" });

            methodsList.Add(new ExotecMethod { Json = "StockFillStatus", Url = "stock/status" });

            methodsList.Add(new ExotecMethod { Json = "ReplenishmentRequest", Url = "replenishment/request/{id}" });
            methodsList.Add(new ExotecMethod { Json = "ReplenishmentRequestStatus", Url = "replenishment/request/{id}" });

            methodsList.Add(new ExotecMethod { Json = "PreparationRequest", Url = "preparation/request/{id}" });
            methodsList.Add(new ExotecMethod { Json = "PreparationRequestStatus", Url = "preparation/request/{rid}/status" });
            methodsList.Add(new ExotecMethod { Json = "PreparationLineStatus", Url = "preparation/request/{rid}/line/{lid}/status" });
            methodsList.Add(new ExotecMethod { Json = "PreparationTransferStatus", Url = "preparation/request/{rid}/transfer/{id}/status" });
            methodsList.Add(new ExotecMethod { Json = "PreparationContainerStatus", Url = "preparation/container/{cid}/status" });
            methodsList.Add(new ExotecMethod { Json = "PreparationContainerDefinition", Url = "preparation/container/{cid}" });
            methodsList.Add(new ExotecMethod { Json = "PreparationContainer", Url = "preparation/containers" });
            methodsList.Add(new ExotecMethod { Json = "PreparationSkypodConveyingSystems", Url = "preparation/skypodconveyingsystems" });

            methodsList.Add(new ExotecMethod { Json = "BinInRequest", Url = "bin/in/request/{id}" });
            methodsList.Add(new ExotecMethod { Json = "BinInRequestStatus", Url = "bin/in/request/{id}/status" });
            methodsList.Add(new ExotecMethod { Json = "BinInLineStatus", Url = "bin/in/request/{rid}/line/{lid}/status" });
            methodsList.Add(new ExotecMethod { Json = "BinOutRequest", Url = "bin/out/request/{id}" });
            methodsList.Add(new ExotecMethod { Json = "BinOutRequestStatus", Url = "bin/out/request/{id}/status" });
            methodsList.Add(new ExotecMethod { Json = "BinOutLineStatus", Url = "bin/out/request/{rid}/line/{lid}/status" });
            methodsList.Add(new ExotecMethod { Json = "BinDefinition", Url = "bin/{id}" });
            methodsList.Add(new ExotecMethod { Json = "BinStatus", Url = "bin/{id}/status" });
            methodsList.Add(new ExotecMethod { Json = "Bins", Url = "bins" });

            return methodsList;
        }


    }
}
