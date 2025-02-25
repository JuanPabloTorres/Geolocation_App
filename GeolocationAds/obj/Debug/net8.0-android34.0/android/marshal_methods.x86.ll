; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [396 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [786 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 296
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 341
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39109920, ; 6: Newtonsoft.Json.dll => 0x254c520 => 217
	i32 39485524, ; 7: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 8: System.Threading.Thread => 0x28aa24d => 143
	i32 66541672, ; 9: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 10: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 387
	i32 68219467, ; 11: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 12: Microsoft.Maui.Graphics.dll => 0x44bb714 => 214
	i32 82292897, ; 13: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 14: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 315
	i32 117431740, ; 15: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 16: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 315
	i32 122350210, ; 17: System.Threading.Channels.dll => 0x74aea82 => 137
	i32 134690465, ; 18: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 350
	i32 142721839, ; 19: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149764678, ; 20: Svg.Skia.dll => 0x8ed3a46 => 231
	i32 149972175, ; 21: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 22: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 23: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 268
	i32 176265551, ; 24: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 25: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 317
	i32 184328833, ; 26: System.ValueTuple.dll => 0xafca281 => 149
	i32 195452805, ; 27: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 384
	i32 199333315, ; 28: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 385
	i32 205061960, ; 29: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 30: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 266
	i32 220171995, ; 31: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 32: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 289
	i32 230752869, ; 33: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 34: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 35: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 36: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 37: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 271
	i32 266337479, ; 38: Xamarin.Google.Guava.FailureAccess.dll => 0xfdffcc7 => 340
	i32 276479776, ; 39: System.Threading.Timer.dll => 0x107abf20 => 145
	i32 278686392, ; 40: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 292
	i32 280482487, ; 41: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 287
	i32 280992041, ; 42: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 356
	i32 291076382, ; 43: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 293579439, ; 44: ExoPlayer.Dash.dll => 0x117faaaf => 241
	i32 298918909, ; 45: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 46: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 384
	i32 318968648, ; 47: Xamarin.AndroidX.Activity.dll => 0x13031348 => 256
	i32 321597661, ; 48: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 49: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 369
	i32 342366114, ; 50: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 291
	i32 356389973, ; 51: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 368
	i32 360082299, ; 52: System.ServiceModel.Web => 0x15766b7b => 131
	i32 364942007, ; 53: SkiaSharp.Extended.UI => 0x15c092b7 => 221
	i32 367780167, ; 54: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 55: System.Transactions.Local => 0x1658bf94 => 147
	i32 375677976, ; 56: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 57: System.Threading.Thread.dll => 0x16a510e1 => 143
	i32 382590210, ; 58: SkiaSharp.Extended.dll => 0x16cddd02 => 220
	i32 385762202, ; 59: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 60: System.Threading.ThreadPool.dll => 0x1766c1f7 => 144
	i32 395744057, ; 61: _Microsoft.Android.Resource.Designer => 0x17969339 => 392
	i32 403441872, ; 62: WindowsBase => 0x180c08d0 => 163
	i32 435591531, ; 63: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 380
	i32 441335492, ; 64: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 272
	i32 442565967, ; 65: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 66: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 285
	i32 451504562, ; 67: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 452127346, ; 68: ExoPlayer.Database.dll => 0x1af2ea72 => 242
	i32 456227837, ; 69: System.Web.HttpUtility.dll => 0x1b317bfd => 150
	i32 459347974, ; 70: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 463726861, ; 71: Xamarin.Facebook.AppLinks.Android => 0x1ba3e90d => 328
	i32 465658307, ; 72: ExCSS => 0x1bc161c3 => 177
	i32 465846621, ; 73: mscorlib => 0x1bc4415d => 164
	i32 469710990, ; 74: System.dll => 0x1bff388e => 162
	i32 469965489, ; 75: Svg.Model => 0x1c031ab1 => 230
	i32 476646585, ; 76: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 287
	i32 486930444, ; 77: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 302
	i32 490002678, ; 78: Microsoft.AspNetCore.Hosting.Server.Abstractions.dll => 0x1d34d8f6 => 185
	i32 498788369, ; 79: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 80: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 367
	i32 503918385, ; 81: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 361
	i32 504833739, ; 82: SkiaSharp.SceneGraph => 0x1e1726cb => 223
	i32 513247710, ; 83: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 207
	i32 517857212, ; 84: Xamarin.Facebook.Common.Android => 0x1edddfbc => 329
	i32 525008092, ; 85: SkiaSharp.dll => 0x1f4afcdc => 219
	i32 526420162, ; 86: System.Transactions.dll => 0x1f6088c2 => 148
	i32 527452488, ; 87: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 350
	i32 530272170, ; 88: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 89: Microsoft.Extensions.Logging => 0x20216150 => 204
	i32 540030774, ; 90: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 91: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 92: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 93: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 94: Jsr305Binding => 0x213954e7 => 336
	i32 569601784, ; 95: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 326
	i32 577335427, ; 96: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 586578074, ; 97: MimeKit => 0x22f6789a => 216
	i32 592146354, ; 98: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 375
	i32 597488923, ; 99: CommunityToolkit.Maui => 0x239cf51b => 173
	i32 601371474, ; 100: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 101: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 102: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 624688707, ; 103: Xamarin.Facebook.Messenger.Android => 0x253bfe43 => 333
	i32 626887733, ; 104: ExoPlayer.Container => 0x255d8c35 => 239
	i32 627609679, ; 105: Xamarin.AndroidX.CustomView => 0x2568904f => 277
	i32 627931235, ; 106: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 373
	i32 639843206, ; 107: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 283
	i32 643868501, ; 108: System.Net => 0x2660a755 => 81
	i32 662205335, ; 109: System.Text.Encodings.Web.dll => 0x27787397 => 234
	i32 663517072, ; 110: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 322
	i32 666292255, ; 111: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 263
	i32 672442732, ; 112: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 113: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 114: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 355
	i32 690569205, ; 115: System.Xml.Linq.dll => 0x29293ff5 => 153
	i32 690602616, ; 116: SkiaSharp.Skottie.dll => 0x2929c278 => 224
	i32 691348768, ; 117: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 352
	i32 693804605, ; 118: System.Windows => 0x295a9e3d => 152
	i32 699345723, ; 119: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 120: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 347
	i32 700358131, ; 121: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 122: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 370
	i32 709152836, ; 123: System.Security.Cryptography.Pkcs.dll => 0x2a44d044 => 233
	i32 709557578, ; 124: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 358
	i32 720511267, ; 125: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 351
	i32 722857257, ; 126: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 127: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 738469988, ; 128: SkiaSharp.SceneGraph.dll => 0x2c042864 => 223
	i32 752232764, ; 129: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 130: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 253
	i32 759454413, ; 131: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 132: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 133: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 134: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 379
	i32 778756650, ; 135: SkiaSharp.HarfBuzz.dll => 0x2e6ae22a => 222
	i32 778804420, ; 136: SkiaSharp.Extended.UI.dll => 0x2e6b9cc4 => 221
	i32 789151979, ; 137: Microsoft.Extensions.Options => 0x2f0980eb => 206
	i32 790371945, ; 138: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 278
	i32 804715423, ; 139: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 140: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 294
	i32 812693636, ; 141: ExoPlayer.Dash => 0x3070b884 => 241
	i32 823281589, ; 142: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 143: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 144: System.Xml.XPath.dll => 0x31a103c6 => 158
	i32 834051424, ; 145: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 146: Xamarin.AndroidX.Print => 0x3246f6cd => 308
	i32 873119928, ; 147: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 148: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 149: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 150: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 908888060, ; 151: Microsoft.Maui.Maps => 0x362c87fc => 215
	i32 911108515, ; 152: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 921035005, ; 153: ToolsLibrary => 0x36e5e0fd => 391
	i32 926902833, ; 154: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 382
	i32 928116545, ; 155: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 341
	i32 939704684, ; 156: ExoPlayer.Extractor => 0x3802c16c => 245
	i32 944435932, ; 157: Xabe.FFmpeg.dll => 0x384af2dc => 236
	i32 952186615, ; 158: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 159: Newtonsoft.Json => 0x38f24a24 => 217
	i32 956575887, ; 160: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 351
	i32 966729478, ; 161: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 337
	i32 967690846, ; 162: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 291
	i32 975236339, ; 163: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 164: System.Xml.XDocument => 0x3a2aaa1d => 156
	i32 986514023, ; 165: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 166: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 167: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 168: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 999186168, ; 169: Microsoft.Extensions.FileSystemGlobbing.dll => 0x3b8e5ef8 => 202
	i32 999372639, ; 170: Xamarin.Facebook.Core.Android => 0x3b91375f => 330
	i32 1001831731, ; 171: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 172: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 312
	i32 1019214401, ; 173: System.Drawing => 0x3cbffa41 => 36
	i32 1028013380, ; 174: ExoPlayer.UI.dll => 0x3d463d44 => 250
	i32 1028951442, ; 175: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 199
	i32 1029334545, ; 176: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 357
	i32 1031528504, ; 177: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 338
	i32 1035644815, ; 178: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 261
	i32 1036536393, ; 179: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 180: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 181: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 298
	i32 1067306892, ; 182: GoogleGson => 0x3f9dcf8c => 181
	i32 1082857460, ; 183: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 184: Xamarin.Kotlin.StdLib => 0x409e66d8 => 348
	i32 1098259244, ; 185: System => 0x41761b2c => 162
	i32 1106973742, ; 186: Microsoft.Extensions.Configuration.FileExtensions.dll => 0x41fb142e => 196
	i32 1110309514, ; 187: Microsoft.Extensions.Hosting.Abstractions => 0x422dfa8a => 203
	i32 1118262833, ; 188: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 370
	i32 1121599056, ; 189: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 297
	i32 1149092582, ; 190: Xamarin.AndroidX.Window => 0x447dc2e6 => 325
	i32 1151313727, ; 191: ExoPlayer.Rtsp => 0x449fa73f => 247
	i32 1157931901, ; 192: Microsoft.EntityFrameworkCore.Abstractions => 0x4504a37d => 189
	i32 1168523401, ; 193: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 376
	i32 1170634674, ; 194: System.Web.dll => 0x45c677b2 => 151
	i32 1173126369, ; 195: Microsoft.Extensions.FileProviders.Abstractions.dll => 0x45ec7ce1 => 200
	i32 1175144683, ; 196: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 321
	i32 1178241025, ; 197: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 306
	i32 1202000627, ; 198: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x47a512f3 => 189
	i32 1203215381, ; 199: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 374
	i32 1204270330, ; 200: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 263
	i32 1204575371, ; 201: Microsoft.Extensions.Caching.Memory.dll => 0x47cc5c8b => 192
	i32 1208641965, ; 202: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 203: CommunityToolkit.Mvvm => 0x4868cc7b => 176
	i32 1219128291, ; 204: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1234928153, ; 205: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 372
	i32 1236289705, ; 206: Microsoft.AspNetCore.Hosting.Server.Abstractions => 0x49b048a9 => 185
	i32 1243150071, ; 207: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 326
	i32 1253011324, ; 208: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 209: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 356
	i32 1263886435, ; 210: Xamarin.Google.Guava.dll => 0x4b556063 => 339
	i32 1264511973, ; 211: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 316
	i32 1267360935, ; 212: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 320
	i32 1273260888, ; 213: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 269
	i32 1275534314, ; 214: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 352
	i32 1278448581, ; 215: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 260
	i32 1293217323, ; 216: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 280
	i32 1309188875, ; 217: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1309209905, ; 218: ExoPlayer.DataSource => 0x4e08f531 => 243
	i32 1322716291, ; 219: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 325
	i32 1324164729, ; 220: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 221: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 222: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 223: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 386
	i32 1376866003, ; 224: Xamarin.AndroidX.SavedState => 0x52114ed3 => 312
	i32 1379779777, ; 225: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1395857551, ; 226: Xamarin.AndroidX.Media.dll => 0x5333188f => 303
	i32 1402170036, ; 227: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 228: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 273
	i32 1406299041, ; 229: Xamarin.Google.Guava.FailureAccess => 0x53d26ba1 => 340
	i32 1408764838, ; 230: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 231: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 232: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 233: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 354
	i32 1434145427, ; 234: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 235: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 337
	i32 1439761251, ; 236: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 237: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 238: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 239: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 240: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 241: es\Microsoft.Maui.Controls.resources => 0x57152abe => 360
	i32 1461234159, ; 242: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 243: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 244: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 245: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 262
	i32 1470490898, ; 246: Microsoft.Extensions.Primitives => 0x57a5e912 => 207
	i32 1479771757, ; 247: System.Collections.Immutable => 0x5833866d => 9
	i32 1480156764, ; 248: ExoPlayer.DataSource.dll => 0x5839665c => 243
	i32 1480492111, ; 249: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 250: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 251: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 313
	i32 1493001747, ; 252: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 364
	i32 1514721132, ; 253: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 359
	i32 1521091094, ; 254: Microsoft.Extensions.FileSystemGlobbing => 0x5aaa0216 => 202
	i32 1536373174, ; 255: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 256: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 136
	i32 1543355203, ; 257: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 258: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 259: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 379
	i32 1565862583, ; 260: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 261: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 139
	i32 1573704789, ; 262: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 263: System.Threading.Overlapped => 0x5e2d7514 => 138
	i32 1582372066, ; 264: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 279
	i32 1592978981, ; 265: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 266: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 338
	i32 1600541741, ; 267: ShimSkiaSharp => 0x5f66542d => 218
	i32 1601112923, ; 268: System.Xml.Serialization => 0x5f6f0b5b => 155
	i32 1603525486, ; 269: Microsoft.Maui.Controls.HotReload.Forms.dll => 0x5f93db6e => 388
	i32 1604827217, ; 270: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 271: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 272: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 301
	i32 1622358360, ; 273: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1623212457, ; 274: SkiaSharp.Views.Maui.Controls => 0x60c041a9 => 226
	i32 1624863272, ; 275: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 324
	i32 1632842087, ; 276: Microsoft.Extensions.Configuration.Json => 0x61533167 => 197
	i32 1634654947, ; 277: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 174
	i32 1635184631, ; 278: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 283
	i32 1636350590, ; 279: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 276
	i32 1638652436, ; 280: CommunityToolkit.Maui.MediaElement => 0x61abda14 => 175
	i32 1639515021, ; 281: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 282: System.Text.RegularExpressions => 0x61c036ca => 136
	i32 1641389582, ; 283: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 284: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 285: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 318
	i32 1658251792, ; 286: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 335
	i32 1670060433, ; 287: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 271
	i32 1675553242, ; 288: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 289: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 290: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 291: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1689493916, ; 292: Microsoft.EntityFrameworkCore.dll => 0x64b3a19c => 188
	i32 1691477237, ; 293: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 294: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 295: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 349
	i32 1700397376, ; 296: ExoPlayer.Transformer => 0x655a0140 => 249
	i32 1701541528, ; 297: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1720223769, ; 298: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 294
	i32 1724472758, ; 299: SkiaSharp.Extended => 0x66c95db6 => 220
	i32 1726116996, ; 300: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 301: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 302: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 267
	i32 1736233607, ; 303: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 377
	i32 1743415430, ; 304: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 355
	i32 1744735666, ; 305: System.Transactions.Local.dll => 0x67fe8db2 => 147
	i32 1746115085, ; 306: System.IO.Pipelines.dll => 0x68139a0d => 232
	i32 1746316138, ; 307: Mono.Android.Export => 0x6816ab6a => 167
	i32 1750313021, ; 308: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1751678353, ; 309: ToolsLibrary.dll => 0x68687d91 => 391
	i32 1758240030, ; 310: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 311: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765620304, ; 312: ExoPlayer.Core => 0x693d3a50 => 240
	i32 1765942094, ; 313: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 314: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 317
	i32 1770582343, ; 315: Microsoft.Extensions.Logging.dll => 0x6988f147 => 204
	i32 1776026572, ; 316: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 317: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 318: Mono.Android.Runtime.dll => 0x6a216153 => 168
	i32 1782862114, ; 319: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 371
	i32 1788241197, ; 320: Xamarin.AndroidX.Fragment => 0x6a96652d => 285
	i32 1793755602, ; 321: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 363
	i32 1808609942, ; 322: Xamarin.AndroidX.Loader => 0x6bcd3296 => 301
	i32 1813058853, ; 323: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 348
	i32 1813201214, ; 324: Xamarin.Google.Android.Material => 0x6c13413e => 335
	i32 1818569960, ; 325: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 307
	i32 1818787751, ; 326: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1819327070, ; 327: Microsoft.AspNetCore.Http.Features.dll => 0x6c70ba5e => 187
	i32 1824175904, ; 328: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 329: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827303595, ; 330: Microsoft.VisualStudio.DesignTools.TapContract => 0x6cea70ab => 390
	i32 1828688058, ; 331: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 205
	i32 1842015223, ; 332: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 383
	i32 1847515442, ; 333: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 253
	i32 1853025655, ; 334: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 380
	i32 1858542181, ; 335: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1866095656, ; 336: Xamarin.Android.Binding.InstallReferrer => 0x6f3a5c28 => 251
	i32 1870277092, ; 337: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 338: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 362
	i32 1879696579, ; 339: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 340: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 264
	i32 1885918049, ; 341: Microsoft.VisualStudio.DesignTools.TapContract.dll => 0x7068d361 => 390
	i32 1888955245, ; 342: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 343: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 344: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 345: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1908813208, ; 346: Xamarin.GooglePlayServices.Basement => 0x71c62d98 => 344
	i32 1910275211, ; 347: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1926145099, ; 348: ExoPlayer.Container.dll => 0x72cea44b => 239
	i32 1928288591, ; 349: Microsoft.AspNetCore.Http.Abstractions => 0x72ef594f => 186
	i32 1939592360, ; 350: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 351: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 352: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 314
	i32 1968388702, ; 353: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 193
	i32 1983156543, ; 354: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 349
	i32 1985761444, ; 355: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 255
	i32 2003115576, ; 356: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 359
	i32 2011961780, ; 357: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 358: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 298
	i32 2025202353, ; 359: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 354
	i32 2027186647, ; 360: Xamarin.Facebook.Login.Android => 0x78d469d7 => 332
	i32 2031763787, ; 361: Xamarin.Android.Glide => 0x791a414b => 252
	i32 2036392541, ; 362: Xamarin.Facebook.Android => 0x7960e25d => 327
	i32 2045470958, ; 363: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 364: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 195
	i32 2051717622, ; 365: Google.ZXing.Core.dll => 0x7a4ab9f6 => 342
	i32 2055257422, ; 366: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 293
	i32 2060060697, ; 367: System.Windows.dll => 0x7aca0819 => 152
	i32 2066184531, ; 368: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 358
	i32 2067982161, ; 369: Xamarin.Facebook.Common.Android.dll => 0x7b42e751 => 329
	i32 2070888862, ; 370: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2072397586, ; 371: Microsoft.Extensions.FileProviders.Physical => 0x7b864712 => 201
	i32 2075706075, ; 372: Microsoft.AspNetCore.Http.Abstractions.dll => 0x7bb8c2db => 186
	i32 2079903147, ; 373: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 374: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2097448633, ; 375: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 288
	i32 2100927423, ; 376: Xamarin.Android.Binding.InstallReferrer.dll => 0x7d399bbf => 251
	i32 2106312818, ; 377: ExoPlayer.Decoder => 0x7d8bc872 => 244
	i32 2113912252, ; 378: ExoPlayer.UI => 0x7dffbdbc => 250
	i32 2127167465, ; 379: System.Console => 0x7ec9ffe9 => 20
	i32 2129483829, ; 380: Xamarin.GooglePlayServices.Base.dll => 0x7eed5835 => 343
	i32 2142473426, ; 381: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 382: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 160
	i32 2146852085, ; 383: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 384: Microsoft.Maui => 0x80bd55ad => 212
	i32 2169148018, ; 385: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 366
	i32 2181898931, ; 386: Microsoft.Extensions.Options.dll => 0x820d22b3 => 206
	i32 2192057212, ; 387: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 205
	i32 2193016926, ; 388: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 389: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 353
	i32 2201231467, ; 390: System.Net.Http => 0x8334206b => 64
	i32 2201706308, ; 391: Xamarin.Facebook.Messenger.Android.dll => 0x833b5f44 => 333
	i32 2202964214, ; 392: ExoPlayer.dll => 0x834e90f6 => 237
	i32 2207618523, ; 393: it\Microsoft.Maui.Controls.resources => 0x839595db => 368
	i32 2216717168, ; 394: Firebase.Auth.dll => 0x84206b70 => 180
	i32 2217644978, ; 395: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 321
	i32 2222056684, ; 396: System.Threading.Tasks.Parallel => 0x8471e4ec => 141
	i32 2239138732, ; 397: ExoPlayer.SmoothStreaming => 0x85768bac => 248
	i32 2244775296, ; 398: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 302
	i32 2252106437, ; 399: System.Xml.Serialization.dll => 0x863c6ac5 => 155
	i32 2252897993, ; 400: Microsoft.EntityFrameworkCore => 0x86487ec9 => 188
	i32 2256313426, ; 401: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2261435625, ; 402: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 290
	i32 2265110946, ; 403: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 404: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 194
	i32 2267999099, ; 405: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 254
	i32 2270573516, ; 406: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 362
	i32 2279755925, ; 407: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 310
	i32 2286792735, ; 408: Xamarin.Facebook.AppLinks.Android.dll => 0x884db01f => 328
	i32 2293034957, ; 409: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 410: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 411: System.Net.Mail => 0x88ffe49e => 66
	i32 2303073227, ; 412: Microsoft.Maui.Controls.Maps.dll => 0x89461bcb => 210
	i32 2303942373, ; 413: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 372
	i32 2305521784, ; 414: System.Private.CoreLib.dll => 0x896b7878 => 170
	i32 2315684594, ; 415: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 258
	i32 2320631194, ; 416: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 141
	i32 2327893114, ; 417: ExCSS.dll => 0x8ac0d47a => 177
	i32 2340441535, ; 418: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 419: System.ValueTuple => 0x8bbaa2cd => 149
	i32 2353062107, ; 420: System.Net.Primitives => 0x8c40e0db => 70
	i32 2354643623, ; 421: Xamarin.Facebook.Share.Android.dll => 0x8c5902a7 => 334
	i32 2364201794, ; 422: SkiaSharp.Views.Maui.Core => 0x8ceadb42 => 228
	i32 2368005991, ; 423: System.Xml.ReaderWriter.dll => 0x8d24e767 => 154
	i32 2371007202, ; 424: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 193
	i32 2378619854, ; 425: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 426: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 427: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 367
	i32 2401565422, ; 428: System.Web.HttpUtility => 0x8f24faee => 150
	i32 2403452196, ; 429: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 282
	i32 2409983638, ; 430: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 0x8fa56e96 => 389
	i32 2421380589, ; 431: System.Threading.Tasks.Dataflow => 0x905355ed => 139
	i32 2423080555, ; 432: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 269
	i32 2427813419, ; 433: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 364
	i32 2435356389, ; 434: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 435: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2437192331, ; 436: CommunityToolkit.Maui.MediaElement.dll => 0x91449a8b => 175
	i32 2454642406, ; 437: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 438: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 439: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465532216, ; 440: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 272
	i32 2466355063, ; 441: FFImageLoading.Maui.dll => 0x93019777 => 178
	i32 2471841756, ; 442: netstandard.dll => 0x93554fdc => 165
	i32 2475788418, ; 443: Java.Interop.dll => 0x93918882 => 166
	i32 2476233210, ; 444: ExoPlayer => 0x939851fa => 237
	i32 2480646305, ; 445: Microsoft.Maui.Controls => 0x93dba8a1 => 209
	i32 2483903535, ; 446: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 447: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 448: System.AppContext.dll => 0x94798bc5 => 6
	i32 2498657740, ; 449: BouncyCastle.Cryptography.dll => 0x94ee7dcc => 172
	i32 2501346920, ; 450: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 451: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 296
	i32 2508463432, ; 452: BCrypt.Net-Core => 0x95841d48 => 171
	i32 2515854816, ; 453: ExoPlayer.Common => 0x95f4e5e0 => 238
	i32 2521915375, ; 454: SkiaSharp.Views.Maui.Controls.Compatibility => 0x96515fef => 227
	i32 2522472828, ; 455: Xamarin.Android.Glide.dll => 0x9659e17c => 252
	i32 2523023297, ; 456: Svg.Custom.dll => 0x966247c1 => 229
	i32 2538310050, ; 457: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 458: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 365
	i32 2562349572, ; 459: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 460: System.Text.Encodings.Web => 0x9930ee42 => 234
	i32 2581783588, ; 461: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 297
	i32 2581819634, ; 462: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 320
	i32 2585220780, ; 463: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 464: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 465: System.Threading.ThreadPool => 0x9a5a3337 => 144
	i32 2592341985, ; 466: Microsoft.Extensions.FileProviders.Abstractions => 0x9a83ffe1 => 200
	i32 2593496499, ; 467: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 374
	i32 2594125473, ; 468: Microsoft.AspNetCore.Hosting.Abstractions => 0x9a9f36a1 => 184
	i32 2602257211, ; 469: Svg.Model.dll => 0x9b1b4b3b => 230
	i32 2605712449, ; 470: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 353
	i32 2609324236, ; 471: Svg.Custom => 0x9b8720cc => 229
	i32 2615233544, ; 472: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 286
	i32 2617129537, ; 473: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 474: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 475: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 276
	i32 2621070698, ; 476: GeolocationAds => 0x9c3a5d6a => 0
	i32 2624644809, ; 477: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 281
	i32 2625339995, ; 478: SkiaSharp.Views.Maui.Core.dll => 0x9c7b825b => 228
	i32 2626028643, ; 479: ExoPlayer.Rtsp.dll => 0x9c860463 => 247
	i32 2626831493, ; 480: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 369
	i32 2627185994, ; 481: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 482: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 483: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 292
	i32 2634653062, ; 484: Microsoft.EntityFrameworkCore.Relational.dll => 0x9d099d86 => 190
	i32 2663391936, ; 485: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 254
	i32 2663698177, ; 486: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 487: System.Xml.XDocument.dll => 0x9ecf752a => 156
	i32 2665622720, ; 488: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 489: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686248867, ; 490: Xamarin.Facebook.Core.Android.dll => 0xa01ce7a3 => 330
	i32 2686887180, ; 491: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 492: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 493: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 318
	i32 2713040075, ; 494: ExoPlayer.Hls => 0xa1b5b4cb => 246
	i32 2715334215, ; 495: System.Threading.Tasks.dll => 0xa1d8b647 => 142
	i32 2717744543, ; 496: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 497: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 498: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 499: Xamarin.AndroidX.Activity => 0xa2e0939b => 256
	i32 2735172069, ; 500: System.Threading.Channels => 0xa30769e5 => 137
	i32 2737747696, ; 501: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 262
	i32 2740948882, ; 502: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 503: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 504: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 375
	i32 2758225723, ; 505: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 211
	i32 2764765095, ; 506: Microsoft.Maui.dll => 0xa4caf7a7 => 212
	i32 2765824710, ; 507: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 508: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 347
	i32 2778768386, ; 509: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 323
	i32 2779977773, ; 510: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 311
	i32 2785988530, ; 511: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 381
	i32 2788224221, ; 512: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 286
	i32 2795602088, ; 513: SkiaSharp.Views.Android.dll => 0xa6a180a8 => 225
	i32 2796087574, ; 514: ExoPlayer.Extractor.dll => 0xa6a8e916 => 245
	i32 2801831435, ; 515: Microsoft.Maui.Graphics => 0xa7008e0b => 214
	i32 2803228030, ; 516: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 157
	i32 2806116107, ; 517: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 360
	i32 2810250172, ; 518: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 273
	i32 2819470561, ; 519: System.Xml.dll => 0xa80db4e1 => 161
	i32 2821205001, ; 520: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 521: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 311
	i32 2822463729, ; 522: BCrypt.Net-Core.dll => 0xa83b60f1 => 171
	i32 2824502124, ; 523: System.Xml.XmlDocument => 0xa85a7b6c => 159
	i32 2831556043, ; 524: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 373
	i32 2838993487, ; 525: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 299
	i32 2847418871, ; 526: Xamarin.GooglePlayServices.Base => 0xa9b829f7 => 343
	i32 2847789619, ; 527: Microsoft.EntityFrameworkCore.Relational => 0xa9bdd233 => 190
	i32 2849599387, ; 528: System.Threading.Overlapped.dll => 0xa9d96f9b => 138
	i32 2850549256, ; 529: Microsoft.AspNetCore.Http.Features => 0xa9e7ee08 => 187
	i32 2853208004, ; 530: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 323
	i32 2855708567, ; 531: Xamarin.AndroidX.Transition => 0xaa36a797 => 319
	i32 2861098320, ; 532: Mono.Android.Export.dll => 0xaa88e550 => 167
	i32 2861189240, ; 533: Microsoft.Maui.Essentials => 0xaa8a4878 => 213
	i32 2868488919, ; 534: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 174
	i32 2870099610, ; 535: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 257
	i32 2875164099, ; 536: Jsr305Binding.dll => 0xab5f85c3 => 336
	i32 2875220617, ; 537: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 538: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 284
	i32 2887636118, ; 539: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 540: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 541: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 542: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 543: mscorlib.dll => 0xad2a79b6 => 164
	i32 2909740682, ; 544: System.Private.CoreLib => 0xad6f1e8a => 170
	i32 2911054922, ; 545: Microsoft.Extensions.FileProviders.Physical.dll => 0xad832c4a => 201
	i32 2912489636, ; 546: SkiaSharp.Views.Android => 0xad9910a4 => 225
	i32 2916838712, ; 547: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 324
	i32 2919462931, ; 548: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 549: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 259
	i32 2933534917, ; 550: Xamarin.Facebook.GamingServices.Android.dll => 0xaeda30c5 => 331
	i32 2936416060, ; 551: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 552: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 553: System.Xml.XPath.XDocument => 0xaf624531 => 157
	i32 2959614098, ; 554: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2960379616, ; 555: Xamarin.Google.Guava => 0xb073cee0 => 339
	i32 2968338931, ; 556: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 557: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978368250, ; 558: Microsoft.AspNetCore.Hosting.Abstractions.dll => 0xb1864afa => 184
	i32 2978675010, ; 559: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 280
	i32 2987532451, ; 560: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 314
	i32 2996846495, ; 561: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 295
	i32 3016983068, ; 562: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 316
	i32 3017076677, ; 563: Xamarin.GooglePlayServices.Maps => 0xb3d4efc5 => 345
	i32 3023353419, ; 564: WindowsBase.dll => 0xb434b64b => 163
	i32 3024354802, ; 565: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 289
	i32 3027462113, ; 566: ExoPlayer.Common.dll => 0xb47367e1 => 238
	i32 3038032645, ; 567: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 392
	i32 3056245963, ; 568: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 313
	i32 3057625584, ; 569: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 304
	i32 3058099980, ; 570: Xamarin.GooglePlayServices.Tasks => 0xb646e70c => 346
	i32 3059408633, ; 571: Mono.Android.Runtime => 0xb65adef9 => 168
	i32 3059793426, ; 572: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3069363400, ; 573: Microsoft.Extensions.Caching.Abstractions.dll => 0xb6f2c4c8 => 191
	i32 3075834255, ; 574: System.Threading.Tasks => 0xb755818f => 142
	i32 3077302341, ; 575: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 366
	i32 3090735792, ; 576: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 577: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103574161, ; 578: FFmpeg.AutoGen.dll => 0xb8fcc891 => 179
	i32 3103600923, ; 579: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3104590590, ; 580: FFmpeg.AutoGen => 0xb90c4afe => 179
	i32 3111772706, ; 581: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 582: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 583: System.Threading.Tasks.Extensions => 0xba4127cb => 140
	i32 3132293585, ; 584: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3134694676, ; 585: ShimSkiaSharp.dll => 0xbad7a514 => 218
	i32 3144327419, ; 586: ExoPlayer.Hls.dll => 0xbb6aa0fb => 246
	i32 3147165239, ; 587: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 588: GoogleGson.dll => 0xbba64c02 => 181
	i32 3159123045, ; 589: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 590: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3168458674, ; 591: Xamarin.Facebook.Login.Android.dll => 0xbcdad7b2 => 332
	i32 3171180504, ; 592: MimeKit.dll => 0xbd045fd8 => 216
	i32 3178803400, ; 593: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 305
	i32 3182640895, ; 594: Xamarin.Facebook.Android.dll => 0xbdb33eff => 327
	i32 3190271366, ; 595: ExoPlayer.Decoder.dll => 0xbe27ad86 => 244
	i32 3192346100, ; 596: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 597: System.Web => 0xbe592c0c => 151
	i32 3195844289, ; 598: Microsoft.Extensions.Caching.Abstractions => 0xbe7cb6c1 => 191
	i32 3204380047, ; 599: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 600: System.Xml.XmlDocument.dll => 0xbf506931 => 159
	i32 3210765148, ; 601: Xabe.FFmpeg => 0xbf60635c => 236
	i32 3211777861, ; 602: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 279
	i32 3220365878, ; 603: System.Threading => 0xbff2e236 => 146
	i32 3226221578, ; 604: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3230466174, ; 605: Xamarin.GooglePlayServices.Basement.dll => 0xc08d007e => 344
	i32 3251039220, ; 606: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 607: Xamarin.AndroidX.CardView => 0xc235e84d => 267
	i32 3265493905, ; 608: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 609: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 140
	i32 3267021929, ; 610: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 265
	i32 3277815716, ; 611: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 612: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 613: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 614: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3291879728, ; 615: Xamarin.Facebook.Share.Android => 0xc4361930 => 334
	i32 3299363146, ; 616: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 617: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 618: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 361
	i32 3316684772, ; 619: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 620: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 277
	i32 3317144872, ; 621: System.Data => 0xc5b79d28 => 24
	i32 3329734229, ; 622: ExoPlayer.Database => 0xc677b655 => 242
	i32 3340387945, ; 623: SkiaSharp => 0xc71a4669 => 219
	i32 3340431453, ; 624: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 264
	i32 3345895724, ; 625: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 309
	i32 3346324047, ; 626: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 306
	i32 3353484488, ; 627: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 288
	i32 3357674450, ; 628: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 378
	i32 3358260929, ; 629: System.Text.Json => 0xc82afec1 => 235
	i32 3362336904, ; 630: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 257
	i32 3362522851, ; 631: Xamarin.AndroidX.Core => 0xc86c06e3 => 274
	i32 3366347497, ; 632: Java.Interop => 0xc8a662e9 => 166
	i32 3374999561, ; 633: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 310
	i32 3381016424, ; 634: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 357
	i32 3395150330, ; 635: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3396979385, ; 636: ExoPlayer.Transformer.dll => 0xca79cab9 => 249
	i32 3403906625, ; 637: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 638: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 278
	i32 3413343012, ; 639: GoogleMapsApi => 0xcb737b24 => 182
	i32 3421170118, ; 640: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 195
	i32 3428513518, ; 641: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 198
	i32 3429136800, ; 642: System.Xml => 0xcc6479a0 => 161
	i32 3430777524, ; 643: netstandard => 0xcc7d82b4 => 165
	i32 3439098628, ; 644: GoogleMapsApi.dll => 0xccfc7b04 => 182
	i32 3441283291, ; 645: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 281
	i32 3445260447, ; 646: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 647: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 208
	i32 3463511458, ; 648: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 365
	i32 3466574376, ; 649: SkiaSharp.Views.Maui.Controls.Compatibility.dll => 0xce9fba28 => 227
	i32 3471940407, ; 650: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3473156932, ; 651: SkiaSharp.Views.Maui.Controls.dll => 0xcf042b44 => 226
	i32 3476120550, ; 652: Mono.Android => 0xcf3163e6 => 169
	i32 3479583265, ; 653: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 378
	i32 3484440000, ; 654: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 377
	i32 3485117614, ; 655: System.Text.Json.dll => 0xcfbaacae => 235
	i32 3486566296, ; 656: System.Transactions => 0xcfd0c798 => 148
	i32 3493954962, ; 657: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 270
	i32 3500773090, ; 658: Microsoft.Maui.Controls.Maps => 0xd0a98ee2 => 210
	i32 3501239056, ; 659: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 265
	i32 3509114376, ; 660: System.Xml.Linq => 0xd128d608 => 153
	i32 3515174580, ; 661: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 662: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 663: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 664: System.Threading.Timer => 0xd432d20b => 145
	i32 3570554715, ; 665: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 666: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 385
	i32 3586228282, ; 667: Xamarin.Facebook.GamingServices.Android => 0xd5c1803a => 331
	i32 3597029428, ; 668: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 255
	i32 3598340787, ; 669: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3605570793, ; 670: BouncyCastle.Cryptography => 0xd6e8a4e9 => 172
	i32 3608519521, ; 671: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 672: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 673: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 308
	i32 3633644679, ; 674: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 259
	i32 3638274909, ; 675: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 676: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 293
	i32 3643446276, ; 677: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 382
	i32 3643854240, ; 678: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 305
	i32 3645089577, ; 679: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 680: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 194
	i32 3660523487, ; 681: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3663323240, ; 682: SkiaSharp.Skottie => 0xda59e068 => 224
	i32 3672681054, ; 683: Mono.Android.dll => 0xdae8aa5e => 169
	i32 3676670898, ; 684: Microsoft.Maui.Controls.HotReload.Forms => 0xdb258bb2 => 388
	i32 3682565725, ; 685: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 266
	i32 3684561358, ; 686: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 270
	i32 3697841164, ; 687: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 387
	i32 3700866549, ; 688: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 689: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 275
	i32 3716563718, ; 690: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 691: Xamarin.AndroidX.Annotation => 0xdda814c6 => 258
	i32 3722202641, ; 692: Microsoft.Extensions.Configuration.Json.dll => 0xdddc4e11 => 197
	i32 3724971120, ; 693: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 304
	i32 3732100267, ; 694: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 695: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 696: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 697: System.Xml.XPath => 0xdf9a7f42 => 158
	i32 3758424670, ; 698: Microsoft.Extensions.Configuration.FileExtensions => 0xe005025e => 196
	i32 3758932259, ; 699: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 290
	i32 3786282454, ; 700: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 268
	i32 3792276235, ; 701: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3792835768, ; 702: HarfBuzzSharp => 0xe21214b8 => 183
	i32 3800979733, ; 703: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 208
	i32 3802395368, ; 704: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3807198597, ; 705: System.Security.Cryptography.Pkcs => 0xe2ed3d85 => 233
	i32 3817368567, ; 706: CommunityToolkit.Maui.dll => 0xe3886bf7 => 173
	i32 3819260425, ; 707: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3822602673, ; 708: Xamarin.AndroidX.Media => 0xe3d849b1 => 303
	i32 3823082795, ; 709: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 710: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 711: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 199
	i32 3844307129, ; 712: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 713: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 714: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 715: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 716: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 717: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 718: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 319
	i32 3888767677, ; 719: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 309
	i32 3889960447, ; 720: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 386
	i32 3896106733, ; 721: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 722: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 274
	i32 3901907137, ; 723: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 724: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 725: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 322
	i32 3928044579, ; 726: System.Xml.ReaderWriter => 0xea213423 => 154
	i32 3930554604, ; 727: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 728: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 307
	i32 3945713374, ; 729: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953583589, ; 730: Svg.Skia => 0xeba6e5e5 => 231
	i32 3953953790, ; 731: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 732: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 261
	i32 3959773229, ; 733: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 295
	i32 3970018735, ; 734: Xamarin.GooglePlayServices.Tasks.dll => 0xeca1adaf => 346
	i32 3980434154, ; 735: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 381
	i32 3987592930, ; 736: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 363
	i32 4000318121, ; 737: Google.ZXing.Core => 0xee7002a9 => 342
	i32 4003436829, ; 738: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4003906742, ; 739: HarfBuzzSharp.dll => 0xeea6c4b6 => 183
	i32 4015948917, ; 740: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 260
	i32 4023392905, ; 741: System.IO.Pipelines => 0xefd01a89 => 232
	i32 4024013275, ; 742: Firebase.Auth => 0xefd991db => 180
	i32 4024771894, ; 743: GeolocationAds.dll => 0xefe52536 => 0
	i32 4025784931, ; 744: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 745: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 211
	i32 4054681211, ; 746: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4066802364, ; 747: SkiaSharp.HarfBuzz => 0xf2667abc => 222
	i32 4068434129, ; 748: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 749: System.Threading.dll => 0xf2ce3c98 => 146
	i32 4078967171, ; 750: Microsoft.Extensions.Hosting.Abstractions.dll => 0xf3201983 => 203
	i32 4094352644, ; 751: Microsoft.Maui.Essentials.dll => 0xf40add04 => 213
	i32 4099507663, ; 752: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 753: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 754: Xamarin.AndroidX.Emoji2 => 0xf479582c => 282
	i32 4101842092, ; 755: Microsoft.Extensions.Caching.Memory => 0xf47d24ac => 192
	i32 4102112229, ; 756: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 376
	i32 4125707920, ; 757: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 371
	i32 4126470640, ; 758: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 198
	i32 4127667938, ; 759: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 760: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 761: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 762: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 383
	i32 4151237749, ; 763: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 764: System.Xml.XmlSerializer => 0xf7e95c85 => 160
	i32 4161255271, ; 765: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 766: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4173364138, ; 767: ExoPlayer.SmoothStreaming.dll => 0xf8c07baa => 248
	i32 4173862379, ; 768: FFImageLoading.Maui => 0xf8c815eb => 178
	i32 4181436372, ; 769: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 770: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 300
	i32 4182880526, ; 771: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 0xf951b10e => 389
	i32 4185676441, ; 772: System.Security => 0xf97c5a99 => 130
	i32 4190597220, ; 773: ExoPlayer.Core.dll => 0xf9c77064 => 240
	i32 4190991637, ; 774: Microsoft.Maui.Maps.dll => 0xf9cd7515 => 215
	i32 4196529839, ; 775: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 776: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 777: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 275
	i32 4258378803, ; 778: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 299
	i32 4260525087, ; 779: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 780: Microsoft.Maui.Controls.dll => 0xfea12dee => 209
	i32 4274623895, ; 781: CommunityToolkit.Mvvm.dll => 0xfec99597 => 176
	i32 4274976490, ; 782: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4278134329, ; 783: Xamarin.GooglePlayServices.Maps.dll => 0xfeff2639 => 345
	i32 4292120959, ; 784: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 300
	i32 4294763496 ; 785: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 284
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [786 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 296, ; 3
	i32 341, ; 4
	i32 48, ; 5
	i32 217, ; 6
	i32 80, ; 7
	i32 143, ; 8
	i32 30, ; 9
	i32 387, ; 10
	i32 124, ; 11
	i32 214, ; 12
	i32 102, ; 13
	i32 315, ; 14
	i32 107, ; 15
	i32 315, ; 16
	i32 137, ; 17
	i32 350, ; 18
	i32 77, ; 19
	i32 231, ; 20
	i32 124, ; 21
	i32 13, ; 22
	i32 268, ; 23
	i32 132, ; 24
	i32 317, ; 25
	i32 149, ; 26
	i32 384, ; 27
	i32 385, ; 28
	i32 18, ; 29
	i32 266, ; 30
	i32 26, ; 31
	i32 289, ; 32
	i32 1, ; 33
	i32 59, ; 34
	i32 42, ; 35
	i32 91, ; 36
	i32 271, ; 37
	i32 340, ; 38
	i32 145, ; 39
	i32 292, ; 40
	i32 287, ; 41
	i32 356, ; 42
	i32 54, ; 43
	i32 241, ; 44
	i32 69, ; 45
	i32 384, ; 46
	i32 256, ; 47
	i32 83, ; 48
	i32 369, ; 49
	i32 291, ; 50
	i32 368, ; 51
	i32 131, ; 52
	i32 221, ; 53
	i32 55, ; 54
	i32 147, ; 55
	i32 74, ; 56
	i32 143, ; 57
	i32 220, ; 58
	i32 62, ; 59
	i32 144, ; 60
	i32 392, ; 61
	i32 163, ; 62
	i32 380, ; 63
	i32 272, ; 64
	i32 12, ; 65
	i32 285, ; 66
	i32 125, ; 67
	i32 242, ; 68
	i32 150, ; 69
	i32 113, ; 70
	i32 328, ; 71
	i32 177, ; 72
	i32 164, ; 73
	i32 162, ; 74
	i32 230, ; 75
	i32 287, ; 76
	i32 302, ; 77
	i32 185, ; 78
	i32 84, ; 79
	i32 367, ; 80
	i32 361, ; 81
	i32 223, ; 82
	i32 207, ; 83
	i32 329, ; 84
	i32 219, ; 85
	i32 148, ; 86
	i32 350, ; 87
	i32 60, ; 88
	i32 204, ; 89
	i32 51, ; 90
	i32 103, ; 91
	i32 114, ; 92
	i32 40, ; 93
	i32 336, ; 94
	i32 326, ; 95
	i32 120, ; 96
	i32 216, ; 97
	i32 375, ; 98
	i32 173, ; 99
	i32 52, ; 100
	i32 44, ; 101
	i32 119, ; 102
	i32 333, ; 103
	i32 239, ; 104
	i32 277, ; 105
	i32 373, ; 106
	i32 283, ; 107
	i32 81, ; 108
	i32 234, ; 109
	i32 322, ; 110
	i32 263, ; 111
	i32 8, ; 112
	i32 73, ; 113
	i32 355, ; 114
	i32 153, ; 115
	i32 224, ; 116
	i32 352, ; 117
	i32 152, ; 118
	i32 92, ; 119
	i32 347, ; 120
	i32 45, ; 121
	i32 370, ; 122
	i32 233, ; 123
	i32 358, ; 124
	i32 351, ; 125
	i32 109, ; 126
	i32 129, ; 127
	i32 223, ; 128
	i32 25, ; 129
	i32 253, ; 130
	i32 72, ; 131
	i32 55, ; 132
	i32 46, ; 133
	i32 379, ; 134
	i32 222, ; 135
	i32 221, ; 136
	i32 206, ; 137
	i32 278, ; 138
	i32 22, ; 139
	i32 294, ; 140
	i32 241, ; 141
	i32 86, ; 142
	i32 43, ; 143
	i32 158, ; 144
	i32 71, ; 145
	i32 308, ; 146
	i32 3, ; 147
	i32 42, ; 148
	i32 63, ; 149
	i32 16, ; 150
	i32 215, ; 151
	i32 53, ; 152
	i32 391, ; 153
	i32 382, ; 154
	i32 341, ; 155
	i32 245, ; 156
	i32 236, ; 157
	i32 105, ; 158
	i32 217, ; 159
	i32 351, ; 160
	i32 337, ; 161
	i32 291, ; 162
	i32 34, ; 163
	i32 156, ; 164
	i32 85, ; 165
	i32 32, ; 166
	i32 12, ; 167
	i32 51, ; 168
	i32 202, ; 169
	i32 330, ; 170
	i32 56, ; 171
	i32 312, ; 172
	i32 36, ; 173
	i32 250, ; 174
	i32 199, ; 175
	i32 357, ; 176
	i32 338, ; 177
	i32 261, ; 178
	i32 35, ; 179
	i32 58, ; 180
	i32 298, ; 181
	i32 181, ; 182
	i32 17, ; 183
	i32 348, ; 184
	i32 162, ; 185
	i32 196, ; 186
	i32 203, ; 187
	i32 370, ; 188
	i32 297, ; 189
	i32 325, ; 190
	i32 247, ; 191
	i32 189, ; 192
	i32 376, ; 193
	i32 151, ; 194
	i32 200, ; 195
	i32 321, ; 196
	i32 306, ; 197
	i32 189, ; 198
	i32 374, ; 199
	i32 263, ; 200
	i32 192, ; 201
	i32 29, ; 202
	i32 176, ; 203
	i32 52, ; 204
	i32 372, ; 205
	i32 185, ; 206
	i32 326, ; 207
	i32 5, ; 208
	i32 356, ; 209
	i32 339, ; 210
	i32 316, ; 211
	i32 320, ; 212
	i32 269, ; 213
	i32 352, ; 214
	i32 260, ; 215
	i32 280, ; 216
	i32 85, ; 217
	i32 243, ; 218
	i32 325, ; 219
	i32 61, ; 220
	i32 112, ; 221
	i32 57, ; 222
	i32 386, ; 223
	i32 312, ; 224
	i32 99, ; 225
	i32 303, ; 226
	i32 19, ; 227
	i32 273, ; 228
	i32 340, ; 229
	i32 111, ; 230
	i32 101, ; 231
	i32 102, ; 232
	i32 354, ; 233
	i32 104, ; 234
	i32 337, ; 235
	i32 71, ; 236
	i32 38, ; 237
	i32 32, ; 238
	i32 103, ; 239
	i32 73, ; 240
	i32 360, ; 241
	i32 9, ; 242
	i32 123, ; 243
	i32 46, ; 244
	i32 262, ; 245
	i32 207, ; 246
	i32 9, ; 247
	i32 243, ; 248
	i32 43, ; 249
	i32 4, ; 250
	i32 313, ; 251
	i32 364, ; 252
	i32 359, ; 253
	i32 202, ; 254
	i32 31, ; 255
	i32 136, ; 256
	i32 92, ; 257
	i32 93, ; 258
	i32 379, ; 259
	i32 49, ; 260
	i32 139, ; 261
	i32 112, ; 262
	i32 138, ; 263
	i32 279, ; 264
	i32 115, ; 265
	i32 338, ; 266
	i32 218, ; 267
	i32 155, ; 268
	i32 388, ; 269
	i32 76, ; 270
	i32 79, ; 271
	i32 301, ; 272
	i32 37, ; 273
	i32 226, ; 274
	i32 324, ; 275
	i32 197, ; 276
	i32 174, ; 277
	i32 283, ; 278
	i32 276, ; 279
	i32 175, ; 280
	i32 64, ; 281
	i32 136, ; 282
	i32 15, ; 283
	i32 116, ; 284
	i32 318, ; 285
	i32 335, ; 286
	i32 271, ; 287
	i32 48, ; 288
	i32 70, ; 289
	i32 80, ; 290
	i32 126, ; 291
	i32 188, ; 292
	i32 94, ; 293
	i32 121, ; 294
	i32 349, ; 295
	i32 249, ; 296
	i32 26, ; 297
	i32 294, ; 298
	i32 220, ; 299
	i32 97, ; 300
	i32 28, ; 301
	i32 267, ; 302
	i32 377, ; 303
	i32 355, ; 304
	i32 147, ; 305
	i32 232, ; 306
	i32 167, ; 307
	i32 4, ; 308
	i32 391, ; 309
	i32 98, ; 310
	i32 33, ; 311
	i32 240, ; 312
	i32 93, ; 313
	i32 317, ; 314
	i32 204, ; 315
	i32 21, ; 316
	i32 41, ; 317
	i32 168, ; 318
	i32 371, ; 319
	i32 285, ; 320
	i32 363, ; 321
	i32 301, ; 322
	i32 348, ; 323
	i32 335, ; 324
	i32 307, ; 325
	i32 2, ; 326
	i32 187, ; 327
	i32 134, ; 328
	i32 111, ; 329
	i32 390, ; 330
	i32 205, ; 331
	i32 383, ; 332
	i32 253, ; 333
	i32 380, ; 334
	i32 58, ; 335
	i32 251, ; 336
	i32 95, ; 337
	i32 362, ; 338
	i32 39, ; 339
	i32 264, ; 340
	i32 390, ; 341
	i32 25, ; 342
	i32 94, ; 343
	i32 89, ; 344
	i32 99, ; 345
	i32 344, ; 346
	i32 10, ; 347
	i32 239, ; 348
	i32 186, ; 349
	i32 87, ; 350
	i32 100, ; 351
	i32 314, ; 352
	i32 193, ; 353
	i32 349, ; 354
	i32 255, ; 355
	i32 359, ; 356
	i32 7, ; 357
	i32 298, ; 358
	i32 354, ; 359
	i32 332, ; 360
	i32 252, ; 361
	i32 327, ; 362
	i32 88, ; 363
	i32 195, ; 364
	i32 342, ; 365
	i32 293, ; 366
	i32 152, ; 367
	i32 358, ; 368
	i32 329, ; 369
	i32 33, ; 370
	i32 201, ; 371
	i32 186, ; 372
	i32 116, ; 373
	i32 82, ; 374
	i32 288, ; 375
	i32 251, ; 376
	i32 244, ; 377
	i32 250, ; 378
	i32 20, ; 379
	i32 343, ; 380
	i32 11, ; 381
	i32 160, ; 382
	i32 3, ; 383
	i32 212, ; 384
	i32 366, ; 385
	i32 206, ; 386
	i32 205, ; 387
	i32 84, ; 388
	i32 353, ; 389
	i32 64, ; 390
	i32 333, ; 391
	i32 237, ; 392
	i32 368, ; 393
	i32 180, ; 394
	i32 321, ; 395
	i32 141, ; 396
	i32 248, ; 397
	i32 302, ; 398
	i32 155, ; 399
	i32 188, ; 400
	i32 41, ; 401
	i32 290, ; 402
	i32 117, ; 403
	i32 194, ; 404
	i32 254, ; 405
	i32 362, ; 406
	i32 310, ; 407
	i32 328, ; 408
	i32 131, ; 409
	i32 75, ; 410
	i32 66, ; 411
	i32 210, ; 412
	i32 372, ; 413
	i32 170, ; 414
	i32 258, ; 415
	i32 141, ; 416
	i32 177, ; 417
	i32 106, ; 418
	i32 149, ; 419
	i32 70, ; 420
	i32 334, ; 421
	i32 228, ; 422
	i32 154, ; 423
	i32 193, ; 424
	i32 121, ; 425
	i32 127, ; 426
	i32 367, ; 427
	i32 150, ; 428
	i32 282, ; 429
	i32 389, ; 430
	i32 139, ; 431
	i32 269, ; 432
	i32 364, ; 433
	i32 20, ; 434
	i32 14, ; 435
	i32 175, ; 436
	i32 135, ; 437
	i32 75, ; 438
	i32 59, ; 439
	i32 272, ; 440
	i32 178, ; 441
	i32 165, ; 442
	i32 166, ; 443
	i32 237, ; 444
	i32 209, ; 445
	i32 15, ; 446
	i32 74, ; 447
	i32 6, ; 448
	i32 172, ; 449
	i32 23, ; 450
	i32 296, ; 451
	i32 171, ; 452
	i32 238, ; 453
	i32 227, ; 454
	i32 252, ; 455
	i32 229, ; 456
	i32 91, ; 457
	i32 365, ; 458
	i32 1, ; 459
	i32 234, ; 460
	i32 297, ; 461
	i32 320, ; 462
	i32 134, ; 463
	i32 69, ; 464
	i32 144, ; 465
	i32 200, ; 466
	i32 374, ; 467
	i32 184, ; 468
	i32 230, ; 469
	i32 353, ; 470
	i32 229, ; 471
	i32 286, ; 472
	i32 88, ; 473
	i32 96, ; 474
	i32 276, ; 475
	i32 0, ; 476
	i32 281, ; 477
	i32 228, ; 478
	i32 247, ; 479
	i32 369, ; 480
	i32 31, ; 481
	i32 45, ; 482
	i32 292, ; 483
	i32 190, ; 484
	i32 254, ; 485
	i32 109, ; 486
	i32 156, ; 487
	i32 35, ; 488
	i32 22, ; 489
	i32 330, ; 490
	i32 114, ; 491
	i32 57, ; 492
	i32 318, ; 493
	i32 246, ; 494
	i32 142, ; 495
	i32 118, ; 496
	i32 120, ; 497
	i32 110, ; 498
	i32 256, ; 499
	i32 137, ; 500
	i32 262, ; 501
	i32 54, ; 502
	i32 105, ; 503
	i32 375, ; 504
	i32 211, ; 505
	i32 212, ; 506
	i32 133, ; 507
	i32 347, ; 508
	i32 323, ; 509
	i32 311, ; 510
	i32 381, ; 511
	i32 286, ; 512
	i32 225, ; 513
	i32 245, ; 514
	i32 214, ; 515
	i32 157, ; 516
	i32 360, ; 517
	i32 273, ; 518
	i32 161, ; 519
	i32 132, ; 520
	i32 311, ; 521
	i32 171, ; 522
	i32 159, ; 523
	i32 373, ; 524
	i32 299, ; 525
	i32 343, ; 526
	i32 190, ; 527
	i32 138, ; 528
	i32 187, ; 529
	i32 323, ; 530
	i32 319, ; 531
	i32 167, ; 532
	i32 213, ; 533
	i32 174, ; 534
	i32 257, ; 535
	i32 336, ; 536
	i32 40, ; 537
	i32 284, ; 538
	i32 81, ; 539
	i32 56, ; 540
	i32 37, ; 541
	i32 97, ; 542
	i32 164, ; 543
	i32 170, ; 544
	i32 201, ; 545
	i32 225, ; 546
	i32 324, ; 547
	i32 82, ; 548
	i32 259, ; 549
	i32 331, ; 550
	i32 98, ; 551
	i32 30, ; 552
	i32 157, ; 553
	i32 18, ; 554
	i32 339, ; 555
	i32 127, ; 556
	i32 119, ; 557
	i32 184, ; 558
	i32 280, ; 559
	i32 314, ; 560
	i32 295, ; 561
	i32 316, ; 562
	i32 345, ; 563
	i32 163, ; 564
	i32 289, ; 565
	i32 238, ; 566
	i32 392, ; 567
	i32 313, ; 568
	i32 304, ; 569
	i32 346, ; 570
	i32 168, ; 571
	i32 16, ; 572
	i32 191, ; 573
	i32 142, ; 574
	i32 366, ; 575
	i32 125, ; 576
	i32 118, ; 577
	i32 179, ; 578
	i32 38, ; 579
	i32 179, ; 580
	i32 115, ; 581
	i32 47, ; 582
	i32 140, ; 583
	i32 117, ; 584
	i32 218, ; 585
	i32 246, ; 586
	i32 34, ; 587
	i32 181, ; 588
	i32 95, ; 589
	i32 53, ; 590
	i32 332, ; 591
	i32 216, ; 592
	i32 305, ; 593
	i32 327, ; 594
	i32 244, ; 595
	i32 129, ; 596
	i32 151, ; 597
	i32 191, ; 598
	i32 24, ; 599
	i32 159, ; 600
	i32 236, ; 601
	i32 279, ; 602
	i32 146, ; 603
	i32 104, ; 604
	i32 344, ; 605
	i32 89, ; 606
	i32 267, ; 607
	i32 60, ; 608
	i32 140, ; 609
	i32 265, ; 610
	i32 100, ; 611
	i32 5, ; 612
	i32 13, ; 613
	i32 122, ; 614
	i32 334, ; 615
	i32 135, ; 616
	i32 28, ; 617
	i32 361, ; 618
	i32 72, ; 619
	i32 277, ; 620
	i32 24, ; 621
	i32 242, ; 622
	i32 219, ; 623
	i32 264, ; 624
	i32 309, ; 625
	i32 306, ; 626
	i32 288, ; 627
	i32 378, ; 628
	i32 235, ; 629
	i32 257, ; 630
	i32 274, ; 631
	i32 166, ; 632
	i32 310, ; 633
	i32 357, ; 634
	i32 101, ; 635
	i32 249, ; 636
	i32 123, ; 637
	i32 278, ; 638
	i32 182, ; 639
	i32 195, ; 640
	i32 198, ; 641
	i32 161, ; 642
	i32 165, ; 643
	i32 182, ; 644
	i32 281, ; 645
	i32 39, ; 646
	i32 208, ; 647
	i32 365, ; 648
	i32 227, ; 649
	i32 17, ; 650
	i32 226, ; 651
	i32 169, ; 652
	i32 378, ; 653
	i32 377, ; 654
	i32 235, ; 655
	i32 148, ; 656
	i32 270, ; 657
	i32 210, ; 658
	i32 265, ; 659
	i32 153, ; 660
	i32 130, ; 661
	i32 19, ; 662
	i32 65, ; 663
	i32 145, ; 664
	i32 47, ; 665
	i32 385, ; 666
	i32 331, ; 667
	i32 255, ; 668
	i32 79, ; 669
	i32 172, ; 670
	i32 61, ; 671
	i32 106, ; 672
	i32 308, ; 673
	i32 259, ; 674
	i32 49, ; 675
	i32 293, ; 676
	i32 382, ; 677
	i32 305, ; 678
	i32 14, ; 679
	i32 194, ; 680
	i32 68, ; 681
	i32 224, ; 682
	i32 169, ; 683
	i32 388, ; 684
	i32 266, ; 685
	i32 270, ; 686
	i32 387, ; 687
	i32 78, ; 688
	i32 275, ; 689
	i32 108, ; 690
	i32 258, ; 691
	i32 197, ; 692
	i32 304, ; 693
	i32 67, ; 694
	i32 63, ; 695
	i32 27, ; 696
	i32 158, ; 697
	i32 196, ; 698
	i32 290, ; 699
	i32 268, ; 700
	i32 10, ; 701
	i32 183, ; 702
	i32 208, ; 703
	i32 11, ; 704
	i32 233, ; 705
	i32 173, ; 706
	i32 78, ; 707
	i32 303, ; 708
	i32 126, ; 709
	i32 83, ; 710
	i32 199, ; 711
	i32 66, ; 712
	i32 107, ; 713
	i32 65, ; 714
	i32 128, ; 715
	i32 122, ; 716
	i32 77, ; 717
	i32 319, ; 718
	i32 309, ; 719
	i32 386, ; 720
	i32 8, ; 721
	i32 274, ; 722
	i32 2, ; 723
	i32 44, ; 724
	i32 322, ; 725
	i32 154, ; 726
	i32 128, ; 727
	i32 307, ; 728
	i32 23, ; 729
	i32 231, ; 730
	i32 133, ; 731
	i32 261, ; 732
	i32 295, ; 733
	i32 346, ; 734
	i32 381, ; 735
	i32 363, ; 736
	i32 342, ; 737
	i32 29, ; 738
	i32 183, ; 739
	i32 260, ; 740
	i32 232, ; 741
	i32 180, ; 742
	i32 0, ; 743
	i32 62, ; 744
	i32 211, ; 745
	i32 90, ; 746
	i32 222, ; 747
	i32 87, ; 748
	i32 146, ; 749
	i32 203, ; 750
	i32 213, ; 751
	i32 36, ; 752
	i32 86, ; 753
	i32 282, ; 754
	i32 192, ; 755
	i32 376, ; 756
	i32 371, ; 757
	i32 198, ; 758
	i32 50, ; 759
	i32 6, ; 760
	i32 90, ; 761
	i32 383, ; 762
	i32 21, ; 763
	i32 160, ; 764
	i32 96, ; 765
	i32 50, ; 766
	i32 248, ; 767
	i32 178, ; 768
	i32 113, ; 769
	i32 300, ; 770
	i32 389, ; 771
	i32 130, ; 772
	i32 240, ; 773
	i32 215, ; 774
	i32 76, ; 775
	i32 27, ; 776
	i32 275, ; 777
	i32 299, ; 778
	i32 7, ; 779
	i32 209, ; 780
	i32 176, ; 781
	i32 110, ; 782
	i32 345, ; 783
	i32 300, ; 784
	i32 284 ; 785
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

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
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
