using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle.Models
{
    public class Meta
    {
        /// <summary>
        /// The set name is not an actual resource, but the root name common to all parts belonging to one set
        /// e.g. O4_2_1.png and O4_2_2.png will have a set name of O4_2.png 
        /// </summary>
        public string SetName { get; set; }
        public bool IsSet => !string.IsNullOrEmpty(SetName);
        public bool IsFrame { get; set; } = false;
        public string ResourceName { get; set; }
        /// <summary>
        /// Doesn't include the path, includes the extension
        /// </summary>
        public string FileName { get; set; }
        public List<string> ResourceNames { get; set; }
        public bool CannotDuplicate { get; set; }

        public ImgPoint MainAnchor { get; set; }

        public Overlay Overlay { get; set; }
    }
}
