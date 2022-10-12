using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using TSZ.RevitBaseDll.Extends;

namespace Ribbon
{
    public static class ASamples
    {
        /// <summary>
        /// 把元素转化为族实例
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static FamilyInstance ToFamilyInstanceExt(this Element elem) => elem as FamilyInstance;


        /// <summary>定位点、以柱为例Z坐标都是0，常规模型不0</summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Autodesk.Revit.DB.XYZ GetPointExt(this Autodesk.Revit.DB.Element element)
        {
            if (element == null)
            {
                return null;
            }

            if (!element.IsValidObject)
            {
                return null;
            }

            return element.Location.GetPointExt();
        }
        public static Autodesk.Revit.DB.XYZ GetPointExt(this Location loc)
        {
            return (loc as LocationPoint)?.Point;
        }

        /// <summary>
        /// 获取外接框
        /// </summary>
        /// <param name="element"></param>
        /// <param name="view"></param>
        /// <param name="blnCheck"></param>
        /// <returns></returns>
        public static BoundingBoxXYZ GetBoundingBoxExt(this Element element,View view = null,bool blnCheck = true)
        {
            BoundingBoxXYZ boundingBoxExt = element.get_BoundingBox(view);
            if (boundingBoxExt == null && view != null)
                boundingBoxExt = element.get_BoundingBox((View)null);
            return boundingBoxExt;
        }

        /// <summary>
        /// 创建直线，加了一个“两个点是否重合”的判断
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Line CreateBoundExt(XYZ pt1, XYZ pt2) => !pt1.IsEqualExt(pt2, ExternalDataWrapper.Current.App.ShortCurveTolerance) ? Line.CreateBound(pt1, pt2) : (Line)null;

        /// <summary>
        /// 判断两个点坐标是否重合
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsEqualExt(this XYZ first, XYZ second)
        {
            if ((first.X == second.X)&&(first.Y == second.Y)&&(first.Z == second.Z))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 定位线
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static Curve GetCurveExt(this Element elem) => !elem.IsValidObject ? (Curve)null : (!(elem is Grid) ? elem.Location.GetCurveExt() : (elem as Grid).Curve);

        /// <summary>
        /// 柱顶标高
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Autodesk.Revit.DB.Level GetTopLevelExt(this Autodesk.Revit.DB.FamilyInstance instance)
        {
            Autodesk.Revit.DB.Level result = null;
            Autodesk.Revit.DB.ElementId parameterElementId = instance.GetParameterElementId(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
            if (parameterElementId != Autodesk.Revit.DB.ElementId.InvalidElementId)
            {
                result = instance.Document.GetElement(parameterElementId) as Autodesk.Revit.DB.Level;
            }

            return result;
        }

        /// <summary>构件的标高</summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Level GetLevelExt(this Element element)
        {
            if (!(element.Document.GetElement(element.LevelId) is Level levelExt))
                levelExt = element.GetRefLevelExt();//获取参考水平
            if (levelExt == null)//没获取到
                levelExt = element.GetParameterElement(BuiltInParameter.SCHEDULE_LEVEL_PARAM) as Level;//获取另一个水平参数
            if (levelExt == null)//没获取到
                levelExt = element.GetParameterElement(BuiltInParameter.RBS_START_LEVEL_PARAM) as Level;//获取另一个水平参数
            if (levelExt == null && element.IsFamilyInstanceExt())//还没获取到，且你确实是个族
            {
                ElementId parameterElementId = element.GetParameterElementId(BuiltInParameter.FAMILY_LEVEL_PARAM);
                levelExt = element.Document.GetElement(parameterElementId) as Level;
            }
            if (levelExt == null)//还没还没还没获取到
                levelExt = element.GetRefLevelExt();//再赋一次值
            return levelExt;
        }

        /// <summary>结构材质</summary>
        /// <param name="elem"></param>
        /// <param name="mat"></param>
        public static void SetStMaterialExt(this Element elem, Material mat)
        {
            if (mat == null)
                return;
            elem.SetStMaterialExt(mat.Id);
            //elem.SetParameterExt(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM, mat.Id);
        }

        /// <summary>结构材质</summary>
        /// <param name="elem"></param>
        /// <param name="eleId"></param>
        public static void SetStMaterialExt(this Element elem, ElementId eleId) => elem.SetParameterExt(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM, eleId);


    }



}
