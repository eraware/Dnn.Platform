using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNuke.Entities.Portals
{
    public class PortalStyles
    {
        /// <summary>
        /// The Portal primary Color
        /// </summary>
        public StyleColorBase PrimaryColor { get; set; }

        /// <summary>
        /// A lighter version of the primary color
        /// </summary>
        public StyleColorBase PrimaryColorLight { get; set; }

        /// <summary>
        /// A darker version of the primary color
        /// </summary>
        public StyleColorBase PrimaryColorDark { get; set; }

        /// <summary>
        /// A color that has great contrasts against the primary color
        /// </summary>
        public StyleColorBase PrimaryColorContrast { get; set; }

        /// <summary>
        /// The Portal secondary Color
        /// </summary>
        public StyleColorBase SecondaryColor { get; set; }

        /// <summary>
        /// A lighter version of the secondary color
        /// </summary>
        public StyleColorBase SecondaryColorLight { get; set; }

        /// <summary>
        /// A darker version of the secondary color
        /// </summary>
        public StyleColorBase SecondaryColorDark { get; set; }

        /// <summary>
        /// A color that has great contrasts against the secondary color
        /// </summary>
        public StyleColorBase SecondaryColorContrast { get; set; }

        /// <summary>
        /// The Portal tertiary Color
        /// </summary>
        public StyleColorBase TertiaryColor { get; set; }

        /// <summary>
        /// A lighter version of the tertiary color
        /// </summary>
        public StyleColorBase TertiaryColorLight { get; set; }

        /// <summary>
        /// A darker version of the tertiary color
        /// </summary>
        public StyleColorBase TertiaryColorDark { get; set; }

        /// <summary>
        /// A color that has great contrasts against the tertiary color
        /// </summary>
        public StyleColorBase TertiaryColorContrast { get; set; }

        /// <summary>
        /// The default border radius for common UI elements such a dialogs, tabs, cards, etc.
        /// </summary>
        public int DefaultRadius { get; set; }

        /// <summary>
        /// The radius of button borders
        /// </summary>
        public int ButtonRadius { get; set; }


    }
}
