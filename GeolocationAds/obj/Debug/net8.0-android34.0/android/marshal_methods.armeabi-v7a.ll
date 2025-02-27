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

@assembly_image_cache = dso_local local_unnamed_addr global [383 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [760 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 292
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 329
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39109920, ; 6: Newtonsoft.Json.dll => 0x254c520 => 217
	i32 39485524, ; 7: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 8: System.Threading.Thread => 0x28aa24d => 143
	i32 66541672, ; 9: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 10: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 374
	i32 68219467, ; 11: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 12: Microsoft.Maui.Graphics.dll => 0x44bb714 => 214
	i32 82292897, ; 13: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 14: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 311
	i32 117431740, ; 15: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 16: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 311
	i32 122350210, ; 17: System.Threading.Channels.dll => 0x74aea82 => 137
	i32 134690465, ; 18: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 337
	i32 142721839, ; 19: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149764678, ; 20: Svg.Skia.dll => 0x8ed3a46 => 231
	i32 149972175, ; 21: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 22: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 23: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 266
	i32 176265551, ; 24: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 25: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 313
	i32 184328833, ; 26: System.ValueTuple.dll => 0xafca281 => 149
	i32 195452805, ; 27: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 371
	i32 199333315, ; 28: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 372
	i32 205061960, ; 29: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 30: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 264
	i32 220171995, ; 31: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 32: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 286
	i32 230752869, ; 33: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 34: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 35: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 36: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 37: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 269
	i32 266337479, ; 38: Xamarin.Google.Guava.FailureAccess.dll => 0xfdffcc7 => 328
	i32 276479776, ; 39: System.Threading.Timer.dll => 0x107abf20 => 145
	i32 278686392, ; 40: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 288
	i32 280482487, ; 41: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 285
	i32 280992041, ; 42: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 343
	i32 291076382, ; 43: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 293579439, ; 44: ExoPlayer.Dash.dll => 0x117faaaf => 241
	i32 298918909, ; 45: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 46: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 371
	i32 318968648, ; 47: Xamarin.AndroidX.Activity.dll => 0x13031348 => 255
	i32 321597661, ; 48: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 49: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 356
	i32 342366114, ; 50: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 287
	i32 356389973, ; 51: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 355
	i32 360082299, ; 52: System.ServiceModel.Web => 0x15766b7b => 131
	i32 364942007, ; 53: SkiaSharp.Extended.UI => 0x15c092b7 => 221
	i32 367780167, ; 54: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 55: System.Transactions.Local => 0x1658bf94 => 147
	i32 375677976, ; 56: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 57: System.Threading.Thread.dll => 0x16a510e1 => 143
	i32 382590210, ; 58: SkiaSharp.Extended.dll => 0x16cddd02 => 220
	i32 385762202, ; 59: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 60: System.Threading.ThreadPool.dll => 0x1766c1f7 => 144
	i32 395744057, ; 61: _Microsoft.Android.Resource.Designer => 0x17969339 => 379
	i32 403441872, ; 62: WindowsBase => 0x180c08d0 => 163
	i32 435591531, ; 63: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 367
	i32 441335492, ; 64: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 270
	i32 442565967, ; 65: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 66: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 283
	i32 451504562, ; 67: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 452127346, ; 68: ExoPlayer.Database.dll => 0x1af2ea72 => 242
	i32 456227837, ; 69: System.Web.HttpUtility.dll => 0x1b317bfd => 150
	i32 459347974, ; 70: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465658307, ; 71: ExCSS => 0x1bc161c3 => 177
	i32 465846621, ; 72: mscorlib => 0x1bc4415d => 164
	i32 469710990, ; 73: System.dll => 0x1bff388e => 162
	i32 469965489, ; 74: Svg.Model => 0x1c031ab1 => 230
	i32 476646585, ; 75: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 285
	i32 486930444, ; 76: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 298
	i32 490002678, ; 77: Microsoft.AspNetCore.Hosting.Server.Abstractions.dll => 0x1d34d8f6 => 185
	i32 498788369, ; 78: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 79: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 354
	i32 503918385, ; 80: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 348
	i32 504833739, ; 81: SkiaSharp.SceneGraph => 0x1e1726cb => 223
	i32 513247710, ; 82: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 207
	i32 525008092, ; 83: SkiaSharp.dll => 0x1f4afcdc => 219
	i32 526420162, ; 84: System.Transactions.dll => 0x1f6088c2 => 148
	i32 527452488, ; 85: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 337
	i32 530272170, ; 86: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 87: Microsoft.Extensions.Logging => 0x20216150 => 204
	i32 540030774, ; 88: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 89: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 90: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 91: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 92: Jsr305Binding => 0x213954e7 => 324
	i32 569601784, ; 93: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 322
	i32 577335427, ; 94: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 586578074, ; 95: MimeKit => 0x22f6789a => 216
	i32 592146354, ; 96: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 362
	i32 597488923, ; 97: CommunityToolkit.Maui => 0x239cf51b => 173
	i32 601371474, ; 98: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 99: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 100: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 626887733, ; 101: ExoPlayer.Container => 0x255d8c35 => 239
	i32 627609679, ; 102: Xamarin.AndroidX.CustomView => 0x2568904f => 275
	i32 627931235, ; 103: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 360
	i32 639843206, ; 104: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 281
	i32 643868501, ; 105: System.Net => 0x2660a755 => 81
	i32 662205335, ; 106: System.Text.Encodings.Web.dll => 0x27787397 => 234
	i32 663517072, ; 107: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 318
	i32 666292255, ; 108: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 262
	i32 672442732, ; 109: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 110: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 111: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 342
	i32 690569205, ; 112: System.Xml.Linq.dll => 0x29293ff5 => 153
	i32 690602616, ; 113: SkiaSharp.Skottie.dll => 0x2929c278 => 224
	i32 691348768, ; 114: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 339
	i32 693804605, ; 115: System.Windows => 0x295a9e3d => 152
	i32 699345723, ; 116: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 117: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 334
	i32 700358131, ; 118: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 119: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 357
	i32 709152836, ; 120: System.Security.Cryptography.Pkcs.dll => 0x2a44d044 => 233
	i32 709557578, ; 121: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 345
	i32 720511267, ; 122: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 338
	i32 722857257, ; 123: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 124: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 738469988, ; 125: SkiaSharp.SceneGraph.dll => 0x2c042864 => 223
	i32 752232764, ; 126: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 127: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 252
	i32 759454413, ; 128: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 129: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 130: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 131: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 366
	i32 778756650, ; 132: SkiaSharp.HarfBuzz.dll => 0x2e6ae22a => 222
	i32 778804420, ; 133: SkiaSharp.Extended.UI.dll => 0x2e6b9cc4 => 221
	i32 789151979, ; 134: Microsoft.Extensions.Options => 0x2f0980eb => 206
	i32 790371945, ; 135: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 276
	i32 804715423, ; 136: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 137: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 290
	i32 812693636, ; 138: ExoPlayer.Dash => 0x3070b884 => 241
	i32 823281589, ; 139: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 140: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 141: System.Xml.XPath.dll => 0x31a103c6 => 158
	i32 834051424, ; 142: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 143: Xamarin.AndroidX.Print => 0x3246f6cd => 304
	i32 873119928, ; 144: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 145: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 146: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 147: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 908888060, ; 148: Microsoft.Maui.Maps => 0x362c87fc => 215
	i32 911108515, ; 149: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 921035005, ; 150: ToolsLibrary => 0x36e5e0fd => 378
	i32 926902833, ; 151: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 369
	i32 928116545, ; 152: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 329
	i32 939704684, ; 153: ExoPlayer.Extractor => 0x3802c16c => 245
	i32 944435932, ; 154: Xabe.FFmpeg.dll => 0x384af2dc => 236
	i32 952186615, ; 155: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 156: Newtonsoft.Json => 0x38f24a24 => 217
	i32 956575887, ; 157: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 338
	i32 966729478, ; 158: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 325
	i32 967690846, ; 159: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 287
	i32 975236339, ; 160: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 161: System.Xml.XDocument => 0x3a2aaa1d => 156
	i32 986514023, ; 162: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 163: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 164: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 165: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 999186168, ; 166: Microsoft.Extensions.FileSystemGlobbing.dll => 0x3b8e5ef8 => 202
	i32 1001831731, ; 167: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 168: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 308
	i32 1019214401, ; 169: System.Drawing => 0x3cbffa41 => 36
	i32 1028013380, ; 170: ExoPlayer.UI.dll => 0x3d463d44 => 250
	i32 1028951442, ; 171: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 199
	i32 1029334545, ; 172: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 344
	i32 1031528504, ; 173: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 326
	i32 1035644815, ; 174: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 260
	i32 1036536393, ; 175: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 176: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 177: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 294
	i32 1067306892, ; 178: GoogleGson => 0x3f9dcf8c => 181
	i32 1082857460, ; 179: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 180: Xamarin.Kotlin.StdLib => 0x409e66d8 => 335
	i32 1098259244, ; 181: System => 0x41761b2c => 162
	i32 1106973742, ; 182: Microsoft.Extensions.Configuration.FileExtensions.dll => 0x41fb142e => 196
	i32 1110309514, ; 183: Microsoft.Extensions.Hosting.Abstractions => 0x422dfa8a => 203
	i32 1118262833, ; 184: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 357
	i32 1121599056, ; 185: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 293
	i32 1149092582, ; 186: Xamarin.AndroidX.Window => 0x447dc2e6 => 321
	i32 1151313727, ; 187: ExoPlayer.Rtsp => 0x449fa73f => 247
	i32 1157931901, ; 188: Microsoft.EntityFrameworkCore.Abstractions => 0x4504a37d => 189
	i32 1168523401, ; 189: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 363
	i32 1170634674, ; 190: System.Web.dll => 0x45c677b2 => 151
	i32 1173126369, ; 191: Microsoft.Extensions.FileProviders.Abstractions.dll => 0x45ec7ce1 => 200
	i32 1175144683, ; 192: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 317
	i32 1178241025, ; 193: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 302
	i32 1202000627, ; 194: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x47a512f3 => 189
	i32 1203215381, ; 195: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 361
	i32 1204270330, ; 196: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 262
	i32 1204575371, ; 197: Microsoft.Extensions.Caching.Memory.dll => 0x47cc5c8b => 192
	i32 1208641965, ; 198: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 199: CommunityToolkit.Mvvm => 0x4868cc7b => 176
	i32 1219128291, ; 200: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1234928153, ; 201: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 359
	i32 1236289705, ; 202: Microsoft.AspNetCore.Hosting.Server.Abstractions => 0x49b048a9 => 185
	i32 1243150071, ; 203: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 322
	i32 1253011324, ; 204: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 205: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 343
	i32 1263886435, ; 206: Xamarin.Google.Guava.dll => 0x4b556063 => 327
	i32 1264511973, ; 207: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 312
	i32 1267360935, ; 208: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 316
	i32 1273260888, ; 209: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 267
	i32 1275534314, ; 210: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 339
	i32 1278448581, ; 211: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 259
	i32 1293217323, ; 212: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 278
	i32 1309188875, ; 213: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1309209905, ; 214: ExoPlayer.DataSource => 0x4e08f531 => 243
	i32 1322716291, ; 215: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 321
	i32 1324164729, ; 216: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 217: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 218: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 219: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 373
	i32 1376866003, ; 220: Xamarin.AndroidX.SavedState => 0x52114ed3 => 308
	i32 1379779777, ; 221: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1395857551, ; 222: Xamarin.AndroidX.Media.dll => 0x5333188f => 299
	i32 1402170036, ; 223: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 224: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 271
	i32 1406299041, ; 225: Xamarin.Google.Guava.FailureAccess => 0x53d26ba1 => 328
	i32 1408764838, ; 226: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 227: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 228: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 229: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 341
	i32 1434145427, ; 230: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 231: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 325
	i32 1439761251, ; 232: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 233: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 234: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 235: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 236: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 237: es\Microsoft.Maui.Controls.resources => 0x57152abe => 347
	i32 1461234159, ; 238: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 239: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 240: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 241: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 261
	i32 1470490898, ; 242: Microsoft.Extensions.Primitives => 0x57a5e912 => 207
	i32 1479771757, ; 243: System.Collections.Immutable => 0x5833866d => 9
	i32 1480156764, ; 244: ExoPlayer.DataSource.dll => 0x5839665c => 243
	i32 1480492111, ; 245: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 246: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 247: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 309
	i32 1493001747, ; 248: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 351
	i32 1514721132, ; 249: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 346
	i32 1521091094, ; 250: Microsoft.Extensions.FileSystemGlobbing => 0x5aaa0216 => 202
	i32 1536373174, ; 251: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 252: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 136
	i32 1543355203, ; 253: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 254: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 255: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 366
	i32 1565862583, ; 256: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 257: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 139
	i32 1573704789, ; 258: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 259: System.Threading.Overlapped => 0x5e2d7514 => 138
	i32 1582372066, ; 260: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 277
	i32 1592978981, ; 261: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 262: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 326
	i32 1600541741, ; 263: ShimSkiaSharp => 0x5f66542d => 218
	i32 1601112923, ; 264: System.Xml.Serialization => 0x5f6f0b5b => 155
	i32 1603525486, ; 265: Microsoft.Maui.Controls.HotReload.Forms.dll => 0x5f93db6e => 375
	i32 1604827217, ; 266: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 267: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 268: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 297
	i32 1622358360, ; 269: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1623212457, ; 270: SkiaSharp.Views.Maui.Controls => 0x60c041a9 => 226
	i32 1624863272, ; 271: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 320
	i32 1632842087, ; 272: Microsoft.Extensions.Configuration.Json => 0x61533167 => 197
	i32 1634654947, ; 273: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 174
	i32 1635184631, ; 274: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 281
	i32 1636350590, ; 275: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 274
	i32 1638652436, ; 276: CommunityToolkit.Maui.MediaElement => 0x61abda14 => 175
	i32 1639515021, ; 277: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 278: System.Text.RegularExpressions => 0x61c036ca => 136
	i32 1641389582, ; 279: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 280: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 281: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 314
	i32 1658251792, ; 282: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 323
	i32 1670060433, ; 283: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 269
	i32 1675553242, ; 284: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 285: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 286: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 287: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1689493916, ; 288: Microsoft.EntityFrameworkCore.dll => 0x64b3a19c => 188
	i32 1691477237, ; 289: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 290: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 291: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 336
	i32 1700397376, ; 292: ExoPlayer.Transformer => 0x655a0140 => 249
	i32 1701541528, ; 293: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1720223769, ; 294: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 290
	i32 1724472758, ; 295: SkiaSharp.Extended => 0x66c95db6 => 220
	i32 1726116996, ; 296: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 297: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 298: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 265
	i32 1736233607, ; 299: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 364
	i32 1743415430, ; 300: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 342
	i32 1744735666, ; 301: System.Transactions.Local.dll => 0x67fe8db2 => 147
	i32 1746115085, ; 302: System.IO.Pipelines.dll => 0x68139a0d => 232
	i32 1746316138, ; 303: Mono.Android.Export => 0x6816ab6a => 167
	i32 1750313021, ; 304: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1751678353, ; 305: ToolsLibrary.dll => 0x68687d91 => 378
	i32 1758240030, ; 306: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 307: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765620304, ; 308: ExoPlayer.Core => 0x693d3a50 => 240
	i32 1765942094, ; 309: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 310: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 313
	i32 1770582343, ; 311: Microsoft.Extensions.Logging.dll => 0x6988f147 => 204
	i32 1776026572, ; 312: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 313: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 314: Mono.Android.Runtime.dll => 0x6a216153 => 168
	i32 1782862114, ; 315: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 358
	i32 1788241197, ; 316: Xamarin.AndroidX.Fragment => 0x6a96652d => 283
	i32 1793755602, ; 317: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 350
	i32 1808609942, ; 318: Xamarin.AndroidX.Loader => 0x6bcd3296 => 297
	i32 1813058853, ; 319: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 335
	i32 1813201214, ; 320: Xamarin.Google.Android.Material => 0x6c13413e => 323
	i32 1818569960, ; 321: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 303
	i32 1818787751, ; 322: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1819327070, ; 323: Microsoft.AspNetCore.Http.Features.dll => 0x6c70ba5e => 187
	i32 1824175904, ; 324: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 325: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827303595, ; 326: Microsoft.VisualStudio.DesignTools.TapContract => 0x6cea70ab => 377
	i32 1828688058, ; 327: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 205
	i32 1842015223, ; 328: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 370
	i32 1847515442, ; 329: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 252
	i32 1853025655, ; 330: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 367
	i32 1858542181, ; 331: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 332: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 333: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 349
	i32 1879696579, ; 334: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 335: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 263
	i32 1885918049, ; 336: Microsoft.VisualStudio.DesignTools.TapContract.dll => 0x7068d361 => 377
	i32 1888955245, ; 337: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 338: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 339: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 340: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1908813208, ; 341: Xamarin.GooglePlayServices.Basement => 0x71c62d98 => 331
	i32 1910275211, ; 342: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1926145099, ; 343: ExoPlayer.Container.dll => 0x72cea44b => 239
	i32 1928288591, ; 344: Microsoft.AspNetCore.Http.Abstractions => 0x72ef594f => 186
	i32 1939592360, ; 345: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 346: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 347: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 310
	i32 1968388702, ; 348: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 193
	i32 1983156543, ; 349: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 336
	i32 1985761444, ; 350: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 254
	i32 2003115576, ; 351: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 346
	i32 2011961780, ; 352: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 353: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 294
	i32 2025202353, ; 354: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 341
	i32 2031763787, ; 355: Xamarin.Android.Glide => 0x791a414b => 251
	i32 2045470958, ; 356: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 357: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 195
	i32 2055257422, ; 358: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 289
	i32 2060060697, ; 359: System.Windows.dll => 0x7aca0819 => 152
	i32 2066184531, ; 360: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 345
	i32 2070888862, ; 361: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2072397586, ; 362: Microsoft.Extensions.FileProviders.Physical => 0x7b864712 => 201
	i32 2075706075, ; 363: Microsoft.AspNetCore.Http.Abstractions.dll => 0x7bb8c2db => 186
	i32 2079903147, ; 364: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 365: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2106312818, ; 366: ExoPlayer.Decoder => 0x7d8bc872 => 244
	i32 2113912252, ; 367: ExoPlayer.UI => 0x7dffbdbc => 250
	i32 2127167465, ; 368: System.Console => 0x7ec9ffe9 => 20
	i32 2129483829, ; 369: Xamarin.GooglePlayServices.Base.dll => 0x7eed5835 => 330
	i32 2142473426, ; 370: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 371: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 160
	i32 2146852085, ; 372: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 373: Microsoft.Maui => 0x80bd55ad => 212
	i32 2169148018, ; 374: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 353
	i32 2181898931, ; 375: Microsoft.Extensions.Options.dll => 0x820d22b3 => 206
	i32 2192057212, ; 376: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 205
	i32 2193016926, ; 377: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 378: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 340
	i32 2201231467, ; 379: System.Net.Http => 0x8334206b => 64
	i32 2202964214, ; 380: ExoPlayer.dll => 0x834e90f6 => 237
	i32 2207618523, ; 381: it\Microsoft.Maui.Controls.resources => 0x839595db => 355
	i32 2216717168, ; 382: Firebase.Auth.dll => 0x84206b70 => 180
	i32 2217644978, ; 383: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 317
	i32 2222056684, ; 384: System.Threading.Tasks.Parallel => 0x8471e4ec => 141
	i32 2239138732, ; 385: ExoPlayer.SmoothStreaming => 0x85768bac => 248
	i32 2244775296, ; 386: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 298
	i32 2252106437, ; 387: System.Xml.Serialization.dll => 0x863c6ac5 => 155
	i32 2252897993, ; 388: Microsoft.EntityFrameworkCore => 0x86487ec9 => 188
	i32 2256313426, ; 389: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 390: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 391: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 194
	i32 2267999099, ; 392: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 253
	i32 2270573516, ; 393: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 349
	i32 2279755925, ; 394: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 306
	i32 2293034957, ; 395: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 396: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 397: System.Net.Mail => 0x88ffe49e => 66
	i32 2303073227, ; 398: Microsoft.Maui.Controls.Maps.dll => 0x89461bcb => 210
	i32 2303942373, ; 399: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 359
	i32 2305521784, ; 400: System.Private.CoreLib.dll => 0x896b7878 => 170
	i32 2315684594, ; 401: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 257
	i32 2320631194, ; 402: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 141
	i32 2327893114, ; 403: ExCSS.dll => 0x8ac0d47a => 177
	i32 2340441535, ; 404: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 405: System.ValueTuple => 0x8bbaa2cd => 149
	i32 2353062107, ; 406: System.Net.Primitives => 0x8c40e0db => 70
	i32 2364201794, ; 407: SkiaSharp.Views.Maui.Core => 0x8ceadb42 => 228
	i32 2368005991, ; 408: System.Xml.ReaderWriter.dll => 0x8d24e767 => 154
	i32 2371007202, ; 409: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 193
	i32 2378619854, ; 410: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 411: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 412: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 354
	i32 2401565422, ; 413: System.Web.HttpUtility => 0x8f24faee => 150
	i32 2403452196, ; 414: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 280
	i32 2409983638, ; 415: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 0x8fa56e96 => 376
	i32 2421380589, ; 416: System.Threading.Tasks.Dataflow => 0x905355ed => 139
	i32 2423080555, ; 417: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 267
	i32 2427813419, ; 418: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 351
	i32 2435356389, ; 419: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 420: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2437192331, ; 421: CommunityToolkit.Maui.MediaElement.dll => 0x91449a8b => 175
	i32 2454642406, ; 422: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 423: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 424: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465532216, ; 425: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 270
	i32 2466355063, ; 426: FFImageLoading.Maui.dll => 0x93019777 => 178
	i32 2471841756, ; 427: netstandard.dll => 0x93554fdc => 165
	i32 2475788418, ; 428: Java.Interop.dll => 0x93918882 => 166
	i32 2476233210, ; 429: ExoPlayer => 0x939851fa => 237
	i32 2480646305, ; 430: Microsoft.Maui.Controls => 0x93dba8a1 => 209
	i32 2483903535, ; 431: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 432: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 433: System.AppContext.dll => 0x94798bc5 => 6
	i32 2498657740, ; 434: BouncyCastle.Cryptography.dll => 0x94ee7dcc => 172
	i32 2501346920, ; 435: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 436: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 292
	i32 2508463432, ; 437: BCrypt.Net-Core => 0x95841d48 => 171
	i32 2515854816, ; 438: ExoPlayer.Common => 0x95f4e5e0 => 238
	i32 2521915375, ; 439: SkiaSharp.Views.Maui.Controls.Compatibility => 0x96515fef => 227
	i32 2522472828, ; 440: Xamarin.Android.Glide.dll => 0x9659e17c => 251
	i32 2523023297, ; 441: Svg.Custom.dll => 0x966247c1 => 229
	i32 2538310050, ; 442: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 443: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 352
	i32 2562349572, ; 444: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 445: System.Text.Encodings.Web => 0x9930ee42 => 234
	i32 2581783588, ; 446: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 293
	i32 2581819634, ; 447: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 316
	i32 2585220780, ; 448: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 449: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 450: System.Threading.ThreadPool => 0x9a5a3337 => 144
	i32 2592341985, ; 451: Microsoft.Extensions.FileProviders.Abstractions => 0x9a83ffe1 => 200
	i32 2593496499, ; 452: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 361
	i32 2594125473, ; 453: Microsoft.AspNetCore.Hosting.Abstractions => 0x9a9f36a1 => 184
	i32 2602257211, ; 454: Svg.Model.dll => 0x9b1b4b3b => 230
	i32 2605712449, ; 455: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 340
	i32 2609324236, ; 456: Svg.Custom => 0x9b8720cc => 229
	i32 2615233544, ; 457: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 284
	i32 2617129537, ; 458: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 459: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 460: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 274
	i32 2621070698, ; 461: GeolocationAds => 0x9c3a5d6a => 0
	i32 2624644809, ; 462: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 279
	i32 2625339995, ; 463: SkiaSharp.Views.Maui.Core.dll => 0x9c7b825b => 228
	i32 2626028643, ; 464: ExoPlayer.Rtsp.dll => 0x9c860463 => 247
	i32 2626831493, ; 465: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 356
	i32 2627185994, ; 466: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 467: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 468: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 288
	i32 2634653062, ; 469: Microsoft.EntityFrameworkCore.Relational.dll => 0x9d099d86 => 190
	i32 2663391936, ; 470: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 253
	i32 2663698177, ; 471: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 472: System.Xml.XDocument.dll => 0x9ecf752a => 156
	i32 2665622720, ; 473: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 474: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 475: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 476: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 477: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 314
	i32 2713040075, ; 478: ExoPlayer.Hls => 0xa1b5b4cb => 246
	i32 2715334215, ; 479: System.Threading.Tasks.dll => 0xa1d8b647 => 142
	i32 2717744543, ; 480: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 481: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 482: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 483: Xamarin.AndroidX.Activity => 0xa2e0939b => 255
	i32 2735172069, ; 484: System.Threading.Channels => 0xa30769e5 => 137
	i32 2737747696, ; 485: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 261
	i32 2740948882, ; 486: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 487: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 488: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 362
	i32 2758225723, ; 489: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 211
	i32 2764765095, ; 490: Microsoft.Maui.dll => 0xa4caf7a7 => 212
	i32 2765824710, ; 491: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 492: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 334
	i32 2778768386, ; 493: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 319
	i32 2779977773, ; 494: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 307
	i32 2785988530, ; 495: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 368
	i32 2788224221, ; 496: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 284
	i32 2795602088, ; 497: SkiaSharp.Views.Android.dll => 0xa6a180a8 => 225
	i32 2796087574, ; 498: ExoPlayer.Extractor.dll => 0xa6a8e916 => 245
	i32 2801831435, ; 499: Microsoft.Maui.Graphics => 0xa7008e0b => 214
	i32 2803228030, ; 500: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 157
	i32 2806116107, ; 501: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 347
	i32 2810250172, ; 502: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 271
	i32 2819470561, ; 503: System.Xml.dll => 0xa80db4e1 => 161
	i32 2821205001, ; 504: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 505: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 307
	i32 2822463729, ; 506: BCrypt.Net-Core.dll => 0xa83b60f1 => 171
	i32 2824502124, ; 507: System.Xml.XmlDocument => 0xa85a7b6c => 159
	i32 2831556043, ; 508: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 360
	i32 2838993487, ; 509: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 295
	i32 2847418871, ; 510: Xamarin.GooglePlayServices.Base => 0xa9b829f7 => 330
	i32 2847789619, ; 511: Microsoft.EntityFrameworkCore.Relational => 0xa9bdd233 => 190
	i32 2849599387, ; 512: System.Threading.Overlapped.dll => 0xa9d96f9b => 138
	i32 2850549256, ; 513: Microsoft.AspNetCore.Http.Features => 0xa9e7ee08 => 187
	i32 2853208004, ; 514: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 319
	i32 2855708567, ; 515: Xamarin.AndroidX.Transition => 0xaa36a797 => 315
	i32 2861098320, ; 516: Mono.Android.Export.dll => 0xaa88e550 => 167
	i32 2861189240, ; 517: Microsoft.Maui.Essentials => 0xaa8a4878 => 213
	i32 2868488919, ; 518: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 174
	i32 2870099610, ; 519: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 256
	i32 2875164099, ; 520: Jsr305Binding.dll => 0xab5f85c3 => 324
	i32 2875220617, ; 521: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 522: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 282
	i32 2887636118, ; 523: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 524: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 525: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 526: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 527: mscorlib.dll => 0xad2a79b6 => 164
	i32 2909740682, ; 528: System.Private.CoreLib => 0xad6f1e8a => 170
	i32 2911054922, ; 529: Microsoft.Extensions.FileProviders.Physical.dll => 0xad832c4a => 201
	i32 2912489636, ; 530: SkiaSharp.Views.Android => 0xad9910a4 => 225
	i32 2916838712, ; 531: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 320
	i32 2919462931, ; 532: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 533: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 258
	i32 2936416060, ; 534: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 535: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 536: System.Xml.XPath.XDocument => 0xaf624531 => 157
	i32 2959614098, ; 537: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2960379616, ; 538: Xamarin.Google.Guava => 0xb073cee0 => 327
	i32 2968338931, ; 539: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 540: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978368250, ; 541: Microsoft.AspNetCore.Hosting.Abstractions.dll => 0xb1864afa => 184
	i32 2978675010, ; 542: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 278
	i32 2987532451, ; 543: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 310
	i32 2996846495, ; 544: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 291
	i32 3016983068, ; 545: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 312
	i32 3017076677, ; 546: Xamarin.GooglePlayServices.Maps => 0xb3d4efc5 => 332
	i32 3023353419, ; 547: WindowsBase.dll => 0xb434b64b => 163
	i32 3024354802, ; 548: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 286
	i32 3027462113, ; 549: ExoPlayer.Common.dll => 0xb47367e1 => 238
	i32 3038032645, ; 550: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 379
	i32 3056245963, ; 551: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 309
	i32 3057625584, ; 552: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 300
	i32 3058099980, ; 553: Xamarin.GooglePlayServices.Tasks => 0xb646e70c => 333
	i32 3059408633, ; 554: Mono.Android.Runtime => 0xb65adef9 => 168
	i32 3059793426, ; 555: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3069363400, ; 556: Microsoft.Extensions.Caching.Abstractions.dll => 0xb6f2c4c8 => 191
	i32 3075834255, ; 557: System.Threading.Tasks => 0xb755818f => 142
	i32 3077302341, ; 558: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 353
	i32 3090735792, ; 559: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 560: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103574161, ; 561: FFmpeg.AutoGen.dll => 0xb8fcc891 => 179
	i32 3103600923, ; 562: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3104590590, ; 563: FFmpeg.AutoGen => 0xb90c4afe => 179
	i32 3111772706, ; 564: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 565: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 566: System.Threading.Tasks.Extensions => 0xba4127cb => 140
	i32 3132293585, ; 567: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3134694676, ; 568: ShimSkiaSharp.dll => 0xbad7a514 => 218
	i32 3144327419, ; 569: ExoPlayer.Hls.dll => 0xbb6aa0fb => 246
	i32 3147165239, ; 570: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 571: GoogleGson.dll => 0xbba64c02 => 181
	i32 3159123045, ; 572: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 573: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3171180504, ; 574: MimeKit.dll => 0xbd045fd8 => 216
	i32 3178803400, ; 575: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 301
	i32 3190271366, ; 576: ExoPlayer.Decoder.dll => 0xbe27ad86 => 244
	i32 3192346100, ; 577: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 578: System.Web => 0xbe592c0c => 151
	i32 3195844289, ; 579: Microsoft.Extensions.Caching.Abstractions => 0xbe7cb6c1 => 191
	i32 3204380047, ; 580: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 581: System.Xml.XmlDocument.dll => 0xbf506931 => 159
	i32 3210765148, ; 582: Xabe.FFmpeg => 0xbf60635c => 236
	i32 3211777861, ; 583: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 277
	i32 3220365878, ; 584: System.Threading => 0xbff2e236 => 146
	i32 3226221578, ; 585: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3230466174, ; 586: Xamarin.GooglePlayServices.Basement.dll => 0xc08d007e => 331
	i32 3251039220, ; 587: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 588: Xamarin.AndroidX.CardView => 0xc235e84d => 265
	i32 3265493905, ; 589: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 590: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 140
	i32 3277815716, ; 591: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 592: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 593: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 594: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 595: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 596: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 597: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 348
	i32 3316684772, ; 598: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 599: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 275
	i32 3317144872, ; 600: System.Data => 0xc5b79d28 => 24
	i32 3329734229, ; 601: ExoPlayer.Database => 0xc677b655 => 242
	i32 3340387945, ; 602: SkiaSharp => 0xc71a4669 => 219
	i32 3340431453, ; 603: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 263
	i32 3345895724, ; 604: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 305
	i32 3346324047, ; 605: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 302
	i32 3357674450, ; 606: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 365
	i32 3358260929, ; 607: System.Text.Json => 0xc82afec1 => 235
	i32 3362336904, ; 608: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 256
	i32 3362522851, ; 609: Xamarin.AndroidX.Core => 0xc86c06e3 => 272
	i32 3366347497, ; 610: Java.Interop => 0xc8a662e9 => 166
	i32 3374999561, ; 611: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 306
	i32 3381016424, ; 612: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 344
	i32 3395150330, ; 613: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3396979385, ; 614: ExoPlayer.Transformer.dll => 0xca79cab9 => 249
	i32 3403906625, ; 615: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 616: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 276
	i32 3413343012, ; 617: GoogleMapsApi => 0xcb737b24 => 182
	i32 3421170118, ; 618: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 195
	i32 3428513518, ; 619: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 198
	i32 3429136800, ; 620: System.Xml => 0xcc6479a0 => 161
	i32 3430777524, ; 621: netstandard => 0xcc7d82b4 => 165
	i32 3439098628, ; 622: GoogleMapsApi.dll => 0xccfc7b04 => 182
	i32 3441283291, ; 623: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 279
	i32 3445260447, ; 624: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 625: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 208
	i32 3463511458, ; 626: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 352
	i32 3466574376, ; 627: SkiaSharp.Views.Maui.Controls.Compatibility.dll => 0xce9fba28 => 227
	i32 3471940407, ; 628: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3473156932, ; 629: SkiaSharp.Views.Maui.Controls.dll => 0xcf042b44 => 226
	i32 3476120550, ; 630: Mono.Android => 0xcf3163e6 => 169
	i32 3479583265, ; 631: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 365
	i32 3484440000, ; 632: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 364
	i32 3485117614, ; 633: System.Text.Json.dll => 0xcfbaacae => 235
	i32 3486566296, ; 634: System.Transactions => 0xcfd0c798 => 148
	i32 3493954962, ; 635: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 268
	i32 3500773090, ; 636: Microsoft.Maui.Controls.Maps => 0xd0a98ee2 => 210
	i32 3509114376, ; 637: System.Xml.Linq => 0xd128d608 => 153
	i32 3515174580, ; 638: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 639: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 640: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 641: System.Threading.Timer => 0xd432d20b => 145
	i32 3570554715, ; 642: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 643: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 372
	i32 3597029428, ; 644: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 254
	i32 3598340787, ; 645: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3605570793, ; 646: BouncyCastle.Cryptography => 0xd6e8a4e9 => 172
	i32 3608519521, ; 647: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 648: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 649: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 304
	i32 3633644679, ; 650: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 258
	i32 3638274909, ; 651: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 652: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 289
	i32 3643446276, ; 653: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 369
	i32 3643854240, ; 654: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 301
	i32 3645089577, ; 655: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 656: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 194
	i32 3660523487, ; 657: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3663323240, ; 658: SkiaSharp.Skottie => 0xda59e068 => 224
	i32 3672681054, ; 659: Mono.Android.dll => 0xdae8aa5e => 169
	i32 3676670898, ; 660: Microsoft.Maui.Controls.HotReload.Forms => 0xdb258bb2 => 375
	i32 3682565725, ; 661: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 264
	i32 3684561358, ; 662: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 268
	i32 3697841164, ; 663: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 374
	i32 3700866549, ; 664: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 665: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 273
	i32 3716563718, ; 666: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 667: Xamarin.AndroidX.Annotation => 0xdda814c6 => 257
	i32 3722202641, ; 668: Microsoft.Extensions.Configuration.Json.dll => 0xdddc4e11 => 197
	i32 3724971120, ; 669: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 300
	i32 3732100267, ; 670: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 671: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 672: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 673: System.Xml.XPath => 0xdf9a7f42 => 158
	i32 3758424670, ; 674: Microsoft.Extensions.Configuration.FileExtensions => 0xe005025e => 196
	i32 3786282454, ; 675: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 266
	i32 3792276235, ; 676: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3792835768, ; 677: HarfBuzzSharp => 0xe21214b8 => 183
	i32 3800979733, ; 678: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 208
	i32 3802395368, ; 679: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3807198597, ; 680: System.Security.Cryptography.Pkcs => 0xe2ed3d85 => 233
	i32 3817368567, ; 681: CommunityToolkit.Maui.dll => 0xe3886bf7 => 173
	i32 3819260425, ; 682: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3822602673, ; 683: Xamarin.AndroidX.Media => 0xe3d849b1 => 299
	i32 3823082795, ; 684: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 685: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 686: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 199
	i32 3844307129, ; 687: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 688: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 689: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 690: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 691: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 692: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 693: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 315
	i32 3888767677, ; 694: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 305
	i32 3889960447, ; 695: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 373
	i32 3896106733, ; 696: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 697: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 272
	i32 3901907137, ; 698: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 699: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 700: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 318
	i32 3928044579, ; 701: System.Xml.ReaderWriter => 0xea213423 => 154
	i32 3930554604, ; 702: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 703: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 303
	i32 3945713374, ; 704: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953583589, ; 705: Svg.Skia => 0xeba6e5e5 => 231
	i32 3953953790, ; 706: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 707: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 260
	i32 3959773229, ; 708: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 291
	i32 3970018735, ; 709: Xamarin.GooglePlayServices.Tasks.dll => 0xeca1adaf => 333
	i32 3980434154, ; 710: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 368
	i32 3987592930, ; 711: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 350
	i32 4003436829, ; 712: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4003906742, ; 713: HarfBuzzSharp.dll => 0xeea6c4b6 => 183
	i32 4015948917, ; 714: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 259
	i32 4023392905, ; 715: System.IO.Pipelines => 0xefd01a89 => 232
	i32 4024013275, ; 716: Firebase.Auth => 0xefd991db => 180
	i32 4024771894, ; 717: GeolocationAds.dll => 0xefe52536 => 0
	i32 4025784931, ; 718: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 719: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 211
	i32 4054681211, ; 720: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4066802364, ; 721: SkiaSharp.HarfBuzz => 0xf2667abc => 222
	i32 4068434129, ; 722: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 723: System.Threading.dll => 0xf2ce3c98 => 146
	i32 4078967171, ; 724: Microsoft.Extensions.Hosting.Abstractions.dll => 0xf3201983 => 203
	i32 4094352644, ; 725: Microsoft.Maui.Essentials.dll => 0xf40add04 => 213
	i32 4099507663, ; 726: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 727: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 728: Xamarin.AndroidX.Emoji2 => 0xf479582c => 280
	i32 4101842092, ; 729: Microsoft.Extensions.Caching.Memory => 0xf47d24ac => 192
	i32 4102112229, ; 730: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 363
	i32 4125707920, ; 731: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 358
	i32 4126470640, ; 732: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 198
	i32 4127667938, ; 733: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 734: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 735: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 736: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 370
	i32 4151237749, ; 737: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 738: System.Xml.XmlSerializer => 0xf7e95c85 => 160
	i32 4161255271, ; 739: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 740: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4173364138, ; 741: ExoPlayer.SmoothStreaming.dll => 0xf8c07baa => 248
	i32 4173862379, ; 742: FFImageLoading.Maui => 0xf8c815eb => 178
	i32 4181436372, ; 743: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 744: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 296
	i32 4182880526, ; 745: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 0xf951b10e => 376
	i32 4185676441, ; 746: System.Security => 0xf97c5a99 => 130
	i32 4190597220, ; 747: ExoPlayer.Core.dll => 0xf9c77064 => 240
	i32 4190991637, ; 748: Microsoft.Maui.Maps.dll => 0xf9cd7515 => 215
	i32 4196529839, ; 749: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 750: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 751: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 273
	i32 4258378803, ; 752: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 295
	i32 4260525087, ; 753: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 754: Microsoft.Maui.Controls.dll => 0xfea12dee => 209
	i32 4274623895, ; 755: CommunityToolkit.Mvvm.dll => 0xfec99597 => 176
	i32 4274976490, ; 756: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4278134329, ; 757: Xamarin.GooglePlayServices.Maps.dll => 0xfeff2639 => 332
	i32 4292120959, ; 758: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 296
	i32 4294763496 ; 759: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 282
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [760 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 292, ; 3
	i32 329, ; 4
	i32 48, ; 5
	i32 217, ; 6
	i32 80, ; 7
	i32 143, ; 8
	i32 30, ; 9
	i32 374, ; 10
	i32 124, ; 11
	i32 214, ; 12
	i32 102, ; 13
	i32 311, ; 14
	i32 107, ; 15
	i32 311, ; 16
	i32 137, ; 17
	i32 337, ; 18
	i32 77, ; 19
	i32 231, ; 20
	i32 124, ; 21
	i32 13, ; 22
	i32 266, ; 23
	i32 132, ; 24
	i32 313, ; 25
	i32 149, ; 26
	i32 371, ; 27
	i32 372, ; 28
	i32 18, ; 29
	i32 264, ; 30
	i32 26, ; 31
	i32 286, ; 32
	i32 1, ; 33
	i32 59, ; 34
	i32 42, ; 35
	i32 91, ; 36
	i32 269, ; 37
	i32 328, ; 38
	i32 145, ; 39
	i32 288, ; 40
	i32 285, ; 41
	i32 343, ; 42
	i32 54, ; 43
	i32 241, ; 44
	i32 69, ; 45
	i32 371, ; 46
	i32 255, ; 47
	i32 83, ; 48
	i32 356, ; 49
	i32 287, ; 50
	i32 355, ; 51
	i32 131, ; 52
	i32 221, ; 53
	i32 55, ; 54
	i32 147, ; 55
	i32 74, ; 56
	i32 143, ; 57
	i32 220, ; 58
	i32 62, ; 59
	i32 144, ; 60
	i32 379, ; 61
	i32 163, ; 62
	i32 367, ; 63
	i32 270, ; 64
	i32 12, ; 65
	i32 283, ; 66
	i32 125, ; 67
	i32 242, ; 68
	i32 150, ; 69
	i32 113, ; 70
	i32 177, ; 71
	i32 164, ; 72
	i32 162, ; 73
	i32 230, ; 74
	i32 285, ; 75
	i32 298, ; 76
	i32 185, ; 77
	i32 84, ; 78
	i32 354, ; 79
	i32 348, ; 80
	i32 223, ; 81
	i32 207, ; 82
	i32 219, ; 83
	i32 148, ; 84
	i32 337, ; 85
	i32 60, ; 86
	i32 204, ; 87
	i32 51, ; 88
	i32 103, ; 89
	i32 114, ; 90
	i32 40, ; 91
	i32 324, ; 92
	i32 322, ; 93
	i32 120, ; 94
	i32 216, ; 95
	i32 362, ; 96
	i32 173, ; 97
	i32 52, ; 98
	i32 44, ; 99
	i32 119, ; 100
	i32 239, ; 101
	i32 275, ; 102
	i32 360, ; 103
	i32 281, ; 104
	i32 81, ; 105
	i32 234, ; 106
	i32 318, ; 107
	i32 262, ; 108
	i32 8, ; 109
	i32 73, ; 110
	i32 342, ; 111
	i32 153, ; 112
	i32 224, ; 113
	i32 339, ; 114
	i32 152, ; 115
	i32 92, ; 116
	i32 334, ; 117
	i32 45, ; 118
	i32 357, ; 119
	i32 233, ; 120
	i32 345, ; 121
	i32 338, ; 122
	i32 109, ; 123
	i32 129, ; 124
	i32 223, ; 125
	i32 25, ; 126
	i32 252, ; 127
	i32 72, ; 128
	i32 55, ; 129
	i32 46, ; 130
	i32 366, ; 131
	i32 222, ; 132
	i32 221, ; 133
	i32 206, ; 134
	i32 276, ; 135
	i32 22, ; 136
	i32 290, ; 137
	i32 241, ; 138
	i32 86, ; 139
	i32 43, ; 140
	i32 158, ; 141
	i32 71, ; 142
	i32 304, ; 143
	i32 3, ; 144
	i32 42, ; 145
	i32 63, ; 146
	i32 16, ; 147
	i32 215, ; 148
	i32 53, ; 149
	i32 378, ; 150
	i32 369, ; 151
	i32 329, ; 152
	i32 245, ; 153
	i32 236, ; 154
	i32 105, ; 155
	i32 217, ; 156
	i32 338, ; 157
	i32 325, ; 158
	i32 287, ; 159
	i32 34, ; 160
	i32 156, ; 161
	i32 85, ; 162
	i32 32, ; 163
	i32 12, ; 164
	i32 51, ; 165
	i32 202, ; 166
	i32 56, ; 167
	i32 308, ; 168
	i32 36, ; 169
	i32 250, ; 170
	i32 199, ; 171
	i32 344, ; 172
	i32 326, ; 173
	i32 260, ; 174
	i32 35, ; 175
	i32 58, ; 176
	i32 294, ; 177
	i32 181, ; 178
	i32 17, ; 179
	i32 335, ; 180
	i32 162, ; 181
	i32 196, ; 182
	i32 203, ; 183
	i32 357, ; 184
	i32 293, ; 185
	i32 321, ; 186
	i32 247, ; 187
	i32 189, ; 188
	i32 363, ; 189
	i32 151, ; 190
	i32 200, ; 191
	i32 317, ; 192
	i32 302, ; 193
	i32 189, ; 194
	i32 361, ; 195
	i32 262, ; 196
	i32 192, ; 197
	i32 29, ; 198
	i32 176, ; 199
	i32 52, ; 200
	i32 359, ; 201
	i32 185, ; 202
	i32 322, ; 203
	i32 5, ; 204
	i32 343, ; 205
	i32 327, ; 206
	i32 312, ; 207
	i32 316, ; 208
	i32 267, ; 209
	i32 339, ; 210
	i32 259, ; 211
	i32 278, ; 212
	i32 85, ; 213
	i32 243, ; 214
	i32 321, ; 215
	i32 61, ; 216
	i32 112, ; 217
	i32 57, ; 218
	i32 373, ; 219
	i32 308, ; 220
	i32 99, ; 221
	i32 299, ; 222
	i32 19, ; 223
	i32 271, ; 224
	i32 328, ; 225
	i32 111, ; 226
	i32 101, ; 227
	i32 102, ; 228
	i32 341, ; 229
	i32 104, ; 230
	i32 325, ; 231
	i32 71, ; 232
	i32 38, ; 233
	i32 32, ; 234
	i32 103, ; 235
	i32 73, ; 236
	i32 347, ; 237
	i32 9, ; 238
	i32 123, ; 239
	i32 46, ; 240
	i32 261, ; 241
	i32 207, ; 242
	i32 9, ; 243
	i32 243, ; 244
	i32 43, ; 245
	i32 4, ; 246
	i32 309, ; 247
	i32 351, ; 248
	i32 346, ; 249
	i32 202, ; 250
	i32 31, ; 251
	i32 136, ; 252
	i32 92, ; 253
	i32 93, ; 254
	i32 366, ; 255
	i32 49, ; 256
	i32 139, ; 257
	i32 112, ; 258
	i32 138, ; 259
	i32 277, ; 260
	i32 115, ; 261
	i32 326, ; 262
	i32 218, ; 263
	i32 155, ; 264
	i32 375, ; 265
	i32 76, ; 266
	i32 79, ; 267
	i32 297, ; 268
	i32 37, ; 269
	i32 226, ; 270
	i32 320, ; 271
	i32 197, ; 272
	i32 174, ; 273
	i32 281, ; 274
	i32 274, ; 275
	i32 175, ; 276
	i32 64, ; 277
	i32 136, ; 278
	i32 15, ; 279
	i32 116, ; 280
	i32 314, ; 281
	i32 323, ; 282
	i32 269, ; 283
	i32 48, ; 284
	i32 70, ; 285
	i32 80, ; 286
	i32 126, ; 287
	i32 188, ; 288
	i32 94, ; 289
	i32 121, ; 290
	i32 336, ; 291
	i32 249, ; 292
	i32 26, ; 293
	i32 290, ; 294
	i32 220, ; 295
	i32 97, ; 296
	i32 28, ; 297
	i32 265, ; 298
	i32 364, ; 299
	i32 342, ; 300
	i32 147, ; 301
	i32 232, ; 302
	i32 167, ; 303
	i32 4, ; 304
	i32 378, ; 305
	i32 98, ; 306
	i32 33, ; 307
	i32 240, ; 308
	i32 93, ; 309
	i32 313, ; 310
	i32 204, ; 311
	i32 21, ; 312
	i32 41, ; 313
	i32 168, ; 314
	i32 358, ; 315
	i32 283, ; 316
	i32 350, ; 317
	i32 297, ; 318
	i32 335, ; 319
	i32 323, ; 320
	i32 303, ; 321
	i32 2, ; 322
	i32 187, ; 323
	i32 134, ; 324
	i32 111, ; 325
	i32 377, ; 326
	i32 205, ; 327
	i32 370, ; 328
	i32 252, ; 329
	i32 367, ; 330
	i32 58, ; 331
	i32 95, ; 332
	i32 349, ; 333
	i32 39, ; 334
	i32 263, ; 335
	i32 377, ; 336
	i32 25, ; 337
	i32 94, ; 338
	i32 89, ; 339
	i32 99, ; 340
	i32 331, ; 341
	i32 10, ; 342
	i32 239, ; 343
	i32 186, ; 344
	i32 87, ; 345
	i32 100, ; 346
	i32 310, ; 347
	i32 193, ; 348
	i32 336, ; 349
	i32 254, ; 350
	i32 346, ; 351
	i32 7, ; 352
	i32 294, ; 353
	i32 341, ; 354
	i32 251, ; 355
	i32 88, ; 356
	i32 195, ; 357
	i32 289, ; 358
	i32 152, ; 359
	i32 345, ; 360
	i32 33, ; 361
	i32 201, ; 362
	i32 186, ; 363
	i32 116, ; 364
	i32 82, ; 365
	i32 244, ; 366
	i32 250, ; 367
	i32 20, ; 368
	i32 330, ; 369
	i32 11, ; 370
	i32 160, ; 371
	i32 3, ; 372
	i32 212, ; 373
	i32 353, ; 374
	i32 206, ; 375
	i32 205, ; 376
	i32 84, ; 377
	i32 340, ; 378
	i32 64, ; 379
	i32 237, ; 380
	i32 355, ; 381
	i32 180, ; 382
	i32 317, ; 383
	i32 141, ; 384
	i32 248, ; 385
	i32 298, ; 386
	i32 155, ; 387
	i32 188, ; 388
	i32 41, ; 389
	i32 117, ; 390
	i32 194, ; 391
	i32 253, ; 392
	i32 349, ; 393
	i32 306, ; 394
	i32 131, ; 395
	i32 75, ; 396
	i32 66, ; 397
	i32 210, ; 398
	i32 359, ; 399
	i32 170, ; 400
	i32 257, ; 401
	i32 141, ; 402
	i32 177, ; 403
	i32 106, ; 404
	i32 149, ; 405
	i32 70, ; 406
	i32 228, ; 407
	i32 154, ; 408
	i32 193, ; 409
	i32 121, ; 410
	i32 127, ; 411
	i32 354, ; 412
	i32 150, ; 413
	i32 280, ; 414
	i32 376, ; 415
	i32 139, ; 416
	i32 267, ; 417
	i32 351, ; 418
	i32 20, ; 419
	i32 14, ; 420
	i32 175, ; 421
	i32 135, ; 422
	i32 75, ; 423
	i32 59, ; 424
	i32 270, ; 425
	i32 178, ; 426
	i32 165, ; 427
	i32 166, ; 428
	i32 237, ; 429
	i32 209, ; 430
	i32 15, ; 431
	i32 74, ; 432
	i32 6, ; 433
	i32 172, ; 434
	i32 23, ; 435
	i32 292, ; 436
	i32 171, ; 437
	i32 238, ; 438
	i32 227, ; 439
	i32 251, ; 440
	i32 229, ; 441
	i32 91, ; 442
	i32 352, ; 443
	i32 1, ; 444
	i32 234, ; 445
	i32 293, ; 446
	i32 316, ; 447
	i32 134, ; 448
	i32 69, ; 449
	i32 144, ; 450
	i32 200, ; 451
	i32 361, ; 452
	i32 184, ; 453
	i32 230, ; 454
	i32 340, ; 455
	i32 229, ; 456
	i32 284, ; 457
	i32 88, ; 458
	i32 96, ; 459
	i32 274, ; 460
	i32 0, ; 461
	i32 279, ; 462
	i32 228, ; 463
	i32 247, ; 464
	i32 356, ; 465
	i32 31, ; 466
	i32 45, ; 467
	i32 288, ; 468
	i32 190, ; 469
	i32 253, ; 470
	i32 109, ; 471
	i32 156, ; 472
	i32 35, ; 473
	i32 22, ; 474
	i32 114, ; 475
	i32 57, ; 476
	i32 314, ; 477
	i32 246, ; 478
	i32 142, ; 479
	i32 118, ; 480
	i32 120, ; 481
	i32 110, ; 482
	i32 255, ; 483
	i32 137, ; 484
	i32 261, ; 485
	i32 54, ; 486
	i32 105, ; 487
	i32 362, ; 488
	i32 211, ; 489
	i32 212, ; 490
	i32 133, ; 491
	i32 334, ; 492
	i32 319, ; 493
	i32 307, ; 494
	i32 368, ; 495
	i32 284, ; 496
	i32 225, ; 497
	i32 245, ; 498
	i32 214, ; 499
	i32 157, ; 500
	i32 347, ; 501
	i32 271, ; 502
	i32 161, ; 503
	i32 132, ; 504
	i32 307, ; 505
	i32 171, ; 506
	i32 159, ; 507
	i32 360, ; 508
	i32 295, ; 509
	i32 330, ; 510
	i32 190, ; 511
	i32 138, ; 512
	i32 187, ; 513
	i32 319, ; 514
	i32 315, ; 515
	i32 167, ; 516
	i32 213, ; 517
	i32 174, ; 518
	i32 256, ; 519
	i32 324, ; 520
	i32 40, ; 521
	i32 282, ; 522
	i32 81, ; 523
	i32 56, ; 524
	i32 37, ; 525
	i32 97, ; 526
	i32 164, ; 527
	i32 170, ; 528
	i32 201, ; 529
	i32 225, ; 530
	i32 320, ; 531
	i32 82, ; 532
	i32 258, ; 533
	i32 98, ; 534
	i32 30, ; 535
	i32 157, ; 536
	i32 18, ; 537
	i32 327, ; 538
	i32 127, ; 539
	i32 119, ; 540
	i32 184, ; 541
	i32 278, ; 542
	i32 310, ; 543
	i32 291, ; 544
	i32 312, ; 545
	i32 332, ; 546
	i32 163, ; 547
	i32 286, ; 548
	i32 238, ; 549
	i32 379, ; 550
	i32 309, ; 551
	i32 300, ; 552
	i32 333, ; 553
	i32 168, ; 554
	i32 16, ; 555
	i32 191, ; 556
	i32 142, ; 557
	i32 353, ; 558
	i32 125, ; 559
	i32 118, ; 560
	i32 179, ; 561
	i32 38, ; 562
	i32 179, ; 563
	i32 115, ; 564
	i32 47, ; 565
	i32 140, ; 566
	i32 117, ; 567
	i32 218, ; 568
	i32 246, ; 569
	i32 34, ; 570
	i32 181, ; 571
	i32 95, ; 572
	i32 53, ; 573
	i32 216, ; 574
	i32 301, ; 575
	i32 244, ; 576
	i32 129, ; 577
	i32 151, ; 578
	i32 191, ; 579
	i32 24, ; 580
	i32 159, ; 581
	i32 236, ; 582
	i32 277, ; 583
	i32 146, ; 584
	i32 104, ; 585
	i32 331, ; 586
	i32 89, ; 587
	i32 265, ; 588
	i32 60, ; 589
	i32 140, ; 590
	i32 100, ; 591
	i32 5, ; 592
	i32 13, ; 593
	i32 122, ; 594
	i32 135, ; 595
	i32 28, ; 596
	i32 348, ; 597
	i32 72, ; 598
	i32 275, ; 599
	i32 24, ; 600
	i32 242, ; 601
	i32 219, ; 602
	i32 263, ; 603
	i32 305, ; 604
	i32 302, ; 605
	i32 365, ; 606
	i32 235, ; 607
	i32 256, ; 608
	i32 272, ; 609
	i32 166, ; 610
	i32 306, ; 611
	i32 344, ; 612
	i32 101, ; 613
	i32 249, ; 614
	i32 123, ; 615
	i32 276, ; 616
	i32 182, ; 617
	i32 195, ; 618
	i32 198, ; 619
	i32 161, ; 620
	i32 165, ; 621
	i32 182, ; 622
	i32 279, ; 623
	i32 39, ; 624
	i32 208, ; 625
	i32 352, ; 626
	i32 227, ; 627
	i32 17, ; 628
	i32 226, ; 629
	i32 169, ; 630
	i32 365, ; 631
	i32 364, ; 632
	i32 235, ; 633
	i32 148, ; 634
	i32 268, ; 635
	i32 210, ; 636
	i32 153, ; 637
	i32 130, ; 638
	i32 19, ; 639
	i32 65, ; 640
	i32 145, ; 641
	i32 47, ; 642
	i32 372, ; 643
	i32 254, ; 644
	i32 79, ; 645
	i32 172, ; 646
	i32 61, ; 647
	i32 106, ; 648
	i32 304, ; 649
	i32 258, ; 650
	i32 49, ; 651
	i32 289, ; 652
	i32 369, ; 653
	i32 301, ; 654
	i32 14, ; 655
	i32 194, ; 656
	i32 68, ; 657
	i32 224, ; 658
	i32 169, ; 659
	i32 375, ; 660
	i32 264, ; 661
	i32 268, ; 662
	i32 374, ; 663
	i32 78, ; 664
	i32 273, ; 665
	i32 108, ; 666
	i32 257, ; 667
	i32 197, ; 668
	i32 300, ; 669
	i32 67, ; 670
	i32 63, ; 671
	i32 27, ; 672
	i32 158, ; 673
	i32 196, ; 674
	i32 266, ; 675
	i32 10, ; 676
	i32 183, ; 677
	i32 208, ; 678
	i32 11, ; 679
	i32 233, ; 680
	i32 173, ; 681
	i32 78, ; 682
	i32 299, ; 683
	i32 126, ; 684
	i32 83, ; 685
	i32 199, ; 686
	i32 66, ; 687
	i32 107, ; 688
	i32 65, ; 689
	i32 128, ; 690
	i32 122, ; 691
	i32 77, ; 692
	i32 315, ; 693
	i32 305, ; 694
	i32 373, ; 695
	i32 8, ; 696
	i32 272, ; 697
	i32 2, ; 698
	i32 44, ; 699
	i32 318, ; 700
	i32 154, ; 701
	i32 128, ; 702
	i32 303, ; 703
	i32 23, ; 704
	i32 231, ; 705
	i32 133, ; 706
	i32 260, ; 707
	i32 291, ; 708
	i32 333, ; 709
	i32 368, ; 710
	i32 350, ; 711
	i32 29, ; 712
	i32 183, ; 713
	i32 259, ; 714
	i32 232, ; 715
	i32 180, ; 716
	i32 0, ; 717
	i32 62, ; 718
	i32 211, ; 719
	i32 90, ; 720
	i32 222, ; 721
	i32 87, ; 722
	i32 146, ; 723
	i32 203, ; 724
	i32 213, ; 725
	i32 36, ; 726
	i32 86, ; 727
	i32 280, ; 728
	i32 192, ; 729
	i32 363, ; 730
	i32 358, ; 731
	i32 198, ; 732
	i32 50, ; 733
	i32 6, ; 734
	i32 90, ; 735
	i32 370, ; 736
	i32 21, ; 737
	i32 160, ; 738
	i32 96, ; 739
	i32 50, ; 740
	i32 248, ; 741
	i32 178, ; 742
	i32 113, ; 743
	i32 296, ; 744
	i32 376, ; 745
	i32 130, ; 746
	i32 240, ; 747
	i32 215, ; 748
	i32 76, ; 749
	i32 27, ; 750
	i32 273, ; 751
	i32 295, ; 752
	i32 7, ; 753
	i32 209, ; 754
	i32 176, ; 755
	i32 110, ; 756
	i32 332, ; 757
	i32 296, ; 758
	i32 282 ; 759
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
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
