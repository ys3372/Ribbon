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
    public class ObjectIsolate : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View actView = uiDoc.ActiveView;

            //获取当前选集
            ICollection<ElementId> elementIds = uiDoc.Selection.GetElementIds();
            //求所选图元类型集合
            List<ElementId> categorrIds = new List<ElementId>();
            foreach (ElementId id in elementIds)
            {
                ElementId categoryId = doc.GetElement(id).Category.Id;
                if (!categorrIds.Contains(categoryId))
                {
                    categorrIds.Add(categoryId);
                }
            }

            //新建事务并启动
            Transaction trans = new Transaction(doc, "隔离元素");
            trans.Start();

            //隔离类型集合
            actView.IsolateCategoriesTemporary(categorrIds);
            //判断是否应用视图样板，如果有视图样板，本案例直接设为“无”
            if (actView.ViewTemplateId != null)
            {
                actView.ViewTemplateId = ElementId.InvalidElementId;
            }

            //将临时隔离状态应用到视图
            actView.ConvertTemporaryHideIsolateToPermanent();

            //提交
            trans.Commit();
            return Result.Succeeded;


        }
    }
}
