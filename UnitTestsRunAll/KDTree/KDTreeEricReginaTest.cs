using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using Automated;
using System.Diagnostics;
using UnitTestsOpenTK;
namespace Automated.KDTree
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeEricReginaTest : KDTreeBaseTest
    {
     
        [SetUp]
        public void Prepare()
        {
            tree = new KDTreeEricRegina();
        }
        
    }
}
