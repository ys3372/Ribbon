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
    public class BreakMEPCurve : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            MEPCurveSelectionFilter filter = new MEPCurveSelectionFilter();

            //选取需打断的管线，点选位置即为断点，未作取消处理
            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, filter, "点选管线");
            XYZ pickPoint = refer.GlobalPoint;

            //获取选中的管线对象
            MEPCurve mepCurve = doc.GetElement(refer) as MEPCurve;

            //获取原理的定位线、端点
            Line line = (mepCurve.Location as LocationCurve).Curve as Line;
            XYZ p1 = line.GetEndPoint(0);
            XYZ p2 = line.GetEndPoint(1);

            //获取管道起终点的连接件
            ConnectorSet conSet = mepCurve.ConnectorManager.Connectors;
            Connector conStart = ConnectorAtPoint(conSet, p1);
            Connector conEnd = ConnectorAtPoint(conSet, p2);

            //关键步骤1：获得与指定连接件项链的连接件
            Connector fittingConStart = GetConToConnector(conStart);
            Connector fittingConEnd = GetConToConnector(conEnd);

            //将拾取点投影到管线定位线上获取投影点
            XYZ newPoint = line.Project(pickPoint).XYZPoint;

            //将线以投影点为界分成两端新定位线
            Line line1 = Line.CreateBound(newPoint, p1);
            Line line2 = Line.CreateBound(newPoint, p2);

            //新建事物并开启
            Transaction trans = new Transaction(doc, "管线打断");
            trans.Start();

            //关键步骤2： 复制管线并将其定位线设置为新定位线
            MEPCurve mepCurve1 = CopyMEPToLine(doc, mepCurve, line1);
            MEPCurve mepCurve2 = CopyMEPToLine(doc, mepCurve, line2);

            //删除原来的管线
            doc.Delete(mepCurve.Id);


            //关键步骤3：恢复原有链接
            //获取新管线在原点处的Connector
            ConnectorSet newConSet1 = mepCurve1.ConnectorManager.Connectors;
            ConnectorSet newConSet2 = mepCurve2.ConnectorManager.Connectors;
            Connector newConStart = ConnectorAtPoint(newConSet1, p1);
            Connector newConEnd = ConnectorAtPoint(newConSet2, p2);

            //恢复与原管件的连接
            newConStart.ConnectTo(fittingConStart);
            newConEnd.ConnectTo(fittingConEnd);

            //提交事务
            trans.Commit();
            return Result.Succeeded;


        }

        public Connector ConnectorAtPoint(ConnectorSet conSet, XYZ point)
        {
            //遍历连接件集合
            foreach (Connector connector in conSet)
            {
                //距离容差设为1mm
                if (connector.Origin.DistanceTo(point) < 1 / 304.8)
                {
                    //返回该连接件
                    return connector;
                }
            }
            //如果没有匹配到，就返回null
            return null;
        }

        public Connector GetConToConnector(Connector conector)
        {
            foreach (Connector con in conector.AllRefs)
            {
                //仅选择管件
                if (con.Owner is FamilyInstance)
                {
                    return con;
                }

            }
            return null;
        }

        public MEPCurve CopyMEPToLine(Document doc, MEPCurve mepCurve, Line line)
        {
            //原位复制一份
            ElementId copyId = ElementTransformUtils.CopyElement(doc, mepCurve.Id, XYZ.Zero).First();

            //获得复制的对象
            MEPCurve mepCurveNew = doc.GetElement(copyId) as MEPCurve;

            //设置新对象的定位线
            (mepCurveNew.Location as LocationCurve).Curve = line;
            return mepCurveNew;
        }

        public class MEPCurveSelectionFilter : ISelectionFilter
        {

            public bool AllowElement(Element elem)
            {

                if (elem is MEPCurve && !(elem is InsulationLiningBase))
                {
                    return true;
                }

                else
                    return false;
            }


            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }

        }

    }

}
