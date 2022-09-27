﻿using Autodesk.Revit;
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
    public class Create3DIso : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;//这句等于 cD.Application.ActiveUIDocument.Document;)
            Autodesk.Revit.DB.View activeView = uiDoc.ActiveView;

            
            try
            {
            #region 先选定一个剖切框范围
            PickedBox pickedBox = null;
            pickedBox = uiDoc.Selection.PickBox(PickBoxStyle.Directional, "框选范围");
            //由于用户框选的方向无法确定，求出矩形框的左下和右上角
            XYZ pick1 = pickedBox.Min;
            XYZ pick2 = pickedBox.Max;
            double xmax = Math.Max(pick1.X, pick2.X);
            double ymax = Math.Max(pick1.Y, pick2.Y);
            double xmin = Math.Min(pick1.X, pick2.X);
            double ymin = Math.Min(pick1.Y, pick2.Y);

            //本案例高度设计为当前楼层开始，至3000高处
            //用View.GenLevel获取楼层，前提是当前视图为平面视图，本案例未作限制
            double zmin = activeView.GenLevel.Elevation;
            double zmax = zmin + 3000 / 304.8;
            XYZ min = new XYZ(xmin, ymin, zmin);
            XYZ max = new XYZ(xmax, ymax, zmax);


            //构造一个BoundingBox作为范围框
            BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
            boundingBox.Min = min;
            boundingBox.Max = max;
            #endregion


            //获取默认三维视图
            ElementTypeGroup typeGroup = ElementTypeGroup.ViewType3D;
            ElementId vId = doc.GetDefaultElementTypeId(typeGroup);

            //新建事物并启动
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

            catch
            {
                return Result.Succeeded;
            }
           

        }
    }
}
