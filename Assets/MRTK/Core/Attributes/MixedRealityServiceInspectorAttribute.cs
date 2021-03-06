// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Microsoft.MixedReality.Toolkit
{
    /// <summary>
    /// Attach to a class implementing IMixedRealityServiceInspector to generate a facade inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MixedRealityServiceInspectorAttribute : Attribute
    {
        public MixedRealityServiceInspectorAttribute(Type serviceType)
        {
            if (!typeof(IMixedRealityService).IsAssignableFrom(serviceType))
                throw new Exception("Can't use this attribute with type " + serviceType + " - service must implement " + typeof(IMixedRealityService) + " interface.");

            ServiceType = serviceType;
        }

        public Type ServiceType { get; private set; }
    }
}
