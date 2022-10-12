// Decompiled with JetBrains decompiler
// Type: TSZ.RevitBaseDll.Extends.FamilyInstanceExtend
// Assembly: TSZ.RevitBaseDll, Version=2016.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E1D2EB6-119E-4114-B3FE-9A5B12A45FBC
// Assembly location: D:\revit文件\TSZ\TSRevitFor2018-TSTEDS\DLL\TSZ.RevitBaseDll.dll
// XML documentation location: D:\revit文件\TSZ\TSRevitFor2018-TSTEDS\DLL\TSZ.RevitBaseDll.xml

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

using TSZ.RevitBaseDll.Extends;
#endregion

namespace Ribbon
{
    /// <summary>
    /// TSZ.RevitBaseDll.Extends
    /// FamilyInstanceExtend
    /// </summary>
    public static class AToolList
    {
        /// <summary>建筑柱</summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsAcColumnExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_Columns);
        public static bool IsEqualExt(this Category cat, BuiltInCategory bic) => cat != null && cat.GetBuiltInCategoryExt() == bic;//类型cat不为null，且内置类型和输入（上一句）的一致
        //public static BuiltInCategory GetBuiltInCategoryExt(this Category cat) => cat.Id.GetBuiltInCategoryExt();

        /// <summary>结构柱</summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsStColumnExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_StructuralColumns);

        /// <summary>常规模型</summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsGenericModelExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_GenericModel);

        /// <summary>建筑柱、结构柱</summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsColumnExt(this Element instance) => instance.IsAcColumnExt() || instance.IsStColumnExt();

        ///// <summary>转角度</summary>
        ///// <param name="instance"></param>
        ///// <param name="dValue"></param>
        ///// <param name="pt"></param>
        //public static void SetRotation(this FamilyInstance instance, double dValue, XYZ pt = null)
        //{
        //    if (pt == null)
        //        pt = instance.GetPointExt();
        //    if (pt == null)
        //        return;
        //    instance.Document.RotateElementExt((Element)instance, pt, dValue);
        //}

        ///// <summary>结构框架</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsStFramingExt(this Element instance) => instance?.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_StructuralFraming);

        ///// <summary>梁</summary>
        ///// <param name="elem"></param>
        ///// <returns></returns>
        //public static bool IsBeamExt(this Element elem)
        //{
        //    FamilyInstance familyInstanceExt = elem.ToFamilyInstanceExt();
        //    return familyInstanceExt != null && familyInstanceExt.IsStFramingExt() && familyInstanceExt.StructuralType == StructuralType.Beam;
        //}

        ///// <summary>支撑</summary>
        ///// <param name="elem"></param>
        ///// <returns></returns>
        //public static bool IsBraceExt(this Element elem)
        //{
        //    FamilyInstance familyInstanceExt = elem.ToFamilyInstanceExt();
        //    return familyInstanceExt != null && familyInstanceExt.IsStFramingExt() && familyInstanceExt.StructuralType == StructuralType.Brace;
        //}

        ///// <summary>门</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsDoorExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_Doors);

        ///// <summary>
        ///// 屋顶
        ///// zxc add
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsRoofExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_Roofs);

        ///// <summary>窗</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsWindowExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_Windows);

        ///// <summary>门窗</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsDoorWindowExt(this FamilyInstance instance) => instance.IsDoorExt() || instance.IsWindowExt();

        ///// <summary>结构基础</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsStFoundationExt(this Element instance) => instance.Category != null && instance.Category.IsEqualExt(BuiltInCategory.OST_StructuralFoundation);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetGenericModelsExt(this Document doc) => doc.GetFamilyInstancesExt(BuiltInCategory.OST_GenericModel);

        ///// <summary>所有门</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetDoorsExt(this Document doc) => doc.GetFamilyInstancesExt(BuiltInCategory.OST_Doors);

        ///// <summary>所有窗</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetWindowsExt(this Document doc) => doc.GetFamilyInstancesExt(BuiltInCategory.OST_Windows);

        ///// <summary>所有门窗</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetDoorWindowsExt(this Document doc)
        //{
        //    List<FamilyInstance> doorWindowsExt = new List<FamilyInstance>();
        //    doorWindowsExt.AddRange((IEnumerable<FamilyInstance>)doc.GetDoorsExt());
        //    doorWindowsExt.AddRange((IEnumerable<FamilyInstance>)doc.GetWindowsExt());
        //    return doorWindowsExt;
        //}

        ///// <summary>项目中的所有梁构件 ljy</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetBeamsExt(this Document doc) => doc.GetFamilyInstancesExt(StructuralType.Beam);

        ///// <summary>项目中的所有支撑构件 th</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetBracesExt(this Document doc) => doc.GetFamilyInstancesExt(StructuralType.Brace);

        ///// <summary>结构柱</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetStColumnsExt(this Document doc) => doc.GetFamilyInstancesExt(StructuralType.Column);

        ///// <summary>柱、排除斜柱</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetStColumnsExt2(this Document doc) => doc.GetFamilyInstancesExt(StructuralType.Column, true);

        ///// <summary>建筑柱</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetAcColumnsExt(this Document doc) => doc.GetFamilyInstancesExt(BuiltInCategory.OST_Columns);

        ///// <summary>获取实例</summary>
        ///// <param name="doc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetFamilyInstancesExt(this Document doc) => doc.FilterElementsExt<FamilyInstance>();

        ///// <summary>获取实例</summary>
        ///// <param name="doc"></param>
        ///// <param name="strFamilyName">族名称</param>
        ///// <param name="listIns"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetFamilyInstancesExt(
        //  this Document doc,
        //  string strFamilyName,
        //  List<FamilyInstance> listIns = null)
        //{
        //    if (listIns == null)
        //        listIns = doc.GetFamilyInstancesExt();
        //    return listIns.FindAll((Predicate<FamilyInstance>)(p => p.GetFamilyNameExt() == strFamilyName));
        //}

        ///// <summary>按分类取实例</summary>
        ///// <param name="doc"></param>
        ///// <param name="bic"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetFamilyInstancesExt(
        //  this Document doc,
        //  BuiltInCategory bic)
        //{
        //    return doc.FilterElementsExt<FamilyInstance>((ElementFilter)new ElementCategoryFilter(bic));
        //}

        ///// <summary>结构用途获取实例</summary>
        ///// <param name="doc"></param>
        ///// <param name="stType"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetFamilyInstancesExt(
        //  this Document doc,
        //  StructuralType stType)
        //{
        //    ElementStructuralTypeFilter filter = new ElementStructuralTypeFilter(stType);
        //    return doc.FilterElementsExt<FamilyInstance>((ElementFilter)filter);
        //}

        ///// <summary>结构用途获取实例：点定位、线定位</summary>
        ///// <param name="doc"></param>
        ///// <param name="stType"></param>
        ///// <param name="isPointLoc"></param>
        ///// <returns></returns>
        //public static List<FamilyInstance> GetFamilyInstancesExt(
        //  this Document doc,
        //  StructuralType stType,
        //  bool isPointLoc)
        //{
        //    LogicalAndFilter filter = new LogicalAndFilter((ElementFilter)new ElementIsCurveDrivenFilter(isPointLoc), (ElementFilter)new ElementStructuralTypeFilter(stType));
        //    return doc.FilterElementsExt<FamilyInstance>((ElementFilter)filter);
        //}

        ///// <summary>矩形梁的大小(长 ,宽 ,高)</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static XYZ GetRectangleBeamSize(this FamilyInstance beam)
        //{
        //    if (!beam.IsBeamExt())
        //        throw new InvalidOperationException();
        //    double length = beam.Location.GetCurveExt().Length;
        //    XYZ rectangleOrCircleSize = beam.GetRectangleOrCircleSize();
        //    double y = rectangleOrCircleSize.Y;
        //    double z = rectangleOrCircleSize.Z;
        //    return new XYZ(length, y, z);
        //}

        ///// <summary>获取圆形或矩形截面尺寸</summary>
        ///// <param name="fi"></param>
        ///// <returns></returns>
        //public static XYZ GetRectangleOrCircleSize(this FamilyInstance fi)
        //{
        //    double y = 0.0;
        //    double z = 0.0;
        //    XYZ xyz = -XYZ.BasisX;
        //    Line lineExt = fi.Location.GetLineExt();
        //    if ((GeometryObject)lineExt != (GeometryObject)null)
        //    {
        //        xyz = lineExt.Direction;
        //    }
        //    else
        //    {
        //        Arc arcExt = fi.Location.GetCurveExt().GetArcExt();
        //        if ((GeometryObject)arcExt != (GeometryObject)null)
        //            xyz = arcExt.TangentVectorExt(arcExt.StartPointExt());
        //    }
        //    XYZ normal = fi.IsColumnExt() ? XYZ.BasisZ : xyz;
        //    PlanarFace planarFace = fi.GetOriginalPlanarFaceExt(normal) ?? fi.GetOriginalPlanarFaceExt(-normal);
        //    if ((GeometryObject)null != (GeometryObject)planarFace)
        //    {
        //        foreach (EdgeArray edgeLoop in planarFace.EdgeLoops)
        //        {
        //            List<Edge> edgeList = new List<Edge>();
        //            foreach (Edge edge in edgeLoop)
        //                edgeList.Add(edge);
        //            if (edgeList.Count == 4)
        //            {
        //                if (edgeList[1].AsCurve().UnitVectorExt().IsParallelExt(XYZ.BasisZ))
        //                {
        //                    y = edgeList[0].AsCurve().Length;
        //                    z = edgeList[1].AsCurve().Length;
        //                }
        //                else
        //                {
        //                    y = edgeList[1].AsCurve().Length;
        //                    z = edgeList[0].AsCurve().Length;
        //                }
        //            }
        //            else
        //            {
        //                Arc arcExt = edgeList[0].AsCurve().GetArcExt();
        //                if ((GeometryObject)null != (GeometryObject)arcExt)
        //                {
        //                    y = arcExt.Radius;
        //                    z = -1.0;
        //                }
        //            }
        //        }
        //    }
        //    return new XYZ(0.0, y, z);
        //}

        ///// <summary>梁的几何中心线与侧向对正参数有关ljy</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static Curve GetBeamCenterCurve(this FamilyInstance beam)
        //{
        //    Curve curve = (Curve)null;
        //    Curve curveExt = beam.Location.GetCurveExt();
        //    double dValue = beam.GetRectangleBeamSize().Y / 2.0;
        //    switch (beam.GetHJustification())
        //    {
        //        case LateralJustification.Center:
        //            curve = curveExt;
        //            break;
        //        case LateralJustification.Side1:
        //            curve = curveExt.OffsetExt(dValue);
        //            break;
        //        case LateralJustification.Side2:
        //            curve = curveExt.OffsetExt(-dValue);
        //            break;
        //    }
        //    if ((GeometryObject)curve != (GeometryObject)null)
        //    {
        //        double zoffset = beam.GetZOffset();
        //        if (zoffset.IsNotZero())
        //            curve = curve.AddZ(zoffset);
        //        double yoffset = beam.GetYOffset();
        //        if (yoffset.IsNotZero())
        //            curve = curve.OffsetExt(yoffset);
        //    }
        //    return curve;
        //}

        //public static Curve GetBeamSideCurve(this FamilyInstance beam, SideTypes sideType) => beam.GetBeamSideCurve(0.0, sideType);

        //public static Curve GetBeamSideCurve(
        //  this FamilyInstance beam,
        //  double offsetExtend,
        //  SideTypes sideType)
        //{
        //    double num = beam.GetRectangleBeamSize().Y / 2.0;
        //    Curve curveExt = beam.Location.GetCurveExt();
        //    XYZ xyz1 = (curveExt.EndPointExt() - curveExt.StartPointExt()).Normalize().VectorRotateExt(Math.PI / 2.0).Normalize();
        //    XYZ xyz2 = xyz1.Negate();
        //    switch (sideType)
        //    {
        //        case SideTypes.Exterior:
        //        case SideTypes.Interior:
        //            if (num == 0.0)
        //                return curveExt;
        //            if (curveExt.IsLineExt())
        //            {
        //                Transform translation = Transform.CreateTranslation((sideType != SideTypes.Exterior ? xyz2 : xyz1) * (num + offsetExtend.ToApiExt()));
        //                return curveExt.CreateTransformed(translation);
        //            }
        //            Arc curve = curveExt.IsArcExt() ? curveExt.GetArcExt() : throw new InvalidOperationException();
        //            XYZ pt1 = curveExt.StartPointExt();
        //            XYZ pt2 = curveExt.EndPointExt();
        //            XYZ xyz3 = curve.MiddlePointExt();
        //            XYZ xyz4 = LineExtend.CreateBoundExt(pt1, pt2).MiddlePointExt();
        //            XYZ end0;
        //            XYZ end1;
        //            XYZ pointOnArc;
        //            if (sideType == SideTypes.Exterior)
        //            {
        //                end0 = pt1 + (pt1 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //                end1 = pt2 + (pt2 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //                pointOnArc = xyz3 + (xyz3 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //            }
        //            else
        //            {
        //                end0 = pt1 - (pt1 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //                end1 = pt2 - (pt2 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //                pointOnArc = xyz3 - (xyz3 - xyz4).Normalize() * (num + offsetExtend.ToApiExt());
        //            }
        //            return (Curve)Arc.Create(end0, end1, pointOnArc);
        //        default:
        //            return curveExt;
        //    }
        //}

        ///// <summary>柱底标高</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static Level GetBaseLevelExt(this FamilyInstance instance)
        //{
        //    Level baseLevelExt = (Level)null;
        //    ElementId parameterElementId = instance.GetParameterElementId(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);
        //    if (parameterElementId != ElementId.InvalidElementId)
        //        baseLevelExt = instance.Document.GetElement(parameterElementId) as Level;
        //    return baseLevelExt;
        //}

        ///// <summary>柱底标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="level"></param>
        //public static void SetBaseLevelExt(this FamilyInstance instance, Level level)
        //{
        //    if (level == null)
        //        return;
        //    instance.SetBaseLevelExt(level.Id);
        //}

        ///// <summary>柱底标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="levelId"></param>
        //public static void SetBaseLevelExt(this FamilyInstance instance, ElementId levelId) => instance.SetParameterExt(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM, levelId);

        ///// <summary>柱顶标高</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static Level GetTopLevelExt(this FamilyInstance instance)
        //{
        //    Level topLevelExt = (Level)null;
        //    ElementId parameterElementId = instance.GetParameterElementId(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
        //    if (parameterElementId != ElementId.InvalidElementId)
        //        topLevelExt = instance.Document.GetElement(parameterElementId) as Level;
        //    return topLevelExt;
        //}

        ///// <summary>柱顶标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="level"></param>
        //public static void SetTopLevelExt(this FamilyInstance instance, Level level)
        //{
        //    if (level == null)
        //        return;
        //    instance.SetTopLevelExt(level.Id);
        //}

        ///// <summary>柱顶标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="levelId"></param>
        //public static void SetTopLevelExt(this FamilyInstance instance, ElementId levelId) => instance.SetParameterExt(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM, levelId);

        ///// <summary>柱底标高</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetBaseOffsetExt(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM);

        ///// <summary>柱底标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetBaseOffsetExt(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM, dVlaue);

        ///// <summary>柱顶标高</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetTopOffsetExt(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM);

        ///// <summary>柱顶标高</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetTopOffsetExt(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM, dVlaue);

        ///// <summary>参照标高</summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //public static Level GetRefLevelExt(this Element element)
        //{
        //    Level refLevelExt = (Level)null;
        //    ElementId refLevelId = element.GetRefLevelId();
        //    if (refLevelId != ElementId.InvalidElementId)
        //        refLevelExt = element.Document.GetElement(refLevelId) as Level;
        //    return refLevelExt;
        //}

        ///// <summary>参照标高</summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //public static ElementId GetRefLevelId(this Element element) => element.GetParameterElementId(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);

        ///// <summary>参照标高</summary>
        ///// <param name="element"></param>
        ///// <param name="level"></param>
        //public static void SetRefLevelExt(this Element element, Level level)
        //{
        //    if (level == null)
        //        return;
        //    element.SetRefLevelExt(level.Id);
        //}

        ///// <summary>参照标高</summary>
        ///// <param name="element"></param>
        ///// <param name="levelId"></param>
        //public static void SetRefLevelExt(this Element element, ElementId levelId) => element.SetParameterExt(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, levelId);

        ///// <summary>起点标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetStartOffset(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION);

        ///// <summary>起点标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetStartOffset(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION, dVlaue);

        ///// <summary>终点标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetEndOffset(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION);

        ///// <summary>终点标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetEndOffset(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION, dVlaue);

        ///// <summary>侧向对正</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static LateralJustification GetHJustification(
        //  this FamilyInstance beam)
        //{
        //    return (LateralJustification)beam.GetParameterInteger(BuiltInParameter.BEAM_H_JUSTIFICATION);
        //}

        ///// <summary>侧向对正</summary>
        ///// <param name="beam"></param>
        ///// <param name="just"></param>
        //public static void SetHJustification(this FamilyInstance beam, LateralJustification just) => beam.SetHJustification((int)just);

        ///// <summary>侧向对正</summary>
        ///// <param name="beam"></param>
        ///// <param name="just"></param>
        //public static void SetHJustification(this FamilyInstance beam, int just) => beam.SetParameterExt(BuiltInParameter.BEAM_H_JUSTIFICATION, just);

        ///// <summary>Z方向对正</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static ZDirectionJustification GetVJustification(
        //  this FamilyInstance beam)
        //{
        //    return (ZDirectionJustification)beam.GetParameterInteger(BuiltInParameter.Z_JUSTIFICATION);
        //}

        ///// <summary>获取Y轴对正</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static YDirectionJustification GetYJustification(
        //  this FamilyInstance beam)
        //{
        //    return (YDirectionJustification)beam.GetParameterInteger(BuiltInParameter.Y_JUSTIFICATION);
        //}

        ///// <summary>获取Y轴偏移</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static double GetYOffset(this FamilyInstance beam) => beam.GetParameterDouble(BuiltInParameter.Y_OFFSET_VALUE);

        ///// <summary>Z方向对正</summary>
        ///// <param name="beam"></param>
        ///// <param name="just"></param>
        //public static void SetVJustification(this FamilyInstance beam, ZDirectionJustification just) => beam.SetVJustification((int)just);

        ///// <summary>Z方向对正</summary>
        ///// <param name="beam"></param>
        ///// <param name="just"></param>
        //public static void SetVJustification(this FamilyInstance beam, int just) => beam.SetParameterExt(BuiltInParameter.Z_JUSTIFICATION, just);

        ///// <summary>Z方向偏移</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetZOffset(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.BEAM_V_JUSTIFICATION_OTHER_VALUE);

        ///// <summary>Z方向偏移</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetZOffset(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.BEAM_V_JUSTIFICATION_OTHER_VALUE, dVlaue);

        ///// <summary>截面旋转角度</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetBendAngle(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE);

        ///// <summary>截面旋转角度</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetBendAngle(this FamilyInstance instance, double dVlaue)
        //{
        //    double num = dVlaue % (2.0 * Math.PI);
        //    instance.SetParameterExt(BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE, num);
        //}

        ///// <summary>常规构件的标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static double GetGenericLevelOffset(this FamilyInstance instance) => instance.GetParameterDouble(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM);

        ///// <summary>Z常规构件的标高偏移</summary>
        ///// <param name="instance"></param>
        ///// <param name="dVlaue"></param>
        //public static void SetGenericLevelOffset(this FamilyInstance instance, double dVlaue) => instance.SetParameterExt(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM, dVlaue);

        ///// <summary>
        ///// 门窗等的内置高度,注 14,15,16都不一样,但值都是（-1001300），请用此方法 ljy 2015-9-22
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        ///// 
        /////             2015-12-28 th 增加实例参数的获取(幕墙门是实例参数)
        //public static double GetBuiltInHeight(this FamilyInstance instance)
        //{
        //    double builtInHeight1 = instance.Symbol.GetBuiltInHeight1();
        //    return builtInHeight1.IsThanExt(0.0) ? builtInHeight1 : instance.GetBuiltInHeight1();
        //}

        ///// <summary>
        ///// 门窗等的内置高度,注 14,15,16都不一样,但值都是（-1001300），请用此方法 ljy 2015-9-22
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //private static double GetBuiltInHeight1(this Element instance)
        //{
        //    double parameterDouble1 = instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //    if (parameterDouble1.IsThanExt(0.0))
        //        return parameterDouble1;
        //    double parameterDouble2 = instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //    if (parameterDouble2.IsThanExt(0.0))
        //        return parameterDouble2;
        //    double parameterDouble3 = instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //    if (parameterDouble3.IsThanExt(0.0))
        //        return parameterDouble3;
        //    double parameterDouble4 = instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //    if (parameterDouble4.IsThanExt(0.0))
        //        return parameterDouble4;
        //    double parameterDouble5 = instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //    return parameterDouble5.IsThanExt(0.0) ? parameterDouble5 : instance.GetParameterDouble(BuiltInParameter.FAMILY_HEIGHT_PARAM);
        //}

        ///// <summary>
        ///// 门窗等的内置宽度,注 14,15,16都不一样,但值都是（-1001301），请用此方法 ljy 2015-9-22
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        ///// 
        /////             2015-12-28 th 增加实例参数的获取(幕墙门是实例参数)
        //public static double GetBuiltInWidth(this FamilyInstance instance)
        //{
        //    double builtInWidth1 = instance.Symbol.GetBuiltInWidth1();
        //    return builtInWidth1.IsThanExt(0.0) ? builtInWidth1 : instance.GetBuiltInWidth1();
        //}

        ///// <summary>
        ///// 门窗等的内置宽度,注 14,15,16都不一样,但值都是（-1001301），请用此方法 ljy 2015-9-22
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //private static double GetBuiltInWidth1(this Element instance)
        //{
        //    double parameterDouble1 = instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //    if (parameterDouble1.IsThanExt(0.0))
        //        return parameterDouble1;
        //    double parameterDouble2 = instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //    if (parameterDouble2.IsThanExt(0.0))
        //        return parameterDouble2;
        //    double parameterDouble3 = instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //    if (parameterDouble3.IsThanExt(0.0))
        //        return parameterDouble3;
        //    double parameterDouble4 = instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //    if (parameterDouble4.IsThanExt(0.0))
        //        return parameterDouble4;
        //    double parameterDouble5 = instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //    return parameterDouble5.IsThanExt(0.0) ? parameterDouble5 : instance.GetParameterDouble(BuiltInParameter.FAMILY_WIDTH_PARAM);
        //}

        ///// <summary>门、窗的中心定位线 ljy 2015-9-22</summary>
        ///// <param name="instance"></param>
        ///// <param name="vect"></param>
        ///// <returns></returns>
        //public static Line GetDoorWindowLocLine(this FamilyInstance instance, XYZ vect = null)
        //{
        //    double builtInWidth = instance.GetBuiltInWidth();
        //    double dRotate = 0.0;
        //    XYZ pt = instance.Location.GetPointExt(out dRotate);
        //    if (pt == null)
        //    {
        //        Transform totalTransform = instance.GetTotalTransform();
        //        if (totalTransform != null)
        //            pt = totalTransform.Origin;
        //    }
        //    if (vect == null)
        //        vect = XYZ.BasisX.VectorRotateExt(dRotate);
        //    return pt == null ? (Line)null : pt.PointNewLineExt(vect, builtInWidth, true);
        //}

        ///// <summary>有些族确实有子对象，族特殊</summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static List<Element> GetSubComponents(this FamilyInstance instance)
        //{
        //    List<Element> subComponents = new List<Element>();
        //    foreach (ElementId id in instance.GetSubComponentIds().ToList<ElementId>())
        //    {
        //        Element element = instance.Document.GetElement(id);
        //        subComponents.Add(element);
        //    }
        //    return subComponents;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <returns></returns>
        //public static bool IsOpeningExt(this Element instance)
        //{
        //    if (instance == null || instance.Category == null)
        //        return false;
        //    if (instance is Opening)
        //        return true;
        //    BuiltInCategory categoryExt = instance.GetCategoryExt();
        //    if (!categoryExt.Equals((object)BuiltInCategory.OST_ShaftOpening))
        //    {
        //        categoryExt = instance.GetCategoryExt();
        //        if (!categoryExt.Equals((object)BuiltInCategory.OST_FloorOpening))
        //        {
        //            categoryExt = instance.GetCategoryExt();
        //            return categoryExt.Equals((object)BuiltInCategory.OST_SWallRectOpening);
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fi"></param>
        ///// <param name="index"></param>
        //public static void DisallowJoinAtEndExt(this FamilyInstance fi, int index) => StructuralFramingUtils.DisallowJoinAtEnd(fi, index);

        ///// <summary>允许连接</summary>
        ///// <param name="fi"></param>
        ///// <param name="index"></param>
        //public static void AllowJoinAtEndExt(this FamilyInstance fi, int index) => StructuralFramingUtils.AllowJoinAtEnd(fi, index);

        ///// <summary>
        ///// 获取梁的高度
        ///// 根据顶部高程和底部高程计算
        ///// hch add 2017-11-24
        ///// </summary>
        ///// <param name="fi"></param>
        ///// <returns></returns>
        //public static double GetBeamHeight(this FamilyInstance fi) => fi.GetParameterDouble(BuiltInParameter.STRUCTURAL_ELEVATION_AT_TOP) - fi.GetParameterDouble(BuiltInParameter.STRUCTURAL_ELEVATION_AT_BOTTOM);

        ///// <summary>随轴网移动</summary>
        ///// <param name="beam"></param>
        ///// <param name="blnMove"></param>
        //public static void SetGridMove(this FamilyInstance beam, bool blnMove)
        //{
        //    int num = blnMove ? 1 : 0;
        //    beam.SetParameterExt(BuiltInParameter.INSTANCE_MOVES_WITH_GRID_PARAM, num);
        //}

        ///// <summary>随轴网移动</summary>
        ///// <param name="beam"></param>
        ///// <returns></returns>
        //public static bool GetGridMove(this FamilyInstance beam) => beam.GetParameterInteger(BuiltInParameter.INSTANCE_MOVES_WITH_GRID_PARAM) == 1;

        ///// <summary>竖向构件底标高</summary>
        ///// <param name="fi"></param>
        ///// <returns></returns>
        //public static double GetInstBottomElev(this FamilyInstance fi)
        //{
        //    double instBottomElev = 0.0;
        //    Level baseLevelExt = fi.GetBaseLevelExt();
        //    if (baseLevelExt != null)
        //        instBottomElev = baseLevelExt.Elevation + fi.GetBaseOffsetExt();
        //    return instBottomElev;
        //}

        ///// <summary>竖向构件高度</summary>
        ///// <param name="fi"></param>
        ///// <returns></returns>
        //public static double GetInstHeight(this FamilyInstance fi)
        //{
        //    double num1 = 0.0;
        //    double num2 = 0.0;
        //    Level baseLevelExt = fi.GetBaseLevelExt();
        //    if (baseLevelExt != null)
        //        num1 = baseLevelExt.Elevation + fi.GetBaseOffsetExt();
        //    Level topLevelExt = fi.GetTopLevelExt();
        //    if (topLevelExt != null)
        //        num2 = topLevelExt.Elevation + fi.GetTopOffsetExt();
        //    return num2 - num1;
        //}

        ///// <summary>竖向构件顶标高</summary>
        ///// <param name="fi"></param>
        ///// <returns></returns>
        //public static double GetInstTopElev(this FamilyInstance fi) => fi.GetInstBottomElev() + fi.GetInstHeight();

        ///// <summary>获取柱底部中心坐标、以柱为例Z坐标都是0</summary>
        ///// <param name="elem"></param>
        ///// <returns></returns>
        //public static XYZ GetInstBottomPoint(this FamilyInstance elem)
        //{
        //    XYZ instBottomPoint = XYZ.Zero;
        //    if (elem != null)
        //        instBottomPoint = elem.GetPointExt().NewZ(elem.GetInstBottomElev());
        //    return instBottomPoint;
        //}

        ///// <summary>获取柱底部中心坐标、以柱为例Z坐标都是0</summary>
        ///// <param name="elem"></param>
        ///// <returns></returns>
        //public static XYZ GetInstTopPoint(this FamilyInstance elem)
        //{
        //    XYZ instTopPoint = XYZ.Zero;
        //    if (elem != null)
        //        instTopPoint = elem.GetPointExt().NewZ(elem.GetInstTopElev());
        //    return instTopPoint;
        //}

        //public static List<Tuple<Transform, string>> GetConnectorInfos(
        //  this FamilyInstance familyInstance)
        //{
        //    Dictionary<int, Tuple<Transform, string>> source = new Dictionary<int, Tuple<Transform, string>>();
        //    Family familyExt = familyInstance.GetFamilyExt();
        //    if (familyExt == null)
        //        return (List<Tuple<Transform, string>>)null;
        //    List<ConnectorElement> list1 = familyInstance.Document.EditFamily(familyExt).FilterElementsExt(typeof(ConnectorElement)).OfType<ConnectorElement>().ToList<ConnectorElement>();
        //    Tuple<Transform, string> tuple = (Tuple<Transform, string>)null;
        //    foreach (FamilyParameter familyParameter in familyInstance.Document.EditFamily(familyExt).FamilyManager.GetParameters().ToList<FamilyParameter>())
        //    {
        //        foreach (Parameter parameter in familyParameter.AssociatedParameters.ToParameters())
        //        {
        //            int refElementId = parameter.Element.Id.IntegerValue;
        //            ConnectorElement connectorElement = list1.FirstOrDefault<ConnectorElement>((Func<ConnectorElement, bool>)(a => a.Id.IntegerValue == refElementId));
        //            if (connectorElement != null)
        //            {
        //                if (!source.ContainsKey(connectorElement.Id.IntegerValue) && (familyParameter.Definition.Name.Contains("直径") || familyParameter.Definition.Name.Contains("半径")))
        //                    source[refElementId] = new Tuple<Transform, string>(connectorElement.CoordinateSystem, familyParameter.Definition.Name);
        //                if (familyParameter.Definition.Name.Contains("角度"))
        //                    tuple = new Tuple<Transform, string>(connectorElement.CoordinateSystem, familyParameter.Definition.Name);
        //            }
        //        }
        //    }
        //    List<Tuple<Transform, string>> list2 = source.Select<KeyValuePair<int, Tuple<Transform, string>>, Tuple<Transform, string>>((Func<KeyValuePair<int, Tuple<Transform, string>>, Tuple<Transform, string>>)(a => a.Value)).ToList<Tuple<Transform, string>>();
        //    if (tuple != null)
        //        list2.Add(tuple);
        //    return list2;
        //}

        //public static TSZ.RevitBaseDll.FamilyHostType FamilyHostType(
        //  this FamilyInstance familyInstance)
        //{
        //    return familyInstance.GetFamilyExt().GetHostTypeExt();
        //}

        //public static List<Curve> GetColumnCurvesExt(this FamilyInstance column, bool blnNewZ = true)
        //{
        //    List<Curve> columnCurvesExt = new List<Curve>();
        //    XYZ basisZ = XYZ.BasisZ;
        //    Transform translation;
        //    Transform rotationAtPoint;
        //    if (!(column.Location is LocationPoint location1))
        //    {
        //        if (!(column.Location is LocationCurve location))
        //            return columnCurvesExt;
        //        Line lineExt = location.Curve.GetLineExt();
        //        translation = Transform.CreateTranslation(lineExt.StartPointExt());
        //        rotationAtPoint = Transform.CreateRotationAtPoint(basisZ, 0.0, lineExt.StartPointExt());
        //    }
        //    else
        //    {
        //        translation = Transform.CreateTranslation(location1.Point);
        //        rotationAtPoint = Transform.CreateRotationAtPoint(XYZ.BasisZ, location1.Rotation, location1.Point);
        //    }
        //    Transform tf = rotationAtPoint.Multiply(translation);
        //    foreach (Curve curve in column.Symbol.GetCurvesExt(true, basisZ.Negate()))
        //    {
        //        if (blnNewZ)
        //            columnCurvesExt.Add(tf.OfCurve(curve).NewZ());
        //        else
        //            columnCurvesExt.Add(tf.OfCurve(curve));
        //    }
        //    return columnCurvesExt;
        //}
    }

    [Transaction(TransactionMode.Manual)]
    public class ShowCategory : IExternalCommand
    {
        public Result Execute(ExternalCommandData cD, ref string ms, ElementSet set)
        {
            UIDocument uiDoc = cD.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //ColumnSelectionFilter filter = new ColumnSelectionFilter();

            IList<Reference> refers = new List<Reference>();

            Reference refer = uiDoc.Selection.PickObject(ObjectType.Element, "点选物体");
            Element element = doc.GetElement(refer) as Element;



            Transaction trans = new Transaction(doc, "物体属性");
            trans.Start();

            FamilyInstance familyInstance = element.ToFamilyInstanceExt();

            Category elementCategory = element.Category;
            BuiltInCategory enumCategory = (BuiltInCategory)elementCategory.Id.IntegerValue;

            MessageBox.Show("类别：" + elementCategory.Name.ToString()
                + "\r\n" + "类别ID：" + elementCategory.Id.ToString()
                + "\r\n" + "后台类别：" + enumCategory.ToString()
                + "\r\n" + "当前物件属于（Model模型/Annotation标注）：" + elementCategory.CategoryType.ToString());

            trans.Commit();

            return Result.Succeeded;
        }
    }
    public class ShowFiltered : IExternalCommand
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






}
