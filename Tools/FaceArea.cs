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


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class FaceArea : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Reference refer;

            //使用try catch避免取消选中的弹窗，防止选择时Esc推出导致refer为空
            try
            {
                //选择面
                refer = uiDoc.Selection.PickObject(ObjectType.Face);
            }

            catch
            {
                //如果中断选择则结束命令
                return Result.Succeeded;
            }

            //得到元素
            Element ele = doc.GetElement(refer);

            //通过元素得到元素的面
            PlanarFace pFace = ele.GetGeometryObjectFromReference(refer) as PlanarFace;

            //获取面积
            double area = pFace.Area;
            double cvtArea = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);

            //保留两位小数
            TaskDialog.Show("面积", "所选面的面积为：" + Math.Round(cvtArea, 2) + "m\x00B2");

            return Result.Succeeded;
        }
    }
}
