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
    public class CreateFlexDuct : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //单位转换系数，1mm转换为Revit内部单位
            double s = UnitUtils.ConvertToInternalUnits(1, DisplayUnitType.DUT_MILLIMETERS);

            //新建类型收集器，过滤出所有系统类型
            FilteredElementCollector typeCol = new FilteredElementCollector(doc);
            typeCol.OfClass(typeof(MechanicalSystemType));

            //获得风管系统类型，以默认为例
            MechanicalSystemType msType = typeCol.First() as MechanicalSystemType;

            #region 选择圆形软风管类型
            FilteredElementCollector fdtCollector = new FilteredElementCollector(doc);
            fdtCollector.OfClass(typeof(FlexDuctType));
            //获得风管类型，找出第一个圆形形状的软风管类型
            List<FlexDuctType> fdts = fdtCollector.ToList().ConvertAll(m => m as FlexDuctType);
            FlexDuctType fdt = fdts.Find(m => m.Shape == ConnectorProfileType.Round);
            #endregion

            //当前视图标高
            Level level = doc.ActiveView.GenLevel;

            //软风管控制点示例
            XYZ p1 = new XYZ(0, 0, level.Elevation + 3000 * s);
            XYZ p2 = new XYZ(1000 * s, 2000 * s, level.Elevation + 3000 * s);
            XYZ p3 = new XYZ(2000 * s, 2000 * s, level.Elevation + 2500 * s);
            List<XYZ> points = new List<XYZ> { p1, p2, p3 };

            //起点方向以X方向为例；终点方向以Y方向为例；
            XYZ startDir = XYZ.BasisX;
            XYZ endDir = -XYZ.BasisZ;

            //创建事务并启动
            Transaction trans = new Transaction(doc, "创建软风管");
            trans.Start();

            //创建软风管
            FlexDuct fDuct = FlexDuct.Create(doc, msType.Id, fdt.Id, level.Id, startDir, endDir, points);
            fDuct.get_Parameter(BuiltInParameter.RBS_CURVE_DIAMETER_PARAM).Set(200 * s);

            //提交事务
            trans.Commit();
            return Result.Succeeded;
        }
    }
}
