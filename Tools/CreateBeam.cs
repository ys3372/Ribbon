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
    public class CreateBeam : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //单位转换，1mm转换为内部单位
            double s = UnitUtils.ConvertToInternalUnits(1, UnitTypeId.Millimeters);

            //调用自带的FindSymbol方法
            FamilySymbol symbol = FindSymbol(doc, "混凝土 - 矩形梁", "400 x 800mm");

            //如果没有加载，提示
            if (symbol == null)
            {
                MessageBox.Show("请先加载结构框架族：“混凝土 - 矩形梁”");
                return Result.Succeeded;
            }

            //要求所在视图是平面视图，之后最好修改成任意
            Level level = doc.ActiveView.GenLevel;
            double height = level.Elevation;

            //梁定位线端点，本例直接输入坐标
            XYZ p1 = new XYZ(0, 0, height);
            XYZ p2 = new XYZ(4000 * s, 0, height);
            Line line = Line.CreateBound(p1, p2);

            //结构类型
            StructuralType sType = StructuralType.Beam;

            //新建并启动事务
            Transaction trans = new Transaction(doc, "创建梁实例");
            trans.Start();

            //激活族类型
            if (!symbol.IsActive)
                symbol.Activate();
            //创造梁实例
            FamilyInstance fi = doc.Create.NewFamilyInstance(line, symbol, level, sType);

            //要创建斜梁可以参考书本P104

            trans.Commit();
            return Result.Succeeded;
        }

        public FamilySymbol FindSymbol(Document doc, string familyName, string symbolName)
        {
            //声明族和
            Family family = null;
            FamilySymbol symbol = null;

            //用过滤器查找所有族
            FilteredElementCollector familyCol = new FilteredElementCollector(doc);
            familyCol.OfClass(typeof(Family));

            //按族名查找
            foreach (Family f in familyCol)
            {
                if (f.Name == familyName)
                {
                    family = f;
                    break;
                }
            }
            //如果没有找到族，直接返回
            if (family == null)
                return null;

            //获得所有类型的ID,再遍历
            foreach (ElementId fsId in family.GetFamilySymbolIds())
            {
                //将ID转换回FamilySymbol再按名称查找
                FamilySymbol fs = doc.GetElement(fsId) as FamilySymbol;
                if (fs.Name == symbolName)
                {
                    symbol = fs;
                    break;
                }
            }

            return symbol;


        }
    }
}
