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
    public class PlaceFireHydrant : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            FamilySymbol symbol = FindSymbol(doc, "室内组合消火栓箱 - 单栓 - 背面进水接口不带卷盘", "类型 A - 背面 - 50 mm");

            //如果没有加载则提示
            if (symbol == null)
            {
                MessageBox.Show("请先加载族：“室内组合消火栓箱 - 单栓 - 背面进水接口不带卷盘”");
                return Result.Succeeded;
            }

            //点选墙面，获取面的Reference和点击的点，此处忽略用户中断选择的处理
            Reference refer = uiDoc.Selection.PickObject(ObjectType.Face, "选择墙面");
            XYZ pickPoint = refer.GlobalPoint;

            //族坐标系X轴方向；此处为墙定位线方向
            Wall wall = doc.GetElement(refer) as Wall;
            Curve curve = (wall.Location as LocationCurve).Curve;

            //通过ComputeDerevatives取得curve的局部坐标，X方向即为线方向
            XYZ direct = curve.ComputeDerivatives(0.5, true).BasisX.Normalize();//这里的概念不难懂但非常复杂，书P108细看

            //新建事物并启动
            Transaction trans = new Transaction(doc, "放置消火栓");
            trans.Start();

            //激活族类型
            if (!symbol.IsActive)
                symbol.Activate();

            //放置消火栓
            FamilyInstance fi = doc.Create.NewFamilyInstance(refer, pickPoint, direct, symbol);

            //提交
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
