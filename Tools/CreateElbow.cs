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
    public class CreateElbow : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //选择风管，此处略去过滤选择及取消选择处理
            Reference refer1 = uiDoc.Selection.PickObject(ObjectType.Element, "风管1");
            Reference refer2 = uiDoc.Selection.PickObject(ObjectType.Element, "风管2");

            //获取风管对象
            Duct duct1 = duct1 = doc.GetElement(refer1) as Duct;
            Duct duct2 = duct2 = doc.GetElement(refer2) as Duct;

            //获得风管定位线
            Line line1 = (duct1.Location as LocationCurve).Curve as Line;
            Line line2 = (duct2.Location as LocationCurve).Curve as Line;

            //获得风管端点集合
            List<XYZ> ps1 = new List<XYZ> { line1.GetEndPoint(0), line1.GetEndPoint(1) };
            List<XYZ> ps2 = new List<XYZ> { line2.GetEndPoint(0), line2.GetEndPoint(1) };

            //求最相近的两个点。预设一个大数，然后遍历找最小的距离
            double distance = int.MaxValue;
            XYZ origin1 = null;
            XYZ origin2 = null;
            foreach (XYZ xyz1 in ps1)
            {
                foreach (XYZ xyz2 in ps2)
                {
                    if (xyz1.DistanceTo(xyz2) < distance)
                    {
                        origin1 = xyz1;
                        origin2 = xyz2;
                        distance = xyz1.DistanceTo(xyz2);
                    }
                }
            }

            //获得匹配风管的连接件方法参考前文连接件获取
            Connector connector1 = ConnectorAtPoint(duct1, origin1);
            Connector connector2 = ConnectorAtPoint(duct2, origin2);

            //创建事物并启动
            Transaction trans = new Transaction(doc, "创建弯头");
            trans.Start();

            //创建弯头，为避免创建失败弹错，用try-catch规避
            try
            {
                doc.Create.NewElbowFitting(connector1, connector2);
            }
            catch
            {
                MessageBox.Show("弯头创建失败，请重新选择");
            }

            trans.Commit();
            return Result.Succeeded;
        }

        public Connector ConnectorAtPoint(Element e, XYZ point)
        {
            ConnectorSet connectorSet = null;

            //风管连接集合
            if (e is Duct)
                connectorSet = (e as Duct).ConnectorManager.Connectors;

            //管道连接件集合
            if (e is Pipe)
                connectorSet = (e as Pipe).ConnectorManager.Connectors;

            //桥架连接件集合
            if (e is CableTray)
                connectorSet = (e as CableTray).ConnectorManager.Connectors;

            //管件等可载入族的连接件集合
            if (e is FamilyInstance)
            {
                FamilyInstance fi = e as FamilyInstance;
                connectorSet = fi.MEPModel.ConnectorManager.Connectors;
            }

            //遍历连接件集合
            foreach (Connector connector in connectorSet)
            {
                //如果连接件的中心和目标的相距很小时视为目标连件
                if (connector.Origin.DistanceTo(point) < 1 / 304.8)
                {
                    return connector;
                }
            }

            //前面都是匹配到的情况，如果一个都没匹配上，则返回null
            return null;
        }

    }
}
