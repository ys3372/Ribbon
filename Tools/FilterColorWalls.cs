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
    public class FilterColorWalls : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View activeView = doc.ActiveView;//获得当前视图

            #region 自定义样式设置
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            //RGB颜色参照资源对照
            Color c = new Color(255, 0, 0);

            FilteredElementCollector fillCollector = new FilteredElementCollector(doc);
            List<Element> fplist = fillCollector.OfClass(typeof(FillPatternElement)).ToList();

            //用lambda表达式求实体填充
            ElementId solidId = fplist.FirstOrDefault(x => (x as FillPatternElement).GetFillPattern().IsSolidFill)?.Id;

            //线填充图案。获得第一个图案为例
            FilteredElementCollector linePatternCollector = new FilteredElementCollector(doc);
            linePatternCollector.OfClass(typeof(LinePatternElement));
            ElementId linePatternId = linePatternCollector.ToElementIds().ToList().First();

            //设置一系列参数，不知道少设置几个（使用默认）可不可以
            ogs.SetProjectionLinePatternId(linePatternId);
            ogs.SetProjectionLineColor(c);
            ogs.SetProjectionLineWeight(1);

            ogs.SetSurfaceForegroundPatternColor(c);
            ogs.SetSurfaceForegroundPatternId(solidId);
            ogs.SetSurfaceForegroundPatternVisible(true);

            ogs.SetSurfaceBackgroundPatternColor(c);
            ogs.SetSurfaceBackgroundPatternId(solidId);
            ogs.SetSurfaceBackgroundPatternVisible(true);

            ogs.SetSurfaceTransparency(1);

            ogs.SetCutForegroundPatternColor(c);
            ogs.SetCutForegroundPatternId(solidId);
            ogs.SetCutForegroundPatternVisible(true);

            ogs.SetCutForegroundPatternColor(c);
            ogs.SetCutForegroundPatternId(solidId);
            ogs.SetCutForegroundPatternVisible(true);

            ogs.SetHalftone(false);
            #endregion

            //新建事物并启动
            Transaction trans = new Transaction(doc, "5m墙体过滤器");
            trans.Start();

            #region 过滤器设置
            //过滤类别集合
            ICollection<ElementId> cgIds = new List<ElementId>();
            //添加墙类别
            cgIds.Add(doc.Settings.Categories.get_Item(BuiltInCategory.OST_Walls).Id);
            //创建过滤器规则
            List<FilterRule> fRules = new List<FilterRule>();//这里fRules就是规则，我们准备给它赋值
            //长度参数ID
            ElementId lengthParaId = new ElementId(BuiltInParameter.CURVE_ELEM_LENGTH);

            //墙体大于5m时设置构造柱
            double limit = 5000 / 304.8;
            fRules.Add(ParameterFilterRuleFactory.CreateGreaterRule(lengthParaId, limit, 0));

            //创建名称为“超过5m墙体”的过滤器
            string str = "超过5m墙体变红";
            ParameterFilterElement pfElement = ParameterFilterElement.Create(doc, str, cgIds);

            //绑定过滤规则
            pfElement.SetElementFilter(new ElementParameterFilter(fRules));
            #endregion

            #region 将自定义显示样式应用于视图过滤器
            //视图添加过滤器
            activeView.AddFilter(pfElement.Id);
            //视图设置过滤器可见性
            activeView.SetFilterVisibility(pfElement.Id, true);

            //自定义设置覆盖原有设置
            activeView.SetFilterOverrides(pfElement.Id, ogs);
            #endregion

            trans.Commit();
            return Result.Succeeded;
        }
    }
}
