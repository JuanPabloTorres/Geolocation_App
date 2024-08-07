; ModuleID = 'marshal_methods.x86_64.ll'
source_filename = "marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [376 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [752 x i64] [
	i64 24362543149721218, ; 0: Xamarin.AndroidX.DynamicAnimation => 0x568d9a9a43a682 => 278
	i64 40218994123153105, ; 1: ExCSS.dll => 0x8ee2f649ef1ed1 => 177
	i64 98382396393917666, ; 2: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 206
	i64 120698629574877762, ; 3: Mono.Android => 0x1accec39cafe242 => 169
	i64 131669012237370309, ; 4: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 212
	i64 160518225272466977, ; 5: Microsoft.Extensions.Hosting.Abstractions => 0x23a4679b5576e21 => 202
	i64 184471870596806994, ; 6: Svg.Skia => 0x28f60305df97952 => 230
	i64 196720943101637631, ; 7: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 58
	i64 210515253464952879, ; 8: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 265
	i64 229794953483747371, ; 9: System.ValueTuple.dll => 0x330654aed93802b => 149
	i64 232391251801502327, ; 10: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 307
	i64 295915112840604065, ; 11: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 310
	i64 308826992458506653, ; 12: SkiaSharp.Extended.dll => 0x4492c836e8aa19d => 219
	i64 316157742385208084, ; 13: Xamarin.AndroidX.Core.Core.Ktx.dll => 0x46337caa7dc1b14 => 272
	i64 350667413455104241, ; 14: System.ServiceProcess.dll => 0x4ddd227954be8f1 => 132
	i64 404665707914610830, ; 15: Svg.Custom => 0x59da9513d08488e => 228
	i64 422779754995088667, ; 16: System.IO.UnmanagedMemoryStream => 0x5de03f27ab57d1b => 56
	i64 435118502366263740, ; 17: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x609d9f8f8bdb9bc => 309
	i64 560278790331054453, ; 18: System.Reflection.Primitives => 0x7c6829760de3975 => 95
	i64 590337075967009532, ; 19: Microsoft.Maui.Maps.dll => 0x8314c715ec1a2fc => 214
	i64 634308326490598313, ; 20: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 291
	i64 648449422406355874, ; 21: Microsoft.Extensions.Configuration.FileExtensions.dll => 0x8ffc15065568ba2 => 195
	i64 649145001856603771, ; 22: System.Security.SecureString => 0x90239f09b62167b => 129
	i64 668723562677762733, ; 23: Microsoft.Extensions.Configuration.Binder.dll => 0x947c88986577aad => 194
	i64 687654259221141486, ; 24: Xamarin.GooglePlayServices.Base => 0x98b09e7c92917ee => 329
	i64 750875890346172408, ; 25: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 143
	i64 798450721097591769, ; 26: Xamarin.AndroidX.Collection.Ktx.dll => 0xb14aab351ad2bd9 => 266
	i64 799765834175365804, ; 27: System.ComponentModel.dll => 0xb1956c9f18442ac => 18
	i64 805302231655005164, ; 28: hu\Microsoft.Maui.Controls.resources.dll => 0xb2d021ceea03bec => 352
	i64 872800313462103108, ; 29: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 277
	i64 895210737996778430, ; 30: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0xc6c6d6c5569cbbe => 292
	i64 940822596282819491, ; 31: System.Transactions => 0xd0e792aa81923a3 => 148
	i64 943650302560566006, ; 32: ExoPlayer.Dash.dll => 0xd1884f3544ffaf6 => 240
	i64 960778385402502048, ; 33: System.Runtime.Handles.dll => 0xd555ed9e1ca1ba0 => 104
	i64 1010599046655515943, ; 34: System.Reflection.Primitives.dll => 0xe065e7a82401d27 => 95
	i64 1120440138749646132, ; 35: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 322
	i64 1188721839004621111, ; 36: Xabe.FFmpeg => 0x107f3036e69e9937 => 235
	i64 1268860745194512059, ; 37: System.Drawing.dll => 0x119be62002c19ebb => 36
	i64 1301626418029409250, ; 38: System.Diagnostics.FileVersionInfo => 0x12104e54b4e833e2 => 28
	i64 1315114680217950157, ; 39: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 261
	i64 1369545283391376210, ; 40: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 300
	i64 1404195534211153682, ; 41: System.IO.FileSystem.Watcher.dll => 0x137cb4660bd87f12 => 50
	i64 1425944114962822056, ; 42: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 115
	i64 1433520707554318520, ; 43: SkiaSharp.Extended.UI.dll => 0x13e4e37d07f118b8 => 220
	i64 1476839205573959279, ; 44: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 70
	i64 1486715745332614827, ; 45: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 208
	i64 1492954217099365037, ; 46: System.Net.HttpListener => 0x14b809f350210aad => 65
	i64 1513467482682125403, ; 47: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 168
	i64 1537168428375924959, ; 48: System.Threading.Thread.dll => 0x15551e8a954ae0df => 143
	i64 1576750169145655260, ; 49: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x15e1bdecc376bfdc => 321
	i64 1624659445732251991, ; 50: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 260
	i64 1628611045998245443, ; 51: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 295
	i64 1636321030536304333, ; 52: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 285
	i64 1651782184287836205, ; 53: System.Globalization.Calendars => 0x16ec4f2524cb982d => 40
	i64 1659332977923810219, ; 54: System.Reflection.DispatchProxy => 0x1707228d493d63ab => 89
	i64 1682513316613008342, ; 55: System.Net.dll => 0x17597cf276952bd6 => 81
	i64 1731380447121279447, ; 56: Newtonsoft.Json => 0x18071957e9b889d7 => 216
	i64 1735388228521408345, ; 57: System.Net.Mail.dll => 0x181556663c69b759 => 66
	i64 1743969030606105336, ; 58: System.Memory.dll => 0x1833d297e88f2af8 => 62
	i64 1767386781656293639, ; 59: System.Private.Uri.dll => 0x188704e9f5582107 => 86
	i64 1795316252682057001, ; 60: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 259
	i64 1825687700144851180, ; 61: System.Runtime.InteropServices.RuntimeInformation.dll => 0x1956254a55ef08ec => 106
	i64 1835311033149317475, ; 62: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 346
	i64 1836611346387731153, ; 63: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 307
	i64 1854145951182283680, ; 64: System.Runtime.CompilerServices.VisualC => 0x19bb3feb3df2e3a0 => 102
	i64 1875417405349196092, ; 65: System.Drawing.Primitives => 0x1a06d2319b6c713c => 35
	i64 1875917498431009007, ; 66: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 256
	i64 1881198190668717030, ; 67: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 368
	i64 1897575647115118287, ; 68: Xamarin.AndroidX.Security.SecurityCrypto => 0x1a558aff4cba86cf => 309
	i64 1920760634179481754, ; 69: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 210
	i64 1930726298510463061, ; 70: CommunityToolkit.Mvvm.dll => 0x1acb5156cd389055 => 176
	i64 1963507636676687784, ; 71: MimeKit => 0x1b3fc7cadde177a8 => 215
	i64 1972385128188460614, ; 72: System.Security.Cryptography.Algorithms => 0x1b5f51d2edefbe46 => 119
	i64 1981742497975770890, ; 73: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 293
	i64 1996473713535492147, ; 74: CommunityToolkit.Maui.MediaElement.dll => 0x1bb4e643c2b02033 => 175
	i64 2040001226662520565, ; 75: System.Threading.Tasks.Extensions.dll => 0x1c4f8a4ea894a6f5 => 140
	i64 2062890601515140263, ; 76: System.Threading.Tasks.Dataflow => 0x1ca0dc1289cd44a7 => 139
	i64 2064708342624596306, ; 77: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x1ca7514c5eecb152 => 336
	i64 2080945842184875448, ; 78: System.IO.MemoryMappedFiles => 0x1ce10137d8416db8 => 53
	i64 2102659300918482391, ; 79: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 35
	i64 2106033277907880740, ; 80: System.Threading.Tasks.Dataflow.dll => 0x1d3a221ba6d9cb24 => 139
	i64 2133195048986300728, ; 81: Newtonsoft.Json.dll => 0x1d9aa1984b735138 => 216
	i64 2165310824878145998, ; 82: Xamarin.Android.Glide.GifDecoder => 0x1e0cbab9112b81ce => 253
	i64 2165725771938924357, ; 83: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 263
	i64 2188974421706709258, ; 84: SkiaSharp.HarfBuzz.dll => 0x1e60cca38c3e990a => 221
	i64 2192948757939169934, ; 85: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x1e6eeb46cf992a8e => 188
	i64 2262844636196693701, ; 86: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 277
	i64 2287834202362508563, ; 87: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 8
	i64 2287887973817120656, ; 88: System.ComponentModel.DataAnnotations.dll => 0x1fc035fd8d41f790 => 14
	i64 2304837677853103545, ; 89: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0x1ffc6da80d5ed5b9 => 306
	i64 2315304989185124968, ; 90: System.IO.FileSystem.dll => 0x20219d9ee311aa68 => 51
	i64 2329709569556905518, ; 91: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 288
	i64 2335503487726329082, ; 92: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 233
	i64 2337758774805907496, ; 93: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 101
	i64 2379805940701141695, ; 94: ExoPlayer.Rtsp => 0x2106c4e4f1db1abf => 246
	i64 2470498323731680442, ; 95: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 270
	i64 2479423007379663237, ; 96: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 316
	i64 2497223385847772520, ; 97: System.Runtime => 0x22a7eb7046413568 => 116
	i64 2547086958574651984, ; 98: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 254
	i64 2592350477072141967, ; 99: System.Xml.dll => 0x23f9e10627330e8f => 161
	i64 2602673633151553063, ; 100: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 367
	i64 2624866290265602282, ; 101: mscorlib.dll => 0x246d65fbde2db8ea => 164
	i64 2632269733008246987, ; 102: System.Net.NameResolution => 0x2487b36034f808cb => 67
	i64 2656907746661064104, ; 103: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 197
	i64 2662981627730767622, ; 104: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 342
	i64 2706075432581334785, ; 105: System.Net.WebSockets => 0x258de944be6c0701 => 80
	i64 2783046991838674048, ; 106: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 101
	i64 2787234703088983483, ; 107: Xamarin.AndroidX.Startup.StartupRuntime => 0x26ae3f31ef429dbb => 311
	i64 2815524396660695947, ; 108: System.Security.AccessControl => 0x2712c0857f68238b => 117
	i64 2895129759130297543, ; 109: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 347
	i64 2923871038697555247, ; 110: Jsr305Binding => 0x2893ad37e69ec52f => 323
	i64 3017136373564924869, ; 111: System.Net.WebProxy => 0x29df058bd93f63c5 => 78
	i64 3017704767998173186, ; 112: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 322
	i64 3106852385031680087, ; 113: System.Runtime.Serialization.Xml => 0x2b1dc1c88b637057 => 114
	i64 3110390492489056344, ; 114: System.Security.Cryptography.Csp.dll => 0x2b2a53ac61900058 => 121
	i64 3135773902340015556, ; 115: System.IO.FileSystem.DriveInfo.dll => 0x2b8481c008eac5c4 => 48
	i64 3168817962471953758, ; 116: Microsoft.Extensions.Hosting.Abstractions.dll => 0x2bf9e725d304955e => 202
	i64 3188824379904900412, ; 117: ExoPlayer.Common.dll => 0x2c40fae0df563d3c => 237
	i64 3254037935674351285, ; 118: SkiaSharp.Views.Maui.Controls.Compatibility.dll => 0x2d28aa430983deb5 => 226
	i64 3281594302220646930, ; 119: System.Security.Principal => 0x2d8a90a198ceba12 => 128
	i64 3289520064315143713, ; 120: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 286
	i64 3303437397778967116, ; 121: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 257
	i64 3311221304742556517, ; 122: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 82
	i64 3325875462027654285, ; 123: System.Runtime.Numerics => 0x2e27e21c8958b48d => 110
	i64 3328853167529574890, ; 124: System.Net.Sockets.dll => 0x2e327651a008c1ea => 75
	i64 3344514922410554693, ; 125: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 339
	i64 3396143930648122816, ; 126: Microsoft.Extensions.FileProviders.Abstractions => 0x2f2186e9506155c0 => 199
	i64 3411255996856937470, ; 127: Xamarin.GooglePlayServices.Basement => 0x2f5737416a942bfe => 330
	i64 3414639567687375782, ; 128: SkiaSharp.Views.Maui.Controls => 0x2f633c9863ffdba6 => 225
	i64 3429672777697402584, ; 129: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 212
	i64 3437845325506641314, ; 130: System.IO.MemoryMappedFiles.dll => 0x2fb5ae1beb8f7da2 => 53
	i64 3461602852075779363, ; 131: SkiaSharp.HarfBuzz => 0x300a15741f74b523 => 221
	i64 3493805808809882663, ; 132: Xamarin.AndroidX.Tracing.Tracing.dll => 0x307c7ddf444f3427 => 313
	i64 3494946837667399002, ; 133: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 192
	i64 3508450208084372758, ; 134: System.Net.Ping => 0x30b084e02d03ad16 => 69
	i64 3522470458906976663, ; 135: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 312
	i64 3523004241079211829, ; 136: Microsoft.Extensions.Caching.Memory.dll => 0x30e439b10bb89735 => 191
	i64 3531994851595924923, ; 137: System.Numerics => 0x31042a9aade235bb => 83
	i64 3551103847008531295, ; 138: System.Private.CoreLib.dll => 0x31480e226177735f => 170
	i64 3567343442040498961, ; 139: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 362
	i64 3571415421602489686, ; 140: System.Runtime.dll => 0x319037675df7e556 => 116
	i64 3638003163729360188, ; 141: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 193
	i64 3647754201059316852, ; 142: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 154
	i64 3655542548057982301, ; 143: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 192
	i64 3658489898830683555, ; 144: Svg.Skia.dll => 0x32c5912df2285da3 => 230
	i64 3659371656528649588, ; 145: Xamarin.Android.Glide.Annotations => 0x32c8b3222885dd74 => 251
	i64 3716579019761409177, ; 146: netstandard.dll => 0x3393f0ed5c8c5c99 => 165
	i64 3727469159507183293, ; 147: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 305
	i64 3772598417116884899, ; 148: Xamarin.AndroidX.DynamicAnimation.dll => 0x345af645b473efa3 => 278
	i64 3869221888984012293, ; 149: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 203
	i64 3869649043256705283, ; 150: System.Diagnostics.Tools => 0x35b3c14d74bf0103 => 32
	i64 3889433610606858880, ; 151: Microsoft.Extensions.FileProviders.Physical.dll => 0x35fa0b4301afd280 => 200
	i64 3890352374528606784, ; 152: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 210
	i64 3919223565570527920, ; 153: System.Security.Cryptography.Encoding => 0x3663e111652bd2b0 => 122
	i64 3933965368022646939, ; 154: System.Net.Requests => 0x369840a8bfadc09b => 72
	i64 3966267475168208030, ; 155: System.Memory => 0x370b03412596249e => 62
	i64 4006972109285359177, ; 156: System.Xml.XmlDocument => 0x379b9fe74ed9fe49 => 159
	i64 4009997192427317104, ; 157: System.Runtime.Serialization.Primitives => 0x37a65f335cf1a770 => 113
	i64 4070326265757318011, ; 158: da\Microsoft.Maui.Controls.resources.dll => 0x387cb42c56683b7b => 343
	i64 4073500526318903918, ; 159: System.Private.Xml.dll => 0x3887fb25779ae26e => 88
	i64 4073631083018132676, ; 160: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 207
	i64 4120493066591692148, ; 161: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 373
	i64 4124717108719830997, ; 162: FFmpeg.AutoGen.dll => 0x393df05b5038c7d5 => 179
	i64 4148881117810174540, ; 163: System.Runtime.InteropServices.JavaScript.dll => 0x3993c9651a66aa4c => 105
	i64 4154383907710350974, ; 164: System.ComponentModel => 0x39a7562737acb67e => 18
	i64 4167269041631776580, ; 165: System.Threading.ThreadPool => 0x39d51d1d3df1cf44 => 144
	i64 4168469861834746866, ; 166: System.Security.Claims.dll => 0x39d96140fb94ebf2 => 118
	i64 4187479170553454871, ; 167: System.Linq.Expressions => 0x3a1cea1e912fa117 => 58
	i64 4201423742386704971, ; 168: Xamarin.AndroidX.Core.Core.Ktx => 0x3a4e74a233da124b => 272
	i64 4205801962323029395, ; 169: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 17
	i64 4235503420553921860, ; 170: System.IO.IsolatedStorage.dll => 0x3ac787eb9b118544 => 52
	i64 4247996603072512073, ; 171: Xamarin.GooglePlayServices.Tasks => 0x3af3ea6755340049 => 332
	i64 4250192876909962317, ; 172: Microsoft.AspNetCore.Hosting.Abstractions => 0x3afbb7e72f1d244d => 183
	i64 4282138915307457788, ; 173: System.Reflection.Emit => 0x3b6d36a7ddc70cfc => 92
	i64 4306612231831054753, ; 174: SkiaSharp.SceneGraph.dll => 0x3bc42901e7a469a1 => 222
	i64 4360412976914417659, ; 175: tr\Microsoft.Maui.Controls.resources.dll => 0x3c834c8002fcc7fb => 368
	i64 4373617458794931033, ; 176: System.IO.Pipes.dll => 0x3cb235e806eb2359 => 55
	i64 4397634830160618470, ; 177: System.Security.SecureString.dll => 0x3d0789940f9be3e6 => 129
	i64 4477672992252076438, ; 178: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 150
	i64 4484706122338676047, ; 179: System.Globalization.Extensions.dll => 0x3e3ce07510042d4f => 41
	i64 4513320955448359355, ; 180: Microsoft.EntityFrameworkCore.Relational => 0x3ea2897f12d379bb => 189
	i64 4533124835995628778, ; 181: System.Reflection.Emit.dll => 0x3ee8e505540534ea => 92
	i64 4612482779465751747, ; 182: Microsoft.EntityFrameworkCore.Abstractions => 0x4002d4a662a99cc3 => 188
	i64 4636684751163556186, ; 183: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 317
	i64 4672453897036726049, ; 184: System.IO.FileSystem.Watcher => 0x40d7e4104a437f21 => 50
	i64 4716677666592453464, ; 185: System.Xml.XmlSerializer => 0x417501590542f358 => 160
	i64 4743821336939966868, ; 186: System.ComponentModel.Annotations => 0x41d5705f4239b194 => 13
	i64 4759461199762736555, ; 187: Xamarin.AndroidX.Lifecycle.Process.dll => 0x420d00be961cc5ab => 290
	i64 4776956635278302661, ; 188: ToolsLibrary => 0x424b28c019355dc5 => 374
	i64 4794310189461587505, ; 189: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 254
	i64 4795410492532947900, ; 190: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 312
	i64 4809057822547766521, ; 191: System.Drawing => 0x42bd349c3145ecf9 => 36
	i64 4814660307502931973, ; 192: System.Net.NameResolution.dll => 0x42d11c0a5ee2a005 => 67
	i64 4853321196694829351, ; 193: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 109
	i64 4871824391508510238, ; 194: nb\Microsoft.Maui.Controls.resources.dll => 0x439c3278d7f2c61e => 358
	i64 4953714692329509532, ; 195: sv\Microsoft.Maui.Controls.resources.dll => 0x44bf21444aef129c => 366
	i64 5055365687667823624, ; 196: Xamarin.AndroidX.Activity.Ktx.dll => 0x4628444ef7239408 => 255
	i64 5081566143765835342, ; 197: System.Resources.ResourceManager.dll => 0x4685597c05d06e4e => 99
	i64 5099468265966638712, ; 198: System.Resources.ResourceManager => 0x46c4f35ea8519678 => 99
	i64 5103417709280584325, ; 199: System.Collections.Specialized => 0x46d2fb5e161b6285 => 11
	i64 5112992870269948397, ; 200: FFImageLoading.Maui.dll => 0x46f4ffecfb8b91ed => 178
	i64 5182934613077526976, ; 201: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 11
	i64 5205316157927637098, ; 202: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 297
	i64 5244375036463807528, ; 203: System.Diagnostics.Contracts.dll => 0x48c7c34f4d59fc28 => 25
	i64 5262971552273843408, ; 204: System.Security.Principal.dll => 0x4909d4be0c44c4d0 => 128
	i64 5272717148637201210, ; 205: ExoPlayer.UI => 0x492c744f85a1833a => 249
	i64 5278787618751394462, ; 206: System.Net.WebClient.dll => 0x4942055efc68329e => 76
	i64 5280980186044710147, ; 207: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x4949cf7fd7123d03 => 289
	i64 5290786973231294105, ; 208: System.Runtime.Loader => 0x496ca6b869b72699 => 109
	i64 5306356071055648198, ; 209: Svg.Model.dll => 0x49a3f6bb7b0265c6 => 229
	i64 5348796042099802469, ; 210: Xamarin.AndroidX.Media => 0x4a3abda9415fc165 => 298
	i64 5376510917114486089, ; 211: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 316
	i64 5389233738419247641, ; 212: ExoPlayer.UI.dll => 0x4aca67881e079a19 => 249
	i64 5408338804355907810, ; 213: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 314
	i64 5423376490970181369, ; 214: System.Runtime.InteropServices.RuntimeInformation => 0x4b43b42f2b7b6ef9 => 106
	i64 5440320908473006344, ; 215: Microsoft.VisualBasic.Core => 0x4b7fe70acda9f908 => 2
	i64 5446034149219586269, ; 216: System.Diagnostics.Debug => 0x4b94333452e150dd => 26
	i64 5451019430259338467, ; 217: Xamarin.AndroidX.ConstraintLayout.dll => 0x4ba5e94a845c2ce3 => 268
	i64 5457765010617926378, ; 218: System.Xml.Serialization => 0x4bbde05c557002ea => 155
	i64 5471532531798518949, ; 219: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 366
	i64 5507995362134886206, ; 220: System.Core.dll => 0x4c705499688c873e => 21
	i64 5522859530602327440, ; 221: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 369
	i64 5527431512186326818, ; 222: System.IO.FileSystem.Primitives.dll => 0x4cb561acbc2a8f22 => 49
	i64 5570799893513421663, ; 223: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 43
	i64 5573260873512690141, ; 224: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 126
	i64 5573669443803475672, ; 225: Microsoft.Maui.Controls.Maps => 0x4d59a6d41d5aeed8 => 209
	i64 5574231584441077149, ; 226: Xamarin.AndroidX.Annotation.Jvm => 0x4d5ba617ae5f8d9d => 258
	i64 5591791169662171124, ; 227: System.Linq.Parallel => 0x4d9a087135e137f4 => 59
	i64 5650097808083101034, ; 228: System.Security.Cryptography.Algorithms.dll => 0x4e692e055d01a56a => 119
	i64 5692067934154308417, ; 229: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 319
	i64 5703838680789880885, ; 230: ExoPlayer.SmoothStreaming.dll => 0x4f281b0f589d1035 => 247
	i64 5724799082821825042, ; 231: Xamarin.AndroidX.ExifInterface => 0x4f72926f3e13b212 => 281
	i64 5757522595884336624, ; 232: Xamarin.AndroidX.Concurrent.Futures.dll => 0x4fe6d44bd9f885f0 => 267
	i64 5783556987928984683, ; 233: Microsoft.VisualBasic => 0x504352701bbc3c6b => 3
	i64 5896680224035167651, ; 234: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 287
	i64 5959344983920014087, ; 235: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x52b3d8b05c8ef307 => 308
	i64 5979151488806146654, ; 236: System.Formats.Asn1 => 0x52fa3699a489d25e => 38
	i64 5984759512290286505, ; 237: System.Security.Cryptography.Primitives => 0x530e23115c33dba9 => 124
	i64 6102788177522843259, ; 238: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0x54b1758374b3de7b => 308
	i64 6200764641006662125, ; 239: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 363
	i64 6222399776351216807, ; 240: System.Text.Json.dll => 0x565a67a0ffe264a7 => 234
	i64 6251069312384999852, ; 241: System.Transactions.Local => 0x56c0426b870da1ac => 147
	i64 6268464631992009879, ; 242: SkiaSharp.Skottie => 0x56fe0f5efcfbc497 => 223
	i64 6278736998281604212, ; 243: System.Private.DataContractSerialization => 0x57228e08a4ad6c74 => 85
	i64 6284145129771520194, ; 244: System.Reflection.Emit.ILGeneration => 0x5735c4b3610850c2 => 90
	i64 6300676701234028427, ; 245: es\Microsoft.Maui.Controls.resources.dll => 0x57708013cda25f8b => 346
	i64 6307229053232932165, ; 246: BCrypt.Net-Core => 0x5787c76822f8e545 => 171
	i64 6313127126423224581, ; 247: ExoPlayer.DataSource => 0x579cbbac5056c105 => 242
	i64 6319713645133255417, ; 248: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 291
	i64 6354612700029906737, ; 249: ShimSkiaSharp.dll => 0x58301e951e77ef31 => 217
	i64 6357457916754632952, ; 250: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 375
	i64 6363787360044722189, ; 251: ShimSkiaSharp => 0x5850b6e31d96280d => 217
	i64 6401687960814735282, ; 252: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 288
	i64 6433271170595107064, ; 253: MimeKit.dll => 0x5947920b731530f8 => 215
	i64 6471714727257221498, ; 254: fi\Microsoft.Maui.Controls.resources.dll => 0x59d026417dd4a97a => 347
	i64 6478287442656530074, ; 255: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 351
	i64 6504860066809920875, ; 256: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 263
	i64 6548213210057960872, ; 257: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 274
	i64 6557084851308642443, ; 258: Xamarin.AndroidX.Window.dll => 0x5aff71ee6c58c08b => 320
	i64 6560151584539558821, ; 259: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 205
	i64 6589202984700901502, ; 260: Xamarin.Google.ErrorProne.Annotations.dll => 0x5b718d34180a787e => 325
	i64 6591971792923354531, ; 261: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x5b7b636b7e9765a3 => 289
	i64 6597152804937602598, ; 262: ExoPlayer.Dash => 0x5b8dcb85db471626 => 240
	i64 6617685658146568858, ; 263: System.Text.Encoding.CodePages => 0x5bd6be0b4905fa9a => 133
	i64 6671798237668743565, ; 264: SkiaSharp => 0x5c96fd260152998d => 218
	i64 6713440830605852118, ; 265: System.Reflection.TypeExtensions.dll => 0x5d2aeeddb8dd7dd6 => 96
	i64 6739853162153639747, ; 266: Microsoft.VisualBasic.dll => 0x5d88c4bde075ff43 => 3
	i64 6743165466166707109, ; 267: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 359
	i64 6772837112740759457, ; 268: System.Runtime.InteropServices.JavaScript => 0x5dfdf378527ec7a1 => 105
	i64 6786606130239981554, ; 269: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 33
	i64 6798329586179154312, ; 270: System.Windows => 0x5e5884bd523ca188 => 152
	i64 6814185388980153342, ; 271: System.Xml.XDocument.dll => 0x5e90d98217d1abfe => 156
	i64 6876862101832370452, ; 272: System.Xml.Linq => 0x5f6f85a57d108914 => 153
	i64 6894844156784520562, ; 273: System.Numerics.Vectors => 0x5faf683aead1ad72 => 82
	i64 6911788284027924527, ; 274: Microsoft.AspNetCore.Hosting.Server.Abstractions => 0x5feb9ad2f830f02f => 184
	i64 7011053663211085209, ; 275: Xamarin.AndroidX.Fragment.Ktx => 0x614c442918e5dd99 => 283
	i64 7060896174307865760, ; 276: System.Threading.Tasks.Parallel.dll => 0x61fd57a90988f4a0 => 141
	i64 7083547580668757502, ; 277: System.Private.Xml.Linq.dll => 0x624dd0fe8f56c5fe => 87
	i64 7101497697220435230, ; 278: System.Configuration => 0x628d9687c0141d1e => 19
	i64 7103753931438454322, ; 279: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 284
	i64 7105430439328552570, ; 280: System.Security.Cryptography.Pkcs => 0x629b8f56a06d167a => 232
	i64 7111139937678078858, ; 281: ExoPlayer.Database => 0x62afd818cd65338a => 241
	i64 7112547816752919026, ; 282: System.IO.FileSystem => 0x62b4d88e3189b1f2 => 51
	i64 7141281584637745974, ; 283: Xamarin.GooglePlayServices.Maps.dll => 0x631aedc3dd5f1b36 => 331
	i64 7192745174564810625, ; 284: Xamarin.Android.Glide.GifDecoder.dll => 0x63d1c3a0a1d72f81 => 253
	i64 7270811800166795866, ; 285: System.Linq => 0x64e71ccf51a90a5a => 61
	i64 7299370801165188114, ; 286: System.IO.Pipes.AccessControl.dll => 0x654c9311e74f3c12 => 54
	i64 7314237870106916923, ; 287: SkiaSharp.Views.Maui.Core.dll => 0x65816497226eb83b => 227
	i64 7316205155833392065, ; 288: Microsoft.Win32.Primitives => 0x658861d38954abc1 => 4
	i64 7338192458477945005, ; 289: System.Reflection => 0x65d67f295d0740ad => 97
	i64 7349431895026339542, ; 290: Xamarin.Android.Glide.DiskLruCache => 0x65fe6d5e9bf88ed6 => 252
	i64 7377312882064240630, ; 291: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 17
	i64 7488575175965059935, ; 292: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 153
	i64 7489048572193775167, ; 293: System.ObjectModel => 0x67ee71ff6b419e3f => 84
	i64 7554258198599408819, ; 294: ExoPlayer.Common => 0x68d61dceb5199cb3 => 237
	i64 7592577537120840276, ; 295: System.Diagnostics.Process => 0x695e410af5b2aa54 => 29
	i64 7637303409920963731, ; 296: System.IO.Compression.ZipFile.dll => 0x69fd26fcb637f493 => 45
	i64 7654504624184590948, ; 297: System.Net.Http => 0x6a3a4366801b8264 => 64
	i64 7694700312542370399, ; 298: System.Net.Mail => 0x6ac9112a7e2cda5f => 66
	i64 7714652370974252055, ; 299: System.Private.CoreLib => 0x6b0ff375198b9c17 => 170
	i64 7723873813026311384, ; 300: SkiaSharp.Views.Maui.Controls.dll => 0x6b30b64f63600cd8 => 225
	i64 7725404731275645577, ; 301: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x6b3626ac11ce9289 => 292
	i64 7735176074855944702, ; 302: Microsoft.CSharp => 0x6b58dda848e391fe => 1
	i64 7735352534559001595, ; 303: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 334
	i64 7742555799473854801, ; 304: it\Microsoft.Maui.Controls.resources.dll => 0x6b73157a51479951 => 354
	i64 7791074099216502080, ; 305: System.IO.FileSystem.AccessControl.dll => 0x6c1f749d468bcd40 => 47
	i64 7820441508502274321, ; 306: System.Data => 0x6c87ca1e14ff8111 => 24
	i64 7836164640616011524, ; 307: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 260
	i64 7927939710195668715, ; 308: SkiaSharp.Views.Android.dll => 0x6e05b32992ed16eb => 224
	i64 7972383140441761405, ; 309: Microsoft.Extensions.Caching.Abstractions.dll => 0x6ea3983a0b58267d => 190
	i64 7975724199463739455, ; 310: sk\Microsoft.Maui.Controls.resources.dll => 0x6eaf76e6f785e03f => 365
	i64 8024811125417330653, ; 311: ExoPlayer.Extractor.dll => 0x6f5ddb33881a47dd => 244
	i64 8025517457475554965, ; 312: WindowsBase => 0x6f605d9b4786ce95 => 163
	i64 8031450141206250471, ; 313: System.Runtime.Intrinsics.dll => 0x6f757159d9dc03e7 => 108
	i64 8059634771736097245, ; 314: ExoPlayer.Decoder => 0x6fd9931f84b771dd => 243
	i64 8064050204834738623, ; 315: System.Collections.dll => 0x6fe942efa61731bf => 12
	i64 8083354569033831015, ; 316: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 286
	i64 8085230611270010360, ; 317: System.Net.Http.Json.dll => 0x703482674fdd05f8 => 63
	i64 8087206902342787202, ; 318: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 27
	i64 8103644804370223335, ; 319: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 23
	i64 8108129031893776750, ; 320: ko\Microsoft.Maui.Controls.resources.dll => 0x7085dc65530f796e => 356
	i64 8113615946733131500, ; 321: System.Reflection.Extensions => 0x70995ab73cf916ec => 93
	i64 8167236081217502503, ; 322: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 166
	i64 8185542183669246576, ; 323: System.Collections => 0x7198e33f4794aa70 => 12
	i64 8187640529827139739, ; 324: Xamarin.KotlinX.Coroutines.Android => 0x71a057ae90f0109b => 338
	i64 8246048515196606205, ; 325: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 213
	i64 8264926008854159966, ; 326: System.Diagnostics.Process.dll => 0x72b2ea6a64a3a25e => 29
	i64 8290740647658429042, ; 327: System.Runtime.Extensions => 0x730ea0b15c929a72 => 103
	i64 8318905602908530212, ; 328: System.ComponentModel.DataAnnotations => 0x7372b092055ea624 => 14
	i64 8368701292315763008, ; 329: System.Security.Cryptography => 0x7423997c6fd56140 => 126
	i64 8386351099740279196, ; 330: zh-HK\Microsoft.Maui.Controls.resources.dll => 0x74624de475b9d19c => 371
	i64 8398329775253868912, ; 331: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x748cdc6f3097d170 => 269
	i64 8400357532724379117, ; 332: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 302
	i64 8410671156615598628, ; 333: System.Reflection.Emit.Lightweight.dll => 0x74b8b4daf4b25224 => 91
	i64 8426919725312979251, ; 334: Xamarin.AndroidX.Lifecycle.Process => 0x74f26ed7aa033133 => 290
	i64 8518412311883997971, ; 335: System.Collections.Immutable => 0x76377add7c28e313 => 9
	i64 8563666267364444763, ; 336: System.Private.Uri => 0x76d841191140ca5b => 86
	i64 8598790081731763592, ; 337: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x77550a055fc61d88 => 280
	i64 8599632406834268464, ; 338: CommunityToolkit.Maui => 0x7758081c784b4930 => 173
	i64 8601935802264776013, ; 339: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 314
	i64 8611142787134128904, ; 340: Microsoft.AspNetCore.Hosting.Server.Abstractions.dll => 0x7780ecbdb94c0308 => 184
	i64 8623059219396073920, ; 341: System.Net.Quic.dll => 0x77ab42ac514299c0 => 71
	i64 8626175481042262068, ; 342: Java.Interop => 0x77b654e585b55834 => 166
	i64 8638972117149407195, ; 343: Microsoft.CSharp.dll => 0x77e3cb5e8b31d7db => 1
	i64 8639588376636138208, ; 344: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 301
	i64 8648495978913578441, ; 345: Microsoft.Win32.Registry.dll => 0x7805a1456889bdc9 => 5
	i64 8677882282824630478, ; 346: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 361
	i64 8684531736582871431, ; 347: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 44
	i64 8690461831448123647, ; 348: ExoPlayer.Hls => 0x789ab8fddd8e58ff => 245
	i64 8725526185868997716, ; 349: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 27
	i64 8834232125107082525, ; 350: ExCSS => 0x7a997f4fe05a151d => 177
	i64 8853378295825400934, ; 351: Xamarin.Kotlin.StdLib.Common.dll => 0x7add84a720d38466 => 335
	i64 8941376889969657626, ; 352: System.Xml.XDocument => 0x7c1626e87187471a => 156
	i64 8951477988056063522, ; 353: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0x7c3a09cd9ccf5e22 => 304
	i64 8954753533646919997, ; 354: System.Runtime.Serialization.Json => 0x7c45ace50032d93d => 112
	i64 9045785047181495996, ; 355: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 371
	i64 9111603110219107042, ; 356: Microsoft.Extensions.Caching.Memory => 0x7e72eac0def44ae2 => 191
	i64 9138683372487561558, ; 357: System.Security.Cryptography.Csp => 0x7ed3201bc3e3d156 => 121
	i64 9214933258326717790, ; 358: GoogleMapsApi.dll => 0x7fe204f9c37f355e => 181
	i64 9225789786819666723, ; 359: ExoPlayer.SmoothStreaming => 0x800896ee47d02323 => 247
	i64 9248940107580716988, ; 360: Svg.Custom.dll => 0x805ad6065d3637bc => 228
	i64 9250544137016314866, ; 361: Microsoft.EntityFrameworkCore => 0x806088e191ee0bf2 => 187
	i64 9286073997824813334, ; 362: BouncyCastle.Cryptography => 0x80dec319ee56e916 => 172
	i64 9312692141327339315, ; 363: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 319
	i64 9324707631942237306, ; 364: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 259
	i64 9334813198578103615, ; 365: SkiaSharp.Extended.UI => 0x818beb2569e0353f => 220
	i64 9363564275759486853, ; 366: el\Microsoft.Maui.Controls.resources.dll => 0x81f21019382e9785 => 345
	i64 9393449387701355454, ; 367: FFImageLoading.Maui => 0x825c3c73118cc3be => 178
	i64 9413000421947348542, ; 368: Microsoft.AspNetCore.Hosting.Abstractions.dll => 0x82a1b202f4c6163e => 183
	i64 9468215723722196442, ; 369: System.Xml.XPath.XDocument.dll => 0x8365dc09353ac5da => 157
	i64 9551379474083066910, ; 370: uk\Microsoft.Maui.Controls.resources.dll => 0x848d5106bbadb41e => 369
	i64 9554839972845591462, ; 371: System.ServiceModel.Web => 0x84999c54e32a1ba6 => 131
	i64 9575902398040817096, ; 372: Xamarin.Google.Crypto.Tink.Android.dll => 0x84e4707ee708bdc8 => 324
	i64 9584643793929893533, ; 373: System.IO.dll => 0x85037ebfbbd7f69d => 57
	i64 9650158550865574924, ; 374: Microsoft.Extensions.Configuration.Json => 0x85ec4012c28a7c0c => 196
	i64 9659729154652888475, ; 375: System.Text.RegularExpressions => 0x860e407c9991dd9b => 136
	i64 9662334977499516867, ; 376: System.Numerics.dll => 0x8617827802b0cfc3 => 83
	i64 9667360217193089419, ; 377: System.Diagnostics.StackTrace => 0x86295ce5cd89898b => 30
	i64 9678050649315576968, ; 378: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 270
	i64 9702891218465930390, ; 379: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 10
	i64 9711637524876806384, ; 380: Xamarin.AndroidX.Media.dll => 0x86c6aadfd9a2c8f0 => 298
	i64 9722368759809762166, ; 381: ExoPlayer.Core => 0x86eccae02fd0e376 => 239
	i64 9773637193738963987, ; 382: ca\Microsoft.Maui.Controls.resources.dll => 0x87a2ef3ea85b4c13 => 341
	i64 9780093022148426479, ; 383: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x87b9dec9576efaef => 321
	i64 9808709177481450983, ; 384: Mono.Android.dll => 0x881f890734e555e7 => 169
	i64 9825649861376906464, ; 385: Xamarin.AndroidX.Concurrent.Futures => 0x885bb87d8abc94e0 => 267
	i64 9834056768316610435, ; 386: System.Transactions.dll => 0x8879968718899783 => 148
	i64 9836529246295212050, ; 387: System.Reflection.Metadata => 0x88825f3bbc2ac012 => 94
	i64 9875200773399460291, ; 388: Xamarin.GooglePlayServices.Base.dll => 0x890bc2c8482339c3 => 329
	i64 9907349773706910547, ; 389: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x897dfa20b758db53 => 280
	i64 9933555792566666578, ; 390: System.Linq.Queryable.dll => 0x89db145cf475c552 => 60
	i64 9944345468791389265, ; 391: ExoPlayer.Core.dll => 0x8a01698437137c51 => 239
	i64 9956195530459977388, ; 392: Microsoft.Maui => 0x8a2b8315b36616ac => 211
	i64 9974604633896246661, ; 393: System.Xml.Serialization.dll => 0x8a6cea111a59dd85 => 155
	i64 10038780035334861115, ; 394: System.Net.Http.dll => 0x8b50e941206af13b => 64
	i64 10051358222726253779, ; 395: System.Private.Xml => 0x8b7d990c97ccccd3 => 88
	i64 10075958396420552332, ; 396: ExoPlayer => 0x8bd4fec6de42f68c => 236
	i64 10078727084704864206, ; 397: System.Net.WebSockets.Client => 0x8bded4e257f117ce => 79
	i64 10089571585547156312, ; 398: System.IO.FileSystem.AccessControl => 0x8c055be67469bb58 => 47
	i64 10092835686693276772, ; 399: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 208
	i64 10099427421688105860, ; 400: ExoPlayer.Container.dll => 0x8c285fbb208f0b84 => 238
	i64 10105485790837105934, ; 401: System.Threading.Tasks.Parallel => 0x8c3de5c91d9a650e => 141
	i64 10143853363526200146, ; 402: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 343
	i64 10205853378024263619, ; 403: Microsoft.Extensions.Configuration.Binder => 0x8da279930adb4fc3 => 194
	i64 10209869394718195525, ; 404: nl\Microsoft.Maui.Controls.resources.dll => 0x8db0be1ecb4f7f45 => 359
	i64 10226222362177979215, ; 405: Xamarin.Kotlin.StdLib.Jdk7 => 0x8dead70ebbc6434f => 336
	i64 10229024438826829339, ; 406: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 274
	i64 10236703004850800690, ; 407: System.Net.ServicePoint.dll => 0x8e101325834e4832 => 74
	i64 10243523786148452761, ; 408: Microsoft.AspNetCore.Http.Abstractions => 0x8e284e9c69a49999 => 185
	i64 10245369515835430794, ; 409: System.Reflection.Emit.Lightweight => 0x8e2edd4ad7fc978a => 91
	i64 10321854143672141184, ; 410: Xamarin.Jetbrains.Annotations.dll => 0x8f3e97a7f8f8c580 => 333
	i64 10360651442923773544, ; 411: System.Text.Encoding => 0x8fc86d98211c1e68 => 135
	i64 10364469296367737616, ; 412: System.Reflection.Emit.ILGeneration.dll => 0x8fd5fde967711b10 => 90
	i64 10376576884623852283, ; 413: Xamarin.AndroidX.Tracing.Tracing => 0x900101b2f888c2fb => 313
	i64 10406448008575299332, ; 414: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 339
	i64 10430153318873392755, ; 415: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 271
	i64 10506226065143327199, ; 416: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 341
	i64 10546663366131771576, ; 417: System.Runtime.Serialization.Json.dll => 0x925d4673efe8e8b8 => 112
	i64 10566960649245365243, ; 418: System.Globalization.dll => 0x92a562b96dcd13fb => 42
	i64 10595762989148858956, ; 419: System.Xml.XPath.XDocument => 0x930bb64cc472ea4c => 157
	i64 10670374202010151210, ; 420: Microsoft.Win32.Primitives.dll => 0x9414c8cd7b4ea92a => 4
	i64 10714184849103829812, ; 421: System.Runtime.Extensions.dll => 0x94b06e5aa4b4bb34 => 103
	i64 10742296988234215840, ; 422: BCrypt.Net-Core.dll => 0x95144e321772f1a0 => 171
	i64 10761706286639228993, ; 423: zh-Hant\Microsoft.Maui.Controls.resources.dll => 0x955942d988382841 => 373
	i64 10785150219063592792, ; 424: System.Net.Primitives => 0x95ac8cfb68830758 => 70
	i64 10811915265162633087, ; 425: Microsoft.EntityFrameworkCore.Relational.dll => 0x960ba3a651a45f7f => 189
	i64 10822644899632537592, ; 426: System.Linq.Queryable => 0x9631c23204ca5ff8 => 60
	i64 10830817578243619689, ; 427: System.Formats.Tar => 0x964ecb340a447b69 => 39
	i64 10847732767863316357, ; 428: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 261
	i64 10880838204485145808, ; 429: CommunityToolkit.Maui.dll => 0x970080b2a4d614d0 => 173
	i64 10899834349646441345, ; 430: System.Web => 0x9743fd975946eb81 => 151
	i64 10943875058216066601, ; 431: System.IO.UnmanagedMemoryStream.dll => 0x97e07461df39de29 => 56
	i64 10964653383833615866, ; 432: System.Diagnostics.Tracing => 0x982a4628ccaffdfa => 34
	i64 11002576679268595294, ; 433: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 204
	i64 11009005086950030778, ; 434: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 211
	i64 11019817191295005410, ; 435: Xamarin.AndroidX.Annotation.Jvm.dll => 0x98ee415998e1b2e2 => 258
	i64 11023048688141570732, ; 436: System.Core => 0x98f9bc61168392ac => 21
	i64 11037814507248023548, ; 437: System.Xml => 0x992e31d0412bf7fc => 161
	i64 11050168729868392624, ; 438: Microsoft.AspNetCore.Http.Features => 0x995a15e9dbef58b0 => 186
	i64 11071824625609515081, ; 439: Xamarin.Google.ErrorProne.Annotations => 0x99a705d600e0a049 => 325
	i64 11103762113411436187, ; 440: ExoPlayer.Container => 0x9a187ccfd8544e9b => 238
	i64 11103970607964515343, ; 441: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 352
	i64 11136029745144976707, ; 442: Jsr305Binding.dll => 0x9a8b200d4f8cd543 => 323
	i64 11156122287428000958, ; 443: th\Microsoft.Maui.Controls.resources.dll => 0x9ad2821cdcf6dcbe => 367
	i64 11157797727133427779, ; 444: fr\Microsoft.Maui.Controls.resources.dll => 0x9ad875ea9172e843 => 348
	i64 11162124722117608902, ; 445: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 318
	i64 11188319605227840848, ; 446: System.Threading.Overlapped => 0x9b44e5671724e550 => 138
	i64 11216600183782743182, ; 447: Svg.Model => 0x9ba95e7065f39c8e => 229
	i64 11220793807500858938, ; 448: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 355
	i64 11226290749488709958, ; 449: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 205
	i64 11235648312900863002, ; 450: System.Reflection.DispatchProxy.dll => 0x9bed0a9c8fac441a => 89
	i64 11329751333533450475, ; 451: System.Threading.Timer.dll => 0x9d3b5ccf6cc500eb => 145
	i64 11340910727871153756, ; 452: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 273
	i64 11347436699239206956, ; 453: System.Xml.XmlSerializer.dll => 0x9d7a318e8162502c => 160
	i64 11366194298415195693, ; 454: CommunityToolkit.Maui.MediaElement => 0x9dbcd57e651ba62d => 175
	i64 11387716764763632936, ; 455: ExoPlayer.dll => 0x9e094c10167f3528 => 236
	i64 11392833485892708388, ; 456: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 303
	i64 11432101114902388181, ; 457: System.AppContext => 0x9ea6fb64e61a9dd5 => 6
	i64 11446671985764974897, ; 458: Mono.Android.Export => 0x9edabf8623efc131 => 167
	i64 11448276831755070604, ; 459: System.Diagnostics.TextWriterTraceListener => 0x9ee0731f77186c8c => 31
	i64 11485890710487134646, ; 460: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 107
	i64 11508496261504176197, ; 461: Xamarin.AndroidX.Fragment.Ktx.dll => 0x9fb664600dde1045 => 283
	i64 11513602507638267977, ; 462: System.IO.Pipelines.dll => 0x9fc8887aa0d36049 => 231
	i64 11518296021396496455, ; 463: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 353
	i64 11529969570048099689, ; 464: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 318
	i64 11530571088791430846, ; 465: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 203
	i64 11580057168383206117, ; 466: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 256
	i64 11591352189662810718, ; 467: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0xa0dcc167234c525e => 311
	i64 11597940890313164233, ; 468: netstandard => 0xa0f429ca8d1805c9 => 165
	i64 11672361001936329215, ; 469: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 284
	i64 11687474876782398100, ; 470: ExoPlayer.Decoder.dll => 0xa232407a3feaca94 => 243
	i64 11692977985522001935, ; 471: System.Threading.Overlapped.dll => 0xa245cd869980680f => 138
	i64 11707554492040141440, ; 472: System.Linq.Parallel.dll => 0xa27996c7fe94da80 => 59
	i64 11743665907891708234, ; 473: System.Threading.Tasks => 0xa2f9e1ec30c0214a => 142
	i64 11855031688536399763, ; 474: vi\Microsoft.Maui.Controls.resources.dll => 0xa485888294361f93 => 370
	i64 11991047634523762324, ; 475: System.Net => 0xa668c24ad493ae94 => 81
	i64 12040886584167504988, ; 476: System.Net.ServicePoint => 0xa719d28d8e121c5c => 74
	i64 12048689113179125827, ; 477: Microsoft.Extensions.FileProviders.Physical => 0xa7358ae968287843 => 200
	i64 12058074296353848905, ; 478: Microsoft.Extensions.FileSystemGlobbing.dll => 0xa756e2afa5707e49 => 201
	i64 12063623837170009990, ; 479: System.Security => 0xa76a99f6ce740786 => 130
	i64 12096697103934194533, ; 480: System.Diagnostics.Contracts => 0xa7e019eccb7e8365 => 25
	i64 12102847907131387746, ; 481: System.Buffers => 0xa7f5f40c43256f62 => 7
	i64 12123043025855404482, ; 482: System.Reflection.Extensions.dll => 0xa83db366c0e359c2 => 93
	i64 12137774235383566651, ; 483: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 315
	i64 12145679461940342714, ; 484: System.Text.Json => 0xa88e1f1ebcb62fba => 234
	i64 12191646537372739477, ; 485: Xamarin.Android.Glide.dll => 0xa9316dee7f392795 => 250
	i64 12201331334810686224, ; 486: System.Runtime.Serialization.Primitives.dll => 0xa953d6341e3bd310 => 113
	i64 12269460666702402136, ; 487: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 9
	i64 12313367145828839434, ; 488: System.IO.Pipelines => 0xaae1de2e1c17f00a => 231
	i64 12332222936682028543, ; 489: System.Runtime.Handles => 0xab24db6c07db5dff => 104
	i64 12341818387765915815, ; 490: CommunityToolkit.Maui.Core.dll => 0xab46f26f152bf0a7 => 174
	i64 12375446203996702057, ; 491: System.Configuration.dll => 0xabbe6ac12e2e0569 => 19
	i64 12451044538927396471, ; 492: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 282
	i64 12466513435562512481, ; 493: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 296
	i64 12475113361194491050, ; 494: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 375
	i64 12487638416075308985, ; 495: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 276
	i64 12517810545449516888, ; 496: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 33
	i64 12538491095302438457, ; 497: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 264
	i64 12550732019250633519, ; 498: System.IO.Compression => 0xae2d28465e8e1b2f => 46
	i64 12699999919562409296, ; 499: System.Diagnostics.StackTrace.dll => 0xb03f76a3ad01c550 => 30
	i64 12700543734426720211, ; 500: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 265
	i64 12708238894395270091, ; 501: System.IO => 0xb05cbbf17d3ba3cb => 57
	i64 12708922737231849740, ; 502: System.Text.Encoding.Extensions => 0xb05f29e50e96e90c => 134
	i64 12717050818822477433, ; 503: System.Runtime.Serialization.Xml.dll => 0xb07c0a5786811679 => 114
	i64 12753841065332862057, ; 504: Xamarin.AndroidX.Window => 0xb0febee04cf46c69 => 320
	i64 12828192437253469131, ; 505: Xamarin.Kotlin.StdLib.Jdk8.dll => 0xb206e50e14d873cb => 337
	i64 12835242264250840079, ; 506: System.IO.Pipes => 0xb21ff0d5d6c0740f => 55
	i64 12843321153144804894, ; 507: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 206
	i64 12843770487262409629, ; 508: System.AppContext.dll => 0xb23e3d357debf39d => 6
	i64 12859557719246324186, ; 509: System.Net.WebHeaderCollection.dll => 0xb276539ce04f41da => 77
	i64 12982280885948128408, ; 510: Xamarin.AndroidX.CustomView.PoolingContainer => 0xb42a53aec5481c98 => 275
	i64 12989346753972519995, ; 511: ar\Microsoft.Maui.Controls.resources.dll => 0xb4436e0d5ee7c43b => 340
	i64 13005833372463390146, ; 512: pt-BR\Microsoft.Maui.Controls.resources.dll => 0xb47e008b5d99ddc2 => 361
	i64 13068258254871114833, ; 513: System.Runtime.Serialization.Formatters.dll => 0xb55bc7a4eaa8b451 => 111
	i64 13106026140046202731, ; 514: HarfBuzzSharp.dll => 0xb5e1f555ee70176b => 182
	i64 13129914918964716986, ; 515: Xamarin.AndroidX.Emoji2.dll => 0xb636d40db3fe65ba => 279
	i64 13173818576982874404, ; 516: System.Runtime.CompilerServices.VisualC.dll => 0xb6d2ce32a8819924 => 102
	i64 13343850469010654401, ; 517: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 168
	i64 13370592475155966277, ; 518: System.Runtime.Serialization => 0xb98de304062ea945 => 115
	i64 13381594904270902445, ; 519: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 349
	i64 13385736475199088545, ; 520: ExoPlayer.Extractor => 0xb9c3b0674d3b27a1 => 244
	i64 13401370062847626945, ; 521: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 315
	i64 13402939433517888790, ; 522: Xamarin.Google.Guava.FailureAccess => 0xba00ce6728e8b516 => 327
	i64 13404347523447273790, ; 523: Xamarin.AndroidX.ConstraintLayout.Core => 0xba05cf0da4f6393e => 269
	i64 13404984788036896679, ; 524: Microsoft.AspNetCore.Http.Abstractions.dll => 0xba0812a45e7447a7 => 185
	i64 13431476299110033919, ; 525: System.Net.WebClient => 0xba663087f18829ff => 76
	i64 13454009404024712428, ; 526: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 328
	i64 13463706743370286408, ; 527: System.Private.DataContractSerialization.dll => 0xbad8b1f3069e0548 => 85
	i64 13465488254036897740, ; 528: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 334
	i64 13491513212026656886, ; 529: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 262
	i64 13540124433173649601, ; 530: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 370
	i64 13550417756503177631, ; 531: Microsoft.Extensions.FileProviders.Abstractions.dll => 0xbc0cc1280684799f => 199
	i64 13572454107664307259, ; 532: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 305
	i64 13578472628727169633, ; 533: System.Xml.XPath => 0xbc706ce9fba5c261 => 158
	i64 13580399111273692417, ; 534: Microsoft.VisualBasic.Core.dll => 0xbc77450a277fbd01 => 2
	i64 13621154251410165619, ; 535: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0xbd080f9faa1acf73 => 275
	i64 13647894001087880694, ; 536: System.Data.dll => 0xbd670f48cb071df6 => 24
	i64 13675589307506966157, ; 537: Xamarin.AndroidX.Activity.Ktx => 0xbdc97404d0153e8d => 255
	i64 13702626353344114072, ; 538: System.Diagnostics.Tools.dll => 0xbe29821198fb6d98 => 32
	i64 13710614125866346983, ; 539: System.Security.AccessControl.dll => 0xbe45e2e7d0b769e7 => 117
	i64 13713329104121190199, ; 540: System.Dynamic.Runtime => 0xbe4f8829f32b5737 => 37
	i64 13717397318615465333, ; 541: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 16
	i64 13768883594457632599, ; 542: System.IO.IsolatedStorage => 0xbf14e6adb159cf57 => 52
	i64 13828521679616088467, ; 543: Xamarin.Kotlin.StdLib.Common => 0xbfe8c733724e1993 => 335
	i64 13865727802090930648, ; 544: Xamarin.Google.Guava.dll => 0xc06cf5f8e3e341d8 => 326
	i64 13881769479078963060, ; 545: System.Console.dll => 0xc0a5f3cade5c6774 => 20
	i64 13911222732217019342, ; 546: System.Security.Cryptography.OpenSsl.dll => 0xc10e975ec1226bce => 123
	i64 13928444506500929300, ; 547: System.Windows.dll => 0xc14bc67b8bba9714 => 152
	i64 13959074834287824816, ; 548: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 282
	i64 13975254687929967048, ; 549: Xamarin.Google.Guava => 0xc1f2141837ada1c8 => 326
	i64 13982638193275851912, ; 550: ExoPlayer.Hls.dll => 0xc20c4f5a85045488 => 245
	i64 14075334701871371868, ; 551: System.ServiceModel.Web.dll => 0xc355a25647c5965c => 131
	i64 14124974489674258913, ; 552: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 264
	i64 14125464355221830302, ; 553: System.Threading.dll => 0xc407bafdbc707a9e => 146
	i64 14133832980772275001, ; 554: Microsoft.EntityFrameworkCore.dll => 0xc425763635a1c339 => 187
	i64 14178052285788134900, ; 555: Xamarin.Android.Glide.Annotations.dll => 0xc4c28f6f75511df4 => 251
	i64 14212104595480609394, ; 556: System.Security.Cryptography.Cng.dll => 0xc53b89d4a4518272 => 120
	i64 14220608275227875801, ; 557: System.Diagnostics.FileVersionInfo.dll => 0xc559bfe1def019d9 => 28
	i64 14226382999226559092, ; 558: System.ServiceProcess => 0xc56e43f6938e2a74 => 132
	i64 14232023429000439693, ; 559: System.Resources.Writer.dll => 0xc5824de7789ba78d => 100
	i64 14254574811015963973, ; 560: System.Text.Encoding.Extensions.dll => 0xc5d26c4442d66545 => 134
	i64 14261073672896646636, ; 561: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 303
	i64 14298246716367104064, ; 562: System.Web.dll => 0xc66d93a217f4e840 => 151
	i64 14327695147300244862, ; 563: System.Reflection.dll => 0xc6d632d338eb4d7e => 97
	i64 14327709162229390963, ; 564: System.Security.Cryptography.X509Certificates => 0xc6d63f9253cade73 => 125
	i64 14331727281556788554, ; 565: Xamarin.Android.Glide.DiskLruCache.dll => 0xc6e48607a2f7954a => 252
	i64 14346402571976470310, ; 566: System.Net.Ping.dll => 0xc718a920f3686f26 => 69
	i64 14349907877893689639, ; 567: ro\Microsoft.Maui.Controls.resources.dll => 0xc7251d2f956ed127 => 363
	i64 14461014870687870182, ; 568: System.Net.Requests.dll => 0xc8afd8683afdece6 => 72
	i64 14464374589798375073, ; 569: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 364
	i64 14486659737292545672, ; 570: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 287
	i64 14491877563792607819, ; 571: zh-Hans\Microsoft.Maui.Controls.resources.dll => 0xc91d7ddcee4fca4b => 372
	i64 14495724990987328804, ; 572: Xamarin.AndroidX.ResourceInspection.Annotation => 0xc92b2913e18d5d24 => 306
	i64 14551742072151931844, ; 573: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 233
	i64 14552901170081803662, ; 574: SkiaSharp.Views.Maui.Core => 0xc9f64a827617ad8e => 227
	i64 14556034074661724008, ; 575: CommunityToolkit.Maui.Core => 0xca016bdea6b19f68 => 174
	i64 14561513370130550166, ; 576: System.Security.Cryptography.Primitives.dll => 0xca14e3428abb8d96 => 124
	i64 14574160591280636898, ; 577: System.Net.Quic => 0xca41d1d72ec783e2 => 71
	i64 14610046442689856844, ; 578: cs\Microsoft.Maui.Controls.resources.dll => 0xcac14fd5107d054c => 342
	i64 14622043554576106986, ; 579: System.Runtime.Serialization.Formatters => 0xcaebef2458cc85ea => 111
	i64 14644440854989303794, ; 580: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 297
	i64 14669215534098758659, ; 581: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 197
	i64 14690985099581930927, ; 582: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 150
	i64 14705122255218365489, ; 583: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 356
	i64 14735017007120366644, ; 584: ja\Microsoft.Maui.Controls.resources.dll => 0xcc7d4be604bfbc34 => 355
	i64 14744092281598614090, ; 585: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 372
	i64 14792063746108907174, ; 586: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 328
	i64 14832630590065248058, ; 587: System.Security.Claims => 0xcdd816ef5d6e873a => 118
	i64 14852515768018889994, ; 588: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 273
	i64 14889905118082851278, ; 589: GoogleGson.dll => 0xcea391d0969961ce => 180
	i64 14904040806490515477, ; 590: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 340
	i64 14912225920358050525, ; 591: System.Security.Principal.Windows => 0xcef2de7759506add => 127
	i64 14931407803744742450, ; 592: HarfBuzzSharp => 0xcf3704499ab36c32 => 182
	i64 14935719434541007538, ; 593: System.Text.Encoding.CodePages.dll => 0xcf4655b160b702b2 => 133
	i64 14954917835170835695, ; 594: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 198
	i64 14984936317414011727, ; 595: System.Net.WebHeaderCollection => 0xcff5302fe54ff34f => 77
	i64 14987728460634540364, ; 596: System.IO.Compression.dll => 0xcfff1ba06622494c => 46
	i64 14988210264188246988, ; 597: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 276
	i64 15015154896917945444, ; 598: System.Net.Security.dll => 0xd0608bd33642dc64 => 73
	i64 15024878362326791334, ; 599: System.Net.Http.Json => 0xd0831743ebf0f4a6 => 63
	i64 15071021337266399595, ; 600: System.Resources.Reader.dll => 0xd127060e7a18a96b => 98
	i64 15076659072870671916, ; 601: System.ObjectModel.dll => 0xd13b0d8c1620662c => 84
	i64 15101927338945785474, ; 602: SkiaSharp.SceneGraph => 0xd194d2e6bd9fae82 => 222
	i64 15111608613780139878, ; 603: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 357
	i64 15115185479366240210, ; 604: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 43
	i64 15133485256822086103, ; 605: System.Linq.dll => 0xd204f0a9127dd9d7 => 61
	i64 15150743910298169673, ; 606: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xd2424150783c3149 => 304
	i64 15203009853192377507, ; 607: pt\Microsoft.Maui.Controls.resources.dll => 0xd2fbf0e9984b94a3 => 362
	i64 15227001540531775957, ; 608: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 193
	i64 15234786388537674379, ; 609: System.Dynamic.Runtime.dll => 0xd36cd580c5be8a8b => 37
	i64 15250465174479574862, ; 610: System.Globalization.Calendars.dll => 0xd3a489469852174e => 40
	i64 15272359115529052076, ; 611: Xamarin.AndroidX.Collection.Ktx => 0xd3f251b2fb4edfac => 266
	i64 15279429628684179188, ; 612: Xamarin.KotlinX.Coroutines.Android.dll => 0xd40b704b1c4c96f4 => 338
	i64 15299439993936780255, ; 613: System.Xml.XPath.dll => 0xd452879d55019bdf => 158
	i64 15338463749992804988, ; 614: System.Resources.Reader => 0xd4dd2b839286f27c => 98
	i64 15370334346939861994, ; 615: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 271
	i64 15391712275433856905, ; 616: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 198
	i64 15526743539506359484, ; 617: System.Text.Encoding.dll => 0xd77a12fc26de2cbc => 135
	i64 15527772828719725935, ; 618: System.Console => 0xd77dbb1e38cd3d6f => 20
	i64 15530465045505749832, ; 619: System.Net.HttpListener.dll => 0xd7874bacc9fdb348 => 65
	i64 15536481058354060254, ; 620: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 344
	i64 15541854775306130054, ; 621: System.Security.Cryptography.X509Certificates.dll => 0xd7afc292e8d49286 => 125
	i64 15557562860424774966, ; 622: System.Net.Sockets => 0xd7e790fe7a6dc536 => 75
	i64 15582737692548360875, ; 623: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 295
	i64 15609085926864131306, ; 624: System.dll => 0xd89e9cf3334914ea => 162
	i64 15620612276725577442, ; 625: BouncyCastle.Cryptography.dll => 0xd8c7901aa85576e2 => 172
	i64 15661133872274321916, ; 626: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 154
	i64 15710114879900314733, ; 627: Microsoft.Win32.Registry => 0xda058a3f5d096c6d => 5
	i64 15755368083429170162, ; 628: System.IO.FileSystem.Primitives => 0xdaa64fcbde529bf2 => 49
	i64 15777549416145007739, ; 629: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 310
	i64 15783653065526199428, ; 630: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 345
	i64 15809615578106971996, ; 631: Xabe.FFmpeg.dll => 0xdb67099af88c475c => 235
	i64 15817206913877585035, ; 632: System.Threading.Tasks.dll => 0xdb8201e29086ac8b => 142
	i64 15827202283623377193, ; 633: Microsoft.Extensions.Configuration.Json.dll => 0xdba5849eef9f6929 => 196
	i64 15847085070278954535, ; 634: System.Threading.Channels.dll => 0xdbec27e8f35f8e27 => 137
	i64 15852824340364052161, ; 635: Microsoft.AspNetCore.Http.Features.dll => 0xdc008bbee610c6c1 => 186
	i64 15885744048853936810, ; 636: System.Resources.Writer => 0xdc75800bd0b6eaaa => 100
	i64 15928521404965645318, ; 637: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 207
	i64 15930129725311349754, ; 638: Xamarin.GooglePlayServices.Tasks.dll => 0xdd1330956f12f3fa => 332
	i64 15934062614519587357, ; 639: System.Security.Cryptography.OpenSsl => 0xdd2129868f45a21d => 123
	i64 15937190497610202713, ; 640: System.Security.Cryptography.Cng => 0xdd2c465197c97e59 => 120
	i64 15963349826457351533, ; 641: System.Threading.Tasks.Extensions => 0xdd893616f748b56d => 140
	i64 15971679995444160383, ; 642: System.Formats.Tar.dll => 0xdda6ce5592a9677f => 39
	i64 16018552496348375205, ; 643: System.Net.NetworkInformation.dll => 0xde4d54a020caa8a5 => 68
	i64 16048255734569022341, ; 644: ExoPlayer.Transformer => 0xdeb6db90339cb385 => 248
	i64 16054465462676478687, ; 645: System.Globalization.Extensions => 0xdecceb47319bdadf => 41
	i64 16056281320585338352, ; 646: ru\Microsoft.Maui.Controls.resources.dll => 0xded35eca8f3a9df0 => 364
	i64 16069846902195211555, ; 647: ExoPlayer.DataSource.dll => 0xdf03909da841cd23 => 242
	i64 16154507427712707110, ; 648: System => 0xe03056ea4e39aa26 => 162
	i64 16182611612321266217, ; 649: Microsoft.Maui.Maps => 0xe0942f85b2853a29 => 214
	i64 16219561732052121626, ; 650: System.Net.Security => 0xe1177575db7c781a => 73
	i64 16288847719894691167, ; 651: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 358
	i64 16315482530584035869, ; 652: WindowsBase.dll => 0xe26c3ceb1e8d821d => 163
	i64 16321164108206115771, ; 653: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 204
	i64 16324796876805858114, ; 654: SkiaSharp.dll => 0xe28d5444586b6342 => 218
	i64 16337011941688632206, ; 655: System.Security.Principal.Windows.dll => 0xe2b8b9cdc3aa638e => 127
	i64 16361933716545543812, ; 656: Xamarin.AndroidX.ExifInterface.dll => 0xe3114406a52f1e84 => 281
	i64 16423015068819898779, ; 657: Xamarin.Kotlin.StdLib.Jdk8 => 0xe3ea453135e5c19b => 337
	i64 16454459195343277943, ; 658: System.Net.NetworkInformation => 0xe459fb756d988f77 => 68
	i64 16496768397145114574, ; 659: Mono.Android.Export.dll => 0xe4f04b741db987ce => 167
	i64 16579050217386591297, ; 660: Xamarin.Google.Guava.FailureAccess.dll => 0xe6149e5548b0c441 => 327
	i64 16585556079983149983, ; 661: FFmpeg.AutoGen => 0xe62bbb6175b7439f => 179
	i64 16589693266713801121, ; 662: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xe63a6e214f2a71a1 => 294
	i64 16621146507174665210, ; 663: Xamarin.AndroidX.ConstraintLayout => 0xe6aa2caf87dedbfa => 268
	i64 16648892297579399389, ; 664: CommunityToolkit.Mvvm => 0xe70cbf55c4f508dd => 176
	i64 16649148416072044166, ; 665: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 213
	i64 16677317093839702854, ; 666: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 302
	i64 16702652415771857902, ; 667: System.ValueTuple => 0xe7cbbde0b0e6d3ee => 149
	i64 16709499819875633724, ; 668: System.IO.Compression.ZipFile => 0xe7e4118e32240a3c => 45
	i64 16737807731308835127, ; 669: System.Runtime.Intrinsics => 0xe848a3736f733137 => 108
	i64 16758309481308491337, ; 670: System.IO.FileSystem.DriveInfo => 0xe89179af15740e49 => 48
	i64 16762783179241323229, ; 671: System.Reflection.TypeExtensions => 0xe8a15e7d0d927add => 96
	i64 16765015072123548030, ; 672: System.Diagnostics.TextWriterTraceListener.dll => 0xe8a94c621bfe717e => 31
	i64 16803648858859583026, ; 673: ms\Microsoft.Maui.Controls.resources.dll => 0xe9328d9b8ab93632 => 357
	i64 16822611501064131242, ; 674: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 23
	i64 16824837904095110091, ; 675: GoogleMapsApi => 0xe97dd4ee951de3cb => 181
	i64 16833383113903931215, ; 676: mscorlib => 0xe99c30c1484d7f4f => 164
	i64 16856067890322379635, ; 677: System.Data.Common.dll => 0xe9ecc87060889373 => 22
	i64 16890310621557459193, ; 678: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 136
	i64 16933958494752847024, ; 679: System.Net.WebProxy.dll => 0xeb018187f0f3b4b0 => 78
	i64 16942731696432749159, ; 680: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 365
	i64 16961387572093531548, ; 681: SkiaSharp.Extended => 0xeb62f421ac5c359c => 219
	i64 16977952268158210142, ; 682: System.IO.Pipes.AccessControl => 0xeb9dcda2851b905e => 54
	i64 16989020923549080504, ; 683: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xebc52084add25bb8 => 294
	i64 16998075588627545693, ; 684: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 300
	i64 17008137082415910100, ; 685: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 10
	i64 17024911836938395553, ; 686: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 257
	i64 17027804579603049667, ; 687: Microsoft.Maui.Controls.Maps.dll => 0xec4eea0c48026cc3 => 209
	i64 17031351772568316411, ; 688: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 299
	i64 17037200463775726619, ; 689: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 285
	i64 17047433665992082296, ; 690: Microsoft.Extensions.Configuration.FileExtensions => 0xec94a699197e4378 => 195
	i64 17062143951396181894, ; 691: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 16
	i64 17118171214553292978, ; 692: System.Threading.Channels => 0xed8ff6060fc420b2 => 137
	i64 17187273293601214786, ; 693: System.ComponentModel.Annotations.dll => 0xee8575ff9aa89142 => 13
	i64 17201328579425343169, ; 694: System.ComponentModel.EventBasedAsync => 0xeeb76534d96c16c1 => 15
	i64 17202182880784296190, ; 695: System.Security.Cryptography.Encoding.dll => 0xeeba6e30627428fe => 122
	i64 17203614576212522419, ; 696: pl\Microsoft.Maui.Controls.resources.dll => 0xeebf844ef3e135b3 => 360
	i64 17205988430934219272, ; 697: Microsoft.Extensions.FileSystemGlobbing => 0xeec7f35113509a08 => 201
	i64 17212532834920978392, ; 698: GeolocationAds => 0xeedf336ade3657d8 => 0
	i64 17230721278011714856, ; 699: System.Private.Xml.Linq => 0xef1fd1b5c7a72d28 => 87
	i64 17234219099804750107, ; 700: System.Transactions.Local.dll => 0xef2c3ef5e11d511b => 147
	i64 17260702271250283638, ; 701: System.Data.Common => 0xef8a5543bba6bc76 => 22
	i64 17310278548634113468, ; 702: hi\Microsoft.Maui.Controls.resources.dll => 0xf03a76a04e6695bc => 350
	i64 17317119183122232016, ; 703: ToolsLibrary.dll => 0xf052c425a31a72d0 => 374
	i64 17333249706306540043, ; 704: System.Diagnostics.Tracing.dll => 0xf08c12c5bb8b920b => 34
	i64 17338386382517543202, ; 705: System.Net.WebSockets.Client.dll => 0xf09e528d5c6da122 => 79
	i64 17342750010158924305, ; 706: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 350
	i64 17360349973592121190, ; 707: Xamarin.Google.Crypto.Tink.Android => 0xf0ec5a52686b9f66 => 324
	i64 17375848869554566964, ; 708: ExoPlayer.Database.dll => 0xf1236a7c54ac3734 => 241
	i64 17470386307322966175, ; 709: System.Threading.Timer => 0xf27347c8d0d5709f => 145
	i64 17472189583225440952, ; 710: ExoPlayer.Transformer.dll => 0xf279afdab46ecab8 => 248
	i64 17509662556995089465, ; 711: System.Net.WebSockets.dll => 0xf2fed1534ea67439 => 80
	i64 17514990004910432069, ; 712: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 348
	i64 17522591619082469157, ; 713: GoogleGson => 0xf32cc03d27a5bf25 => 180
	i64 17590473451926037903, ; 714: Xamarin.Android.Glide => 0xf41dea67fcfda58f => 250
	i64 17623389608345532001, ; 715: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 360
	i64 17627500474728259406, ; 716: System.Globalization => 0xf4a176498a351f4e => 42
	i64 17671790519499593115, ; 717: SkiaSharp.Views.Android => 0xf53ecfd92be3959b => 224
	i64 17685921127322830888, ; 718: System.Diagnostics.Debug.dll => 0xf571038fafa74828 => 26
	i64 17704177640604968747, ; 719: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 296
	i64 17710060891934109755, ; 720: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 293
	i64 17712670374920797664, ; 721: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 107
	i64 17777860260071588075, ; 722: System.Runtime.Numerics.dll => 0xf6b7a5b72419c0eb => 110
	i64 17808848867378959707, ; 723: SkiaSharp.Skottie.dll => 0xf725bdb086bd955b => 223
	i64 17827813215687577648, ; 724: hr\Microsoft.Maui.Controls.resources.dll => 0xf7691da9f3129030 => 351
	i64 17838668724098252521, ; 725: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 7
	i64 17880566851387315642, ; 726: GeolocationAds.dll => 0xf82488d0e790d5ba => 0
	i64 17891337867145587222, ; 727: Xamarin.Jetbrains.Annotations => 0xf84accff6fb52a16 => 333
	i64 17928294245072900555, ; 728: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 44
	i64 17942426894774770628, ; 729: de\Microsoft.Maui.Controls.resources.dll => 0xf9004e329f771bc4 => 344
	i64 17969331831154222830, ; 730: Xamarin.GooglePlayServices.Maps => 0xf95fe418471126ee => 331
	i64 17986907704309214542, ; 731: Xamarin.GooglePlayServices.Basement.dll => 0xf99e554223166d4e => 330
	i64 17992315986609351877, ; 732: System.Xml.XmlDocument.dll => 0xf9b18c0ffc6eacc5 => 159
	i64 18017743553296241350, ; 733: Microsoft.Extensions.Caching.Abstractions => 0xfa0be24cb44e92c6 => 190
	i64 18025913125965088385, ; 734: System.Threading => 0xfa28e87b91334681 => 146
	i64 18070190158559153715, ; 735: ExoPlayer.Rtsp.dll => 0xfac6363590ad8e33 => 246
	i64 18116111925905154859, ; 736: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 262
	i64 18121036031235206392, ; 737: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 299
	i64 18132221390331549284, ; 738: SkiaSharp.Views.Maui.Controls.Compatibility => 0xfba297492f739664 => 226
	i64 18146411883821974900, ; 739: System.Formats.Asn1.dll => 0xfbd50176eb22c574 => 38
	i64 18146811631844267958, ; 740: System.ComponentModel.EventBasedAsync.dll => 0xfbd66d08820117b6 => 15
	i64 18203743254473369877, ; 741: System.Security.Cryptography.Pkcs.dll => 0xfca0b00ad94c6915 => 232
	i64 18225059387460068507, ; 742: System.Threading.ThreadPool.dll => 0xfcec6af3cff4a49b => 144
	i64 18245806341561545090, ; 743: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 8
	i64 18260797123374478311, ; 744: Xamarin.AndroidX.Emoji2 => 0xfd6b623bde35f3e7 => 279
	i64 18305135509493619199, ; 745: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 301
	i64 18318849532986632368, ; 746: System.Security.dll => 0xfe39a097c37fa8b0 => 130
	i64 18324163916253801303, ; 747: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 354
	i64 18342408478508122430, ; 748: id\Microsoft.Maui.Controls.resources.dll => 0xfe8d53543697013e => 353
	i64 18358161419737137786, ; 749: he\Microsoft.Maui.Controls.resources.dll => 0xfec54a8ba8b6827a => 349
	i64 18380184030268848184, ; 750: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 317
	i64 18439108438687598470 ; 751: System.Reflection.Metadata.dll => 0xffe4df6e2ee1c786 => 94
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [752 x i32] [
	i32 278, ; 0
	i32 177, ; 1
	i32 206, ; 2
	i32 169, ; 3
	i32 212, ; 4
	i32 202, ; 5
	i32 230, ; 6
	i32 58, ; 7
	i32 265, ; 8
	i32 149, ; 9
	i32 307, ; 10
	i32 310, ; 11
	i32 219, ; 12
	i32 272, ; 13
	i32 132, ; 14
	i32 228, ; 15
	i32 56, ; 16
	i32 309, ; 17
	i32 95, ; 18
	i32 214, ; 19
	i32 291, ; 20
	i32 195, ; 21
	i32 129, ; 22
	i32 194, ; 23
	i32 329, ; 24
	i32 143, ; 25
	i32 266, ; 26
	i32 18, ; 27
	i32 352, ; 28
	i32 277, ; 29
	i32 292, ; 30
	i32 148, ; 31
	i32 240, ; 32
	i32 104, ; 33
	i32 95, ; 34
	i32 322, ; 35
	i32 235, ; 36
	i32 36, ; 37
	i32 28, ; 38
	i32 261, ; 39
	i32 300, ; 40
	i32 50, ; 41
	i32 115, ; 42
	i32 220, ; 43
	i32 70, ; 44
	i32 208, ; 45
	i32 65, ; 46
	i32 168, ; 47
	i32 143, ; 48
	i32 321, ; 49
	i32 260, ; 50
	i32 295, ; 51
	i32 285, ; 52
	i32 40, ; 53
	i32 89, ; 54
	i32 81, ; 55
	i32 216, ; 56
	i32 66, ; 57
	i32 62, ; 58
	i32 86, ; 59
	i32 259, ; 60
	i32 106, ; 61
	i32 346, ; 62
	i32 307, ; 63
	i32 102, ; 64
	i32 35, ; 65
	i32 256, ; 66
	i32 368, ; 67
	i32 309, ; 68
	i32 210, ; 69
	i32 176, ; 70
	i32 215, ; 71
	i32 119, ; 72
	i32 293, ; 73
	i32 175, ; 74
	i32 140, ; 75
	i32 139, ; 76
	i32 336, ; 77
	i32 53, ; 78
	i32 35, ; 79
	i32 139, ; 80
	i32 216, ; 81
	i32 253, ; 82
	i32 263, ; 83
	i32 221, ; 84
	i32 188, ; 85
	i32 277, ; 86
	i32 8, ; 87
	i32 14, ; 88
	i32 306, ; 89
	i32 51, ; 90
	i32 288, ; 91
	i32 233, ; 92
	i32 101, ; 93
	i32 246, ; 94
	i32 270, ; 95
	i32 316, ; 96
	i32 116, ; 97
	i32 254, ; 98
	i32 161, ; 99
	i32 367, ; 100
	i32 164, ; 101
	i32 67, ; 102
	i32 197, ; 103
	i32 342, ; 104
	i32 80, ; 105
	i32 101, ; 106
	i32 311, ; 107
	i32 117, ; 108
	i32 347, ; 109
	i32 323, ; 110
	i32 78, ; 111
	i32 322, ; 112
	i32 114, ; 113
	i32 121, ; 114
	i32 48, ; 115
	i32 202, ; 116
	i32 237, ; 117
	i32 226, ; 118
	i32 128, ; 119
	i32 286, ; 120
	i32 257, ; 121
	i32 82, ; 122
	i32 110, ; 123
	i32 75, ; 124
	i32 339, ; 125
	i32 199, ; 126
	i32 330, ; 127
	i32 225, ; 128
	i32 212, ; 129
	i32 53, ; 130
	i32 221, ; 131
	i32 313, ; 132
	i32 192, ; 133
	i32 69, ; 134
	i32 312, ; 135
	i32 191, ; 136
	i32 83, ; 137
	i32 170, ; 138
	i32 362, ; 139
	i32 116, ; 140
	i32 193, ; 141
	i32 154, ; 142
	i32 192, ; 143
	i32 230, ; 144
	i32 251, ; 145
	i32 165, ; 146
	i32 305, ; 147
	i32 278, ; 148
	i32 203, ; 149
	i32 32, ; 150
	i32 200, ; 151
	i32 210, ; 152
	i32 122, ; 153
	i32 72, ; 154
	i32 62, ; 155
	i32 159, ; 156
	i32 113, ; 157
	i32 343, ; 158
	i32 88, ; 159
	i32 207, ; 160
	i32 373, ; 161
	i32 179, ; 162
	i32 105, ; 163
	i32 18, ; 164
	i32 144, ; 165
	i32 118, ; 166
	i32 58, ; 167
	i32 272, ; 168
	i32 17, ; 169
	i32 52, ; 170
	i32 332, ; 171
	i32 183, ; 172
	i32 92, ; 173
	i32 222, ; 174
	i32 368, ; 175
	i32 55, ; 176
	i32 129, ; 177
	i32 150, ; 178
	i32 41, ; 179
	i32 189, ; 180
	i32 92, ; 181
	i32 188, ; 182
	i32 317, ; 183
	i32 50, ; 184
	i32 160, ; 185
	i32 13, ; 186
	i32 290, ; 187
	i32 374, ; 188
	i32 254, ; 189
	i32 312, ; 190
	i32 36, ; 191
	i32 67, ; 192
	i32 109, ; 193
	i32 358, ; 194
	i32 366, ; 195
	i32 255, ; 196
	i32 99, ; 197
	i32 99, ; 198
	i32 11, ; 199
	i32 178, ; 200
	i32 11, ; 201
	i32 297, ; 202
	i32 25, ; 203
	i32 128, ; 204
	i32 249, ; 205
	i32 76, ; 206
	i32 289, ; 207
	i32 109, ; 208
	i32 229, ; 209
	i32 298, ; 210
	i32 316, ; 211
	i32 249, ; 212
	i32 314, ; 213
	i32 106, ; 214
	i32 2, ; 215
	i32 26, ; 216
	i32 268, ; 217
	i32 155, ; 218
	i32 366, ; 219
	i32 21, ; 220
	i32 369, ; 221
	i32 49, ; 222
	i32 43, ; 223
	i32 126, ; 224
	i32 209, ; 225
	i32 258, ; 226
	i32 59, ; 227
	i32 119, ; 228
	i32 319, ; 229
	i32 247, ; 230
	i32 281, ; 231
	i32 267, ; 232
	i32 3, ; 233
	i32 287, ; 234
	i32 308, ; 235
	i32 38, ; 236
	i32 124, ; 237
	i32 308, ; 238
	i32 363, ; 239
	i32 234, ; 240
	i32 147, ; 241
	i32 223, ; 242
	i32 85, ; 243
	i32 90, ; 244
	i32 346, ; 245
	i32 171, ; 246
	i32 242, ; 247
	i32 291, ; 248
	i32 217, ; 249
	i32 375, ; 250
	i32 217, ; 251
	i32 288, ; 252
	i32 215, ; 253
	i32 347, ; 254
	i32 351, ; 255
	i32 263, ; 256
	i32 274, ; 257
	i32 320, ; 258
	i32 205, ; 259
	i32 325, ; 260
	i32 289, ; 261
	i32 240, ; 262
	i32 133, ; 263
	i32 218, ; 264
	i32 96, ; 265
	i32 3, ; 266
	i32 359, ; 267
	i32 105, ; 268
	i32 33, ; 269
	i32 152, ; 270
	i32 156, ; 271
	i32 153, ; 272
	i32 82, ; 273
	i32 184, ; 274
	i32 283, ; 275
	i32 141, ; 276
	i32 87, ; 277
	i32 19, ; 278
	i32 284, ; 279
	i32 232, ; 280
	i32 241, ; 281
	i32 51, ; 282
	i32 331, ; 283
	i32 253, ; 284
	i32 61, ; 285
	i32 54, ; 286
	i32 227, ; 287
	i32 4, ; 288
	i32 97, ; 289
	i32 252, ; 290
	i32 17, ; 291
	i32 153, ; 292
	i32 84, ; 293
	i32 237, ; 294
	i32 29, ; 295
	i32 45, ; 296
	i32 64, ; 297
	i32 66, ; 298
	i32 170, ; 299
	i32 225, ; 300
	i32 292, ; 301
	i32 1, ; 302
	i32 334, ; 303
	i32 354, ; 304
	i32 47, ; 305
	i32 24, ; 306
	i32 260, ; 307
	i32 224, ; 308
	i32 190, ; 309
	i32 365, ; 310
	i32 244, ; 311
	i32 163, ; 312
	i32 108, ; 313
	i32 243, ; 314
	i32 12, ; 315
	i32 286, ; 316
	i32 63, ; 317
	i32 27, ; 318
	i32 23, ; 319
	i32 356, ; 320
	i32 93, ; 321
	i32 166, ; 322
	i32 12, ; 323
	i32 338, ; 324
	i32 213, ; 325
	i32 29, ; 326
	i32 103, ; 327
	i32 14, ; 328
	i32 126, ; 329
	i32 371, ; 330
	i32 269, ; 331
	i32 302, ; 332
	i32 91, ; 333
	i32 290, ; 334
	i32 9, ; 335
	i32 86, ; 336
	i32 280, ; 337
	i32 173, ; 338
	i32 314, ; 339
	i32 184, ; 340
	i32 71, ; 341
	i32 166, ; 342
	i32 1, ; 343
	i32 301, ; 344
	i32 5, ; 345
	i32 361, ; 346
	i32 44, ; 347
	i32 245, ; 348
	i32 27, ; 349
	i32 177, ; 350
	i32 335, ; 351
	i32 156, ; 352
	i32 304, ; 353
	i32 112, ; 354
	i32 371, ; 355
	i32 191, ; 356
	i32 121, ; 357
	i32 181, ; 358
	i32 247, ; 359
	i32 228, ; 360
	i32 187, ; 361
	i32 172, ; 362
	i32 319, ; 363
	i32 259, ; 364
	i32 220, ; 365
	i32 345, ; 366
	i32 178, ; 367
	i32 183, ; 368
	i32 157, ; 369
	i32 369, ; 370
	i32 131, ; 371
	i32 324, ; 372
	i32 57, ; 373
	i32 196, ; 374
	i32 136, ; 375
	i32 83, ; 376
	i32 30, ; 377
	i32 270, ; 378
	i32 10, ; 379
	i32 298, ; 380
	i32 239, ; 381
	i32 341, ; 382
	i32 321, ; 383
	i32 169, ; 384
	i32 267, ; 385
	i32 148, ; 386
	i32 94, ; 387
	i32 329, ; 388
	i32 280, ; 389
	i32 60, ; 390
	i32 239, ; 391
	i32 211, ; 392
	i32 155, ; 393
	i32 64, ; 394
	i32 88, ; 395
	i32 236, ; 396
	i32 79, ; 397
	i32 47, ; 398
	i32 208, ; 399
	i32 238, ; 400
	i32 141, ; 401
	i32 343, ; 402
	i32 194, ; 403
	i32 359, ; 404
	i32 336, ; 405
	i32 274, ; 406
	i32 74, ; 407
	i32 185, ; 408
	i32 91, ; 409
	i32 333, ; 410
	i32 135, ; 411
	i32 90, ; 412
	i32 313, ; 413
	i32 339, ; 414
	i32 271, ; 415
	i32 341, ; 416
	i32 112, ; 417
	i32 42, ; 418
	i32 157, ; 419
	i32 4, ; 420
	i32 103, ; 421
	i32 171, ; 422
	i32 373, ; 423
	i32 70, ; 424
	i32 189, ; 425
	i32 60, ; 426
	i32 39, ; 427
	i32 261, ; 428
	i32 173, ; 429
	i32 151, ; 430
	i32 56, ; 431
	i32 34, ; 432
	i32 204, ; 433
	i32 211, ; 434
	i32 258, ; 435
	i32 21, ; 436
	i32 161, ; 437
	i32 186, ; 438
	i32 325, ; 439
	i32 238, ; 440
	i32 352, ; 441
	i32 323, ; 442
	i32 367, ; 443
	i32 348, ; 444
	i32 318, ; 445
	i32 138, ; 446
	i32 229, ; 447
	i32 355, ; 448
	i32 205, ; 449
	i32 89, ; 450
	i32 145, ; 451
	i32 273, ; 452
	i32 160, ; 453
	i32 175, ; 454
	i32 236, ; 455
	i32 303, ; 456
	i32 6, ; 457
	i32 167, ; 458
	i32 31, ; 459
	i32 107, ; 460
	i32 283, ; 461
	i32 231, ; 462
	i32 353, ; 463
	i32 318, ; 464
	i32 203, ; 465
	i32 256, ; 466
	i32 311, ; 467
	i32 165, ; 468
	i32 284, ; 469
	i32 243, ; 470
	i32 138, ; 471
	i32 59, ; 472
	i32 142, ; 473
	i32 370, ; 474
	i32 81, ; 475
	i32 74, ; 476
	i32 200, ; 477
	i32 201, ; 478
	i32 130, ; 479
	i32 25, ; 480
	i32 7, ; 481
	i32 93, ; 482
	i32 315, ; 483
	i32 234, ; 484
	i32 250, ; 485
	i32 113, ; 486
	i32 9, ; 487
	i32 231, ; 488
	i32 104, ; 489
	i32 174, ; 490
	i32 19, ; 491
	i32 282, ; 492
	i32 296, ; 493
	i32 375, ; 494
	i32 276, ; 495
	i32 33, ; 496
	i32 264, ; 497
	i32 46, ; 498
	i32 30, ; 499
	i32 265, ; 500
	i32 57, ; 501
	i32 134, ; 502
	i32 114, ; 503
	i32 320, ; 504
	i32 337, ; 505
	i32 55, ; 506
	i32 206, ; 507
	i32 6, ; 508
	i32 77, ; 509
	i32 275, ; 510
	i32 340, ; 511
	i32 361, ; 512
	i32 111, ; 513
	i32 182, ; 514
	i32 279, ; 515
	i32 102, ; 516
	i32 168, ; 517
	i32 115, ; 518
	i32 349, ; 519
	i32 244, ; 520
	i32 315, ; 521
	i32 327, ; 522
	i32 269, ; 523
	i32 185, ; 524
	i32 76, ; 525
	i32 328, ; 526
	i32 85, ; 527
	i32 334, ; 528
	i32 262, ; 529
	i32 370, ; 530
	i32 199, ; 531
	i32 305, ; 532
	i32 158, ; 533
	i32 2, ; 534
	i32 275, ; 535
	i32 24, ; 536
	i32 255, ; 537
	i32 32, ; 538
	i32 117, ; 539
	i32 37, ; 540
	i32 16, ; 541
	i32 52, ; 542
	i32 335, ; 543
	i32 326, ; 544
	i32 20, ; 545
	i32 123, ; 546
	i32 152, ; 547
	i32 282, ; 548
	i32 326, ; 549
	i32 245, ; 550
	i32 131, ; 551
	i32 264, ; 552
	i32 146, ; 553
	i32 187, ; 554
	i32 251, ; 555
	i32 120, ; 556
	i32 28, ; 557
	i32 132, ; 558
	i32 100, ; 559
	i32 134, ; 560
	i32 303, ; 561
	i32 151, ; 562
	i32 97, ; 563
	i32 125, ; 564
	i32 252, ; 565
	i32 69, ; 566
	i32 363, ; 567
	i32 72, ; 568
	i32 364, ; 569
	i32 287, ; 570
	i32 372, ; 571
	i32 306, ; 572
	i32 233, ; 573
	i32 227, ; 574
	i32 174, ; 575
	i32 124, ; 576
	i32 71, ; 577
	i32 342, ; 578
	i32 111, ; 579
	i32 297, ; 580
	i32 197, ; 581
	i32 150, ; 582
	i32 356, ; 583
	i32 355, ; 584
	i32 372, ; 585
	i32 328, ; 586
	i32 118, ; 587
	i32 273, ; 588
	i32 180, ; 589
	i32 340, ; 590
	i32 127, ; 591
	i32 182, ; 592
	i32 133, ; 593
	i32 198, ; 594
	i32 77, ; 595
	i32 46, ; 596
	i32 276, ; 597
	i32 73, ; 598
	i32 63, ; 599
	i32 98, ; 600
	i32 84, ; 601
	i32 222, ; 602
	i32 357, ; 603
	i32 43, ; 604
	i32 61, ; 605
	i32 304, ; 606
	i32 362, ; 607
	i32 193, ; 608
	i32 37, ; 609
	i32 40, ; 610
	i32 266, ; 611
	i32 338, ; 612
	i32 158, ; 613
	i32 98, ; 614
	i32 271, ; 615
	i32 198, ; 616
	i32 135, ; 617
	i32 20, ; 618
	i32 65, ; 619
	i32 344, ; 620
	i32 125, ; 621
	i32 75, ; 622
	i32 295, ; 623
	i32 162, ; 624
	i32 172, ; 625
	i32 154, ; 626
	i32 5, ; 627
	i32 49, ; 628
	i32 310, ; 629
	i32 345, ; 630
	i32 235, ; 631
	i32 142, ; 632
	i32 196, ; 633
	i32 137, ; 634
	i32 186, ; 635
	i32 100, ; 636
	i32 207, ; 637
	i32 332, ; 638
	i32 123, ; 639
	i32 120, ; 640
	i32 140, ; 641
	i32 39, ; 642
	i32 68, ; 643
	i32 248, ; 644
	i32 41, ; 645
	i32 364, ; 646
	i32 242, ; 647
	i32 162, ; 648
	i32 214, ; 649
	i32 73, ; 650
	i32 358, ; 651
	i32 163, ; 652
	i32 204, ; 653
	i32 218, ; 654
	i32 127, ; 655
	i32 281, ; 656
	i32 337, ; 657
	i32 68, ; 658
	i32 167, ; 659
	i32 327, ; 660
	i32 179, ; 661
	i32 294, ; 662
	i32 268, ; 663
	i32 176, ; 664
	i32 213, ; 665
	i32 302, ; 666
	i32 149, ; 667
	i32 45, ; 668
	i32 108, ; 669
	i32 48, ; 670
	i32 96, ; 671
	i32 31, ; 672
	i32 357, ; 673
	i32 23, ; 674
	i32 181, ; 675
	i32 164, ; 676
	i32 22, ; 677
	i32 136, ; 678
	i32 78, ; 679
	i32 365, ; 680
	i32 219, ; 681
	i32 54, ; 682
	i32 294, ; 683
	i32 300, ; 684
	i32 10, ; 685
	i32 257, ; 686
	i32 209, ; 687
	i32 299, ; 688
	i32 285, ; 689
	i32 195, ; 690
	i32 16, ; 691
	i32 137, ; 692
	i32 13, ; 693
	i32 15, ; 694
	i32 122, ; 695
	i32 360, ; 696
	i32 201, ; 697
	i32 0, ; 698
	i32 87, ; 699
	i32 147, ; 700
	i32 22, ; 701
	i32 350, ; 702
	i32 374, ; 703
	i32 34, ; 704
	i32 79, ; 705
	i32 350, ; 706
	i32 324, ; 707
	i32 241, ; 708
	i32 145, ; 709
	i32 248, ; 710
	i32 80, ; 711
	i32 348, ; 712
	i32 180, ; 713
	i32 250, ; 714
	i32 360, ; 715
	i32 42, ; 716
	i32 224, ; 717
	i32 26, ; 718
	i32 296, ; 719
	i32 293, ; 720
	i32 107, ; 721
	i32 110, ; 722
	i32 223, ; 723
	i32 351, ; 724
	i32 7, ; 725
	i32 0, ; 726
	i32 333, ; 727
	i32 44, ; 728
	i32 344, ; 729
	i32 331, ; 730
	i32 330, ; 731
	i32 159, ; 732
	i32 190, ; 733
	i32 146, ; 734
	i32 246, ; 735
	i32 262, ; 736
	i32 299, ; 737
	i32 226, ; 738
	i32 38, ; 739
	i32 15, ; 740
	i32 232, ; 741
	i32 144, ; 742
	i32 8, ; 743
	i32 279, ; 744
	i32 301, ; 745
	i32 130, ; 746
	i32 354, ; 747
	i32 353, ; 748
	i32 349, ; 749
	i32 317, ; 750
	i32 94 ; 751
], align 16

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

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
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 16

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.1xx @ af27162bee43b7fecdca59b4f67aa8c175cbc875"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
