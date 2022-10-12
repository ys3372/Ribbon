using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ribbon.Oct
{

    #region 创建墙    
    /// <summary>
    /// 方法一 指定墙线的起点终点
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateWall_1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            
            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("创建墙");

                //设置楼层ID
                ElementId levelId = new ElementId(311);
                View activeView = doc.ActiveView;
                if(activeView is ViewPlan)
                    levelId = activeView.LevelId;

                //选择点
                XYZ p1 = uiDoc.Selection.PickPoint("选择第一个点");
                XYZ p2 = uiDoc.Selection.PickPoint("选择第二个点");

                //创建；属性的最后一项判断是不是结构墙
                Wall wall = Wall.Create(doc, Line.CreateBound(p1, p2) ,levelId, false);

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// 方法二 根据闭合线组来做 默认底部level1顶部level2顶偏3000 无法设置 无法创建斜墙
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateWall_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //声明一个空集
            IList<Curve> curves = new List<Curve>();

            //声明四个点
            XYZ p1 = uiDoc.Selection.PickPoint("选择第一个点");
            XYZ p2 = uiDoc.Selection.PickPoint("选择第二个点");
            XYZ p1b = new XYZ(p1.X, p1.Y, p1.Z+8000/304.8);
            XYZ p2b = new XYZ(p2.X, p2.Y, p2.Z+300);

            //创建四条线
            curves.Add(Line.CreateBound(p1, p2));
            curves.Add(Line.CreateBound(p2, p2b));
            curves.Add(Line.CreateBound(p2b, p1b));
            curves.Add(Line.CreateBound(p1b, p1));

            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("创建墙");
                //创建；属性的最后一项判断是不是结构墙
                Wall wall = Wall.Create(doc, curves, false);

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// 方法三 在方法二的基础上指定 标高 和 墙类型
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateWall_3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            ElementId levelId = new ElementId(311);
            ElementId wallTypeId = new ElementId(397);

            //声明一个空集
            IList<Curve> curves = new List<Curve>();

            //声明四个点
            XYZ p1 = uiDoc.Selection.PickPoint("选择第一个点");
            XYZ p2 = uiDoc.Selection.PickPoint("选择第二个点");
            XYZ p1b = new XYZ(p1.X, p1.Y, p1.Z + 8000 / 304.8);
            XYZ p2b = new XYZ(p2.X, p2.Y, p2.Z + 300);



            //创建四条线
            curves.Add(Line.CreateBound(p1, p2));
            curves.Add(Line.CreateBound(p2, p2b));
            curves.Add(Line.CreateBound(p2b, p1b));
            curves.Add(Line.CreateBound(p1b, p1));

            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("创建墙");
                //创建；属性的最后一项判断是不是结构墙
                Wall wall = Wall.Create(doc, curves, wallTypeId, levelId, false);

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// 方法四 可以指定墙的朝向。注意法向量（最后一个参数normal）很关键，必须要垂直于线组成的墙面
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateWall_4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            ElementId levelId = new ElementId(311);
            ElementId wallTypeId = new ElementId(397);
            IList<Curve> curves = new List<Curve>();

            XYZ[] vertexes = new XYZ[] { new XYZ(0,0,0), new XYZ(0,100,0), new XYZ(0,0,100)};
            for(int ii = 0; ii < vertexes.Length; ii++)
            {
                if(ii != vertexes.Length-1)
                    curves.Add(Line.CreateBound(vertexes[ii], vertexes[ii+1]));
                else
                    curves.Add(Line.CreateBound(vertexes[ii], vertexes[0]));
            }
            Wall wall = null;
            using(Transaction trans = new Transaction(doc))
            {
                trans.Start("Create First Wall");
                wall = Wall.Create(doc, curves, wallTypeId, levelId, false, new XYZ(-1, 0, 0));
                trans.Commit();
            }

            //第二面墙
            curves.Clear();
            vertexes = new XYZ[] { new XYZ(0, 0, 100), new XYZ(0, 100, 100), new XYZ(0, 100, 0) };
            for(int ii = 0; ii < vertexes.Length; ii++)
            {
                if (ii != vertexes.Length-1)
                    curves.Add(Line.CreateBound(vertexes[ii], vertexes[ii + 1]));
                else
                    curves.Add(Line.CreateBound(vertexes[ii], vertexes[0]));
            }
            using(Transaction trans = new Transaction(doc))
            {
                trans.Start("Create Second Wall");
                wall = Wall.Create(doc, curves, wallTypeId, levelId, false, new XYZ(1, 0, 0));
                trans.Commit();
            }
            return Result.Succeeded;
        }
    }

    /// <summary>
    /// 方法五 指定所有相关参数
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateWall_5:IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            ElementId levelId = new ElementId(311);
            ElementId wallTypeId = new ElementId(397);

            using(Transaction trans = new Transaction(doc))
            {
                trans.Start("Create Wall");
                Wall wall = Wall.Create(doc, Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, 100/304.8, 0)), wallTypeId, levelId, 200/304.8, 300 / 304.8, true, false);
                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region 创建楼板

    /// <summary>
    /// 方法一 三角形楼板 三维视图下默认在level1上
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateFloor_1:IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            CurveArray curveArray = new CurveArray();
            XYZ p1 = uiDoc.Selection.PickPoint("选择三个点，1/3");
            XYZ p2 = uiDoc.Selection.PickPoint("选择三个点，2/3");
            XYZ p3 = uiDoc.Selection.PickPoint("选择三个点，3/3");

            curveArray.Append(Line.CreateBound(p1, p2));
            curveArray.Append(Line.CreateBound(p2, p3));
            curveArray.Append(Line.CreateBound(p3, p1));

            using(Transaction trans = new Transaction(doc))
            {
                trans.Start("创建楼板");
                Floor floor = doc.Create.NewFloor(curveArray, false);
                trans.Commit();
            }

            return Result.Succeeded;
        }
    }

    /// <summary>
    /// 方法二 三角形楼板 可设置结构/标高楼层
    /// 还有一个向量normal控制楼板朝向，但朝向几乎完全由三个坐标控制，后面设定的必须垂直于定义的面否则报错。
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CreateFloor_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            ElementId floorId = new ElementId(581371);
            ElementId levelId = new ElementId(245423);

            Level level = doc.GetElement(levelId) as Level;
            FloorType floorType = doc.GetElement(floorId) as FloorType;

            XYZ normal = new XYZ(0, 0, 1);
            XYZ normal1 = new XYZ(0, 0, -1);

            CurveArray curveArray = new CurveArray();


            try
            {
                XYZ p1 = uiDoc.Selection.PickPoint("选择三个点，1/3");
                XYZ p2 = uiDoc.Selection.PickPoint("选择三个点，2/3");
                XYZ p3 = uiDoc.Selection.PickPoint("选择三个点，3/3");

                curveArray.Append(Line.CreateBound(p1, p2));
                curveArray.Append(Line.CreateBound(p2, p3));
                curveArray.Append(Line.CreateBound(p3, p1));

                using (Transaction trans = new Transaction(doc))
                {
                    trans.Start("创建楼板");
                    Floor floor = doc.Create.NewFloor(curveArray, floorType, level, true, normal);

                    trans.Commit();
                }
                return Result.Succeeded;
            }

            catch
            {
                return Result.Succeeded;
            }
        }
    }

    #endregion
}
