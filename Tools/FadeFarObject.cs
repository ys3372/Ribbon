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
    public class FadeFarObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            Autodesk.Revit.ApplicationServices.Application Revit = cD.Application.Application;
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //获得当前视图
            Autodesk.Revit.DB.View actView = doc.ActiveView;
            //如果当前视图不是剖面则退出
            if (!(actView is ViewSection))
            {
                MessageBox.Show("请在立面或剖面视图执行命令。");
                return Result.Succeeded;
            }

            //将当前视图转化为剖面视图
            ViewSection vs = actView as ViewSection;

            //建立剖面坐标系，用以计算试图在的图元与剖切面的距离
            //注意vs.ViewDirection方向是指从屏幕往用户看的方向
            Transform transform = null;
            transform = Transform.Identity;
            transform.Origin = vs.Origin;
            transform.BasisY = vs.UpDirection;
            transform.BasisZ = -vs.ViewDirection;
            transform.BasisX = transform.BasisY.CrossProduct(transform.BasisZ);//用向量叉积计算第三个方向

            //淡显的临界距离
            double distance = 15000 / 304.8;

            //新建当前视图过滤器，获取视图中的元素
            FilteredElementCollector col = new FilteredElementCollector(doc, actView.Id);
            IList<Element> elements = col.ToElements();

            //记录需要淡显的颜色
            IList<Element> eleChangeColors = new List<Element>();
            //遍历当前元素，按BoundingBox中心距离计算
            foreach (Element e in elements)
            {
                //try-catch规避没有BoundingBox的元素
                try
                {
                    BoundingBoxXYZ boundingBox = e.get_BoundingBox(actView);
                    XYZ center = (boundingBox.Max + boundingBox.Min) / 2;
                    //center 在剖面坐标系中的Z坐标，就是其与剖面的距离
                    if (transform.Inverse.OfPoint(center).Z > distance)
                        eleChangeColors.Add(e);
                }
                catch
                {
                }
            }
            //自定义显示样式设置
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            //设置为半色调
            ogs.SetHalftone(true);
            //设置投影线为细线
            ogs.SetProjectionLineWeight(1);

            //为加大对比效果将表面填充为灰色实体
            FilteredElementCollector fillCollector = new FilteredElementCollector(doc);
            List<Element> fplist = fillCollector.OfClass(typeof(FillPatternElement)).ToList();

            //用lambda表达式求实体填充样式
            ElementId solidId = fplist.FirstOrDefault(x => (x as FillPatternElement).GetFillPattern().IsSolidFill)?.Id;
            //设置表面填充
            ogs.SetSurfaceForegroundPatternColor(new Color(128, 128, 128));
            ogs.SetSurfaceBackgroundPatternId(solidId);

            //新建事物并开启
            Transaction trans = new Transaction(doc, "远距淡显");
            trans.Start();
            //遍历需要的元素
            foreach (Element e in eleChangeColors)
            {
                //用ogs覆盖视图中的图元显示
                actView.SetElementOverrides(e.Id, ogs);
            }

            //提交事务
            trans.Commit();
            return Result.Succeeded;

        }
    }
}
