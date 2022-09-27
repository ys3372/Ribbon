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

//本案特殊using
using System.Windows.Media.Imaging;
using System.IO;


namespace Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            //程序目录
            string AddInPath = @"D:\VisualStudio\BIMDevelopment\Ribbon";

            //程序集dll目录
            string dllPath = AddInPath + @"\bin\debug\Ribbon.dll";

            //按钮图标目录
            string iconPath = AddInPath + @"\icon";

            //插件选项卡名
            string tabName = "中国联合";

            //面板底部文字提示
            string panelName = "新能源";

            application.CreateRibbonTab(tabName);

            //RibbonPanel面板 - SplitButton按钮组 - PushButton 按钮

            #region 大按钮图标
            //这里画了一整个叫做“中国联合（tabName）”的面板
            RibbonPanel panel = application.CreateRibbonPanel(tabName, panelName); 
            
            //示例一：按钮组
            SplitButtonData splitButtonData = new SplitButtonData("中国联合", "中国联合");

            //添加到面板
            SplitButton splitButton = panel.AddItem(splitButtonData) as SplitButton;

            //按钮数据，“Rbn.Com1”是执行按钮事件的相应类名
            string tips1 = "1111";
            PushButtonData pD1 = CreatePushButton("Eletrical", "国机集团", dllPath, "Ribbon.Show", iconPath, "gjjt.ico", tips1, "gjjt.ico");
            splitButton.AddPushButton(pD1);

            //按钮组添加第二个按钮
            string tips2 = "2222";
            PushButtonData pD2 = CreatePushButton("框选三维", "框选三维", dllPath, "Ribbon.Object3D", iconPath, "zglh.ico", tips2, "zglh.ico");
            splitButton.AddPushButton(pD2);

            string tips3 = "3333";
            PushButtonData pD33 = CreatePushButton("选中三维", "选中三维", dllPath, "Ribbon.Free3D", iconPath, "16.ico", tips3, "");
            splitButton.AddPushButton(pD33);

            //示例2： 下拉按钮，用于显示下拉命令项
            PulldownButtonData pdData = new PulldownButtonData("Pulldown", "中国联合");
            PulldownButton pdButton = panel.AddItem(pdData) as PulldownButton;
            Uri uriLargeImage = new Uri(Path.Combine(iconPath, "zglh.ico"), UriKind.Absolute);
            pdButton.LargeImage = new BitmapImage(uriLargeImage);
            //添加命令按钮
            PushButtonData pD3;
            pD3 = CreatePushButton("Door", "测试按钮", dllPath, "Ribbon.AEditor", iconPath, "zglh.ico", "", "");
            pdButton.AddPushButton(pD3);
            #endregion

            //示例3： 命令按钮

            #region 建筑结构板块
            RibbonPanel panelArch = application.CreateRibbonPanel(tabName, "建筑结构");

            PushButtonData pD4;
            pD4 = CreatePushButton("t4", "a/创建结构柱", dllPath, "Ribbon.CreateVerticalColumn", iconPath, "4.ico", "在指定位置创建结构柱", "");
            PushButtonData pD5;
            pD5 = CreatePushButton("t5", "b/创建房梁", dllPath, "Ribbon.CreateBeam", iconPath, "5.ico", "在指定位置创建房梁", "");
            PushButtonData pD6;
            pD6 = CreatePushButton("t6", "c/创建楼板", dllPath, "Ribbon.CreateFloor", iconPath, "6.ico", "在指定位置创建楼板", "");
            PushButtonData pD24;
            pD24 = CreatePushButton("t24", "c/创建斜柱", dllPath, "Ribbon.CreateInclinedColulmn", iconPath, "12.ico", "在指定位置创建斜柱", "");
            PushButtonData pD25;
            pD25 = CreatePushButton("t25", "c/创建斜板", dllPath, "Ribbon.CreateSlopeSlab", iconPath, "12.ico", "在指定位置创建斜柱", "");
            PushButtonData pD26;
            pD26 = CreatePushButton("t26", "c/创建墙体", dllPath, "Ribbon.CreateWall", iconPath, "12.ico", "在指定位置创建墙体", "");

            panelArch.AddStackedItems(pD4, pD5, pD6);
            panelArch.AddSeparator();
            panelArch.AddStackedItems(pD24, pD25, pD26);
            #endregion

            #region 视图交互板块
            RibbonPanel panelView = application.CreateRibbonPanel(tabName, "视图交互");

            PushButtonData pD7;
            pD7 = CreatePushButton("t7", "a/创建3D轴测图", dllPath, "Ribbon.Create3DIso", iconPath, "7.ico", "在平面视图中选取物件并生成轴测图", "");
            PushButtonData pD8;
            pD8 = CreatePushButton("t8", "b/创建平面图", dllPath, "Ribbon.CreatePlanView", iconPath, "8.ico", "基于当前平面视图标高创建新平面视图", "");
            PushButtonData pD9;
            pD9 = CreatePushButton("t9", "c/创建立面图", dllPath, "Ribbon.CreateSectionFromWall", iconPath, "9.ico", "选取墙体并创建墙体立面", "");
            PushButtonData pD10;
            pD10 = CreatePushButton("t10", "d/远距淡显", dllPath, "Ribbon.FadeFarObject", iconPath, "10.ico", "距离较远物体弱化显示", "");
            PushButtonData pD11;
            pD11 = CreatePushButton("t11", "e/墙体突出", dllPath, "Ribbon.FilterColorWalls", iconPath, "11.ico", "突出显示超过5m的墙体", "");
            PushButtonData pD12;
            pD12 = CreatePushButton("t12", "f/隔离类型", dllPath, "Ribbon.ObjectIsolate", iconPath, "12.ico", "选择一个图元，隔离相同属性的所有图元", "");
            PushButtonData pD13;
            pD13 = CreatePushButton("t13", "g/隔离门窗", dllPath, "Ribbon.DoorWindowFilter", iconPath, "1.ico", "筛出墙体与窗户两类元素", "");
            PushButtonData pD14;
            pD14 = CreatePushButton("t14", "h/隔离墙体", dllPath, "Ribbon.WallFilter", iconPath, "2.ico", "筛出楼层为一楼的墙体", "");
            PushButtonData pD23;
            pD23 = CreatePushButton("t23", "h/房间体块", dllPath, "Ribbon.RoomVolumeModel", iconPath, "11.ico", "将房间体量可视化（面积x标高）", "");

            panelView.AddStackedItems(pD7, pD8, pD9);
            panelView.AddSeparator();
            panelView.AddStackedItems(pD10, pD11, pD12);
            panelView.AddSeparator();
            panelView.AddStackedItems(pD13, pD14, pD23);
            #endregion

            #region 外接测试板块
            RibbonPanel panelTest = application.CreateRibbonPanel(tabName, "外接测试");

            PushButtonData pDt1;
            pDt1 = CreatePushButton("t1", "a/柱信息", dllPath, "Ribbon.CW_EverythingColumn", iconPath, "15.ico", "", "");
            PushButtonData pDt2;
            pDt2 = CreatePushButton("t2", "b/柱修改", dllPath, "Ribbon.CW_ColumnRevise", iconPath, "16.ico", "", "");
            PushButtonData pDt3;
            pDt3 = CreatePushButton("t3", "c/物件类别", dllPath, "Ribbon.ShowCategory", iconPath, "17.ico", "", "");
            PushButtonData pDt4;
            pDt4 = CreatePushButton("t4", "d/类别枚举", dllPath, "Ribbon.ListIds", iconPath, "3.ico", "", "");
            PushButtonData pDt5;
            pDt5 = CreatePushButton("t5", "e/设置水平", dllPath, "Ribbon.SetLevel", iconPath, "5.ico", "", "");
            PushButtonData pDt6;
            pDt6 = CreatePushButton("t6", "f/导出csv", dllPath, "Ribbon.ExportCSV", iconPath, "7.ico", "", "");
            PushButtonData pDt7;
            pDt7 = CreatePushButton("t7", "g/重命名&打印", dllPath, "Ribbon.ActiveViewManager", iconPath, "7.ico", "", "");
            PushButtonData pDt8;
            pDt8 = CreatePushButton("t8", "h/柱子修改", dllPath, "Ribbon.AEditor", iconPath, "8.ico", "", "");
            PushButtonData pDt9;
            pDt9 = CreatePushButton("t9", "i/测试", dllPath, "Ribbon.ExportCSV", iconPath, "7.ico", "", "");

            panelTest.AddStackedItems(pDt1, pDt2, pDt3);
            panelTest.AddSeparator();
            panelTest.AddStackedItems(pDt4, pDt5, pDt6);
            panelTest.AddSeparator();
            panelTest.AddStackedItems(pDt7, pDt8, pDt9);
            #endregion

            #region 标注计算板块
            RibbonPanel panelMark = application.CreateRibbonPanel(tabName, "标注计算");

            PushButtonData pD15;
            pD15 = CreatePushButton("t15", "a/轴网间距标注", dllPath, "Ribbon.GridDimmension", iconPath, "3.ico", "选择轴网并标注间距", "");
            PushButtonData pD16;
            pD16 = CreatePushButton("t16", "b/柱网参数标注", dllPath, "Ribbon.MarkSample", iconPath, "4.ico", "标注所选柱体的属性", "");
            PushButtonData pD17;
            pD17 = CreatePushButton("t17", "c/文字样式标注", dllPath, "Ribbon.TextSample", iconPath, "5.ico", "在平面视图中展示几种类型的文字样式", "");
            PushButtonData pD18;
            pD18 = CreatePushButton("t18", "d/面积计算器", dllPath, "Ribbon.FaceArea", iconPath, "6.ico", "选中墙体/楼板等，计算面积", "");
            PushButtonData pD19;
            pD19 = CreatePushButton("t19", "e/楼板面积计算器", dllPath, "Ribbon.FloorAreaCalculation", iconPath, "7.ico", "计算选中的楼板总面积", "");
            PushButtonData pD20;
            pD20 = CreatePushButton("t20", "f/体量计算器", dllPath, "Ribbon.GetSolids", iconPath, "8.ico", "选择需要计算的体块，计算总体量", "");
            PushButtonData pD21;
            pD21 = CreatePushButton("t21", "g/框选坐标", dllPath, "Ribbon.PickBox", iconPath, "9.ico", "框选区域，获得起始点和终点坐标", "");
            PushButtonData pD22;
            pD22 = CreatePushButton("t22", "h/文本框计数", dllPath, "Ribbon.TextDetection", iconPath, "10.ico", "计算文件内文本框数量", "");

            panelMark.AddStackedItems(pD15, pD16, pD17);
            panelMark.AddSeparator();
            panelMark.AddStackedItems(pD18, pD19, pD20);
            panelMark.AddSeparator();
            panelMark.AddStackedItems(pD21, pD22);
            #endregion

            #region MEP板块
            RibbonPanel panelMEP = application.CreateRibbonPanel(tabName, "MEP");

            PushButtonData pD27;
            pD27 = CreatePushButton("t27", "a/创建软风管", dllPath, "Ribbon.CreateFlexDuct", iconPath, "1.ico", "创建一个新的软风管", "");
            PushButtonData pD28;
            pD28 = CreatePushButton("t28", "b/风管打断", dllPath, "Ribbon.BreakMEPCurve", iconPath, "2.ico", "在鼠标指定位置打断管线", "");
            PushButtonData pD29;
            pD29 = CreatePushButton("t29", "c/管线连接", dllPath, "Ribbon.CreateElbow", iconPath, "3.ico", "自动连接不相交的管线", "");
            PushButtonData pD30;
            pD30 = CreatePushButton("t30", "d/放置消火栓", dllPath, "Ribbon.PlaceFireHydrant", iconPath, "4.ico", "在鼠标指定位置放置消火栓", "");
            PushButtonData pD31;
            pD31 = CreatePushButton("t31", "e/放置S型号钢筋", dllPath, "Ribbon.SShapeRebarSample", iconPath, "5.ico", "在鼠标指定位置放置S型钢筋", "");
            PushButtonData pD32;
            pD32 = CreatePushButton("t32", "f/谢谢", dllPath, "Ribbon.Show", iconPath, "6.ico", "感谢测试", "");

            panelMEP.AddStackedItems(pD27, pD28, pD29);
            panelMEP.AddSeparator();
            panelMEP.AddStackedItems(pD30, pD31, pD32);
            #endregion

            //下拉组合框
            //ComboBoxData cbData = new ComboBoxData("下拉组合框");
            //ComboBoxMemberData cbMemData;
            //cbMemData = new ComboBoxMemberData("combobox1", "加载的族");
            //Autodesk.Revit.UI.ComboBox cBox = panelMEP.AddItem(cbData) as Autodesk.Revit.UI.ComboBox;
            //cBox.AddItem(cbMemData);

            return Result.Succeeded;
        }
        public PushButtonData CreatePushButton(string name, string txt, string dll, string com, string iconPath, string iconName, string tips, string iconTips)
        {
            //新建按钮绑定命令
            PushButtonData pbData = new PushButtonData(name, txt, dll, com);
            //小图标
            Uri uri1 = new Uri(Path.Combine(iconPath, iconName), UriKind.Absolute);
            pbData.Image = new BitmapImage(uri1);

            //大图标
            Uri uri2 = new Uri(Path.Combine(iconPath, iconName), UriKind.Absolute);
            pbData.LargeImage = new BitmapImage(uri2);

            //提示文字
            pbData.ToolTip = tips;

            if (iconTips != "")
            {
                Uri uri3 = new Uri(Path.Combine(iconPath, iconTips), UriKind.Absolute);
                pbData.ToolTipImage = new BitmapImage(uri3);

            }
            return pbData;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }



     
}
