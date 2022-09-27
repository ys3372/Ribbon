using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit.DB.Structure;
using static System.Net.WebRequestMethods;
using TSZ.RevitBaseDll.Extends;

namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class SetLevel : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, "");
            Element elem = doc.GetElement(refer);

            Transaction trans = new Transaction(doc, "修改对象");


            trans.Start();

            LevelType levelType = doc.GetElement(elem.GetTypeId()) as LevelType;
            Parameter relativeBaseType = levelType.get_Parameter(BuiltInParameter.LEVEL_RELATIVE_BASE_TYPE);
            relativeBaseType.Set(1);

            trans.Commit();
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ListIds : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            try
            {
                Document doc = cD.Application.ActiveUIDocument.Document;

                //类过滤器，过滤所有FamilyInstance的元素
                ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));

                //创建一个类别过滤器过滤所有内建类型为
                ElementCategoryFilter doorsCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                //逻辑过滤器，组合前两个
                LogicalAndFilter doorInstanceFilter = new LogicalAndFilter(familyInstanceFilter, doorsCategoryfilter);

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<ElementId> doors = collector.WherePasses(doorInstanceFilter).ToElementIds();

                String prompt = "The ids of the doors in the current document are: ";
                foreach (ElementId id in doors)
                {
                    prompt += "\n\t" + id.IntegerValue;
                }

                TaskDialog.Show("Revit", prompt);
            }
            catch (Exception ex)
            {
                ms = ex.Message;
                return Result.Failed;
            }
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ExportCSV : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = cD.Application.ActiveUIDocument.Document;

            //单位转换系数
            double s = UnitUtils.ConvertToInternalUnits(1, DisplayUnitType.DUT_MILLIMETERS);

            //导出窗口
            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Title = "导出.csv文件";
            sfDialog.Filter = "csv文件(*.csv)|*.csv";//保存格式

            if (DialogResult.OK != sfDialog.ShowDialog())
            {
                return Result.Cancelled;
            }

            ColumnSelectionFilter filter = new ColumnSelectionFilter();

            #region
            IList<Reference> refers = uiDoc.Selection.PickObjects(ObjectType.Element, filter, "选择柱").ToList();
            //创建字符收集器
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("楼层属性, 顶部偏移, 底部偏移, 顶部参考楼层, 底部参考楼层, 顶部参考楼层ID, 底部参考楼层ID, 顶部标高, 底部标高, 坐标X, 坐标Y, 坐标Z, 族类型, 结构类型");

            foreach (Reference r in refers)
            {
                Element elem = doc.GetElement(r) as Element;
                XYZ point = (elem.Location as LocationPoint).Point;

                Curve curve = elem.Location.GetCurveExt();
                Line line = (elem.Location as LocationCurve).GetLineExt();


                FamilyInstance familyInstance = elem.ToFamilyInstanceExt();


                Double topOffset = familyInstance.get_Parameter(BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;
                Double baseOffset = familyInstance.get_Parameter(BuiltInParameter.SCHEDULE_BASE_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;

                ElementId topLevel = familyInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();//柱顶标高
                String topLevelString = familyInstance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsValueString();//柱顶标高文字版

                ElementId baseLevel = familyInstance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();//柱底标高
                String baseLevelString = familyInstance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsValueString();//柱底标高

                //尝试读取其他参数
                FamilySymbol familySymbol = familyInstance.Symbol;
                StructuralType sType = familyInstance.StructuralType;


                //将底部、顶部的ID转化为Level这一类的元素
                Level top = doc.GetElement(topLevel) as Level;
                Level bottom = doc.GetElement(baseLevel) as Level;

                //导入信息
                sb.AppendLine(top.ToString()
                + "," + topOffset.ToString()
                + "," + baseOffset.ToString()
                + "," + topLevelString
                + "," + baseLevelString
                + "," + topLevel.ToString()
                + "," + baseLevel.ToString()
                + "," + top.Elevation * 304.8
                + "," + bottom.Elevation * 304.8
                + "," + point.ToString()
                + "," + familySymbol.ToString()
                + "," + sType);
            }
            #endregion


            //写入文件
            System.IO.File.WriteAllText(sfDialog.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("导出完成");

            //打开文件夹
            System.Diagnostics.Process.Start(Path.GetDirectoryName(sfDialog.FileName));
            return Result.Succeeded;

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ImportCSV : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = cD.Application.ActiveUIDocument.Document;

            //单位转换系数
            double s = UnitUtils.ConvertToInternalUnits(1, DisplayUnitType.DUT_MILLIMETERS);

            //读取csv
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.Title = "中国联合";
            oDialog.Filter = "csv文件(*.csv)|*.csv";
            if (DialogResult.OK == oDialog.ShowDialog())
            {
                //文件地址
                string path = oDialog.FileName;
                List<string> items = new List<string>();

                //用StringReader读取文本数据
                using (StreamReader sRead = new StreamReader(path, Encoding.Default))
                {
                    string content = sRead.ReadToEnd();
                    string[] lines;
                    lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    items = lines.ToList();
                }
                //新建收集器
                FilteredElementCollector fSymCol = new FilteredElementCollector(doc);
                //过滤FamilySymbol
                fSymCol.OfClass(typeof(FamilySymbol));
                //获得桩族
                IEnumerable<FamilySymbol> fSyms = from elem in fSymCol
                                                  let type = elem as FamilySymbol
                                                  where type.FamilyName == "混凝土圆形柱"
                                                  select type;

                //获得桩的FamilySymbol
                FamilySymbol fs = fSyms.First();

                //新建事物并启动
                Transaction trans = new Transaction(doc, "创建柱");
                trans.Start();

                fs.Activate();

                //第一行是列名。从第二行开始。
                for (int i = 1; i < items.Count; i++)
                {
                    if ((items[i] == "") || (items[i] == null))
                        continue;
                    string[] rows = items[i].Split(',');

                    //获得桩型号、坐标、直径和长度等参数
                    string type = rows[0];
                    double pointX = double.Parse(rows[1]);
                    double pointY = double.Parse(rows[2]);
                    double pointZ = double.Parse(rows[3]);
                    XYZ point = new XYZ(pointX, pointY, 0);

                    //放置桩族
                    StructuralType st = StructuralType.NonStructural;
                    FamilyInstance fi = doc.Create.NewFamilyInstance(point, fs, st);

                    //设置桩参数
                    fi.LookupParameter("").Set(rows[0]);
                    fi.LookupParameter("").Set(pointZ);
                }
                trans.Commit();

            }
            return Result.Succeeded;
        }
    }

    public class BreakMEPBzhan : IExternalCommand
    {
        UIDocument uiDoc = null;
        Document doc = null;
        Application application = null;

        public Result Execute(ExternalCommandData cD, ref string message, ElementSet elements)
        {
            UIApplication uiApp = cD.Application;
            Autodesk.Revit.ApplicationServices.Application application = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            using (Transaction t = new Transaction(doc, "tran"))
            {
                t.Start();
                BreakMEPCurvew();
                t.Commit();
            }
            //创建窗体对象
            //Ribbon.Form.BreakMEPCurveForm breakMEPCurveForm = new Ribbon.Form.BreakMEPCurveForm(this);
            ////throw new NotImplementedException();
            //breakMEPCurveForm.Show();
            return Result.Succeeded;
        }


        public void BreakMEPCurvew()
        {
            MEPCurveSelectionFilter filter = new MEPCurveSelectionFilter();
            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, filter, "");
            MEPCurve mEPCurve = doc.GetElement(refer) as MEPCurve;

            XYZ breakXYZ = uiDoc.Selection.PickPoint();
            ICollection<ElementId> ids = ElementTransformUtils.CopyElement(doc, mEPCurve.Id, new XYZ(0, 0, 0));
            ElementId newId = ids.FirstOrDefault();
            MEPCurve mEPCurveCopy = doc.GetElement(newId) as MEPCurve;


            Curve curve = (mEPCurve.Location as LocationCurve).Curve;
            XYZ startXYZ = curve.GetEndPoint(0);
            XYZ endXYZ = curve.GetEndPoint(1);

            //映射点
            breakXYZ = curve.Project(breakXYZ).XYZPoint;

            Line line1 = Line.CreateBound(startXYZ, breakXYZ);
            Line line2 = Line.CreateBound(breakXYZ, endXYZ);

            (mEPCurve.Location as LocationCurve).Curve = line1;
            (mEPCurveCopy.Location as LocationCurve).Curve = line2;

        }
        public class MEPCurveSelectionFilter : ISelectionFilter
        {

            public bool AllowElement(Element elem)
            {

                if (elem is MEPCurve && !(elem is InsulationLiningBase))
                {
                    return true;
                }

                else
                    return false;
            }


            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }

        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CW_ColumnCombine : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIApplication uiApp;
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            ColumnSelectionFilter filter = new ColumnSelectionFilter();

            IList<Reference> refers = new List<Reference>();

            //refers = uiDoc.Selection.PickObjects(ObjectType.Element, filter, "点选柱体");
            Reference refers1 = uiDoc.Selection.PickObject(ObjectType.Element, filter, "点选柱体");
            Element element1 = doc.GetElement(refers1) as Element;



            Transaction trans = new Transaction(doc, "创建柱子");
            trans.Start();

            FamilyInstance familyInstance1 = element1.ToFamilyInstanceExt();


            //获取顶部和底部偏移
            Double topOffset = familyInstance1.get_Parameter(BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;
            Double baseOffset = familyInstance1.get_Parameter(BuiltInParameter.SCHEDULE_BASE_LEVEL_OFFSET_PARAM).AsDouble() * 304.8;

            //获取顶部标高、底部标高的ID和文字版
            ElementId topLevel = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();//柱顶标高
            String topLevelString = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsValueString();//柱顶标高文字版

            ElementId Level = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();//柱顶标高
            String LevelString = familyInstance1.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsValueString();//柱底标高



            //将底部、顶部的ID转化为Level这一类的元素
            Level level = doc.GetElement(Level) as Level;

            Level top = doc.GetElement(topLevel) as Level;
            Level bottom = doc.GetElement(Level) as Level;

            MessageBox.Show("类型：基础楼层 无法文字显示" + level.ToString() + "\r\n" + "顶部偏移：" + topOffset.ToString() + "\r\n" + "底部偏移：" + baseOffset.ToString() + "\r\n" + "顶部标高楼层名：" + topLevelString + "\r\n" + "底部标高楼层ID：" + Level.ToString());

            FamilyInstance familyInstance = doc.Create.NewFamilyInstance(XYZ.Zero, familyInstance1.Symbol, top, familyInstance1.StructuralType);

            trans.Commit();

            return Result.Succeeded;
        }
    }


}

