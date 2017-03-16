using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;


namespace UnitTestsOpenTK
{
     
    public class PCABase : TestBase
    {
        protected PCA pca;
       
        public PCABase()
        {
            ResetPCA();
        }

        private void ResetPCA()
        {
            pca = new PCA();
            UIMode = false;
        }
        [SetUp]
        protected override void SetupTest()
        {
            base.SetupTest();
            ResetPCA();

        }
        protected void CheckResultTargetAndShow_Cube()
        {
            
            //-----------Show in Window
            if (UIMode)
            {
                this.ShowResultsInWindow_CubeNew(true, true);
              
            }
            //----------------check Result
            Assert.IsTrue(CheckResult(pca.MeanDistance, 0f, this.threshold));
           
        }
        protected void CheckResultTargetAndShow_Cloud(float threshold)
        {
            //-----------Show in Window
            if (UIMode)
            {
                Show3PointCloudsInWindow(true);
                //ShowPointCloudsInWindow_PCAVectors(true);
            }
            //----------------check Result
            Assert.IsTrue(CheckResult(pca.MeanDistance, 0f, threshold));

        }

    
      

        
    }
}
