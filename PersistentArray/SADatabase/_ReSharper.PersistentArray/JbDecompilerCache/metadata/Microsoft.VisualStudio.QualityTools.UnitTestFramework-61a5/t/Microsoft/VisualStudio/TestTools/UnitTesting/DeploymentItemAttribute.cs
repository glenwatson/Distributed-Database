// Type: Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute
// Assembly: Microsoft.VisualStudio.QualityTools.UnitTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll

using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DeploymentItemAttribute : Attribute
    {
        public DeploymentItemAttribute(string path);
        public DeploymentItemAttribute(string path, string outputDirectory);
        public string Path { get; }
        public string OutputDirectory { get; }
    }
}
