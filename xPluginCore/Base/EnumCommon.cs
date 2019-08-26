using System;
using System.Collections.Generic;
using System.Web;



namespace DNNGo.Modules.ThemePlugin
{

    /// <summary>
    /// 移动类型
    /// </summary>
    public enum EnumMoveType
    {
        /// <summary>
        /// Up
        /// </summary>
        [Text("Up")]
        Up,
        /// <summary>
        /// Down
        /// </summary>
        [Text("Down")]
        Down,
        /// <summary>
        /// Top
        /// </summary>
        [Text("Top")]
        Top,
        /// <summary>
        /// Bottom
        /// </summary>
        [Text("Bottom")]
        Bottom,
        /// <summary>
        /// Promote
        /// </summary>
        [Text("Promote")]
        Promote,
        /// <summary>
        /// Demote
        /// </summary>
        [Text("Demote")]
        Demote
    }

    /// <summary>
    /// 是否删除
    /// </summary>
    public enum EnumIsDelete
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 1,
        /// <summary>
        /// 删除
        /// </summary>
        [Text("Delete")]
        Delete = 0
    }



    /// <summary>
    /// 方案类型
    /// </summary>
    public enum EnumProjectType
    {
        /// <summary>
        /// 显示
        /// </summary>
        [Text("View")]
        View = 0,
        /// <summary>
        /// 提交
        /// </summary>
        [Text("Submit")]
        Submit = 1
    }

    /// <summary>
    /// 字段类型
    /// </summary>
    public enum EnumFieldType
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        [Text("SingleLine")]
        SingleLine = 2,
        /// <summary>
        /// 多行文本
        /// </summary>
        [Text("MultiLine")]
        MultiLine = 3,
        /// <summary>
        /// 整形
        /// </summary>
        [Text("Integer")]
        Integer = 0,
        /// <summary>
        /// 浮点型
        /// </summary>
        [Text("Float")]
        Float = 1,
        /// <summary>
        /// Html文本
        /// </summary>
        [Text("Html")]
        Html = 4,
        /// <summary>
        /// 单选项
        /// </summary>
        [Text("Single")]
        Single = 5,
        /// <summary>
        /// 多选项
        /// </summary>
        [Text("Multiple")]
        Multiple = 6,
        /// <summary>
        /// 日期时间
        /// </summary>
        [Text("DateTime")]
        DateTime = 7,
        /// <summary>
        /// 附件
        /// </summary>
        [Text("attachment")]
        Attachment = 8


    }


    /// <summary>
    /// 附件类型
    /// </summary>
    public enum EnumAttachment
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Text("Picture")]
        Picture = 0,
        /// <summary>
        /// 文件
        /// </summary>
        [Text("File")]
        File = 1,
        /// <summary>
        /// 加密文件地址
        /// </summary>
        [Text("EncryptFileUrl")]
        EncryptFileUrl = 2
    }


    /// <summary>
    /// 是否可以搜索
    /// </summary>
    public enum EnumIsSearch
    {
        /// <summary>
        /// 是
        /// </summary>
        [Text("Yes")]
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        [Text("No")]
        No = 0
    }

    /// <summary>
    /// 是否列表显示
    /// </summary>
    public enum EnumIsList
    {
        /// <summary>
        /// 是
        /// </summary>
        [Text("Yes")]
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        [Text("No")]
        No = 0
    }

    /// <summary>
    /// 多选控件类型
    /// </summary>
    public enum EnumListType
    {
        /// <summary>
        /// 复选框
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 0,
        /// <summary>
        /// 多选列表
        /// </summary>
        [Text("ListBox")]
        ListBox = 1,
         /// <summary>
        /// 单选按钮
        /// </summary>
        [Text("RadioButton")]
        RadioButton = 2,
        /// <summary>
        /// 下拉列表
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 3
    }


    /// <summary>
    /// 是否必填
    /// </summary>
    public enum EnumIsEmpty
    {
        /// <summary>
        /// 是
        /// </summary>
        [Text("Yes")]
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        [Text("No")]
        No = 0
    }

    /// <summary>
    /// 列表框类型(0复选框,1多选列表)
    /// </summary>
    public enum EnumMultiType
    {
        /// <summary>
        /// 复选框
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 0,
        /// <summary>
        /// 多选列表
        /// </summary>
        [Text("ListBox")]
        ListBox = 1
    }
    /// <summary>
    /// 单选框类型(0单选按钮,1下拉列表)
    /// </summary>
    public enum EnumSingleType
    {
        /// <summary>
        /// 单选按钮
        /// </summary>
        [Text("RadioButton")]
        RadioButton = 0,
        /// <summary>
        /// 下拉列表
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 1
    }



    /// <summary>
    /// 编辑器类型
    /// </summary>
    public enum EnumEditorType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Text("Default")]
        Default = 0,
        /// <summary>
        /// 基本
        /// </summary>
        [Text("Basic")]
        Basic = 1,
        /// <summary>
        /// 迷你
        /// </summary>
        [Text("Mini")]
        Mini = 2



    }


    /// <summary>
    /// 自定义信息状态
    /// </summary>
    public enum EnumCustomStatus
    {
        /// <summary>
        /// 锁定
        /// </summary>
        [Text("Lock")]
        Lock = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 1,
        /// <summary>
        /// 置顶
        /// </summary>
        [Text("Top")]
        Top = 2,
    }

    /// <summary>
    /// 提示类型
    /// </summary>
    public enum EnumTips
    {
        /// <summary>
        /// Success
        /// </summary>
        [Text("Success")]
        Success = 0,
        /// <summary>
        /// Warning
        /// </summary>
        [Text("Warning")]
        Warning = 1,
        /// <summary>
        /// Error
        /// </summary>
        [Text("Error")]
        Error = 2,
        /// <summary>
        /// Alert
        /// </summary>
        [Text("Alert")]
        Alert = 3
    }

    /// <summary>
    /// 优惠码状态
    /// </summary>
    public enum EnumStatus
    {
        /// <summary>
        /// 锁定
        /// </summary>
        [Text("Lock")]
        Lock = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 1,
        /// <summary>
        /// 绑定用户
        /// </summary>
        [Text("BindUser")]
        BindUser = 2
    }



    /// <summary>
    /// Ajax请求
    /// </summary>
    public enum EnumAjax
    {
        /// <summary>
        /// 分类的Json数据
        /// </summary>
        [Text("CategroyJson")]
        CategroyJson = 1,
        /// <summary>
        /// RSS源
        /// </summary>
        [Text("RssFeeds")]
        RssFeeds = 2,

        /// <summary>
        /// TAG自动请求源
        /// </summary>
        [Text("AutocompletionTag")]
        AutocompletionTag = 3,

        /// <summary>
        /// 缩略图
        /// </summary>
        [Text("Thumbnail")]
        Thumbnail = 4,

        /// <summary>
        /// 站点地图
        /// </summary>
        [Text("Sitemap")]
        Sitemap = 5


    }

 

    /// <summary>
    /// 月份枚举
    /// </summary>
    public enum EnumMonth
    {
        /// <summary>
        /// 1月
        /// </summary>
        [Text("January")]
        January = 1,
        /// <summary>
        /// 2月
        /// </summary>
        [Text("February")]
        February = 2,
        /// <summary>
        /// 3月
        /// </summary>
        [Text("March")]
        March = 3,
        /// <summary>
        /// 4月
        /// </summary>
        [Text("April")]
        April = 4,
        /// <summary>
        /// 5月
        /// </summary>
        [Text("May")]
        May = 5,
        /// <summary>
        /// 6月
        /// </summary>
        [Text("June")]
        June = 6,
        /// <summary>
        /// 7月
        /// </summary>
        [Text("July")]
        July = 7,
        /// <summary>
        /// 8月
        /// </summary>
        [Text("August")]
        Aguest = 8,
        /// <summary>
        /// 9月
        /// </summary>
        [Text("September")]
        September = 9,
        /// <summary>
        /// 10月
        /// </summary>
        [Text("October")]
        October = 10,
        /// <summary>
        /// 11月
        /// </summary>
        [Text("November")]
        November = 11,
        /// <summary>
        /// 12月
        /// </summary>
        [Text("December")]
        December = 12
    }

    /// <summary>
    /// 文章状态(草稿、正常、锁定、删除)
    /// </summary>
    public enum EnumArticleStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Text("Draft")]
        Draft = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Published")]
        Published = 1,
        /// <summary>
        /// 未发布
        /// </summary>
        [Text("Pending")]
        Pending = 2,
        /// <summary>
        /// 锁定
        /// </summary>
        [Text("Locked")]
        Lock = 3,

        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 4

    }


    /// <summary>
    /// 文章状态作者用
    /// </summary>
    public enum EnumArticleStatusByUser
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Text("Draft")]
        Draft = 0,
        /// <summary>
        /// 发布
        /// </summary>
        [Text("Published")]
        Published = 1,
 
    }




    /// <summary>
    /// 评论状态(未审核、正常、锁定/垃圾、删除/回收站)
    /// </summary>
    public enum EnumCommentStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Text("Pending")]
        Pending = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Text("Approved")]
        Approved = 1,

        /// <summary>
        /// 垃圾评论
        /// </summary>
        [Text("Spam")]
        Spam = 2,

        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 3
    }


    /// <summary>
    /// 文章归属状态
    /// </summary>
    public enum EnumArticleAttributionStatus
    {
        /// <summary>
        /// 私有
        /// </summary>
        [Text("Private")]
        Private = 0,
        /// <summary>
        /// 公共
        /// </summary>
        [Text("Public")]
        Public = 1


    }

    /// <summary>
    /// 文章置顶状态(正常、置顶、描红)
    /// </summary>
    public enum EnumArticleTopStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 0,
        ///// <summary>
        ///// 高亮
        ///// </summary>
        //[Text("Highlight")]
        //Highlight = 1,
        /// <summary>
        /// 置顶
        /// </summary>
        [Text("Top")]
        Top = 2
        ///// <summary>
        ///// 置顶+高亮
        ///// </summary>
        //[Text("Highlight & Top")]
        //HighlightTop = 3




    }



    /// <summary>
    /// 发送订阅通知(0:未发,1:已发)
    /// </summary>
    public enum EnumArticleSendSubscribe
    {
        /// <summary>
        /// 已发
        /// </summary>
        [Text("Issued")]
        Issued = 1,
        /// <summary>
        /// 未发
        /// </summary>
        [Text("Not")]
        Not = 0
    }

    /// <summary>
    /// YesNo
    /// </summary>
    public enum EnumYesNo
    {
        /// <summary>
        /// Yes
        /// </summary>
        [Text("Yes")]
        Yes = 1,
        /// <summary>
        /// No
        /// </summary>
        [Text("No")]
        No = 0

    }
    

    /// <summary>
    /// True False
    /// </summary>
    public enum EnumTrueFalse
    {
        /// <summary>
        /// True
        /// </summary>
        [Text("True")]
        True = 1,
        /// <summary>
        /// False
        /// </summary>
        [Text("False")]
        False = 0

    }

    /// <summary>
    /// 文章榜单
    /// </summary>
    public enum EnumTopList
    {
        /// <summary>
        /// 最新文章
        /// </summary>
        [Text("Latest")]
        Latest = 0,
        /// <summary>
        /// 评论最多
        /// </summary>
        [Text("Hottest Comments")]
        Hottest_Comments = 1,
        /// <summary>
        /// 浏览最多
        /// </summary>
        [Text("Hottest View")]
        Hottest_View = 2

    }
      



 



    /// <summary>
    /// 控件类型
    /// </summary>
    public enum EnumControlType
    {
        /// <summary>
        /// TextBox
        /// </summary>
        [Text("TextBox")]
        TextBox = 1,
        /// <summary>
        /// RichTextBox
        /// </summary>
        [Text("RichTextBox")]
        RichTextBox = 2,
        /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 3,
       
        /// <summary>
        /// ListBox
        /// </summary>
        [Text("ListBox")]
        ListBox = 4,
        /// <summary>
        /// RadioButtonList
        /// </summary>
        [Text("RadioButtonList")]
        RadioButtonList = 5,
        /// <summary>
        /// FileUpload
        /// </summary>
        [Text("FileUpload")]
        FileUpload = 6,
        /// <summary>
        /// Urls
        /// </summary>
        [Text("Urls")]
        Urls = 60,
        /// <summary>
        /// CheckBox
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 7,
        /// <summary>
        /// CheckBoxList
        /// </summary>
        [Text("CheckBoxList")]
        CheckBoxList = 8,
        /// <summary>
        /// DatePicker
        /// </summary>
        [Text("DatePicker")]
        DatePicker = 9,
        /// <summary>
        /// ColorPicker
        /// </summary>
        [Text("ColorPicker")]
        ColorPicker = 90,
        /// <summary>
        /// IconPicker
        /// </summary>
        [Text("IconPicker")]
        IconPicker = 91,
        /// <summary>
        /// Label
        /// </summary>
        [Text("Label")]
        Label = 10,
        /// <summary>
        /// Colorpicker
        /// </summary>
        [Text("Colorpicker")]
        Colorpicker = 11,
        /// <summary>
        /// RadioButtonList_Icon
        /// </summary>
        [Text("RadioButtonList_Icon")]
        RadioButtonList_Icon = 121,
        /// <summary>
        /// DropDownList Font
        /// </summary>
        [Text("DropDownList_Font")]
        DropDownList_Font = 31,

        /// <summary>
        /// 字体选择复合控件
        /// </summary>
        [Text("Fonts")]
        Fonts = 32


    }




    /// <summary>
    /// 列表控件方向
    /// </summary>
    public enum EnumControlDirection
    {

        /// <summary>
        /// 横向
        /// </summary>
        [Text("Horizontal")]
        Horizontal = 1,

        /// <summary>
        /// 垂直
        /// </summary>
        [Text("Vertical")]
        Vertical = 0
    }


    /// <summary>
    /// 数据列表类型(相册/相片)
    /// </summary>
    public enum EnumDataListType
    {
        /// <summary>
        /// 相册
        /// </summary>
        [Text("Album list")]
        Album = 0,
        /// <summary>
        /// 相片
        /// </summary>
        [Text("Photo list")]
        Photo = 1
    }

    /// <summary>
    /// 文章排序方式
    /// </summary>
    public enum EnumArticleSequence
    {

        /// <summary>
        /// 默认排序
        /// </summary>
        [Text("Default")]
        Default = 0,
        /// <summary>
        /// 最新的文章
        /// </summary>
        [Text("Latest")]
        Latest = 1,
        /// <summary>
        /// 评论最多的文章
        /// </summary>
        [Text("Hottest Comments")]
        Hottest_Comments = 2,
        /// <summary>
        /// 浏览次数最多的文章
        /// </summary>
        [Text("Hottest View")]
        Hottest_View = 3

    }

    /// <summary>
    /// 相册显示设置
    /// </summary>
    public enum EnumAlbumsDisplay
    {
        /// <summary>
        /// 公开
        /// </summary>
        [Text("Public")]
        Public = 0,
        /// <summary>
        /// 私有
        /// </summary>
        [Text("Private")]
        Private = 1,
        /// <summary>
        /// 尽自己可见
        /// </summary>
        [Text("Only you")]
        Only_you = 2,

    }




    /// <summary>
    /// 验证类型
    /// </summary>
    public enum EnumVerification
    {
        /// <summary>
        /// 表示可选项。若不输入，不要求必填，若有输入，则验证其是否符合要求。
        /// </summary>
        [Text("optional")]
        optional = 0,
        /// <summary>
        /// 验证整数
        /// </summary>
        [Text("integer")]
        integer = 1,
        /// <summary>
        /// 验证数字
        /// </summary>
        [Text("number")]
        number = 2,
        /// <summary>
        /// 验证日期，格式为 YYYY/MM/DD、YYYY/M/D、YYYY-MM-DD、YYYY-M-D
        /// </summary>
        [Text("dateFormat")]
        date = 3,
        /// <summary>
        /// 验证 Email 地址
        /// </summary>
        [Text("email")]
        email = 4,
        /// <summary>
        /// 验证电话号码
        /// </summary>
        [Text("phone")]
        phone = 5,
        /// <summary>
        /// 验证 ipv4 地址
        /// </summary>
        [Text("ipv4")]
        ipv4 = 6,
        /// <summary>
        /// 验证 url 地址，需以 http://、https:// 或 ftp:// 开头
        /// </summary>
        [Text("url")]
        url = 7,
        /// <summary>
        /// 只接受填数字和空格
        /// </summary>
        [Text("onlyNumberSp")]
        onlyNumberSp = 8,
        /// <summary>
        /// 只接受填英文字母（大小写）和单引号(')
        /// </summary>
        [Text("onlyLetterSp")]
        onlyLetterSp = 9,
        /// <summary>
        /// 只接受数字和英文字母
        /// </summary>
        [Text("onlyLetterNumber")]
        onlyLetterNumber = 10



    }


    /// <summary>
    /// 特性模式
    /// </summary>
    public enum EnumFeature
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Text("Default")]
        Default = 0,
        /// <summary>
        /// 开启
        /// </summary>
        [Text("Yes")]
        Yes = 1,
        /// <summary>
        /// 不开启
        /// </summary>
        [Text("No")]
        No = 2
    }
    /// <summary>
    /// 用这个标签告诉Google此链接可能会出现的更新频率
    /// "always", "hourly", "daily", "weekly", "monthly", "yearly"
    /// </summary>
    public enum EnumSitemapChangefreq
    {
        /// <summary>
        /// always
        /// </summary>
        [Text("always")]
        always = 1,
        /// <summary>
        /// hourly
        /// </summary>
        [Text("hourly")]
        hourly = 2,
        /// <summary>
        /// daily
        /// </summary>
        [Text("daily")]
        daily = 3,
        /// <summary>
        /// weekly
        /// </summary>
        [Text("weekly")]
        weekly = 4,
        /// <summary>
        /// monthly
        /// </summary>
        [Text("monthly")]
        monthly = 5,
        /// <summary>
        /// yearly
        /// </summary>
        [Text("yearly")]
        yearly = 6
    }

    /// <summary>
    /// 订阅状态
    /// </summary>
    public enum EnumSubscriptionStatus
    {
        /// <summary>
        /// 锁定
        /// </summary>
        [Text("Lock")]
        Lock = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Approved")]
        Approved = 1,
        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 2

    }

    /// <summary>
    /// 邮件订阅类型
    /// </summary>
    public enum EnumSubscriptionType
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Text("Email")]
        Email = 0,
        /// <summary>
        /// 用户
        /// </summary>
        [Text("User")]
        User = 1,
        /// <summary>
        /// 角色
        /// </summary>
        [Text("Role")]
        Role = 2
    }

    /// <summary>
    /// 邮件的认证方式
    /// </summary>
    public enum EnumEmailAuthentication
    {
        /// <summary>
        /// Anonymous
        /// </summary>
        [Text("Anonymous")]
        Anonymous = 0,
        /// <summary>
        /// Basic
        /// </summary>
        [Text("Basic")]
        Basic = 1,
        /// <summary>
        /// NTLM
        /// </summary>
        [Text("NTLM")]
        NTLM = 2

    }


    /// <summary>
    /// 文件类型
    /// </summary>
    public enum EnumFileMate
    {
        /// <summary>
        /// Image
        /// </summary>
        [Text("Image")]
        Image = 0,
        /// <summary>
        /// Video
        /// </summary>
        [Text("Video")]
        Video = 1,
        /// <summary>
        /// Audio
        /// </summary>
        [Text("Audio")]
        Audio = 2,
        /// <summary>
        /// Zip
        /// </summary>
        [Text("Zip")]
        Zip = 3,
        /// <summary>
        /// Doc
        /// </summary>
        [Text("Doc")]
        Doc = 4,
        /// <summary>
        /// Other
        /// </summary>
        [Text("Other")]
        Other = 9,

        

    }

    /// <summary>
    /// 多媒体文件状态(未审核、正常、删除/回收站)
    /// </summary>
    public enum EnumFileStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Text("Pending")]
        Pending = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Text("Approved")]
        Approved = 1,
        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 2
    }

    /// <summary>
    /// 文件关系
    /// </summary>
    public enum EnumRelationshipsFileType
    {
        /// <summary>
        /// 背景图片
        /// </summary>
        [Text("Background")]
        Background = 0,
        /// <summary>
        /// 面包屑图片
        /// </summary>
        [Text("Breadcrumb")]
        Breadcrumb = 1
    }
 


    /// <summary>
    /// 作用范围
    /// </summary>
    public enum EnumScope
    {
        /// <summary>
        /// 单个
        /// </summary>
        [Text("Single")]
        Single = 0,
        /// <summary>
        /// 全局
        /// </summary>
        [Text("Global")]
        Global = 1
    }


    /// <summary>
    /// 权限枚举
    /// </summary>
    public enum EnumTabType
    {

        [Text("Standard menu")]
        SliderNenu = 0,
        [Text("Mega menu")]
        MegaMenu = 1

    }

    /// <summary>
    /// 方位
    /// </summary>
    public enum EnumPosition
    {
        [Text("Top")]
        Top = 0,
        [Text("Left")]
        Left = 1
        ,
        [Text("Right")]
        Right = 2
        ,
        [Text("Bottom")]
        Bottom = 3
    }


    /// <summary>
    /// 链接控件枚举
    /// </summary>
    public enum EnumUrlControls
    {
        /// <summary>
        /// URL
        /// </summary>
        [Text("URL ( A Link To An External Resource )")]
        Url = 1,
        /// <summary>
        /// 页面
        /// </summary>
        [Text("Page ( A Page On Your Site )")]
        Page = 2,
        /// <summary>
        /// 文件
        /// </summary>
        [Text("Files ( From the media library )")]
        Files = 3

    }

    /// <summary>
    /// 菜单行的类型
    /// </summary>
    public enum EnumRowType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Text("Menu")]
        Menu = 0,
        /// <summary>
        /// HTML 信息
        /// </summary>
        [Text("HTML")]
        HTML = 1,
        /// <summary>
        /// 第三方模块
        /// </summary>
        [Text("Module")]
        Module = 2



    }


    /// <summary>
    /// 菜单定位
    /// </summary>
    public enum EnumMegaPosition
    {
        /// <summary>
        /// 左
        /// </summary>
        [Text("Left")]
        Left = 0,
        /// <summary>
        /// 中
        /// </summary>
        [Text("Center")]
        Center = 1,
        /// <summary>
        /// 又
        /// </summary>
        [Text("Right")]
        Right = 2
    }

    /// <summary>
    /// 系统字体
    /// </summary>
    public enum EnumFontSystem
    {
        /// <summary>
        /// 否
        /// </summary>
        [Text("No")]
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        [Text("Yes")]
        Yes = 1
    }

    /// <summary>
    /// 字体链接类型
    /// </summary>
    public enum EnumFontLink
    {
        /// <summary>
        /// 谷歌字体
        /// </summary>
        [Text("Google Font")]
        Google = 1,
        /// <summary>
        /// 上传字体
        /// </summary>
        [Text("Upload Font")]
        Upload = 0
    }



}