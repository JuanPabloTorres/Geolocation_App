using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationAds.ViewModels
{
    public partial class MetaDataViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime createDate;

        [ObservableProperty]
        public long dataSize;
    }
}