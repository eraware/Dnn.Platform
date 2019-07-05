#region Copyright

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion


#region usings
using DotNetNuke.Collections;
using DotNetNuke.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DotNetNuke.Entities.Portals
{
    class PortalStylesController : IPortalStylesController
    {
        private const string PRIMARY_COLOR = "PrimaryColor";
        private const string PRIMARY_COLOR_LIGHT = "PrimaryColorLight";
        private const string PRIMARY_COLOR_DARK = "PrimaryColorDark";
        private const string PRIMARY_COLOR_CONTRAST = "PrimaryColorContrast";
        private const string SECONDARY_COLOR = "SecondaryColor";
        private const string SECONDARY_COLOR_LIGHT = "SecondaryColorLight";
        private const string SECONDARY_COLOR_DARK = "SecondaryColorDark";
        private const string SECONDARY_COLOR_CONTRAST = "SecondaryColorContrast";
        private const string TERTIARY_COLOR = "TertiaryColor";
        private const string TERTIARY_COLOR_LIGHT = "TertiaryColorLight";
        private const string TERTIARY_COLOR_DARK = "TertiaryColorDark";
        private const string TERTIARY_COLOR_CONTRAST = "TertiaryColorContrast";
        private const string DEFAULT_RADIUS = "DefaultRadius";
        private const string BUTTON_RADIUS = "ButtonRadius";

        /// <summary>
        /// Gets an instance of the PortalStylescontroller
        /// </summary>
        /// <returns></returns>
        public static IPortalStylesController Instance()
        {
            var controller = ComponentFactory.GetComponent<IPortalStylesController>("PortalStylesController");
            if (controller == null)
            {
                ComponentFactory.RegisterComponent<IPortalStylesController, PortalStylesController>("PortalStylesController");
            }
            return ComponentFactory.GetComponent<IPortalStylesController>("PortalStylesController");
        }

        /// <summary>
        /// Gets the portal styles for the given portal
        /// </summary>
        /// <param name="portalId">The Id of the portal</param>
        /// <returns></returns>
        public PortalStyles GetPortalStyles(int portalId)
        {
            var settings = PortalController.Instance.GetPortalSettings(portalId);
            var portalStyles = new PortalStyles();

            portalStyles.PrimaryColor = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR, "3792ED"));
            portalStyles.PrimaryColorLight = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_LIGHT, "6CB6F3"));
            portalStyles.PrimaryColorDark = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_DARK, "0D569E"));
            portalStyles.PrimaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_CONTRAST, GetContrastColor(portalStyles.PrimaryColor)));

            portalStyles.SecondaryColor = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR, "F5F5F5"));
            portalStyles.SecondaryColorLight = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_LIGHT, "FEFEFE"));
            portalStyles.SecondaryColorDark = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_DARK, "E8E8E8"));
            portalStyles.SecondaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_CONTRAST, GetContrastColor(portalStyles.SecondaryColor)));

            portalStyles.TertiaryColor = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR, "EAEAEA"));
            portalStyles.TertiaryColorLight = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_LIGHT, "F2F2F2"));
            portalStyles.TertiaryColorDark = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_DARK, "D8D8D8"));
            portalStyles.TertiaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_CONTRAST, GetContrastColor(portalStyles.TertiaryColor)));

            portalStyles.DefaultRadius = settings.GetValueOrDefault(DEFAULT_RADIUS, 3);
            portalStyles.ButtonRadius = settings.GetValueOrDefault(BUTTON_RADIUS, 3);

            return portalStyles;
        }

        public void UpdatePortalStyles(int portalId, PortalStyles portalStyles)
        {
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR, portalStyles.PrimaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_LIGHT, portalStyles.PrimaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_DARK, portalStyles.PrimaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_CONTRAST, portalStyles.PrimaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR, portalStyles.SecondaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_LIGHT, portalStyles.SecondaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_DARK, portalStyles.SecondaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_CONTRAST, portalStyles.SecondaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR, portalStyles.TertiaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_LIGHT, portalStyles.TertiaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_DARK, portalStyles.TertiaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_CONTRAST, portalStyles.TertiaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, DEFAULT_RADIUS, portalStyles.DefaultRadius.ToString(), false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, BUTTON_RADIUS, portalStyles.ButtonRadius.ToString(), false, null, true);
        }

        /// <summary>
        /// Gets white or black css color depending on which provides the most contrast to the base color
        /// </summary>
        /// <param name="color">The color to contrast agains</param>
        /// <returns>"000000" or "FFFFFF"</returns>
        public string GetContrastColor(StyleColorBase color)
        {
            string result = (color.Red / 255 * 0.299 + color.Green / 255 * 0.587 + color.Blue / 255 * 0.114) > 186 ? "000000" : "FFFFFF";
            return result;
        }
    }
}
