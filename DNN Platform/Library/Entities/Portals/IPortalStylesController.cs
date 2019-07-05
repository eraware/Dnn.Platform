#region Copyright

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace DotNetNuke.Entities.Portals
{
    /// <summary>
    /// Manages reading and updating portal styles
    /// </summary>
    public interface IPortalStylesController
    {
        /// <summary>
        /// Gets the portal styles for the given portalId
        /// </summary>
        /// <param name="portalId">ID of the portal</param>
        /// <returns></returns>
        PortalStyles GetPortalStyles(int portalId);

        /// <summary>
        /// Creates or updates the portal styles for the given portal
        /// </summary>
        /// <param name="portalId">The Id of the portal</param>
        /// <param name="portalStyles">The styles to save</param>
        void UpdatePortalStyles(int portalId, PortalStyles portalStyles);
    }
}
