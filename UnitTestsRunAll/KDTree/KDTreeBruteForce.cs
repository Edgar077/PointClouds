using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using System.Diagnostics;
using UnitTestsOpenTK;

namespace Automated.KDTree
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeBruteForceTest : KDTreeBaseTest
    {
     
        [SetUp]
        public void Prepare()
        {
            tree = new KDTreeBruteForce();
        }
        
    }
}
