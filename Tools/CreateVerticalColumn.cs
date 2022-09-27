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
    public class CreateVerticalColumn : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            FamilySymbol symbol = FindSymbol(doc, "混凝土 - 矩形 - 柱", "450 x 600mm");
            //代码提示用户后台加载
            if (symbol == null)
            {
                MessageBox.Show("请先加载结构柱族：“混凝土 - 矩形 - 柱”");
                return Result.Succeeded;
            }

            //用户点击确定结构柱定位点
            XYZ p = uiDoc.Selection.PickPoint("单机以放置结构柱");

            //结构柱所在楼层按当前视图（必须为平面，但在此未做限制）
            Level level = doc.ActiveView.GenLevel;//

            //结构类型
            StructuralType strType = StructuralType.Column;

            //新建事物并重新启动
            Transaction trans = new Transaction(doc, "创建结构柱");
            trans.Start();

            //激活族类型
            if (!symbol.IsActive)
                symbol.Activate();

            //创建结构柱实例
            FamilyInstance fi = doc.Create.NewFamilyInstance(p, symbol, level, strType);

            //提交事务
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
