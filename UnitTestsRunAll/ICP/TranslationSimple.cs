using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace Automated.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class TranslationSimple : TestBaseICP
    {
         
        
        [Test]
        public void Translation_Horn()
        {

           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            meanDistance = ICPTestData.Test1_Translation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            Assert.IsTrue(this.threshold > meanDistance);


        }
        [Test]
        public void Translation_Umeyama()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            meanDistance = ICPTestData.Test1_Translation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            //have to check why Umeyama is not exact to e-10 - perhaps because of diagonalization lib (for the scale factor) 
            if (! (this.threshold > meanDistance))
            {
                System.Diagnostics.Debug.WriteLine("Translation Umeyama failed");
                Assert.Fail("Translation Umeyama failed");
            }
            
        }
        [Test]
        public void Translation_Du()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            meanDistance = ICPTestData.Test1_Translation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            if (! (this.threshold > meanDistance))
            {
                System.Diagnostics.Debug.WriteLine("Translation Du failed");
                Assert.Fail("Translation Du failed");
            }

        }
        [Test]
        public void Translation_Zinsser()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            meanDistance = ICPTestData.Test1_Translation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            if (! (this.threshold > meanDistance))
            {
                System.Diagnostics.Debug.WriteLine("Translation Zinsser failed");
                Assert.Fail("Translation Zinsser failed");
            }

        }
   
     
    }
}
