using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SATPA
{
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    };

    enum ESAVE_PLATE_OPTION { SAVE_NOTHING, SAVE_PLATE_ONLY, SAVE_PLATE_AND_CAR };

    public struct SLPRParams
    {
        public short resize_thresh;     // if width of input image is larger than this, it will be resized
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] num_valid_chars;   // Number of valid characters usuallay {8, 0}. if e.g. 5 character plates are also available, use {8, 5}
        //public byte num_valid_chars1;   //if Two types of plates are important
        //public byte num_valid_chars2;   //if Two types of plates are important
        public byte medianKernel;       // (0: no kernel) (3, 5, 7 ... median kernel of this size)
        public byte save_plate_option;  //save_plate_option: 0 don't save anything, 1: save plate only, 2: save whole car image and plate							
                                        //اگر عدد صفر انتخاب شود، فقط رشته پلاک و مستطیل آن گزارش شده و تصویر بریده شده پلاک ارسال نمی شود
                                        //عدد 2 سبب استفاده بیشتر از حافظه و کاهش حدود 5درصدی سرعت پلاک خوانی می شود
        public short vlc_net_cache_time;
        //Limits of character dimensions
        public byte min_char_w;  //minimum with of characters
        public byte min_char_h;  //minimum height of characters
        public byte max_char_w;  //maximum with of characters
        public byte max_char_h;  //maximum height of characters

        public float skew_coef;         //more value means more skew: successive characters are not in the same Y position

        public byte ignore_inverted_plates;//may not be used
        public byte detect_motor; //if 1 motor detection is enabled, if 0 No.

        //ب) پارامترهای ویدیو
        public byte n_frm_skip_on_success;  //Number of frames to be skipped after successful plate detection
        public byte diff_thresh;            //Difference threshold between current frame and background to suppose entrance of new car 
        public byte plate_buf_size;         // Buffer length of recent successive plates (max = 50). 
        public int skip_same_plate_frm;// don't report same plate until "some frame" elpased
        public byte detect_multi_plate;

        public byte play_audio_from_camera; //in vlc mode we can play audio (from version 7.45)

        public byte plate_type; //0: Only Iran standard, 1: + Arvand, 2: + Arg 
        public byte report_non_standard_plates; //

        //internal byte horizontal_thresh;
        // از این دو پارامتر برای تعیین ناحیه پردازش استفاده میشود
        public byte min_thresh_hist;
        public byte max_thresh_hist;

        //تنظیمات پیشرفته که به ندرت نیاز است تغییر داده شود
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] blur_kernel;//{ 13, 13 }; //Size of blur kernel used for binarization. Default is 13x13. To handle shadow, try 13x1 or 13x3	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] img_bin_th;//[2] = { 0.9f, 0.95f }; //Adaptive Binarization Threshold (between 0.5 and 1) default is [0.95, 0.9]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] plt_bin_th;//[4] = { 0.8f, 0.85f, 0.92f, 1.0f }; //Adaptive Binarization Threshold (between 0.5 and 1) default is [0.95, 0.9]
        public byte char_diffrence;
        public byte engine;
        public byte economy;
        public byte reserve1;
        public byte reserve2;
        public int  reserve3;

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode/*, Pack = 1*/)]

    public struct camera_info
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string uri;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string ip;
        public int port;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode/*, Pack = 1*/)]
    public struct Lic_Info
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string owner_name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string prod_name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string proud_version;
        public int is_trial;
        public int for_developer;
        public int start_date;
        public int end_date; 
        public int camera_count;
        public int multi_plate_supported;
        public int read_limit_plate;
        public int country_number;    

    }
    public class SLPRPropertyGrid
    {
        [Category("ا) پارامترهای پرکاربرد"), DisplayName("کاهش عرض تصویر"), DefaultValue((short)2500), Description("عرض تصویر برای پردازش سریعتر به این اندازه کوچک خواهد شد.")]
        public short resize_thresh { get; set; } = 2500;     // if width of input image is larger than this, it will be resized

        [Category("ا) پارامترهای پرکاربرد"), DisplayName("نرخ فریم بر ثانیه"), DefaultValue((byte)15), Description("تعداد فریم بر ثانیه دریافتی از دوربین. برای کسب نتیجه ایده آل، نرخ فریم خود دوربین را هم روی همین مقدار تنظیم کنید")]
        public byte frame_rate { get; set; } = 20;

        [Category("ا) پارامترهای پرکاربرد"), DisplayName("تعداد نویسه های پلاک"), Description("اگر فقط پلاک استاندارد مد نظر است، 8 و 0 بگذارید. اگر پلاک مناطق آزاد را هم می خواهید، 8 و 5 بگذارید.")]
        public byte[] num_valid_chars { get; set; } = new byte[2] { 8, 0 };  // Number of valid characters usuallay {8, 0}. if e.g. 5 character plates are also available, use {8, 5}

        [Category("ا) پارامترهای پرکاربرد"), DisplayName("ذخیره سازی تصاویر"), DefaultValue((byte)2), Description("صفر: فقط رشته پلاک و مستطیل آن گزارش می شود. یک: تصویر بریده پلاک هم گزارش می شود. دو: تصویر خودرو هم گزارش می شود.")]
        public byte save_plate_option { get; set; } = (byte)ESAVE_PLATE_OPTION.SAVE_PLATE_AND_CAR; //save_plate_option: 0 don't save anything, 1: save plate only, 2: save whole car image and plate							
                                                                                                   //عدد 2 سبب استفاده بیشتر از حافظه و کاهش حدود 5درصدی سرعت پلاک خوانی می شود

        [Category("ا) پارامترهای پرکاربرد"), DisplayName("موتور"), DefaultValue(false), Description("اگر پلاک موتور برایتان مهم است، تیک بزنید")]
        public bool detect_motor { get; set; } = true;//if 1 motor detection is enabled, if 0 No.

        [Category("ب) پارامترهای ویدیو"), DisplayName("گزارش پلاکهای ناقص"), DefaultValue(false), Description("اگر پلاکی کمتر از تعداد استاندارد، مثلا 8، رقم داشت یا تعداد حروف آن بیش از یکی باشد، ناقص تلقی می شود. با تیک زدن این گزینه، اینها هم گزارش خواهند شد.")]
        public bool report_non_standard_plates { get; set; } = false;

        [Category("ب) پارامترهای ویدیو"), DisplayName("VLC Cache Time"), DefaultValue((short)1000), Description("مدت زمان بافر کردن جریان شبکه برحسب میلی ثانیه، توسط کتابخانه وی ال سی. اگر ارتباط با دوربین برقرار نمی شود، این عدد را افزایش دهید")]
        public short vlc_net_cache_time { get; set; } = 1000;

        [Category("ب) پارامترهای ویدیو"), DisplayName("دریافت تک تصویر"), DefaultValue(false), Description("اگر در حالت عادی نمی توانید به دوربین وصل شوید، این گزینه را تیک بزنید و دوباره تلاش کنید")]
        public bool take_shots_from_camera { get; set; } = false;

        [Category("ج) پارامترهای عمومی"), DisplayName("حداقل عرض نویسه"), DefaultValue((byte)5), Description("اگر عرض نویسه ای (عدد یا حرف) کمتر از این عدد باشد، نادیده گرفته می شود")]
        public byte min_char_w { get; set; } = 5; //minimum with of characters
        [Category("ج) پارامترهای عمومی"), DisplayName("حداقل ارتفاع نویسه"), DefaultValue((byte)7), Description("اگر ارتفاع نویسه ای (عدد یا حرف) کمتر از این عدد باشد، نادیده گرفته می شود")]
        public byte min_char_h { get; set; } = 7; //minimum height of characters
        [Category("ج) پارامترهای عمومی"), DisplayName("حداکثر عرض نویسه"), DefaultValue((byte)100), Description("اگر عرض نویسه ای (عدد یا حرف) بیشتر از این عدد باشد، نادیده گرفته می شود")]
        public byte max_char_w { get; set; } = 100; //maximum with of characters
        [Category("ج) پارامترهای عمومی"), DisplayName("حداکثر ارتفاع نویسه"), DefaultValue((byte)100), Description("اگر ارتفاع نویسه ای (عدد یا حرف) بیشتر از این عدد باشد، نادیده گرفته می شود")]
        public byte max_char_h { get; set; } = 100; //maximum height of characters

        [Category("ج) پارامترهای عمومی"), DisplayName("میزان کجی پلاک"), DefaultValue(1.0f), Description("عددی بین 0 و 2: هر چه زاویه پلاک نسبت به افق بیشتر است، این عدد را بزرگتر بگیرید.")]
        public float skew_coef { get; set; } = 1.0f; //more value means more skew: successive characters are not in the same Y position

        [Category("ج) پارامترهای عمومی"), DisplayName("صرفنظر از دولتی"), DefaultValue(false), Description("اگر پلاک دولتی و پلیس برایتان مهم نیست، این گزینه را تیک بزنید")]
        public bool ignore_inverted_plates { get; set; } = false;//may not be used

        [Category("ج) پارامترهای عمومی"), DisplayName("حداقل دقت پلاک"), DefaultValue(false), Description("پلاک هایی با دقت کمتر نادیده گرفته خواهند شد")]
        public float min_cnf { get; set; } = 0.8f;
        [Category("ج) پارامترهای عمومی"), DisplayName("موتور پلاک خوانی"), DefaultValue((byte)0), Description("موتور شماره یک عملکرد بهتری در پلاک های مخدوش دارد اما مصرف پردازنده را افزایش خواهد داد")]
        public byte engine { get; set; } = 0; //maximum height of characters
        //ب) پارامترهای ویدیو

        [Category("ب) پارامترهای ویدیو"), DisplayName("وقفه پس از موفقیت"), DefaultValue((byte)0), Description("بعد از ثبت موفق یک پلاک، این تعداد فریم را پردازش نکن")]
        public byte n_frm_skip_on_success { get; set; } = 0; //Number of frames to be skipped after successful plate detection

        [Category("ب) پارامترهای ویدیو"), DisplayName("عدم گزارش پلاک تکراری"), DefaultValue((byte)1), Description("برای جلوگیری از گزارش پلاک تکراری مقداری بر حسب تعداد فریم تنظیم نمایید")]
        public int skip_same_plate_frm { get; set; } = 50;

        [Category("ب) پارامترهای ویدیو"), DisplayName("اختلاف نویسه های پلاک تکراری"), DefaultValue((byte)1), Description(".یک یا دو.آستانه شناسایی پلاک جدید.با مشاهده تغییرات( شامل کاهش افزایش ، تغییر یا جابه جایی) کمتر از این آستانه در نویسه های پلاک، آن را تکراری قلمداد کن")]
        public byte char_diff { get; set; } = 1;

        [Category("ب) پارامترهای ویدیو"), DisplayName("ورود خودرو"), DefaultValue((byte)20)]
        [Description("آستانه‏ی تشخیص ورود خودرو\nبرای تصاویر شب، مقدار 7 و برای روز مقدار 20 مناسب است\nمقدار بزرگتر حساسیت کمتری دارد و خودروی کمتری تشخیص می دهد")]
        public byte diff_thresh { get; set; } = 20;         //Difference threshold between current frame and background to suppose entrance of new car 

        [Category("ب) پارامترهای ویدیو"), DisplayName("بافر پلاکها"), DefaultValue((byte)7), Description("برای پیشگیری از گزارش پلاکهای تکراری، این تعداد پلاک مشابه، بافر شده و سپس یکی گزارش می شود.")]
        public byte plate_buf_size { get; set; } = 7;      // Buffer length of recent successive plates (max = 50). 

        [Category("ا) پارامترهای پرکاربرد"), DisplayName("تشخیص چند پلاک"), DefaultValue(true), Description("اگر مجوز چند پلاکه خریده اید، با تیک زدن این پارامتر، می توانید چند پلاک را در یک تصویر بخوانید")]
        public bool detect_multi_plate { get; set; } = true;


        [Category("ب) پارامترهای ویدیو"), DisplayName("پخش صدا"), DefaultValue(false), Description("در حالت کار با وی ال سی، می توانید صدا را هم داشته باشید. برای برخی دوربینها حتما باید این تیک را بزنید که استریم ویدیو را دریافت کنید")]
        public bool play_audio_from_camera { get; set; } = false; //in vlc mode we can play audio (from version 7.45)

        [Category("ج) پارامترهای عمومی"), DisplayName("نوع پلاک"), DefaultValue((byte)0), Description("صفر: فقط پلاک استاندارد، یک: پلاک استاندارد + پلاک اروند، دو: پلاک استاندارد + پلاک ارگ")]
        public byte plate_type { get; set; } = 0;// 

        [Category("ب) پارامترهای ویدیو"), DisplayName("آستانه حداقل هیستوگرام"), DefaultValue((byte)50), Description("عددی بین 0 و 100 به منظور تشخیص محل خودرو و پردازش همان منطقه به جای کل تصویر")]
        public byte min_thresh_hist { get; set; } = 50;

        [Category("ب) پارامترهای ویدیو"), DisplayName("آستانه حداکثر هیستوگرام"), DefaultValue((byte)170), Description("عددی بین 100 و 200 به منظور تشخیص محل خودرو و پردازش همان منطقه به جای کل تصویر")]
        public byte max_thresh_hist { get; set; } = 170;

        [Category("ب) پارامترهای ویدیو"), DisplayName(" صرفه جویی در پردازنده "), DefaultValue((byte)0), Description("با فعال سازی این گزینه در صورت امکان در مصرف پردازنده صرفه جویی خواهد شد.")]
        public byte economy { get; set; } = 0;

        [Category("ب) پارامترهای ویدیو"), DisplayName(" رهگیری پلاک ها "), DefaultValue((byte)0), Description("با فعال سازیاین گزینه،در تصویر پلاک توسط یک نشانه دنبال می شود.")]
        public bool marker { get; set; } = true;

        [Category("ه) پارامترهای عیب یابی"), DisplayName("تکرار ویدیو"), DefaultValue(false), Description("برای اینکه سیستم را با یک فایل ویدیویی زیر بار بگذارید، این تیک را بزنید. با اتمام فایل، از نو شروع می شود.")]
        public bool repeat { get; set; } = true;

        [Category("ه) پارامترهای عیب یابی"), DisplayName("حالت دیباگ"), DefaultValue((byte)0), Description("مورد استفاده توسعه دهندگان کتابخانه است. تغییر ندهید.")]
        public byte debug_level { get; set; } = 0;

        [Category("د) پارامترهای پیشرفته"), DisplayName("کرنل میانه"), DefaultValue((byte)0), Description("فیلتر میانه با این ابعاد. برای عدم اعمال فیلتر، صفر وارد کنید")]
        public byte medianKernel { get; set; } = 0;      // (0: no kernel) (3, 5, 7 ... median kernel of this size)                                                                                                   //اگر عدد صفر انتخاب شود، فقط رشته پلاک و مستطیل آن گزارش شده و تصویر بریده شده پلاک ارسال نمی شود

        [Category("د) پارامترهای پیشرفته"), DisplayName("فیلتر نرم کننده"), DefaultValue((byte)0), Description("برای باینری کردن وفقی استفاده می شود. اگر تصویرتان سایه دارد 13 و 3 را امتحان کنید.")]
        public byte[] blur_kernel { get; set; } = new byte[2] { 13, 13 };

        [Category("د) پارامترهای پیشرفته"), DisplayName("آستانه ی باینری سراسری"), DefaultValue((byte)0), Description("برای باینری سازی وفقی کل تصویر استفاده می شود")]
        public float[] img_bin_th { get; set; } = new float[2] { 0.9f, 0.95f };

        [Category("د) پارامترهای پیشرفته"), DisplayName("آستانه ی باینری محلی"), DefaultValue((byte)0), Description("برای باینری سازی وفقی تصویر پلاک استفاده می شود")]
        public float[] plt_bin_th { get; set; } = new float[4] { 0.8f, 0.85f, 0.92f, 1.0f };

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode/*, Pack = 1*/)]
    public struct SPlateResult
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string str;
        public float cnf;
        public RECT rc;
        public IntPtr img_plate;
        public IntPtr img_car;
        public byte direction;//DIR_UNKNOWN = 0, DIR_COMMING = 1, DIR_DEPARTING = 2
        public byte n_char;//تعداد کل نویسه ها (ارقام و حروف)
        public byte n_letter;//تعداد حروف یافت شده در پلاک
        public byte count;//چند بار یک پلاک در فریمهای مختلف تکرار شده است
        public byte roi;//در کدام ناحیه مورد علاقه، این پلاک یافت شده است
        public byte vehicle_type;//در کدام ناحیه مورد علاقه، این پلاک یافت شده است

    };
    class SATPA_API
    {
        public delegate void ANPR_EVENT_CALLBACK(int event_type, byte stream, int plt_idx);

        public const string DLL_NAME = "ANPR.dll";
        public const int WM_USER = 0x0400;
        public const int WM_NEW_FRAME = WM_USER + 100;
        public const int WM_SCENE_CHANGED = WM_USER + 101;
        public const int WM_PLATE_DETECTED = WM_USER + 102;
        public const int WM_PLATE_NOT_DETECTED = WM_USER + 103; //when a car is in the field of camera but its plate is not recognized
        public const int WM_END_OF_VIDEO = WM_USER + 104; //when video file finished or camera closed
        public const int WM_CONNECTED = WM_USER + 105; //Connected to camera (or video file) from Ver 8.43

        //هنگامی که اولین پلاک در صحنه دیده می شود، برای ترسیم مستطیل اطراف آن
        //رویداد تشخیص قطعی پلاک شماره 102 است
        public const int WM_INITIAL_PLATE = WM_USER + 108;
        public const int WM_CAM_NOT_FOUND = WM_USER + 109;

        public const int WM_CAM_SEARCH = WM_USER + 110;
        public const int WM_CAM_SEARCH_ERROR = WM_USER + 111;
        public const int WM_MESSAGE = WM_USER + 112;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                                                        //
        //                                                                                                                                        //
        //                                                        ERROR CODES                                                                     //
        //                                                                                                                                        //
        //                                                                                                                                        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public const int EXIT                       =                       199 ;                                // در صورت دریافت این کد، نمونه های ساخته شده از کتابخانه ساتپا حذف خواهند شد و کتابخانه در دسترس نخواهد بود. 

public const int ERR_FATAL_100				=						100	;								 
public const int ERR_FATAL_101				=						101	;								 // فایل مجوز مشکل دارد
public const int ERR_FATAL_102				=						102	;								 // فایل مجوز مشکل دارد
public const int ERR_FATAL_103				=						103	;								 // فایل مجوز مشکل دارد
public const int ERR_FATAL_104				=						104	;								 // فایل مجوز مشکل دارد
public const int ERR_FATAL_105				=						105	;								 // فایل مجوز مشکل دارد
public const int ERR_FATAL_106				=						106	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_107				=						107	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_108				=						108	;								 
public const int ERR_FATAL_109				=						109	;								 
public const int ERR_FATAL_110				=						110	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_111				=						111	;								 
public const int ERR_FATAL_112				=						112	;								 
public const int ERR_FATAL_113				=						113	;								 
public const int ERR_FATAL_114				=						114	;								 
public const int ERR_FATAL_115				=						115	;								 
public const int ERR_FATAL_116				=						116	;								 
public const int ERR_FATAL_117				=						117	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_118				=						118	;                                // مشکل در قفل سخت افزاری
public const int ERR_FATAL_119              =                       119 ;								 
public const int ERR_FATAL_120				=						120	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_121				=						121	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_122				=						122	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_123				=						123	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_124				=						124	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_125				=						125	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_126				=						126	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_127				=						127	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_128				=						128	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_129				=						129	;								 // مشکل در قفل سخت افزاری
public const int ERR_FATAL_130				=						130 ;                                // مشکل در قفل سخت افزاری
public const int ERR_MAX_INSTANCE			=						131	;								 
public const int ERR_TRIAL_LICENSE_EXPIRED	=						132	;								 
public const int ERR_INVALID_CODE			=						133	;								 // کدی که در تابع کریت فرستاده شده است اشتباه است
public const int ERR_INIALIZE_SDL			=						134	;								 // خطا در مقدار دهی اولیه کتابخانه اس دی ال
public const int ERR_MAX_FRAME				=						135	;								 // تعداد فریم های ازمایشی به اتمام رسیده است
public const int ERR_MCS_FILE				=						136	;								 // خطا در یافتن فایل های ام سی اس
public const int ERR_PLAY_LICENSE			=						137	;								 // مجوز شما امکان پلاک خوانی ندارد
public const int ERR_SATPA_IS_IN_USE		=						138	;								 // 
public const int ERR_TRAFFIC_FINISHED		=						140	;							     // ترافیک شما به پایان رسیده است
public const int ERR_BLOCKED_LISENCE		=						141	;								 // به دلیل تلاش ها ناموفق در شارژ دانگل، دانگل قفل شده است
public const int ERR_UNKNOWN				=						142	;								 // خطای ناشناخته ای در حین شارژ دانگل رخ داده است.دانگل را مجدد وصل کنید
public const int ERR_FATAL_143				=						143	;								 // سریال شارژ معتبر نیست 
public const int WAR_CHARGE_SUCCESSFULLY	=						144	;								 //  دانگل با موفقیت شارژ شد
public const int ERR_CHARGE_UNSUCCESSFULLY	=						145	;								 //  خطا در شارژ دانگل
public const int ERR_CHARGE_FUNCTION		=					   	146	;								 //  خطا در شارژ دانگل
public const int ERR_UNKNOWN_CHARGE_FUNCTION=						147	;								 //  خطا در شارژ دانگل
public const int ERR_GENERATE_CODE			=						148	;								 //  خطا در تولید سریال فعال سازی
public const int ERR_UNKNOWN_GENERATE_CODE	=						149	;								 //  خطا در تولید سریال فعال سازی
public const int ERR_INCORRECT_DATE			=						150	;								 // تاریخ سیستم اشتباه است 
public const int ERR_LICENSE_EXPIRED		=						151	;								 // مجوز شما منقضی شده است 
public const int ERR_INCORRECT_HID_PATH		=						152	;								 // مسیر ذخیره اچ ای دی مشکل دارد
public const int ERR_VIRTUAL				=						153 ;                                // کتابخانه ساتپا امکان اجرا روی ویرچوال را ندارد
public const int ERR_CFG_NOT_FOUND			=						154 ;                                // فایل مجوز یافت نشد
public const int ERR_EXIT_001				=						170	;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_002				=						171 ;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_003				=						172 ;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_004				=						173	;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_005				=						174 ;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_006				=						175 ;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int ERR_EXIT_007				=						176 ;                                // اشیا ساخته شده از ساتپا حذف خواهند شد
public const int WAR_TRIAL_LICENSE_DAYS  	=						200	;								 // برای مشاهده تعداد روز های باقی مانده از دوره ازمایشی از دمو کمک بگیرید 
public const int ERR_RECOGNIZE_FUNCTION		=						201	;								 // خطا در تابع
public const int ERR_INSTANCE_NOT_CREATED	=						202	;								 // ابتدا باید یک شی از ساتپا ساخته شود
public const int ERR_START_FUNCTION			=						203	;								 // خطا در تابع استارت
public const int ERR_STARTVLC_FUNCTION		=						204	;								 // خطا در تابع استارت VLC
public const int ERR_RECORDING_FUNCTION		=						205	;								 // خطا در تابع استارت VLC
public const int ERR_SDL_RENDERER			=						206	;								 // خطا SDL
public const int ERR_STOP_RECORDING_FUNCTION=						207	;								 // خطا در تابع استارت VLC
public const int ERR_MATRIX					=						208	;								 // خطا در هدر ماتریکس
public const int ERR_CMATRIX				=						209	;								 // خطا در تابع سازنده CMATRIX
public const int ERR_CREATE_TEXTURE			=						210	;								 // خطا در SDL DRAW
public const int ERR_INCORRECT_SIZE			=						211	;								 // خطا در SDL DRAW
public const int WAR_TRIAL					=						212	;								 // خطا در SDL DRAW
public const int WAR_ABOUT					=						213	;								 // خطا در SDL DRAW
public const int ERR_NORMAL_TRAFFIC			=						214	;								 // دانگل دارای شارژ کافی است و امکان شارژ مجدد وجود ندارد
public const int WAR_TEMP					=						215	;								  
public const int ACTIVATION_CODE			=						216	;								 // برای دریافت سریال فعال سازی از دمو کمک بگیرید
public const int WAR_LICENSE_FINISHING		=						217	;								 // مجوز پلاک خوانی شما رو به اتمام است
public const int ERR_ACTIVATION_CODE		=						218	;                                // مجوز شما دارای اعتبار کافی است
public const int ERR_anpr_set_params        =                       219 ;



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                                                        //
        //                                                                                                                                        //
        //                                                        FUNCTIONS                                                                       //
        //                                                                                                                                        //
        //                                                                                                                                        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <typeparam name="Type"></typeparam>

        public class OptionalInput<Type>
        {
            public Type Result { get; set; }
        }
        //1
        //تابع زیر به ازای هر نسخه کتابخانه (مثلا به ازای هر دوربین) حتما باید یکبار فراخوانی شود. 
        //این تابع شبکه های عصبی مورد استفاده را بارگذاری می کند
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_create(byte instance, byte per_plate_license, [MarshalAs(UnmanagedType.LPWStr)] string security_code, byte log_level = 1, [MarshalAs(UnmanagedType.LPWStr)] string cfg_file = null);

        //1-1
        //این تابع مدیریت تمام رویدادهای کتابخانه را بر عهده دارد
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_set_event_callback(ANPR_EVENT_CALLBACK callback_fcn,int reserve = 0);

        //2
        //این تابع مسیر فایل تصویری را دریافت کرده و نتیجه را بر می گرداند: 
        //رشته، میزان اطمینان به رشته حاصله و مستطیل پلاک
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_recognize(byte instance, [MarshalAs(UnmanagedType.LPWStr)] string fn,
            [MarshalAs(UnmanagedType.LPWStr)] string result, ref float cnf, ref RECT prc);

        //3
        //این تابع مانند تابع بالایی است با این تفاوت که اندیس مستطیل مورد علاقه را هم می گیرد.
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_recognizeROI(byte instance, byte roi_idx, [MarshalAs(UnmanagedType.LPWStr)] string fn,
            [MarshalAs(UnmanagedType.LPWStr)] string result, ref float cnf, ref RECT prc);


        //4
        //تابع زیر برای بافری است که از دوربین یا فایل گرفته اید و نوعا یک جریان فشرده مثل جی پگ است.
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_recognize_stream(byte instance, IntPtr compressed_stream, int size, [MarshalAs(UnmanagedType.LPWStr)] string result, ref float cnf, ref RECT prc);

        //5
        //تابع زیر برای زمانی است که بایتهای تصویر به صورت فشرده نشده در آرایه ای قرار دارند
        //مثلا اشاره گر ابتدای یک بیت مپ
        //مثال آن در همین برنامه دیده می شود
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_recognize_buffer(byte instance, IntPtr bytes, int W, int H, int step, [MarshalAs(UnmanagedType.LPWStr)] string result, ref float cnf, ref RECT prc);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_recognize_bufferROI(byte instance, byte roi_idx, IntPtr bytes, int W, int H, int step, [MarshalAs(UnmanagedType.LPWStr)] string result, ref float cnf, ref RECT prc);

        //6
        //خروجی تابع 2 یک رشته فارسی یونیکد است، اگر خروجی انگلیسی «اسکی» را لازم دارید از این تابع استفاده کنید 
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_get_ascii_result([MarshalAs(UnmanagedType.LPWStr)] string result_fa, [MarshalAs(UnmanagedType.LPStr)] string result_en);//Get ascii results in English

        //7
        //خروجی تابع 2 یک رشته فارسی یونیکد است، اگر خروجی انگلیسی «یونیکد» را لازم دارید از این تابع استفاده کنید 
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_get_en_result([MarshalAs(UnmanagedType.LPWStr)] string result_fa, [MarshalAs(UnmanagedType.LPWStr)] string result_en);//Get unicode results in English

        //8
        //یافتن نویسه ها از بافر حافظه ای که تنها شامل تصویر پلاک است
        //به عبارتی محل پلاک باید قبلا یافت شده باشد
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_find_chars(byte instance, IntPtr bytes, int W, int H, int step, RECT roi, [MarshalAs(UnmanagedType.LPWStr)] string result, ref float pcnf);

        //9
        //این تابع برای تنظیم پارامترهای کتابخانه است.
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_set_params(byte instance, ref SLPRParams slpr_params);

        //این تابع برای تست برخی پارامترهای کتابخانه است و حتی الامکان نباید استفاده شود.
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_set_debug_mode(byte instance, byte debug_level);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_add_ROI(byte instance, RECT roi);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_clear_ROIs(byte instance);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_get_plate(byte instance, int plate_idx, ref SPlateResult result);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_about(ref Lic_Info lic_info);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short anpr_charge_license([MarshalAs(UnmanagedType.LPStr)] string serial);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void anpr_get_activation_code();

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_start_grabbing(byte instance, [MarshalAs(UnmanagedType.LPStr)] string URL, float interval_ms, IntPtr hwndDraw, byte take_shots, byte draw_method);
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_stop_grabbing(byte instance);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_start_grabbingVLC(byte instance, [MarshalAs(UnmanagedType.LPStr)] string URL, float interval_ms, IntPtr hwndDraw, byte take_shots, byte draw_method);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_stop_grabbingVLC(byte instance);


        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_pause_or_resume(byte instance, byte pause);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_get_frame_info(byte instance, ref int W, ref int H, ref int channels, ref int step);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern IntPtr vlpr_get_frame(byte instance);

        //Start Processing of Camera Frames
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_start_process(byte instance, bool plate_marker = true);

        //Stop Processing of Camera Frames
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_stop_process(byte instance);
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_start_recording(byte instance, [MarshalAs(UnmanagedType.LPStr)] string path);
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_stop_recording(byte instance);
        //str must be allocated before
        //output is buffer of plate image
        //پلاک پس از عبور خودرو گزارش می شود. لذا به منظور ثبت تصویر پلاک
        //بافر آن نگهداری می شود
        [Obsolete("vlpr_get_last_resultsW is deprecated, please use anpr_get_plate instead.")]
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern IntPtr vlpr_get_last_resultsW(byte stream, [MarshalAs(UnmanagedType.LPWStr)] string str, ref RECT pr, ref float cnf, ref IntPtr img_car_buffer, ref byte direction);

        [Obsolete("vlpr_get_last_results is deprecated, please use anpr_get_plate instead.")]
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern IntPtr vlpr_get_last_results(byte stream, byte[] str, ref RECT pr, ref float cnf, ref IntPtr img_car_buffer, ref byte direction);

        //Recognize Last Frame Grabbed from camera or video file
        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern short vlpr_recognize_cur_frame(byte instance, [MarshalAs(UnmanagedType.LPWStr)] string str, ref RECT pr, ref float cnf);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern void vlpr_camera_search();

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]
        public static extern int vlpr_get_cam(ref camera_info camera, byte idx);

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]

        public static extern short anpr_write_dongle(ref byte data);//*********************************************************************************

        [System.Runtime.InteropServices.DllImport(DLL_NAME)]

        public static extern short anpr_read_dongle(ref byte data);//*********************************************************************************

        public static void SetDefParams(byte instance )
        {
            SLPRParams prm = new SLPRParams();
            prm.min_char_w = 5; //minimum with of characters
            prm.min_char_h = 7; //minimum height of characters
            prm.max_char_w = 100; //maximum with of characters
            prm.max_char_h = 100; //maximum height of characters
            prm.skew_coef = 1.0f; //more value means more skew: successive characters are not in the same Y position
            prm.resize_thresh = 2500;//if width of input image is larger than this, it will be resized
            prm.medianKernel = 0;//Kernel size: 0, 3, 5, 7, etc...
            prm.ignore_inverted_plates = 0;
            prm.detect_motor = 0; //if 1 motor detection is enabled, if 0 No.
            prm.detect_multi_plate = 0;
            prm.num_valid_chars = new byte[2] { 8, 0 };
            //prm.num_valid_chars1 = 8; //5 for free 
            //prm.num_valid_chars2 = 0; //5 for free //mhh - changed 5 to 0

            prm.save_plate_option = (byte)ESAVE_PLATE_OPTION.SAVE_PLATE_AND_CAR;
            prm.n_frm_skip_on_success = 1;
            prm.plate_buf_size = 7;
            prm.report_non_standard_plates = 1;
            prm.vlc_net_cache_time = 1000;
            prm.plate_type = 0;
            prm.diff_thresh = 15; //difference threshold between current frame and background to suppose entrance of new car 
            prm.play_audio_from_camera = 0;
            prm.min_thresh_hist = 60;
            prm.max_thresh_hist = 170;

            prm.blur_kernel = new byte[2] { 13, 13 };
            prm.img_bin_th = new float[2] { 0.9f, 0.95f };
            prm.plt_bin_th = new float[4] { 0.8f, 0.85f, 0.92f, 1.0f };

            prm.char_diffrence = 1;
            prm.engine = 0;
            prm.economy = 0;
            prm.reserve1 = 0;
            prm.reserve2 = 0;
            prm.reserve3 = 0;

            anpr_set_params(instance, ref prm);
        }

    }

 }
