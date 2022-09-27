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
    public class Object3D : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Autodesk.Revit.DB.View activeView = uiDoc.ActiveView;


            //构造一个BoundingBox作为范围框
            BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
            #region 先选设一个BoundingBox
            try
            {
                PickedBox pickedBox = null;
                pickedBox = uiDoc.Selection.PickBox(PickBoxStyle.Directional, "框选范围");

                //矩形框的左下(min)和右上角(max)值
                XYZ pick1 = pickedBox.Min;
                XYZ pick2 = pickedBox.Max;

                double xmax = Math.Max(pick1.X, pick2.X);
                double xmin = Math.Min(pick1.X, pick2.X);

                double ymax = Math.Max(pick1.Y, pick2.Y);
                double ymin = Math.Min(pick1.Y, pick2.Y);

                double zmax = Math.Max(pick1.Z, pick2.Z);
                double zmin = Math.Min(pick1.Z, pick2.Z);

                if ((activeView is View3D))
                {
                    MessageBox.Show("3D视图下框选可能不准确，建议更换视图");
                }
                else if(activeView is ViewSection)
                {
                    ymax = 1500 / 304.8;
                    ymin = -1500 / 304.8;
                }
                else if (activeView is ViewPlan)
                {
                    zmin = -3000 / 304.8;
                    zmax = 3000 / 304.8;
                }
                else
                {
                    zmin = 0;
                    zmax = zmin + 3000 / 304.8;
                }

                XYZ min = new XYZ(xmin, ymin, zmin);
                XYZ max = new XYZ(xmax, ymax, zmax);

                boundingBox.Min = min;
                boundingBox.Max = max;
            }

            catch
            {
                return Result.Cancelled;
            }
            #endregion

            //获取默认三维视图
            ElementTypeGroup typeGroup = ElementTypeGroup.ViewType3D;
            ElementId vId = doc.GetDefaultElementTypeId(typeGroup);

            //新建事物并启动；
            Transaction trans = new Transaction(doc, "创建三维视图");
            trans.Start();

            //创建轴测图 
            View3D view3D = View3D.CreateIsometric(doc, vId);

            //设置剖面框范围
            view3D.SetSectionBox(boundingBox);
            trans.Commit();

            //将当前视图跳转到该平面
            uiDoc.ActiveView = view3D;
            return Result.Succeeded;
        }
    }
}
