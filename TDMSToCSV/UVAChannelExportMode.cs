using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMSToCSV
{
    public enum UVAChannelExportMode
    {
        /// <summary>
        /// Every sample is exported with a timestamp
        /// </summary>
        Raw,


        /// <summary>
        /// N samples per second is exported
        /// </summary>
        NSamplesPerSecond,


        /// <summary>
        /// Summary information about the channel data
        /// </summary>
        Summary,
    }
}
