using Autodesk.Revit;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class GetSolids : IExternalCommand
    {
        public List<Solid> GetSolidsOfElement(Element ele)
        {
            Options options = new Options();
            options.DetailLevel = ViewDetailLevel.Fine;
            options.ComputeReferences = true;
            options.IncludeNonVisibleObjects = true;
            GeometryElement geoElement = ele.get_Geometry(options);

            List<GeometryObject> geoObjects = new List<GeometryObject>();

            GetAllObj(geoElement, ref geoObjects);

            List<Solid> solids = geoObjects.ConvertAll(m => m as Solid);
            return solids;
        }


        //递归算法
        public void GetAllObj(GeometryElement gEle, ref List<GeometryObject> gObjs)
        {
            if (gEle == null)
            {
                return;
            }

            //遍历GeometryElement里的GeometryObject

            IEnumerator<GeometryObject> enumerator = gEle.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GeometryObject geoObject = enumerator.Current;
                Type type = geoObject.GetType();
                //如果是嵌套的GeometryElement，则递归
                if (type.Equals(typeof(GeometryElement)))
                {
                    GetAllObj(geoObject as GeometryElement, ref gObjs);
                }

                //如果是嵌套的GeometryInstance
                else if (type.Equals(typeof(GeometryInstance)))
                {
                    GetAllObj((geoObject as GeometryInstance).GetInstanceGeometry(), ref gObjs);
                }

                else
                {
                    if (type.Equals(typeof(Solid)))
                    {
                        Solid solid = geoObject as Solid;
                        if (solid.Faces.Size > 0 || solid.Edges.Size > 0)
                        {
                            gObjs.Add(geoObject);
                        }
                    }
                }

            }
        }

        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = cD.Application.ActiveUIDocument.Document;

            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, "选择楼梯");

            Element ele = doc.GetElement(refer);

            double vol = 0;

            List<Solid> solids = GetSolidsOfElement(ele);

            foreach (Solid solid in solids)
            {
                vol += solid.Volume;

            }

            double volSum = UnitUtils.ConvertFromInternalUnits(vol, DisplayUnitType.DUT_CUBIC_METERS);

            MessageBox.Show("所选模块体积为：" + Math.Round(volSum, 2) + "m\x00B3");
            return Result.Succeeded;
        }
    }
}
