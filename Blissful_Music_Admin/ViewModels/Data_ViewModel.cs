using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blissful_Music_Admin.ViewModels
{
    public class Data_ViewModel
    {
        public string NAME { get; set; }
        public string IMAGE { get; set; }

        public string VIDEOS_LINK { get; set; }
        public string VIDEOS_NAME { get; set; }

    }

    public class Data_ViewModelKey
    {
        public string Key { get; set; }
        public string NAME { get; set; }
        public string IMAGE { get; set; }
        public string VIDEOS_LINK { get; set; }
        public string VIDEOS_NAME { get; set; }
    }
}
