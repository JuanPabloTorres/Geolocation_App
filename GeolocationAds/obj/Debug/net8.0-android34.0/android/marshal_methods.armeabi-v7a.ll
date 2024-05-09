; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [366 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [726 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 278
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 315
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 38948123, ; 6: ar\Microsoft.Maui.Controls.resources.dll => 0x2524d1b => 327
	i32 39109920, ; 7: Newtonsoft.Json.dll => 0x254c520 => 213
	i32 39485524, ; 8: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42244203, ; 9: he\Microsoft.Maui.Controls.resources.dll => 0x284986b => 336
	i32 42639949, ; 10: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 11: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 12: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 360
	i32 68219467, ; 13: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 14: Microsoft.Maui.Graphics.dll => 0x44bb714 => 210
	i32 82292897, ; 15: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 83839681, ; 16: ms\Microsoft.Maui.Controls.resources.dll => 0x4ff4ac1 => 344
	i32 101534019, ; 17: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 297
	i32 117431740, ; 18: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 19: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 297
	i32 122350210, ; 20: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 21: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 323
	i32 136584136, ; 22: zh-Hans\Microsoft.Maui.Controls.resources.dll => 0x8241bc8 => 359
	i32 140062828, ; 23: sk\Microsoft.Maui.Controls.resources.dll => 0x859306c => 352
	i32 142721839, ; 24: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149764678, ; 25: Svg.Skia.dll => 0x8ed3a46 => 219
	i32 149972175, ; 26: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 27: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 28: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 252
	i32 176265551, ; 29: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 30: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 299
	i32 184328833, ; 31: System.ValueTuple.dll => 0xafca281 => 151
	i32 205061960, ; 32: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 33: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 250
	i32 220171995, ; 34: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 35: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 272
	i32 230752869, ; 36: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 37: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 38: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 39: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 40: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 255
	i32 266337479, ; 41: Xamarin.Google.Guava.FailureAccess.dll => 0xfdffcc7 => 314
	i32 276479776, ; 42: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 43: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 274
	i32 280482487, ; 44: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 271
	i32 291076382, ; 45: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 293579439, ; 46: ExoPlayer.Dash.dll => 0x117faaaf => 227
	i32 298918909, ; 47: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 48: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 357
	i32 318968648, ; 49: Xamarin.AndroidX.Activity.dll => 0x13031348 => 241
	i32 321597661, ; 50: System.Numerics => 0x132b30dd => 83
	i32 321963286, ; 51: fr\Microsoft.Maui.Controls.resources.dll => 0x1330c516 => 335
	i32 342366114, ; 52: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 273
	i32 360082299, ; 53: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 54: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 55: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 56: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 57: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 58: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 59: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 60: _Microsoft.Android.Resource.Designer => 0x17969339 => 362
	i32 403441872, ; 61: WindowsBase => 0x180c08d0 => 165
	i32 409257351, ; 62: tr\Microsoft.Maui.Controls.resources.dll => 0x1864c587 => 355
	i32 441335492, ; 63: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 256
	i32 442565967, ; 64: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 65: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 269
	i32 451504562, ; 66: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 452127346, ; 67: ExoPlayer.Database.dll => 0x1af2ea72 => 228
	i32 456227837, ; 68: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 69: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465658307, ; 70: ExCSS => 0x1bc161c3 => 179
	i32 465846621, ; 71: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 72: System.dll => 0x1bff388e => 164
	i32 469965489, ; 73: Svg.Model => 0x1c031ab1 => 218
	i32 476646585, ; 74: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 271
	i32 486930444, ; 75: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 284
	i32 489220957, ; 76: es\Microsoft.Maui.Controls.resources.dll => 0x1d28eb5d => 333
	i32 490002678, ; 77: Microsoft.AspNetCore.Hosting.Server.Abstractions.dll => 0x1d34d8f6 => 186
	i32 498788369, ; 78: System.ObjectModel => 0x1dbae811 => 84
	i32 513247710, ; 79: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 203
	i32 525008092, ; 80: SkiaSharp.dll => 0x1f4afcdc => 215
	i32 526420162, ; 81: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 82: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 323
	i32 530272170, ; 83: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 538707440, ; 84: th\Microsoft.Maui.Controls.resources.dll => 0x201c05f0 => 354
	i32 539058512, ; 85: Microsoft.Extensions.Logging => 0x20216150 => 200
	i32 540030774, ; 86: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 87: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 88: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 89: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 90: Jsr305Binding => 0x213954e7 => 310
	i32 569601784, ; 91: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 308
	i32 577335427, ; 92: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 586578074, ; 93: MimeKit => 0x22f6789a => 212
	i32 597488923, ; 94: CommunityToolkit.Maui => 0x239cf51b => 175
	i32 601371474, ; 95: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 96: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 97: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 626887733, ; 98: ExoPlayer.Container => 0x255d8c35 => 225
	i32 627609679, ; 99: Xamarin.AndroidX.CustomView => 0x2568904f => 261
	i32 627931235, ; 100: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 346
	i32 639843206, ; 101: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 267
	i32 643868501, ; 102: System.Net => 0x2660a755 => 81
	i32 662205335, ; 103: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 104: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 304
	i32 666292255, ; 105: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 248
	i32 672442732, ; 106: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 107: System.Net.Security => 0x28bdabca => 73
	i32 690569205, ; 108: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 109: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 325
	i32 693804605, ; 110: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 111: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 112: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 320
	i32 700358131, ; 113: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 709152836, ; 114: System.Security.Cryptography.Pkcs.dll => 0x2a44d044 => 221
	i32 720511267, ; 115: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 324
	i32 722857257, ; 116: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 117: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 752232764, ; 118: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 119: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 238
	i32 759454413, ; 120: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 121: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 122: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 123: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 352
	i32 778756650, ; 124: SkiaSharp.HarfBuzz.dll => 0x2e6ae22a => 216
	i32 789151979, ; 125: Microsoft.Extensions.Options => 0x2f0980eb => 202
	i32 790371945, ; 126: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 262
	i32 804715423, ; 127: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 128: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 276
	i32 812693636, ; 129: ExoPlayer.Dash => 0x3070b884 => 227
	i32 823281589, ; 130: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 131: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 132: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 133: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 134: Xamarin.AndroidX.Print => 0x3246f6cd => 290
	i32 869139383, ; 135: hi\Microsoft.Maui.Controls.resources.dll => 0x33ce03b7 => 337
	i32 873119928, ; 136: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 137: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 138: System.Net.Http.Json => 0x3463c971 => 63
	i32 880668424, ; 139: ru\Microsoft.Maui.Controls.resources.dll => 0x347def08 => 351
	i32 904024072, ; 140: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 908888060, ; 141: Microsoft.Maui.Maps => 0x362c87fc => 211
	i32 911108515, ; 142: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 918734561, ; 143: pt-BR\Microsoft.Maui.Controls.resources.dll => 0x36c2c6e1 => 348
	i32 921035005, ; 144: ToolsLibrary => 0x36e5e0fd => 361
	i32 928116545, ; 145: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 315
	i32 939704684, ; 146: ExoPlayer.Extractor => 0x3802c16c => 231
	i32 944435932, ; 147: Xabe.FFmpeg.dll => 0x384af2dc => 222
	i32 952186615, ; 148: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 149: Newtonsoft.Json => 0x38f24a24 => 213
	i32 956575887, ; 150: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 324
	i32 961460050, ; 151: it\Microsoft.Maui.Controls.resources.dll => 0x394eb752 => 341
	i32 966729478, ; 152: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 311
	i32 967690846, ; 153: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 273
	i32 975236339, ; 154: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 155: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 156: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 157: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 990727110, ; 158: ro\Microsoft.Maui.Controls.resources.dll => 0x3b0d4bc6 => 350
	i32 992768348, ; 159: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 160: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 161: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 162: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 294
	i32 1019214401, ; 163: System.Drawing => 0x3cbffa41 => 36
	i32 1028013380, ; 164: ExoPlayer.UI.dll => 0x3d463d44 => 236
	i32 1028951442, ; 165: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 197
	i32 1031528504, ; 166: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 312
	i32 1035644815, ; 167: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 246
	i32 1036536393, ; 168: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1043950537, ; 169: de\Microsoft.Maui.Controls.resources.dll => 0x3e396bc9 => 331
	i32 1044663988, ; 170: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 171: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 280
	i32 1067306892, ; 172: GoogleGson => 0x3f9dcf8c => 182
	i32 1082857460, ; 173: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 174: Xamarin.Kotlin.StdLib => 0x409e66d8 => 321
	i32 1098259244, ; 175: System => 0x41761b2c => 164
	i32 1108272742, ; 176: sv\Microsoft.Maui.Controls.resources.dll => 0x420ee666 => 353
	i32 1110309514, ; 177: Microsoft.Extensions.Hosting.Abstractions => 0x422dfa8a => 199
	i32 1117529484, ; 178: pl\Microsoft.Maui.Controls.resources.dll => 0x429c258c => 347
	i32 1118262833, ; 179: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 343
	i32 1121599056, ; 180: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 279
	i32 1149092582, ; 181: Xamarin.AndroidX.Window => 0x447dc2e6 => 307
	i32 1151313727, ; 182: ExoPlayer.Rtsp => 0x449fa73f => 233
	i32 1157931901, ; 183: Microsoft.EntityFrameworkCore.Abstractions => 0x4504a37d => 190
	i32 1168523401, ; 184: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 349
	i32 1170634674, ; 185: System.Web.dll => 0x45c677b2 => 153
	i32 1173126369, ; 186: Microsoft.Extensions.FileProviders.Abstractions.dll => 0x45ec7ce1 => 198
	i32 1175144683, ; 187: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 303
	i32 1178241025, ; 188: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 288
	i32 1202000627, ; 189: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x47a512f3 => 190
	i32 1204270330, ; 190: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 248
	i32 1204575371, ; 191: Microsoft.Extensions.Caching.Memory.dll => 0x47cc5c8b => 193
	i32 1208641965, ; 192: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 193: CommunityToolkit.Mvvm => 0x4868cc7b => 178
	i32 1219128291, ; 194: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1236289705, ; 195: Microsoft.AspNetCore.Hosting.Server.Abstractions => 0x49b048a9 => 186
	i32 1243150071, ; 196: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 308
	i32 1253011324, ; 197: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 198: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 329
	i32 1263886435, ; 199: Xamarin.Google.Guava.dll => 0x4b556063 => 313
	i32 1264511973, ; 200: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 298
	i32 1267360935, ; 201: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 302
	i32 1273260888, ; 202: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 253
	i32 1275534314, ; 203: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 325
	i32 1278448581, ; 204: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 245
	i32 1293217323, ; 205: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 264
	i32 1308624726, ; 206: hr\Microsoft.Maui.Controls.resources.dll => 0x4e000756 => 338
	i32 1309188875, ; 207: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1309209905, ; 208: ExoPlayer.DataSource => 0x4e08f531 => 229
	i32 1322716291, ; 209: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 307
	i32 1324164729, ; 210: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 211: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1336711579, ; 212: zh-HK\Microsoft.Maui.Controls.resources.dll => 0x4fac999b => 358
	i32 1364015309, ; 213: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 214: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 359
	i32 1376866003, ; 215: Xamarin.AndroidX.SavedState => 0x52114ed3 => 294
	i32 1379779777, ; 216: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1395857551, ; 217: Xamarin.AndroidX.Media.dll => 0x5333188f => 285
	i32 1402170036, ; 218: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 219: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 257
	i32 1406299041, ; 220: Xamarin.Google.Guava.FailureAccess => 0x53d26ba1 => 314
	i32 1408764838, ; 221: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 222: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 223: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 224: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 327
	i32 1434145427, ; 225: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 226: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 311
	i32 1439761251, ; 227: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 228: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 229: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 230: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 231: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 232: es\Microsoft.Maui.Controls.resources => 0x57152abe => 333
	i32 1461234159, ; 233: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 234: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 235: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 236: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 247
	i32 1470490898, ; 237: Microsoft.Extensions.Primitives => 0x57a5e912 => 203
	i32 1479771757, ; 238: System.Collections.Immutable => 0x5833866d => 9
	i32 1480156764, ; 239: ExoPlayer.DataSource.dll => 0x5839665c => 229
	i32 1480492111, ; 240: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 241: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 242: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 295
	i32 1526286932, ; 243: vi\Microsoft.Maui.Controls.resources.dll => 0x5af94a54 => 357
	i32 1536373174, ; 244: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 245: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 246: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 247: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1565862583, ; 248: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 249: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 250: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 251: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 252: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 263
	i32 1592978981, ; 253: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 254: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 312
	i32 1600541741, ; 255: ShimSkiaSharp => 0x5f66542d => 214
	i32 1601112923, ; 256: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 257: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 258: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 259: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 283
	i32 1622358360, ; 260: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 261: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 306
	i32 1634654947, ; 262: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 176
	i32 1635184631, ; 263: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 267
	i32 1636350590, ; 264: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 260
	i32 1638652436, ; 265: CommunityToolkit.Maui.MediaElement => 0x61abda14 => 177
	i32 1639515021, ; 266: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 267: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 268: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 269: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 270: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 300
	i32 1658251792, ; 271: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 309
	i32 1670060433, ; 272: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 255
	i32 1675553242, ; 273: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 274: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 275: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 276: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1689493916, ; 277: Microsoft.EntityFrameworkCore.dll => 0x64b3a19c => 189
	i32 1691477237, ; 278: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 279: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 280: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 322
	i32 1700397376, ; 281: ExoPlayer.Transformer => 0x655a0140 => 235
	i32 1701541528, ; 282: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1720223769, ; 283: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 276
	i32 1726116996, ; 284: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 285: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 286: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 251
	i32 1743415430, ; 287: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 328
	i32 1744735666, ; 288: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 289: System.IO.Pipelines.dll => 0x68139a0d => 220
	i32 1746316138, ; 290: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 291: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1751678353, ; 292: ToolsLibrary.dll => 0x68687d91 => 361
	i32 1758240030, ; 293: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 294: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765620304, ; 295: ExoPlayer.Core => 0x693d3a50 => 226
	i32 1765942094, ; 296: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 297: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 299
	i32 1770582343, ; 298: Microsoft.Extensions.Logging.dll => 0x6988f147 => 200
	i32 1776026572, ; 299: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 300: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 301: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 302: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 344
	i32 1788241197, ; 303: Xamarin.AndroidX.Fragment => 0x6a96652d => 269
	i32 1793755602, ; 304: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 336
	i32 1808609942, ; 305: Xamarin.AndroidX.Loader => 0x6bcd3296 => 283
	i32 1813058853, ; 306: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 321
	i32 1813201214, ; 307: Xamarin.Google.Android.Material => 0x6c13413e => 309
	i32 1818569960, ; 308: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 289
	i32 1818787751, ; 309: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1819327070, ; 310: Microsoft.AspNetCore.Http.Features.dll => 0x6c70ba5e => 188
	i32 1824175904, ; 311: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 312: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1828688058, ; 313: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 201
	i32 1847515442, ; 314: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 238
	i32 1853025655, ; 315: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 353
	i32 1858542181, ; 316: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 317: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 318: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 335
	i32 1879696579, ; 319: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 320: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 249
	i32 1888955245, ; 321: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 322: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1893218855, ; 323: cs\Microsoft.Maui.Controls.resources.dll => 0x70d83a27 => 329
	i32 1898237753, ; 324: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 325: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1908813208, ; 326: Xamarin.GooglePlayServices.Basement => 0x71c62d98 => 317
	i32 1910275211, ; 327: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1926145099, ; 328: ExoPlayer.Container.dll => 0x72cea44b => 225
	i32 1928288591, ; 329: Microsoft.AspNetCore.Http.Abstractions => 0x72ef594f => 187
	i32 1939592360, ; 330: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1953182387, ; 331: id\Microsoft.Maui.Controls.resources.dll => 0x746b32b3 => 340
	i32 1956758971, ; 332: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 333: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 296
	i32 1968388702, ; 334: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 194
	i32 1983156543, ; 335: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 322
	i32 1985761444, ; 336: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 240
	i32 2003115576, ; 337: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 332
	i32 2011961780, ; 338: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 339: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 280
	i32 2031763787, ; 340: Xamarin.Android.Glide => 0x791a414b => 237
	i32 2045470958, ; 341: System.Private.Xml => 0x79eb68ee => 88
	i32 2055257422, ; 342: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 275
	i32 2060060697, ; 343: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 344: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 331
	i32 2070888862, ; 345: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2075706075, ; 346: Microsoft.AspNetCore.Http.Abstractions.dll => 0x7bb8c2db => 187
	i32 2079903147, ; 347: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 348: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2106312818, ; 349: ExoPlayer.Decoder => 0x7d8bc872 => 230
	i32 2113912252, ; 350: ExoPlayer.UI => 0x7dffbdbc => 236
	i32 2127167465, ; 351: System.Console => 0x7ec9ffe9 => 20
	i32 2129483829, ; 352: Xamarin.GooglePlayServices.Base.dll => 0x7eed5835 => 316
	i32 2142473426, ; 353: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 354: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 355: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 356: Microsoft.Maui => 0x80bd55ad => 208
	i32 2169148018, ; 357: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 339
	i32 2181898931, ; 358: Microsoft.Extensions.Options.dll => 0x820d22b3 => 202
	i32 2192057212, ; 359: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 201
	i32 2193016926, ; 360: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 361: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 326
	i32 2201231467, ; 362: System.Net.Http => 0x8334206b => 64
	i32 2202964214, ; 363: ExoPlayer.dll => 0x834e90f6 => 223
	i32 2207618523, ; 364: it\Microsoft.Maui.Controls.resources => 0x839595db => 341
	i32 2217644978, ; 365: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 303
	i32 2222056684, ; 366: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2239138732, ; 367: ExoPlayer.SmoothStreaming => 0x85768bac => 234
	i32 2244775296, ; 368: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 284
	i32 2252106437, ; 369: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2252897993, ; 370: Microsoft.EntityFrameworkCore => 0x86487ec9 => 189
	i32 2256313426, ; 371: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 372: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 373: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 195
	i32 2267999099, ; 374: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 239
	i32 2279755925, ; 375: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 292
	i32 2293034957, ; 376: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 377: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 378: System.Net.Mail => 0x88ffe49e => 66
	i32 2303073227, ; 379: Microsoft.Maui.Controls.Maps.dll => 0x89461bcb => 206
	i32 2303942373, ; 380: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 345
	i32 2305521784, ; 381: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 382: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 243
	i32 2320631194, ; 383: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2327893114, ; 384: ExCSS.dll => 0x8ac0d47a => 179
	i32 2340441535, ; 385: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 386: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 387: System.Net.Primitives => 0x8c40e0db => 70
	i32 2366048013, ; 388: hu\Microsoft.Maui.Controls.resources.dll => 0x8d07070d => 339
	i32 2368005991, ; 389: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2371007202, ; 390: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 194
	i32 2378619854, ; 391: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 392: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 393: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 340
	i32 2401565422, ; 394: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 395: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 266
	i32 2421380589, ; 396: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 397: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 253
	i32 2427813419, ; 398: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 337
	i32 2435356389, ; 399: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 400: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2437192331, ; 401: CommunityToolkit.Maui.MediaElement.dll => 0x91449a8b => 177
	i32 2454642406, ; 402: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 403: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 404: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465532216, ; 405: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 256
	i32 2466355063, ; 406: FFImageLoading.Maui.dll => 0x93019777 => 180
	i32 2471841756, ; 407: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 408: Java.Interop.dll => 0x93918882 => 168
	i32 2476233210, ; 409: ExoPlayer => 0x939851fa => 223
	i32 2480646305, ; 410: Microsoft.Maui.Controls => 0x93dba8a1 => 205
	i32 2483903535, ; 411: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 412: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 413: System.AppContext.dll => 0x94798bc5 => 6
	i32 2498657740, ; 414: BouncyCastle.Cryptography.dll => 0x94ee7dcc => 174
	i32 2501346920, ; 415: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2503351294, ; 416: ko\Microsoft.Maui.Controls.resources.dll => 0x95361bfe => 343
	i32 2505896520, ; 417: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 278
	i32 2508463432, ; 418: BCrypt.Net-Core => 0x95841d48 => 173
	i32 2515854816, ; 419: ExoPlayer.Common => 0x95f4e5e0 => 224
	i32 2522472828, ; 420: Xamarin.Android.Glide.dll => 0x9659e17c => 237
	i32 2523023297, ; 421: Svg.Custom.dll => 0x966247c1 => 217
	i32 2538310050, ; 422: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 423: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 338
	i32 2562349572, ; 424: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 425: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2576534780, ; 426: ja\Microsoft.Maui.Controls.resources.dll => 0x9992ccfc => 342
	i32 2581783588, ; 427: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 279
	i32 2581819634, ; 428: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 302
	i32 2585220780, ; 429: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 430: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 431: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2592341985, ; 432: Microsoft.Extensions.FileProviders.Abstractions => 0x9a83ffe1 => 198
	i32 2593496499, ; 433: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 347
	i32 2594125473, ; 434: Microsoft.AspNetCore.Hosting.Abstractions => 0x9a9f36a1 => 185
	i32 2602257211, ; 435: Svg.Model.dll => 0x9b1b4b3b => 218
	i32 2605712449, ; 436: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 326
	i32 2609324236, ; 437: Svg.Custom => 0x9b8720cc => 217
	i32 2615233544, ; 438: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 270
	i32 2617129537, ; 439: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 440: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 441: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 260
	i32 2621070698, ; 442: GeolocationAds => 0x9c3a5d6a => 0
	i32 2624644809, ; 443: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 265
	i32 2626028643, ; 444: ExoPlayer.Rtsp.dll => 0x9c860463 => 233
	i32 2626831493, ; 445: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 342
	i32 2627185994, ; 446: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 447: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 448: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 274
	i32 2634653062, ; 449: Microsoft.EntityFrameworkCore.Relational.dll => 0x9d099d86 => 191
	i32 2663391936, ; 450: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 239
	i32 2663698177, ; 451: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 452: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 453: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 454: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 455: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 456: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 457: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 300
	i32 2713040075, ; 458: ExoPlayer.Hls => 0xa1b5b4cb => 232
	i32 2715334215, ; 459: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 460: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 461: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 462: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 463: Xamarin.AndroidX.Activity => 0xa2e0939b => 241
	i32 2735172069, ; 464: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 465: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 247
	i32 2740698338, ; 466: ca\Microsoft.Maui.Controls.resources.dll => 0xa35bbce2 => 328
	i32 2740948882, ; 467: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 468: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 469: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 348
	i32 2758225723, ; 470: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 207
	i32 2764765095, ; 471: Microsoft.Maui.dll => 0xa4caf7a7 => 208
	i32 2765824710, ; 472: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 473: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 320
	i32 2778768386, ; 474: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 305
	i32 2779977773, ; 475: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 293
	i32 2785988530, ; 476: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 354
	i32 2788224221, ; 477: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 270
	i32 2796087574, ; 478: ExoPlayer.Extractor.dll => 0xa6a8e916 => 231
	i32 2801831435, ; 479: Microsoft.Maui.Graphics => 0xa7008e0b => 210
	i32 2803228030, ; 480: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2810250172, ; 481: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 257
	i32 2819470561, ; 482: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 483: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 484: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 293
	i32 2822463729, ; 485: BCrypt.Net-Core.dll => 0xa83b60f1 => 173
	i32 2824502124, ; 486: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2838993487, ; 487: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 281
	i32 2847418871, ; 488: Xamarin.GooglePlayServices.Base => 0xa9b829f7 => 316
	i32 2847789619, ; 489: Microsoft.EntityFrameworkCore.Relational => 0xa9bdd233 => 191
	i32 2849599387, ; 490: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2850549256, ; 491: Microsoft.AspNetCore.Http.Features => 0xa9e7ee08 => 188
	i32 2853208004, ; 492: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 305
	i32 2855708567, ; 493: Xamarin.AndroidX.Transition => 0xaa36a797 => 301
	i32 2861098320, ; 494: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 495: Microsoft.Maui.Essentials => 0xaa8a4878 => 209
	i32 2868488919, ; 496: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 176
	i32 2870099610, ; 497: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 242
	i32 2875164099, ; 498: Jsr305Binding.dll => 0xab5f85c3 => 310
	i32 2875220617, ; 499: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 500: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 268
	i32 2887636118, ; 501: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 502: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 503: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 504: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 505: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 506: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2916838712, ; 507: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 306
	i32 2919462931, ; 508: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 509: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 244
	i32 2936416060, ; 510: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 511: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 512: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 513: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2960379616, ; 514: Xamarin.Google.Guava => 0xb073cee0 => 313
	i32 2968338931, ; 515: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 516: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978368250, ; 517: Microsoft.AspNetCore.Hosting.Abstractions.dll => 0xb1864afa => 185
	i32 2978675010, ; 518: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 264
	i32 2987532451, ; 519: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 296
	i32 2996846495, ; 520: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 277
	i32 3016983068, ; 521: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 298
	i32 3017076677, ; 522: Xamarin.GooglePlayServices.Maps => 0xb3d4efc5 => 318
	i32 3023353419, ; 523: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 524: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 272
	i32 3027462113, ; 525: ExoPlayer.Common.dll => 0xb47367e1 => 224
	i32 3038032645, ; 526: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 362
	i32 3053864966, ; 527: fi\Microsoft.Maui.Controls.resources.dll => 0xb6064806 => 334
	i32 3056245963, ; 528: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 295
	i32 3057625584, ; 529: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 286
	i32 3058099980, ; 530: Xamarin.GooglePlayServices.Tasks => 0xb646e70c => 319
	i32 3059408633, ; 531: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 532: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3069363400, ; 533: Microsoft.Extensions.Caching.Abstractions.dll => 0xb6f2c4c8 => 192
	i32 3075834255, ; 534: System.Threading.Tasks => 0xb755818f => 144
	i32 3090735792, ; 535: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 536: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103574161, ; 537: FFmpeg.AutoGen.dll => 0xb8fcc891 => 181
	i32 3103600923, ; 538: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3104590590, ; 539: FFmpeg.AutoGen => 0xb90c4afe => 181
	i32 3111772706, ; 540: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 541: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 542: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 543: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3134694676, ; 544: ShimSkiaSharp.dll => 0xbad7a514 => 214
	i32 3144327419, ; 545: ExoPlayer.Hls.dll => 0xbb6aa0fb => 232
	i32 3147165239, ; 546: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 547: GoogleGson.dll => 0xbba64c02 => 182
	i32 3159123045, ; 548: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 549: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3171180504, ; 550: MimeKit.dll => 0xbd045fd8 => 212
	i32 3178803400, ; 551: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 287
	i32 3190271366, ; 552: ExoPlayer.Decoder.dll => 0xbe27ad86 => 230
	i32 3192346100, ; 553: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 554: System.Web => 0xbe592c0c => 153
	i32 3195844289, ; 555: Microsoft.Extensions.Caching.Abstractions => 0xbe7cb6c1 => 192
	i32 3204380047, ; 556: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 557: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3210765148, ; 558: Xabe.FFmpeg => 0xbf60635c => 222
	i32 3211777861, ; 559: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 263
	i32 3220365878, ; 560: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 561: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3230466174, ; 562: Xamarin.GooglePlayServices.Basement.dll => 0xc08d007e => 317
	i32 3251039220, ; 563: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 564: Xamarin.AndroidX.CardView => 0xc235e84d => 251
	i32 3265493905, ; 565: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 566: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 567: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 568: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 569: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 570: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 571: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 572: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 573: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 334
	i32 3316684772, ; 574: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 575: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 261
	i32 3317144872, ; 576: System.Data => 0xc5b79d28 => 24
	i32 3329734229, ; 577: ExoPlayer.Database => 0xc677b655 => 228
	i32 3340387945, ; 578: SkiaSharp => 0xc71a4669 => 215
	i32 3340431453, ; 579: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 249
	i32 3345895724, ; 580: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 291
	i32 3346324047, ; 581: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 288
	i32 3357674450, ; 582: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 351
	i32 3358260929, ; 583: System.Text.Json => 0xc82afec1 => 137
	i32 3362336904, ; 584: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 242
	i32 3362522851, ; 585: Xamarin.AndroidX.Core => 0xc86c06e3 => 258
	i32 3366347497, ; 586: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 587: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 292
	i32 3381016424, ; 588: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 330
	i32 3395150330, ; 589: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3396979385, ; 590: ExoPlayer.Transformer.dll => 0xca79cab9 => 235
	i32 3403906625, ; 591: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 592: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 262
	i32 3413343012, ; 593: GoogleMapsApi => 0xcb737b24 => 183
	i32 3428513518, ; 594: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 196
	i32 3429136800, ; 595: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 596: netstandard => 0xcc7d82b4 => 167
	i32 3439098628, ; 597: GoogleMapsApi.dll => 0xccfc7b04 => 183
	i32 3441283291, ; 598: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 265
	i32 3445260447, ; 599: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 600: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 204
	i32 3458724246, ; 601: pt\Microsoft.Maui.Controls.resources.dll => 0xce27f196 => 349
	i32 3471940407, ; 602: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 603: Mono.Android => 0xcf3163e6 => 171
	i32 3484440000, ; 604: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 350
	i32 3485117614, ; 605: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 606: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 607: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 254
	i32 3500773090, ; 608: Microsoft.Maui.Controls.Maps => 0xd0a98ee2 => 206
	i32 3509114376, ; 609: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 610: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 611: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 612: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 613: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 614: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 615: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 358
	i32 3592228221, ; 616: zh-Hant\Microsoft.Maui.Controls.resources.dll => 0xd61d0d7d => 360
	i32 3597029428, ; 617: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 240
	i32 3598340787, ; 618: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3605570793, ; 619: BouncyCastle.Cryptography => 0xd6e8a4e9 => 174
	i32 3608519521, ; 620: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 621: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 622: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 290
	i32 3633644679, ; 623: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 244
	i32 3638274909, ; 624: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 625: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 275
	i32 3643446276, ; 626: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 355
	i32 3643854240, ; 627: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 287
	i32 3645089577, ; 628: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 629: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 195
	i32 3660523487, ; 630: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 631: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 632: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 250
	i32 3684561358, ; 633: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 254
	i32 3700866549, ; 634: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 635: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 259
	i32 3716563718, ; 636: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 637: Xamarin.AndroidX.Annotation => 0xdda814c6 => 243
	i32 3724971120, ; 638: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 286
	i32 3732100267, ; 639: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 640: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 641: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 642: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3751619990, ; 643: da\Microsoft.Maui.Controls.resources.dll => 0xdf9d2d96 => 330
	i32 3786282454, ; 644: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 252
	i32 3792276235, ; 645: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3792835768, ; 646: HarfBuzzSharp => 0xe21214b8 => 184
	i32 3800979733, ; 647: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 204
	i32 3802395368, ; 648: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3807198597, ; 649: System.Security.Cryptography.Pkcs => 0xe2ed3d85 => 221
	i32 3817368567, ; 650: CommunityToolkit.Maui.dll => 0xe3886bf7 => 175
	i32 3819260425, ; 651: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3822602673, ; 652: Xamarin.AndroidX.Media => 0xe3d849b1 => 285
	i32 3823082795, ; 653: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 654: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 655: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 197
	i32 3844307129, ; 656: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 657: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 658: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 659: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 660: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 661: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 662: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 301
	i32 3888767677, ; 663: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 291
	i32 3896106733, ; 664: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 665: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 258
	i32 3901907137, ; 666: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920221145, ; 667: nl\Microsoft.Maui.Controls.resources.dll => 0xe9a9d3d9 => 346
	i32 3920810846, ; 668: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 669: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 304
	i32 3928044579, ; 670: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 671: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 672: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 289
	i32 3945713374, ; 673: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953583589, ; 674: Svg.Skia => 0xeba6e5e5 => 219
	i32 3953953790, ; 675: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 676: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 246
	i32 3959773229, ; 677: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 277
	i32 3970018735, ; 678: Xamarin.GooglePlayServices.Tasks.dll => 0xeca1adaf => 319
	i32 4003436829, ; 679: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4003906742, ; 680: HarfBuzzSharp.dll => 0xeea6c4b6 => 184
	i32 4015948917, ; 681: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 245
	i32 4023392905, ; 682: System.IO.Pipelines => 0xefd01a89 => 220
	i32 4024771894, ; 683: GeolocationAds.dll => 0xefe52536 => 0
	i32 4025784931, ; 684: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 685: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 207
	i32 4054681211, ; 686: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4066802364, ; 687: SkiaSharp.HarfBuzz => 0xf2667abc => 216
	i32 4068434129, ; 688: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 689: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4078967171, ; 690: Microsoft.Extensions.Hosting.Abstractions.dll => 0xf3201983 => 199
	i32 4091086043, ; 691: el\Microsoft.Maui.Controls.resources.dll => 0xf3d904db => 332
	i32 4094352644, ; 692: Microsoft.Maui.Essentials.dll => 0xf40add04 => 209
	i32 4099507663, ; 693: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 694: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 695: Xamarin.AndroidX.Emoji2 => 0xf479582c => 266
	i32 4101842092, ; 696: Microsoft.Extensions.Caching.Memory => 0xf47d24ac => 193
	i32 4103439459, ; 697: uk\Microsoft.Maui.Controls.resources.dll => 0xf4958463 => 356
	i32 4126470640, ; 698: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 196
	i32 4127667938, ; 699: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 700: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 701: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 702: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 356
	i32 4151237749, ; 703: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 704: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 705: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 706: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4173364138, ; 707: ExoPlayer.SmoothStreaming.dll => 0xf8c07baa => 234
	i32 4173862379, ; 708: FFImageLoading.Maui => 0xf8c815eb => 180
	i32 4181436372, ; 709: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 710: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 282
	i32 4185676441, ; 711: System.Security => 0xf97c5a99 => 130
	i32 4190597220, ; 712: ExoPlayer.Core.dll => 0xf9c77064 => 226
	i32 4190991637, ; 713: Microsoft.Maui.Maps.dll => 0xf9cd7515 => 211
	i32 4196529839, ; 714: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 715: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4249188766, ; 716: nb\Microsoft.Maui.Controls.resources.dll => 0xfd45799e => 345
	i32 4256097574, ; 717: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 259
	i32 4258378803, ; 718: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 281
	i32 4260525087, ; 719: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 720: Microsoft.Maui.Controls.dll => 0xfea12dee => 205
	i32 4274623895, ; 721: CommunityToolkit.Mvvm.dll => 0xfec99597 => 178
	i32 4274976490, ; 722: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4278134329, ; 723: Xamarin.GooglePlayServices.Maps.dll => 0xfeff2639 => 318
	i32 4292120959, ; 724: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 282
	i32 4294763496 ; 725: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 268
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [726 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 278, ; 3
	i32 315, ; 4
	i32 48, ; 5
	i32 327, ; 6
	i32 213, ; 7
	i32 80, ; 8
	i32 336, ; 9
	i32 145, ; 10
	i32 30, ; 11
	i32 360, ; 12
	i32 124, ; 13
	i32 210, ; 14
	i32 102, ; 15
	i32 344, ; 16
	i32 297, ; 17
	i32 107, ; 18
	i32 297, ; 19
	i32 139, ; 20
	i32 323, ; 21
	i32 359, ; 22
	i32 352, ; 23
	i32 77, ; 24
	i32 219, ; 25
	i32 124, ; 26
	i32 13, ; 27
	i32 252, ; 28
	i32 132, ; 29
	i32 299, ; 30
	i32 151, ; 31
	i32 18, ; 32
	i32 250, ; 33
	i32 26, ; 34
	i32 272, ; 35
	i32 1, ; 36
	i32 59, ; 37
	i32 42, ; 38
	i32 91, ; 39
	i32 255, ; 40
	i32 314, ; 41
	i32 147, ; 42
	i32 274, ; 43
	i32 271, ; 44
	i32 54, ; 45
	i32 227, ; 46
	i32 69, ; 47
	i32 357, ; 48
	i32 241, ; 49
	i32 83, ; 50
	i32 335, ; 51
	i32 273, ; 52
	i32 131, ; 53
	i32 55, ; 54
	i32 149, ; 55
	i32 74, ; 56
	i32 145, ; 57
	i32 62, ; 58
	i32 146, ; 59
	i32 362, ; 60
	i32 165, ; 61
	i32 355, ; 62
	i32 256, ; 63
	i32 12, ; 64
	i32 269, ; 65
	i32 125, ; 66
	i32 228, ; 67
	i32 152, ; 68
	i32 113, ; 69
	i32 179, ; 70
	i32 166, ; 71
	i32 164, ; 72
	i32 218, ; 73
	i32 271, ; 74
	i32 284, ; 75
	i32 333, ; 76
	i32 186, ; 77
	i32 84, ; 78
	i32 203, ; 79
	i32 215, ; 80
	i32 150, ; 81
	i32 323, ; 82
	i32 60, ; 83
	i32 354, ; 84
	i32 200, ; 85
	i32 51, ; 86
	i32 103, ; 87
	i32 114, ; 88
	i32 40, ; 89
	i32 310, ; 90
	i32 308, ; 91
	i32 120, ; 92
	i32 212, ; 93
	i32 175, ; 94
	i32 52, ; 95
	i32 44, ; 96
	i32 119, ; 97
	i32 225, ; 98
	i32 261, ; 99
	i32 346, ; 100
	i32 267, ; 101
	i32 81, ; 102
	i32 136, ; 103
	i32 304, ; 104
	i32 248, ; 105
	i32 8, ; 106
	i32 73, ; 107
	i32 155, ; 108
	i32 325, ; 109
	i32 154, ; 110
	i32 92, ; 111
	i32 320, ; 112
	i32 45, ; 113
	i32 221, ; 114
	i32 324, ; 115
	i32 109, ; 116
	i32 129, ; 117
	i32 25, ; 118
	i32 238, ; 119
	i32 72, ; 120
	i32 55, ; 121
	i32 46, ; 122
	i32 352, ; 123
	i32 216, ; 124
	i32 202, ; 125
	i32 262, ; 126
	i32 22, ; 127
	i32 276, ; 128
	i32 227, ; 129
	i32 86, ; 130
	i32 43, ; 131
	i32 160, ; 132
	i32 71, ; 133
	i32 290, ; 134
	i32 337, ; 135
	i32 3, ; 136
	i32 42, ; 137
	i32 63, ; 138
	i32 351, ; 139
	i32 16, ; 140
	i32 211, ; 141
	i32 53, ; 142
	i32 348, ; 143
	i32 361, ; 144
	i32 315, ; 145
	i32 231, ; 146
	i32 222, ; 147
	i32 105, ; 148
	i32 213, ; 149
	i32 324, ; 150
	i32 341, ; 151
	i32 311, ; 152
	i32 273, ; 153
	i32 34, ; 154
	i32 158, ; 155
	i32 85, ; 156
	i32 32, ; 157
	i32 350, ; 158
	i32 12, ; 159
	i32 51, ; 160
	i32 56, ; 161
	i32 294, ; 162
	i32 36, ; 163
	i32 236, ; 164
	i32 197, ; 165
	i32 312, ; 166
	i32 246, ; 167
	i32 35, ; 168
	i32 331, ; 169
	i32 58, ; 170
	i32 280, ; 171
	i32 182, ; 172
	i32 17, ; 173
	i32 321, ; 174
	i32 164, ; 175
	i32 353, ; 176
	i32 199, ; 177
	i32 347, ; 178
	i32 343, ; 179
	i32 279, ; 180
	i32 307, ; 181
	i32 233, ; 182
	i32 190, ; 183
	i32 349, ; 184
	i32 153, ; 185
	i32 198, ; 186
	i32 303, ; 187
	i32 288, ; 188
	i32 190, ; 189
	i32 248, ; 190
	i32 193, ; 191
	i32 29, ; 192
	i32 178, ; 193
	i32 52, ; 194
	i32 186, ; 195
	i32 308, ; 196
	i32 5, ; 197
	i32 329, ; 198
	i32 313, ; 199
	i32 298, ; 200
	i32 302, ; 201
	i32 253, ; 202
	i32 325, ; 203
	i32 245, ; 204
	i32 264, ; 205
	i32 338, ; 206
	i32 85, ; 207
	i32 229, ; 208
	i32 307, ; 209
	i32 61, ; 210
	i32 112, ; 211
	i32 358, ; 212
	i32 57, ; 213
	i32 359, ; 214
	i32 294, ; 215
	i32 99, ; 216
	i32 285, ; 217
	i32 19, ; 218
	i32 257, ; 219
	i32 314, ; 220
	i32 111, ; 221
	i32 101, ; 222
	i32 102, ; 223
	i32 327, ; 224
	i32 104, ; 225
	i32 311, ; 226
	i32 71, ; 227
	i32 38, ; 228
	i32 32, ; 229
	i32 103, ; 230
	i32 73, ; 231
	i32 333, ; 232
	i32 9, ; 233
	i32 123, ; 234
	i32 46, ; 235
	i32 247, ; 236
	i32 203, ; 237
	i32 9, ; 238
	i32 229, ; 239
	i32 43, ; 240
	i32 4, ; 241
	i32 295, ; 242
	i32 357, ; 243
	i32 31, ; 244
	i32 138, ; 245
	i32 92, ; 246
	i32 93, ; 247
	i32 49, ; 248
	i32 141, ; 249
	i32 112, ; 250
	i32 140, ; 251
	i32 263, ; 252
	i32 115, ; 253
	i32 312, ; 254
	i32 214, ; 255
	i32 157, ; 256
	i32 76, ; 257
	i32 79, ; 258
	i32 283, ; 259
	i32 37, ; 260
	i32 306, ; 261
	i32 176, ; 262
	i32 267, ; 263
	i32 260, ; 264
	i32 177, ; 265
	i32 64, ; 266
	i32 138, ; 267
	i32 15, ; 268
	i32 116, ; 269
	i32 300, ; 270
	i32 309, ; 271
	i32 255, ; 272
	i32 48, ; 273
	i32 70, ; 274
	i32 80, ; 275
	i32 126, ; 276
	i32 189, ; 277
	i32 94, ; 278
	i32 121, ; 279
	i32 322, ; 280
	i32 235, ; 281
	i32 26, ; 282
	i32 276, ; 283
	i32 97, ; 284
	i32 28, ; 285
	i32 251, ; 286
	i32 328, ; 287
	i32 149, ; 288
	i32 220, ; 289
	i32 169, ; 290
	i32 4, ; 291
	i32 361, ; 292
	i32 98, ; 293
	i32 33, ; 294
	i32 226, ; 295
	i32 93, ; 296
	i32 299, ; 297
	i32 200, ; 298
	i32 21, ; 299
	i32 41, ; 300
	i32 170, ; 301
	i32 344, ; 302
	i32 269, ; 303
	i32 336, ; 304
	i32 283, ; 305
	i32 321, ; 306
	i32 309, ; 307
	i32 289, ; 308
	i32 2, ; 309
	i32 188, ; 310
	i32 134, ; 311
	i32 111, ; 312
	i32 201, ; 313
	i32 238, ; 314
	i32 353, ; 315
	i32 58, ; 316
	i32 95, ; 317
	i32 335, ; 318
	i32 39, ; 319
	i32 249, ; 320
	i32 25, ; 321
	i32 94, ; 322
	i32 329, ; 323
	i32 89, ; 324
	i32 99, ; 325
	i32 317, ; 326
	i32 10, ; 327
	i32 225, ; 328
	i32 187, ; 329
	i32 87, ; 330
	i32 340, ; 331
	i32 100, ; 332
	i32 296, ; 333
	i32 194, ; 334
	i32 322, ; 335
	i32 240, ; 336
	i32 332, ; 337
	i32 7, ; 338
	i32 280, ; 339
	i32 237, ; 340
	i32 88, ; 341
	i32 275, ; 342
	i32 154, ; 343
	i32 331, ; 344
	i32 33, ; 345
	i32 187, ; 346
	i32 116, ; 347
	i32 82, ; 348
	i32 230, ; 349
	i32 236, ; 350
	i32 20, ; 351
	i32 316, ; 352
	i32 11, ; 353
	i32 162, ; 354
	i32 3, ; 355
	i32 208, ; 356
	i32 339, ; 357
	i32 202, ; 358
	i32 201, ; 359
	i32 84, ; 360
	i32 326, ; 361
	i32 64, ; 362
	i32 223, ; 363
	i32 341, ; 364
	i32 303, ; 365
	i32 143, ; 366
	i32 234, ; 367
	i32 284, ; 368
	i32 157, ; 369
	i32 189, ; 370
	i32 41, ; 371
	i32 117, ; 372
	i32 195, ; 373
	i32 239, ; 374
	i32 292, ; 375
	i32 131, ; 376
	i32 75, ; 377
	i32 66, ; 378
	i32 206, ; 379
	i32 345, ; 380
	i32 172, ; 381
	i32 243, ; 382
	i32 143, ; 383
	i32 179, ; 384
	i32 106, ; 385
	i32 151, ; 386
	i32 70, ; 387
	i32 339, ; 388
	i32 156, ; 389
	i32 194, ; 390
	i32 121, ; 391
	i32 127, ; 392
	i32 340, ; 393
	i32 152, ; 394
	i32 266, ; 395
	i32 141, ; 396
	i32 253, ; 397
	i32 337, ; 398
	i32 20, ; 399
	i32 14, ; 400
	i32 177, ; 401
	i32 135, ; 402
	i32 75, ; 403
	i32 59, ; 404
	i32 256, ; 405
	i32 180, ; 406
	i32 167, ; 407
	i32 168, ; 408
	i32 223, ; 409
	i32 205, ; 410
	i32 15, ; 411
	i32 74, ; 412
	i32 6, ; 413
	i32 174, ; 414
	i32 23, ; 415
	i32 343, ; 416
	i32 278, ; 417
	i32 173, ; 418
	i32 224, ; 419
	i32 237, ; 420
	i32 217, ; 421
	i32 91, ; 422
	i32 338, ; 423
	i32 1, ; 424
	i32 136, ; 425
	i32 342, ; 426
	i32 279, ; 427
	i32 302, ; 428
	i32 134, ; 429
	i32 69, ; 430
	i32 146, ; 431
	i32 198, ; 432
	i32 347, ; 433
	i32 185, ; 434
	i32 218, ; 435
	i32 326, ; 436
	i32 217, ; 437
	i32 270, ; 438
	i32 88, ; 439
	i32 96, ; 440
	i32 260, ; 441
	i32 0, ; 442
	i32 265, ; 443
	i32 233, ; 444
	i32 342, ; 445
	i32 31, ; 446
	i32 45, ; 447
	i32 274, ; 448
	i32 191, ; 449
	i32 239, ; 450
	i32 109, ; 451
	i32 158, ; 452
	i32 35, ; 453
	i32 22, ; 454
	i32 114, ; 455
	i32 57, ; 456
	i32 300, ; 457
	i32 232, ; 458
	i32 144, ; 459
	i32 118, ; 460
	i32 120, ; 461
	i32 110, ; 462
	i32 241, ; 463
	i32 139, ; 464
	i32 247, ; 465
	i32 328, ; 466
	i32 54, ; 467
	i32 105, ; 468
	i32 348, ; 469
	i32 207, ; 470
	i32 208, ; 471
	i32 133, ; 472
	i32 320, ; 473
	i32 305, ; 474
	i32 293, ; 475
	i32 354, ; 476
	i32 270, ; 477
	i32 231, ; 478
	i32 210, ; 479
	i32 159, ; 480
	i32 257, ; 481
	i32 163, ; 482
	i32 132, ; 483
	i32 293, ; 484
	i32 173, ; 485
	i32 161, ; 486
	i32 281, ; 487
	i32 316, ; 488
	i32 191, ; 489
	i32 140, ; 490
	i32 188, ; 491
	i32 305, ; 492
	i32 301, ; 493
	i32 169, ; 494
	i32 209, ; 495
	i32 176, ; 496
	i32 242, ; 497
	i32 310, ; 498
	i32 40, ; 499
	i32 268, ; 500
	i32 81, ; 501
	i32 56, ; 502
	i32 37, ; 503
	i32 97, ; 504
	i32 166, ; 505
	i32 172, ; 506
	i32 306, ; 507
	i32 82, ; 508
	i32 244, ; 509
	i32 98, ; 510
	i32 30, ; 511
	i32 159, ; 512
	i32 18, ; 513
	i32 313, ; 514
	i32 127, ; 515
	i32 119, ; 516
	i32 185, ; 517
	i32 264, ; 518
	i32 296, ; 519
	i32 277, ; 520
	i32 298, ; 521
	i32 318, ; 522
	i32 165, ; 523
	i32 272, ; 524
	i32 224, ; 525
	i32 362, ; 526
	i32 334, ; 527
	i32 295, ; 528
	i32 286, ; 529
	i32 319, ; 530
	i32 170, ; 531
	i32 16, ; 532
	i32 192, ; 533
	i32 144, ; 534
	i32 125, ; 535
	i32 118, ; 536
	i32 181, ; 537
	i32 38, ; 538
	i32 181, ; 539
	i32 115, ; 540
	i32 47, ; 541
	i32 142, ; 542
	i32 117, ; 543
	i32 214, ; 544
	i32 232, ; 545
	i32 34, ; 546
	i32 182, ; 547
	i32 95, ; 548
	i32 53, ; 549
	i32 212, ; 550
	i32 287, ; 551
	i32 230, ; 552
	i32 129, ; 553
	i32 153, ; 554
	i32 192, ; 555
	i32 24, ; 556
	i32 161, ; 557
	i32 222, ; 558
	i32 263, ; 559
	i32 148, ; 560
	i32 104, ; 561
	i32 317, ; 562
	i32 89, ; 563
	i32 251, ; 564
	i32 60, ; 565
	i32 142, ; 566
	i32 100, ; 567
	i32 5, ; 568
	i32 13, ; 569
	i32 122, ; 570
	i32 135, ; 571
	i32 28, ; 572
	i32 334, ; 573
	i32 72, ; 574
	i32 261, ; 575
	i32 24, ; 576
	i32 228, ; 577
	i32 215, ; 578
	i32 249, ; 579
	i32 291, ; 580
	i32 288, ; 581
	i32 351, ; 582
	i32 137, ; 583
	i32 242, ; 584
	i32 258, ; 585
	i32 168, ; 586
	i32 292, ; 587
	i32 330, ; 588
	i32 101, ; 589
	i32 235, ; 590
	i32 123, ; 591
	i32 262, ; 592
	i32 183, ; 593
	i32 196, ; 594
	i32 163, ; 595
	i32 167, ; 596
	i32 183, ; 597
	i32 265, ; 598
	i32 39, ; 599
	i32 204, ; 600
	i32 349, ; 601
	i32 17, ; 602
	i32 171, ; 603
	i32 350, ; 604
	i32 137, ; 605
	i32 150, ; 606
	i32 254, ; 607
	i32 206, ; 608
	i32 155, ; 609
	i32 130, ; 610
	i32 19, ; 611
	i32 65, ; 612
	i32 147, ; 613
	i32 47, ; 614
	i32 358, ; 615
	i32 360, ; 616
	i32 240, ; 617
	i32 79, ; 618
	i32 174, ; 619
	i32 61, ; 620
	i32 106, ; 621
	i32 290, ; 622
	i32 244, ; 623
	i32 49, ; 624
	i32 275, ; 625
	i32 355, ; 626
	i32 287, ; 627
	i32 14, ; 628
	i32 195, ; 629
	i32 68, ; 630
	i32 171, ; 631
	i32 250, ; 632
	i32 254, ; 633
	i32 78, ; 634
	i32 259, ; 635
	i32 108, ; 636
	i32 243, ; 637
	i32 286, ; 638
	i32 67, ; 639
	i32 63, ; 640
	i32 27, ; 641
	i32 160, ; 642
	i32 330, ; 643
	i32 252, ; 644
	i32 10, ; 645
	i32 184, ; 646
	i32 204, ; 647
	i32 11, ; 648
	i32 221, ; 649
	i32 175, ; 650
	i32 78, ; 651
	i32 285, ; 652
	i32 126, ; 653
	i32 83, ; 654
	i32 197, ; 655
	i32 66, ; 656
	i32 107, ; 657
	i32 65, ; 658
	i32 128, ; 659
	i32 122, ; 660
	i32 77, ; 661
	i32 301, ; 662
	i32 291, ; 663
	i32 8, ; 664
	i32 258, ; 665
	i32 2, ; 666
	i32 346, ; 667
	i32 44, ; 668
	i32 304, ; 669
	i32 156, ; 670
	i32 128, ; 671
	i32 289, ; 672
	i32 23, ; 673
	i32 219, ; 674
	i32 133, ; 675
	i32 246, ; 676
	i32 277, ; 677
	i32 319, ; 678
	i32 29, ; 679
	i32 184, ; 680
	i32 245, ; 681
	i32 220, ; 682
	i32 0, ; 683
	i32 62, ; 684
	i32 207, ; 685
	i32 90, ; 686
	i32 216, ; 687
	i32 87, ; 688
	i32 148, ; 689
	i32 199, ; 690
	i32 332, ; 691
	i32 209, ; 692
	i32 36, ; 693
	i32 86, ; 694
	i32 266, ; 695
	i32 193, ; 696
	i32 356, ; 697
	i32 196, ; 698
	i32 50, ; 699
	i32 6, ; 700
	i32 90, ; 701
	i32 356, ; 702
	i32 21, ; 703
	i32 162, ; 704
	i32 96, ; 705
	i32 50, ; 706
	i32 234, ; 707
	i32 180, ; 708
	i32 113, ; 709
	i32 282, ; 710
	i32 130, ; 711
	i32 226, ; 712
	i32 211, ; 713
	i32 76, ; 714
	i32 27, ; 715
	i32 345, ; 716
	i32 259, ; 717
	i32 281, ; 718
	i32 7, ; 719
	i32 205, ; 720
	i32 178, ; 721
	i32 110, ; 722
	i32 318, ; 723
	i32 282, ; 724
	i32 268 ; 725
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.1xx @ af27162bee43b7fecdca59b4f67aa8c175cbc875"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
