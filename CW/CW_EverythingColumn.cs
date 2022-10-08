using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TSZ.RevitBaseDll.BIMData;
using TSZ.RevitBaseDll.Extends;


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]


    public class CW_EverythingColumn : IExternalCommand
    {

        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            ColumnSelectionFilter filter = new ColumnSelectionFilter();

            IList<Reference> refers = new List<Reference>();

            //refers = uiDoc.Selection.PickObjects(ObjectType.Element, filter, "点选柱体");
            Reference refers1 = uiDoc.Selection.PickObject(ObjectType.Element, filter, "点选柱体");
            Element element1 = doc.GetElement(refers1) as Element;


            //XYZ point = element1.GetPointExt();
            XYZ point = (element1.Location as LocationPoint).Point;

            Curve curve = element1.Location.GetCurveExt();
            Line line = (element1.Location as LocationCurve).GetLineExt();


            Transaction trans = new Transaction(doc, "选择柱子");
            trans.Start();

            FamilyInstance familyInstance1 = element1.ToFamilyInstanceExt();

            Double topOffset = familyInstance1.get_Parameter(BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;
            Double baseOffset = familyInstance1.get_Parameter(BuiltInParameter.SCHEDULE_BASE_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;

            ElementId topLevel = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();//柱顶标高
            String topLevelString = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsValueString();//柱顶标高文字版

            ElementId bottomLevel = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();//柱底标高
            String baseLevelString = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsValueString();//柱底标高

            //尝试读取其他参数
            FamilySymbol familySymbol = familyInstance1.Symbol;
            StructuralType sType = familyInstance1.StructuralType;

            //将底部、顶部的ID转化为Level这一类的元素

            Level top = doc.GetElement(topLevel) as Level;
            Level bottom = doc.GetElement(bottomLevel) as Level;

            MessageBox.Show("类型：楼层 属性" + top.ToString()
                + "\r\n" + "顶部偏移：" + topOffset.ToString()
                + "\r\n" + "底部偏移：" + baseOffset.ToString()
                + "\r\n" + "顶部标高楼层名：" + topLevelString
                + "\r\n" + "底部标高楼层ID：" + bottomLevel.ToString()
                + "\r\n" + "顶部楼层高度：" + top.Elevation * 304.8
                + "\r\n" + "底部楼层高度：" + bottom.Elevation * 304.8
                 + "\r\n" + "定位点坐标：" + point.ToString()
                 + "\r\n" + "族类型：" + familySymbol
                 + "\r\n" + "结构类型：" + sType);

            FamilyInstance familyInstance = doc.Create.NewFamilyInstance(point, familyInstance1.Symbol, bottom, familyInstance1.StructuralType);

            familyInstance.SetTopLevelExt(top.Id);//设置柱顶标高
            familyInstance.SetBaseOffsetExt(200 / 304.8);//对每一项起点减去标高
            familyInstance.SetTopOffsetExt(100 / 304.8);//设置底部偏移
            familyInstance.SetStMaterialExt(familyInstance1.GetStMaterialExt());//设置成和第1个元素相同的材料
            string concreteGradeExt = familyInstance1.GetConcreteGradeExt();//获取第1个元素的水泥等级
            if (!string.IsNullOrEmpty(concreteGradeExt))//如果存在水泥等级
                familyInstance.SetConcreteGradeExt(concreteGradeExt, true);//设置水泥等级


            trans.Commit();

            return Result.Succeeded;
        }
    }
}
