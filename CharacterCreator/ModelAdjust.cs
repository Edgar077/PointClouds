using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTKExtension;
using PointCloudUtils;

namespace CharacterCreator
{
    public partial class ModelAdjust : Form
    {
        Skeleton skeleton;
        private float conversionFactor = 100;
        public ModelAdjust(Skeleton sk)
        {
            skeleton = sk;
            InitializeComponent();
        }
        private void populateControls()
        {
            this.numericFootY.Value = Convert.ToDecimal( Skeleton.ModelExchangeDictionary[ModelExchangeSizes.feet_height_Z] * conversionFactor);
            this.numericUpDownKneeY.Value = Convert.ToDecimal(Skeleton.ModelExchangeDictionary[ModelExchangeSizes.lowerleg_length] * conversionFactor);
            this.numericUpDownUpperLegY.Value = Convert.ToDecimal(Skeleton.ModelExchangeDictionary[ModelExchangeSizes.upperleg_length] * conversionFactor);


        }
    }
}
