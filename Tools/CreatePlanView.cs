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
    public class CreatePlanView : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = cD.Application.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View activeView = uiDoc.ActiveView;

            //获取默认建筑平面类型
            ElementTypeGroup typeGroup = ElementTypeGroup.ViewTypeFloorPlan;
            ElementId vId = doc.GetDefaultElementTypeId(typeGroup);

            //新建事物并启动
            Transaction trans = new Transaction(doc, "创建平面视图");
            trans.Start();

            //创建平面视图
            ViewPlan viewPlan = ViewPlan.Create(doc, vId, activeView.GenLevel.Id);

            //设置视图的名称，注意规避同名视图
            viewPlan.Name = activeView.Name + "平面";

            //分别设置视图的详细程度、比例、显示模式、规程
            viewPlan.DetailLevel = ViewDetailLevel.Medium;
            viewPlan.Scale = 150;
            viewPlan.DisplayStyle = DisplayStyle.HLR;//HLR为隐藏线
            viewPlan.Discipline = ViewDiscipline.Architectural;

            //设置视图范围，以设置剖切高度1500为例
            PlanViewRange pvr = viewPlan.GetViewRange();
            pvr.SetOffset(PlanViewPlane.CutPlane, 1500 / 304.8);
            viewPlan.SetViewRange(pvr);

            //提交事务
            trans.Commit();
            //将当前视图跳转到新建视图
            uiDoc.ActiveView = viewPlan;
            return Result.Succeeded;

        }
    }
}
